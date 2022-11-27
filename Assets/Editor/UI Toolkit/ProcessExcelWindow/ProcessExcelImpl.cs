using Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static UGame_Local_Editor.ProcessExcelWindow;

namespace UGame_Local_Editor
{
    public class ProcessExcelImpl
    {
        enum eFieldTypes { Unknown, Int, Ints, Long, Float, Floats, Vector2, Vector3, Vector4, Rect, Color, String, Strings, Skip }
        private static Regex reg_color32 = new Regex(@"^[A-Fa-f0-9]{8}$");
        private static Regex reg_color24 = new Regex(@"^[A-Fa-f0-9]{6}$");


        public bool Process(Head head, Body body, bool generateCode)
        {
           
            string className = Path.GetFileNameWithoutExtension(body.ExcelPath);

            //检查文件名是否合法
            if (!Regex.IsMatch(className, @"^[A-Z][A-Za-z0-9_]*$"))
            {
                EditorUtility.DisplayDialog("Excel To ScriptableObject", "excel 文件名不合法，因为excel的名称是类名", "OK");
                return false;
            }


            Stream stream = null;
            int lastDot = body.ExcelPath.LastIndexOf('.');
            string tempExcelName = $"{body.ExcelPath.Substring(0, lastDot)}_temp{body.ExcelPath.Substring(lastDot, body.ExcelPath.Length - lastDot)}";


            try//检查文件是否被Excel应用占用
            {
                File.Copy(body.ExcelPath, tempExcelName);
                stream = File.OpenRead(tempExcelName);
            }
            catch
            {
                string msg = $"{className}打开中，请先关闭此Excel应用程序";
                EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
                File.Delete(tempExcelName);
                return false;
            }


            //读取Excel表数据
            IExcelDataReader reader = body.ExcelPath.ToLower().EndsWith(".xls") ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet data = reader.AsDataSet();
            reader.Dispose();
            stream.Close();
            File.Delete(tempExcelName);


            //检查数据是否读取成功
            if (data == null)
            {
                string msg = $"{className}读取失败，请检查";
                EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
                return false;
            }

            //检查文件属性名称和数据类型
            DataTable table = data.Tables[0];
            if (table.Rows.Count < Mathf.Max(head.FieldRow, head.TypeRow) + 1)
            {
                string msg = $"解析失败 {className}，文件应该至少包含2行，用于指定名称和类型";
                EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
                return false;
            }


            //获取文件FieldRow行的内容
            List<string> fieldNames = new List<string>();
            List<int> fieldIndices = new List<int>();
            object[] items = table.Rows[head.FieldRow].ItemArray;
            for (int i = 0; i < items.Length; i++)
            {
                string fieldName = items[i].ToString().Trim();
                if (string.IsNullOrEmpty(fieldName)) { break; }
                if (!Regex.IsMatch(fieldName, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                {
                    var msg = $"{className}解析失败，{fieldName}字段名无效";
                    EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
                    return false;
                }
                fieldNames.Add(fieldName);
                fieldIndices.Add(i);
            }

            if (fieldNames.Count <= 0)
            {
                string msg = $"{className}解析失败，FieldRow行内容为空";
                EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
                return false;
            }


            //枚举
            Dictionary<string, List<string>> enumDict = body.TreaUnknowTypesasEnum ? new Dictionary<string, List<string>>() : null;
            int firstIndex = fieldIndices[0];
            List<eFieldTypes> fieldTypes = new List<eFieldTypes>();
            List<string> fieldTypeNames = new List<string>();
            items = table.Rows[head.TypeRow].ItemArray;

            for (int i = 0; i < fieldNames.Count; i++)
            {
                int fieldIndex = fieldIndices[i];
                string typeName = items[fieldIndex].ToString();
                eFieldTypes fieldType = GetFieldType(typeName);

                //未知字段判断是否是枚举
                if (fieldType == eFieldTypes.Unknown)
                {
                    //未知字段，且不是枚举，则无法解析
                    if (enumDict == null)
                    {
                        string msg = $"{className}解析失败，无效的字段{typeName}";
                        EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
                        return false;
                    }

                    //未知字段，是枚举，则记录枚举
                    if (!enumDict.TryGetValue(typeName, out List<string> enumValues))
                    {
                        enumValues = new List<string>();
                        enumValues.Add("Default");//枚举添加一个默认值
                        enumDict.Add(typeName, enumValues);
                    }


                    for (int j = head.DataFromRow; j < table.Rows.Count; j++)
                    {
                        object[] enumObjs = table.Rows[j].ItemArray;
                        string enumValue = fieldIndex < enumObjs.Length ? enumObjs[fieldIndex].ToString() : null;
                        if (!enumValues.Contains(enumValue))
                        {
                            enumValues.Add(enumValue);
                        }
                    }
                }

                fieldTypes.Add(fieldType);
                fieldTypeNames.Add(typeName);
            }


            //获取表所有的id，id只能为int类型或者string
            List<int> indices = new List<int>();
            if (body.GenerateGetMethodIfPossoble)//生成获取方法
            {
                string err = string.Empty;

                //id是 int 类型
                if (fieldTypes[0] == eFieldTypes.Int)
                {
                    SortedList<int, List<int>> ids = new SortedList<int, List<int>>();

                    for (int i = head.DataFromRow; i < table.Rows.Count; i++)
                    {
                        string info = $"Checking IDs or Keys...{i - 1}/{table.Rows.Count - 2}";
                        float progress = (i - 1) / (table.Rows.Count - 2);

                        if (EditorUtility.DisplayCancelableProgressBar($"解析{className}", info, progress))
                        {
                            EditorUtility.ClearProgressBar();
                            return false;
                        }


                        string idStr = table.Rows[i].ItemArray[firstIndex].ToString().Trim();
                        if (string.IsNullOrEmpty(idStr)) continue;

                        if (!int.TryParse(idStr, out int id))
                        {
                            err = $"excel:{className}--第{i}行中的{ids}不是 int 类型";

                            break;
                        }



                        if (ids.TryGetValue(id, out List<int> idList))
                        {
                            if (body.IDorKeytoMultiValues)
                            {
                                idList.Add(i);
                            }
                            else
                            {
                                err = $"excel:{className}--id 或 key：{id} 出现多次";
                                break;
                            }
                        }
                        else
                        {
                            idList = new List<int>();
                            idList.Add(i);
                            ids.Add(id, idList);
                        }
                    }

                    //记录Id
                    foreach (KeyValuePair<int, List<int>> kv in ids)
                    {
                        indices.AddRange(kv.Value);
                    }
                }
                //id是 string 类型
                else if (fieldTypes[0] == eFieldTypes.String)
                {
                    SortedList<string, List<int>> keys = new SortedList<string, List<int>>();

                    for (int i = head.DataFromRow; i < table.Rows.Count; i++)
                    {
                        string info = $"Checking IDs or Keys...{i - 1}/{table.Rows.Count - 2}";
                        float progress = (i - 1) / (table.Rows.Count - 2);

                        if (EditorUtility.DisplayCancelableProgressBar($"解析{className}", info, progress))
                        {
                            EditorUtility.ClearProgressBar();
                            return false;
                        }

                        string key = table.Rows[i].ItemArray[firstIndex].ToString().Trim();
                        if (string.IsNullOrEmpty(key)) continue;


                        if (keys.TryGetValue(key, out List<int> idList))
                        {
                            if (body.IDorKeytoMultiValues)
                            {
                                idList.Add(i);
                            }
                            else
                            {
                                err = $"excel:{className}--id 或 key：{key} 出现多次";
                                break;
                            }
                        }
                        else
                        {
                            idList = new List<int>();
                            idList.Add(i);
                            keys.Add(key, idList);
                        }
                    }

                    foreach (KeyValuePair<string, List<int>> kv in keys)
                    {
                        indices.AddRange(kv.Value);
                    }
                }


                //解析id，发生错误
                if (!string.IsNullOrEmpty(err))
                {
                    Debug.LogError(err);
                    indices.Clear();
                }
                indices.Reverse();
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();

            }


            EditorUtility.ClearProgressBar();



            //生成代码
            if (generateCode)
            {
                string serializeAttribute = body.HideaAssetProperties ? "[SerializeField, HideInInspector]" : "[SerializeField]";

                StringBuilder content = new StringBuilder();

                //添加说明
                content.AppendLine("//----------------------------------------------");
                content.AppendLine("//    Auto Generated. DO NOT edit manually!");
                content.AppendLine("//----------------------------------------------");
                content.AppendLine();

                //添加引用
                content.AppendLine("using UnityEngine;");

                if (body.GenerateGetMethodIfPossoble && body.IDorKeytoMultiValues && indices.Count > 0)
                {
                    content.AppendLine("using System.Collections.Generic;");
                }


                content.AppendLine();

                string indent = "";//缩进

                //添加命名空间
                if (!string.IsNullOrEmpty(body.NameSpace))
                {
                    content.AppendLine(string.Format("namespace {0} {{", body.NameSpace));
                    content.AppendLine();
                    indent = "\t";
                }

                //添加类名
                content.AppendLine(string.Format("{0}public partial class {1} : ScriptableObject {{", indent, className));
                content.AppendLine();


                if (body.UseHashString)
                {
                    content.AppendLine(string.Format("{0}\t{1}", indent, serializeAttribute));
                    content.AppendLine(string.Format("{0}\tprivate string[] _HashStrings;", indent));
                    content.AppendLine();
                    content.AppendLine(string.Format("{0}\tprivate bool mHashStringSet = false;", indent));
                    content.AppendLine();
                }


                content.AppendLine(string.Format("{0}\t{1}", indent, serializeAttribute));
                content.AppendLine(string.Format("{0}\tprivate {1}Item[] _Items;", indent, className));


                if (body.UseHashString)
                {
                    content.AppendLine(string.Format("{0}\t{1} {2}Item[] items {{", indent, body.PublicItemsGetter ? "public" : "private", className));
                    content.AppendLine($"{indent}\t\tget {{");
                    content.AppendLine(string.Format("{0}\t\t\tif (!mHashStringSet) {{", indent));
                    content.AppendLine(string.Format("{0}\t\t\t\tfor (int i = 0, imax = _Items.Length; i < imax; i++) {{", indent));
                    content.AppendLine(string.Format("{0}\t\t\t\t\t_Items[i].SetStrings(_HashStrings);", indent));
                    content.AppendLine(string.Format("{0}\t\t\t\t}}", indent));
                    content.AppendLine(string.Format("{0}\t\t\t\tmHashStringSet = true;", indent));
                    content.AppendLine(string.Format("{0}\t\t\t}}", indent));
                    content.AppendLine(string.Format("{0}\t\t\treturn _Items;", indent));
                    content.AppendLine(string.Format("{0}\t\t}}", indent));
                    content.AppendLine(string.Format("{0}\t}}", indent));

                    /** 生成如下代码
                           public Item[] items
                           {
                               get
                               {
                                   if (!mHashStringSet)
                                   {
                                       for (int i = 0; i < _Items.Length; i++)
                                       {
                                           _Items[i].SetStrings(_HashStrings);
                                       }
                                       mHashStringSet = true;
                                   }
                                   return _Items;
                               }
                           }
                     */
                }
                else
                {
                    content.AppendLine(string.Format("{0}\t{1} {2}Item[] items {{ get {{ return _Items; }} }}", indent, body.PublicItemsGetter ? "public" : "private", className));
                }


                content.AppendLine();

                if (indices.Count > 0)
                {
                    string idVarName = fieldNames[0];

                    idVarName = idVarName.Substring(0, 1).ToLower() + idVarName.Substring(1, idVarName.Length - 1);

                    if (body.IDorKeytoMultiValues)
                    {
                        content.AppendLine(string.Format("{0}\tpublic List<{1}Item> Get({2} {3}) {{", indent, className, GetFieldTypeName(fieldTypes[0]), idVarName));
                        content.AppendLine(string.Format("{0}\t\tList<{1}Item> list = new List<{1}Item>(); ", indent, className));
                        content.AppendLine(string.Format("{0}\t\tGet({1}, list);", indent, idVarName));
                        content.AppendLine(string.Format("{0}\t\treturn list;", indent));
                        content.AppendLine(string.Format("{0}\t}}", indent));
                        content.AppendLine(string.Format("{0}\tpublic int Get({2} {3}, List<{1}Item> list) {{", indent, className, GetFieldTypeName(fieldTypes[0]), idVarName));
                        content.AppendLine(string.Format("{0}\t\tint min = 0;", indent));
                        content.AppendLine(string.Format("{0}\t\tint len = items.Length;", indent));
                        content.AppendLine(string.Format("{0}\t\tint max = len;", indent));
                        content.AppendLine(string.Format("{0}\t\tint index = -1;", indent));
                        content.AppendLine(string.Format("{0}\t\twhile (min < max) {{", indent));
                        content.AppendLine(string.Format("{0}\t\t\tint i = (min + max) >> 1;", indent));
                        content.AppendLine(string.Format("{0}\t\t\t{1}Item item = _Items[i];", indent, className));
                        content.AppendLine(string.Format("{0}\t\t\tif (item.{1} == {2}) {{", indent, fieldNames[0], idVarName));
                        content.AppendLine(string.Format("{0}\t\t\t\tindex = i;", indent));
                        content.AppendLine(string.Format("{0}\t\t\t\tbreak;", indent));
                        content.AppendLine(string.Format("{0}\t\t\t}}", indent));

                        if (fieldTypes[0] == eFieldTypes.String)
                        {
                            content.AppendLine(string.Format("{0}\t\t\tif (string.Compare({1}, item.{2}) < 0) {{", indent, idVarName, fieldNames[0]));
                        }
                        else
                        {
                            content.AppendLine(string.Format("{0}\t\t\tif ({1} < item.{2}) {{", indent, idVarName, fieldNames[0]));
                        }

                        content.AppendLine(string.Format("{0}\t\t\t\tmax = i;", indent));
                        content.AppendLine(string.Format("{0}\t\t\t}} else {{", indent));
                        content.AppendLine(string.Format("{0}\t\t\t\tmin = i + 1;", indent));
                        content.AppendLine(string.Format("{0}\t\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t\tif (index < 0) {{ return 0; }}", indent));
                        content.AppendLine(string.Format("{0}\t\tint l = index;", indent));
                        content.AppendLine(string.Format("{0}\t\twhile (l - 1 >= 0 && _Items[l - 1].{1} == {2}) {{ l--; }}", indent, fieldNames[0], idVarName));
                        content.AppendLine(string.Format("{0}\t\tint r = index;", indent));
                        content.AppendLine(string.Format("{0}\t\twhile (r + 1 < len && _Items[r + 1].{1} == {2}) {{ r++; }}", indent, fieldNames[0], idVarName));
                        content.AppendLine(string.Format("{0}\t\tfor (int i = l; i <= r; i++) {{", indent));
                        content.AppendLine(string.Format("{0}\t\t\tlist.Add(_Items[i]);", indent));
                        content.AppendLine(string.Format("{0}\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t\treturn r - l + 1;", indent));
                        content.AppendLine(string.Format("{0}\t}}", indent));
                    }
                    else
                    {
                        content.AppendLine(string.Format("{0}\tpublic {1}Item Get({2} {3}) {{", indent, className, GetFieldTypeName(fieldTypes[0]), idVarName));
                        content.AppendLine(string.Format("{0}\t\tint min = 0;", indent));
                        content.AppendLine(string.Format("{0}\t\tint max = items.Length;", indent));
                        content.AppendLine(string.Format("{0}\t\twhile (min < max) {{", indent));
                        content.AppendLine(string.Format("{0}\t\t\tint index = (min + max) >> 1;", indent));
                        content.AppendLine(string.Format("{0}\t\t\t{1}Item item = _Items[index];", indent, className));
                        content.AppendLine(string.Format("{0}\t\t\tif (item.{1} == {2}) {{ return item; }}", indent, fieldNames[0], idVarName));
                        if (fieldTypes[0] == eFieldTypes.String)
                        {
                            content.AppendLine(string.Format("{0}\t\t\tif (string.Compare({1}, item.{2}) < 0) {{", indent, idVarName, fieldNames[0]));
                        }
                        else
                        {
                            content.AppendLine(string.Format("{0}\t\t\tif ({1} < item.{2}) {{", indent, idVarName, fieldNames[0]));
                        }
                        content.AppendLine(string.Format("{0}\t\t\t\tmax = index;", indent));
                        content.AppendLine(string.Format("{0}\t\t\t}} else {{", indent));
                        content.AppendLine(string.Format("{0}\t\t\t\tmin = index + 1;", indent));
                        content.AppendLine(string.Format("{0}\t\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t\treturn null;", indent));
                        content.AppendLine(string.Format("{0}\t}}", indent));
                    }

                    content.AppendLine();

                }

                content.AppendLine(string.Format("{0}}}", indent));
                content.AppendLine();

                content.AppendLine(string.Format("{0}[System.Serializable]", indent));
                content.AppendLine(string.Format("{0}public class {1}Item {{", indent, className));
                content.AppendLine();

                if (enumDict != null)
                {
                    foreach (KeyValuePair<string, List<string>> kv in enumDict)
                    {
                        content.AppendLine(string.Format("{0}\tpublic enum {1} {{", indent, kv.Key));
                        content.Append(indent);
                        content.Append("\t\t");
                        for (int i = 0, imax = kv.Value.Count; i < imax; i++)
                        {
                            content.Append(kv.Value[i]);
                            if (i < imax - 1)
                            {
                                content.Append(", ");
                            }
                            else
                            {
                                content.AppendLine();
                            }
                        }
                        content.AppendLine(string.Format("{0}\t}}", indent));
                        content.AppendLine();
                    }
                }


                if (body.UseHashString)
                {
                    content.AppendLine(string.Format("{0}\tprivate string[] mHashStrings;", indent));
                    content.AppendLine(string.Format("{0}\tpublic void SetStrings(string[] strings) {{ mHashStrings = strings; }}", indent));
                    content.AppendLine();
                }


                for (int i = 0, imax = fieldNames.Count; i < imax; i++)
                {
                    string fieldName = fieldNames[i];
                    eFieldTypes fieldType = fieldTypes[i];
                    if (fieldType == eFieldTypes.Skip) continue;
                    string fieldTypeName = fieldType == eFieldTypes.Unknown && enumDict != null ? fieldTypeNames[i] : GetFieldTypeName(fieldType);
                    if (fieldType == eFieldTypes.Skip) continue;
                    string capitalFieldName = CapitalFirstChar(fieldName);
                    content.AppendLine(string.Format("{0}\t{1}", indent, serializeAttribute));

                    if (body.UseHashString && fieldType == eFieldTypes.String)
                    {
                        content.AppendLine(string.Format("{0}\tprivate int _{1};", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\tpublic {1} {2} {{ get {{ return mHashStrings[_{3}]; }} }}", indent, fieldTypeName, fieldName, capitalFieldName));
                    }
                    else if (body.UseHashString && fieldType == eFieldTypes.Strings)
                    {
                        content.AppendLine(string.Format("{0}\tprivate int[] _{1};", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\tprivate {1} _{2}_;", indent, fieldTypeName, capitalFieldName));
                        content.AppendLine(string.Format("{0}\tpublic {1} {2} {{", indent, fieldTypeName, fieldName));
                        content.AppendLine(string.Format("{0}\t\tget {{", indent));
                        content.AppendLine(string.Format("{0}\t\t\tif (_{1}_ == null || _{1}_.Length != _{1}.Length) {{ _{1}_ = new string[_{1}.Length]; }}",
                            indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\t\t\tfor (int i = _{1}.Length - 1; i >= 0; i--) {{", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\t\t\t\t_{1}_[i] = mHashStrings[_{1}[i]];", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\t\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t\t\treturn _{1}_;", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t}}", indent));
                    }
                    else if (body.CompressColorintoInteger && fieldType == eFieldTypes.Color)
                    {
                        content.AppendLine(string.Format("{0}\tprivate int _{1};", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\tpublic {1} {2} {{", indent, fieldTypeName, fieldName));
                        content.AppendLine(string.Format("{0}\t\tget {{", indent));
                        content.AppendLine(string.Format("{0}\t\t\tfloat inv = 1f / 255f;", indent));
                        content.AppendLine(string.Format("{0}\t\t\tColor c = Color.black;", indent));
                        content.AppendLine(string.Format("{0}\t\t\tc.r = inv * ((_{1} >> 24) & 0xFF);", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\t\t\tc.g = inv * ((_{1} >> 16) & 0xFF);", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\t\t\tc.b = inv * ((_{1} >> 8) & 0xFF);", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\t\t\tc.a = inv * (_{1} & 0xFF);", indent, capitalFieldName));
                        content.AppendLine(string.Format("{0}\t\t\treturn c;", indent));
                        content.AppendLine(string.Format("{0}\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t}}", indent));
                    }
                    else
                    {
                        content.AppendLine(string.Format("{0}\tprivate {1} _{2};", indent, fieldTypeName, capitalFieldName));
                        content.AppendLine(string.Format("{0}\tpublic {1} {2} {{ get {{ return _{3}; }} }}", indent, fieldTypeName, fieldName, capitalFieldName));
                    }
                    content.AppendLine();

                }

                if (body.GengrateToStringMethod)
                {
                    content.AppendLine(string.Format("{0}\tpublic override string ToString() {{", indent));
                    List<string> toStringFormats = new List<string>();
                    List<string> toStringValues = new List<string>();
                    bool toStringContainsArray = false;
                    int formatIndex = 0;


                    for (int i = 0, imax = fieldNames.Count; i < imax; i++)
                    {
                        string fieldName = fieldNames[i];
                        eFieldTypes fieldType = fieldTypes[i];
                        if (fieldType == eFieldTypes.Skip) continue;
                        toStringFormats.Add(string.Format("{0}:{{{1}}}", fieldName, formatIndex));
                        bool isArray = fieldType == eFieldTypes.Floats || fieldType == eFieldTypes.Ints || fieldType == eFieldTypes.Strings;
                        if (isArray)
                        {
                            toStringValues.Add(string.Format("array2string({0})", fieldName));
                        }
                        else
                        {
                            toStringValues.Add(fieldName);
                        }
                        if (!toStringContainsArray)
                        {
                            toStringContainsArray = isArray;
                        }

                        formatIndex++;
                    }

                    content.AppendLine(string.Format("{0}\t\treturn string.Format(\"[{1}Item]{{{{{2}}}}}\",", indent, className, string.Join(", ", toStringFormats.ToArray())));
                    content.AppendLine(string.Format("{0}\t\t\t{1});", indent, string.Join(", ", toStringValues.ToArray())));
                    content.AppendLine(string.Format("{0}\t}}", indent));
                    content.AppendLine();

                    if (toStringContainsArray)
                    {
                        content.AppendLine(string.Format("{0}\tprivate string array2string(System.Array array) {{", indent));
                        content.AppendLine(string.Format("{0}\t\tint len = array.Length;", indent));
                        content.AppendLine(string.Format("{0}\t\tstring[] strs = new string[len];", indent));
                        content.AppendLine(string.Format("{0}\t\tfor (int i = 0; i < len; i++) {{", indent));
                        content.AppendLine(string.Format("{0}\t\t\tstrs[i] = string.Format(\"{{0}}\", array.GetValue(i));", indent));
                        content.AppendLine(string.Format("{0}\t\t}}", indent));
                        content.AppendLine(string.Format("{0}\t\treturn string.Concat(\"[\", string.Join(\", \", strs), \"]\");", indent));
                        content.AppendLine(string.Format("{0}\t}}", indent));
                        content.AppendLine();
                    }
                }

                if (!string.IsNullOrEmpty(body.NameSpace))
                {
                    content.AppendLine("\t}");
                    content.AppendLine();
                }

                content.AppendLine("}");


                if (!Directory.Exists(body.ScriptFolder))
                {
                    Directory.CreateDirectory(body.ScriptFolder);
                }

                string scriptPath = null;
                if (body.ScriptFolder.EndsWith("/"))
                {
                    scriptPath = string.Concat(body.ScriptFolder, className, ".cs");
                }
                else
                {
                    scriptPath = string.Concat(body.ScriptFolder, "/", className, ".cs");
                }

                string fileMD5 = null;
                MD5CryptoServiceProvider md5Calc = null;
                if (File.Exists(scriptPath))
                {
                    md5Calc = new MD5CryptoServiceProvider();
                    try
                    {
                        using (FileStream fs = File.OpenRead(scriptPath))
                        {
                            fileMD5 = System.BitConverter.ToString(md5Calc.ComputeHash(fs));
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                byte[] bytes = Encoding.UTF8.GetBytes(content.ToString());
                bool toWrite = true;
                if (!string.IsNullOrEmpty(fileMD5))
                {
                    if (System.BitConverter.ToString(md5Calc.ComputeHash(bytes)) == fileMD5)
                    {
                        toWrite = false;
                    }
                }
                if (toWrite)
                {
                    File.WriteAllBytes(scriptPath, bytes);
                }

            }
            else
            {
                if (indices.Count <= 0)
                {
                    for (int i = table.Rows.Count - 1; i >= head.DataFromRow; i--)
                    {
                        indices.Add(i);
                    }
                }

                if (!Directory.Exists(body.AssetFolder))
                {
                    Directory.CreateDirectory(body.AssetFolder);
                }
                AssetDatabase.Refresh();

                string assetPath = null;
                if (body.ScriptFolder.EndsWith("/"))
                {
                    assetPath = string.Concat(body.AssetFolder, className, ".asset");
                }
                else
                {
                    assetPath = string.Concat(body.AssetFolder, "/", className, ".asset");
                }

                Debug.Log($"asspath:{assetPath}");
                assetPath = $"Assets/Data/{className}.asset";

                ScriptableObject obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ScriptableObject)) as ScriptableObject;
                bool isAlreadyExists = true;
                if (obj == null)
                {
                    string fullName = !string.IsNullOrEmpty(body.NameSpace) ? body.NameSpace + "." + className : className;
                    obj = ScriptableObject.CreateInstance(fullName);
                    AssetDatabase.CreateAsset(obj, assetPath);
                    isAlreadyExists = false;
                }

                Dictionary<string, int> hashStrings = new Dictionary<string, int>();
                SerializedObject so = new SerializedObject(obj);

                SerializedProperty pItems = so.FindProperty("_Items");
                pItems.ClearArray();

                if (isAlreadyExists)
                {
                    SerializedProperty pStrings = so.FindProperty("_HashStrings");
                    if (pStrings != null)
                    {
                        pStrings.ClearArray();
                    }
                }

                for (int i = 0; i < indices.Count; i++)
                {
                    if (EditorUtility.DisplayCancelableProgressBar($"Process {body.ExcelPath}", string.Format("Serializing datas... {0} / {1}", i, indices.Count), i / (float)indices.Count))
                    {
                        EditorUtility.ClearProgressBar();
                        return false;
                    }
                    pItems.InsertArrayElementAtIndex(0);
                    SerializedProperty pItem = pItems.GetArrayElementAtIndex(0);
                    items = table.Rows[indices[i]].ItemArray;
                    int numItems = items.Length;

                    for (int j = 0; j < fieldNames.Count; j++)
                    {
                        SerializedProperty pField = pItem.FindPropertyRelative("_" + CapitalFirstChar(fieldNames[j]));
                        int itemIndex = fieldIndices[j];
                        object item = itemIndex < numItems ? items[itemIndex] : null;
                        string value = item == null ? "" : item.ToString().Trim();
                        if (itemIndex == firstIndex && string.IsNullOrEmpty(value)) continue;
                        switch (fieldTypes[j])
                        {
                            case eFieldTypes.Int:
                                int intValue;
                                if (int.TryParse(value, out intValue))
                                {
                                    pField.intValue = intValue;
                                }
                                else
                                {
                                    pField.intValue = 0;
                                }
                                break;
                            case eFieldTypes.Long:
                                long longValue;
                                if (long.TryParse(value, out longValue))
                                {
                                    pField.longValue = longValue;
                                }
                                else
                                {
                                    pField.longValue = 0;
                                }
                                break;
                            case eFieldTypes.Ints:
                                int[] ints = GetIntsFromString(value);
                                pField.ClearArray();
                                for (int k = ints.Length - 1; k >= 0; k--)
                                {
                                    pField.InsertArrayElementAtIndex(0);
                                    pField.GetArrayElementAtIndex(0).intValue = ints[k];
                                }
                                break;
                            case eFieldTypes.Float:
                                float floatValue;
                                if (float.TryParse(value, out floatValue))
                                {
                                    pField.floatValue = floatValue;
                                }
                                else
                                {
                                    pField.floatValue = 0f;
                                }
                                break;
                            case eFieldTypes.Floats:
                                float[] floats = GetFloatsFromString(value);
                                pField.ClearArray();
                                for (int k = floats.Length - 1; k >= 0; k--)
                                {
                                    pField.InsertArrayElementAtIndex(0);
                                    pField.GetArrayElementAtIndex(0).floatValue = floats[k];
                                }
                                break;
                            case eFieldTypes.Vector2:
                                float[] floatsV2 = GetFloatsFromString(value);
                                pField.vector2Value = floatsV2.Length == 2 ? new Vector2(floatsV2[0], floatsV2[1]) : Vector2.zero;
                                break;
                            case eFieldTypes.Vector3:
                                float[] floatsV3 = GetFloatsFromString(value);
                                pField.vector3Value = floatsV3.Length == 3 ? new Vector3(floatsV3[0], floatsV3[1], floatsV3[2]) : Vector3.zero;
                                break;
                            case eFieldTypes.Vector4:
                                float[] floatsV4 = GetFloatsFromString(value);
                                pField.vector4Value = floatsV4.Length == 4 ? new Vector4(floatsV4[0], floatsV4[1], floatsV4[2], floatsV4[3]) : Vector4.zero;
                                break;
                            case eFieldTypes.Rect:
                                float[] floatsRect = GetFloatsFromString(value);
                                pField.rectValue = floatsRect.Length == 4 ? new Rect(floatsRect[0], floatsRect[1], floatsRect[2], floatsRect[3]) : new Rect();
                                break;
                            case eFieldTypes.Color:
                                Color c = GetColorFromString(value);
                                if (body.CompressColorintoInteger)
                                {
                                    int colorInt = 0;
                                    colorInt |= Mathf.RoundToInt(c.r * 255f) << 24;
                                    colorInt |= Mathf.RoundToInt(c.g * 255f) << 16;
                                    colorInt |= Mathf.RoundToInt(c.b * 255f) << 8;
                                    colorInt |= Mathf.RoundToInt(c.a * 255f);
                                    pField.intValue = colorInt;
                                }
                                else
                                {
                                    pField.colorValue = c;
                                }
                                break;
                            case eFieldTypes.String:
                                if (body.UseHashString)
                                {
                                    int stringIndex;
                                    if (!hashStrings.TryGetValue(value, out stringIndex))
                                    {
                                        stringIndex = hashStrings.Count;
                                        hashStrings.Add(value, stringIndex);
                                    }
                                    pField.intValue = stringIndex;
                                }
                                else
                                {
                                    pField.stringValue = value;
                                }
                                break;
                            case eFieldTypes.Strings:
                                string[] strs = GetStringsFromString(value);
                                pField.ClearArray();
                                if (body.UseHashString)
                                {
                                    for (int k = strs.Length - 1; k >= 0; k--)
                                    {
                                        string str = strs[k];
                                        int stringIndex;
                                        if (!hashStrings.TryGetValue(str, out stringIndex))
                                        {
                                            stringIndex = hashStrings.Count;
                                            hashStrings.Add(str, stringIndex);
                                        }
                                        pField.InsertArrayElementAtIndex(0);
                                        pField.GetArrayElementAtIndex(0).intValue = stringIndex;
                                    }
                                }
                                else
                                {
                                    for (int k = strs.Length - 1; k >= 0; k--)
                                    {
                                        pField.InsertArrayElementAtIndex(0);
                                        pField.GetArrayElementAtIndex(0).stringValue = strs[k];
                                    }
                                }
                                break;
                            case eFieldTypes.Unknown:
                                List<string> enumValues;
                                if (enumDict != null && enumDict.TryGetValue(fieldTypeNames[j], out enumValues))
                                {
                                    pField.enumValueIndex = string.IsNullOrEmpty(value) ? 0 : enumValues.IndexOf(value);
                                }
                                break;
                        }
                    }
                }

                if (body.UseHashString && hashStrings.Count > 0)
                {
                    string[] strings = new string[hashStrings.Count];
                    foreach (KeyValuePair<string, int> kv in hashStrings)
                    {
                        strings[kv.Value] = kv.Key;
                    }
                    SerializedProperty pStrings = so.FindProperty("_HashStrings");
                    pStrings.ClearArray();
                    int total = strings.Length;
                    for (int i = strings.Length - 1; i >= 0; i--)
                    {
                        if (EditorUtility.DisplayCancelableProgressBar($"Process {body.ExcelPath}", string.Format("Writing hash strings ... {0} / {1}",
                            total - i, total), (float)(total - i) / total))
                        {
                            EditorUtility.ClearProgressBar();
                            return false;
                        }
                        pStrings.InsertArrayElementAtIndex(0);
                        SerializedProperty pString = pStrings.GetArrayElementAtIndex(0);
                        pString.stringValue = strings[i];
                    }
                }

                EditorUtility.ClearProgressBar();
                so.ApplyModifiedProperties();
            }

            return true;
        }


        static eFieldTypes GetFieldType(string typename)
        {
            eFieldTypes type = eFieldTypes.Unknown;
            if (!string.IsNullOrEmpty(typename))
            {
                switch (typename.Trim().ToLower())
                {
                    case "int": type = eFieldTypes.Int; break;
                    case "long": type = eFieldTypes.Long; break;
                    case "ints": case "int[]": case "[int]": type = eFieldTypes.Ints; break;
                    case "float": type = eFieldTypes.Float; break;
                    case "floats": case "float[]": case "[float]": type = eFieldTypes.Floats; break;
                    case "vector2": type = eFieldTypes.Vector2; break;
                    case "vector3": type = eFieldTypes.Vector3; break;
                    case "vector4": type = eFieldTypes.Vector4; break;
                    case "rect": type = eFieldTypes.Rect; break;
                    case "color": type = eFieldTypes.Color; break;
                    case "string": type = eFieldTypes.String; break;
                    case "strings": case "string[]": case "[string]": type = eFieldTypes.Strings; break;
                    case "x": type = eFieldTypes.Skip; break;
                }
            }
            return type;
        }


        static string GetFieldTypeName(eFieldTypes type)
        {
            string name = null;
            switch (type)
            {
                case eFieldTypes.Int: name = "int"; break;
                case eFieldTypes.Long: name = "long"; break;
                case eFieldTypes.Ints: name = "int[]"; break;
                case eFieldTypes.Float: name = "float"; break;
                case eFieldTypes.Floats: name = "float[]"; break;
                case eFieldTypes.Vector2: name = "Vector2"; break;
                case eFieldTypes.Vector3: name = "Vector3"; break;
                case eFieldTypes.Vector4: name = "Vector4"; break;
                case eFieldTypes.Rect: name = "Rect"; break;
                case eFieldTypes.Color: name = "Color"; break;
                case eFieldTypes.String: name = "string"; break;
                case eFieldTypes.Strings: name = "string[]"; break;
            }
            return name;
        }



        static string CapitalFirstChar(string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }



        private static int[] GetIntsFromString(string str)
        {
            str = TrimBracket(str);
            if (string.IsNullOrEmpty(str)) { return new int[0]; }
            string[] splits = str.Split(',');
            int[] ints = new int[splits.Length];
            for (int i = 0, imax = splits.Length; i < imax; i++)
            {
                int intValue;
                if (int.TryParse(splits[i].Trim(), out intValue))
                {
                    ints[i] = intValue;
                }
                else
                {
                    ints[i] = 0;
                }
            }
            return ints;
        }


        static string TrimBracket(string str)
        {
            if (str.StartsWith("[") && str.EndsWith("]"))
            {
                return str.Substring(1, str.Length - 2);
            }
            return str;
        }


        private static float[] GetFloatsFromString(string str)
        {
            str = TrimBracket(str);
            if (string.IsNullOrEmpty(str)) { return new float[0]; }
            string[] splits = str.Split(',');
            float[] floats = new float[splits.Length];
            for (int i = 0, imax = splits.Length; i < imax; i++)
            {
                float floatValue;
                if (float.TryParse(splits[i].Trim(), out floatValue))
                {
                    floats[i] = floatValue;
                }
                else
                {
                    floats[i] = 0;
                }
            }
            return floats;
        }


        private static Color GetColorFromString(string str)
        {
            if (string.IsNullOrEmpty(str)) { return Color.clear; }
            uint colorUInt;
            if (GetColorUIntFromString(str, out colorUInt))
            {
                uint r = (colorUInt >> 24) & 0xffu;
                uint g = (colorUInt >> 16) & 0xffu;
                uint b = (colorUInt >> 8) & 0xffu;
                uint a = colorUInt & 0xffu;
                return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            }
            str = TrimBracket(str);
            string[] splits = str.Split(',');
            if (splits.Length == 4)
            {
                int r, g, b, a;
                if (int.TryParse(splits[0].Trim(), out r) && int.TryParse(splits[1].Trim(), out g) &&
                    int.TryParse(splits[2].Trim(), out b) && int.TryParse(splits[3].Trim(), out a))
                {
                    return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
                }
            }
            else if (splits.Length == 3)
            {
                int r, g, b;
                if (int.TryParse(splits[0].Trim(), out r) && int.TryParse(splits[1].Trim(), out g) &&
                    int.TryParse(splits[2].Trim(), out b))
                {
                    return new Color(r / 255f, g / 255f, b / 255f);
                }
            }
            return Color.clear;
        }


        private static bool GetColorUIntFromString(string str, out uint color)
        {
            if (reg_color32.IsMatch(str))
            {
                color = System.Convert.ToUInt32(str, 16);
            }
            else if (reg_color24.IsMatch(str))
            {
                color = (System.Convert.ToUInt32(str, 16) << 8) | 0xffu;
            }
            else
            {
                color = 0u;
                return false;
            }
            return true;
        }


        private static string[] GetStringsFromString(string str)
        {
            str = TrimBracket(str);
            if (string.IsNullOrEmpty(str)) { return new string[0]; }
            return str.Split(',');
        }


    }

}




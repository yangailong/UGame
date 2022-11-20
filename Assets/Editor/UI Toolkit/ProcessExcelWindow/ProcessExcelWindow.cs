using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace UGame_Local_Editor
{
    public class ProcessExcelWindow : EditorWindow
    {

        [MenuItem("Tools/UGame/Excel表数据转ScriptableObject %E")]
        public static void ShowExample()
        {
            ProcessExcelWindow wnd = GetWindow<ProcessExcelWindow>();
            wnd.titleContent = new GUIContent("ProcessExcelWindow");
        }

        public const string SETTINGS_PATH = "ProjectSettings/ExcelToScriptableObjectSettings.asset";


        private Dictionary<ExcelVisualElement, ExcelVisualElementParams> excel = null;

        private ProcessExcelImpl impl = null;

        private IntegerField FieldRow = null, TypeRow = null, DataFromRow = null;

        private ScrollView ScrollView = null;

        private Button AddNewExcel = null, ProcessAll = null;


        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/ProcessExcelWindow/ProcessExcelWindow.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            impl = new ProcessExcelImpl();
            excel = new Dictionary<ExcelVisualElement, ExcelVisualElementParams>();

            FieldRow = root.Q<IntegerField>(nameof(FieldRow));
            TypeRow = root.Q<IntegerField>(nameof(TypeRow));
            DataFromRow = root.Q<IntegerField>(nameof(DataFromRow));
            AddNewExcel = root.Q<Button>(nameof(AddNewExcel));
            ProcessAll = root.Q<Button>(nameof(ProcessAll));
            ScrollView = root.Q<ScrollView>();

            AddNewExcel.clicked += AddNewExcel_clicked;
            ProcessAll.clicked += ProcessAll_clicked;

            OnLoadCache();
        }


        private void OnDisable()
        {
            OnSaveCache();
        }





        /// <summary>
        /// 打开面板读取缓存设置
        /// </summary>
        private void OnLoadCache()
        {
            string json = File.Exists(SETTINGS_PATH) ? File.ReadAllText(SETTINGS_PATH, Encoding.UTF8) : null;

            if (!string.IsNullOrEmpty(json))
            {
                ExcelToScriptableObjectCache cache = JsonUtility.FromJson<ExcelToScriptableObjectCache>(json);

                FieldRow.value = cache.baseRow.FieldRow;
                TypeRow.value = cache.baseRow.TypeRow;
                DataFromRow.value = cache.baseRow.DataFromRow;

                foreach (var item in cache.excels)
                {
                    ExcelVisualElement element = new ExcelVisualElement(ScrollView, InsertBtn_clicked, DeleteBtn_clicked, ProcessBtn_clicked);
                    element.SetData(item);
                    excel.Add(element, element.Params);
                }


            }
        }


        /// <summary>
        /// 关闭面板记录设置到缓存
        /// </summary>
        private void OnSaveCache()
        {
            ExcelToScriptableObjectCache cache = new ExcelToScriptableObjectCache();
            cache.baseRow = new BaseRow();
            cache.baseRow.FieldRow = FieldRow.value;
            cache.baseRow.TypeRow = TypeRow.value;
            cache.baseRow.DataFromRow = DataFromRow.value;

            cache.excels = excel.Values.ToArray();

            File.WriteAllText(SETTINGS_PATH, JsonUtility.ToJson(cache, true), Encoding.UTF8);
        }


        /// <summary>
        /// 添加Excel
        /// </summary>
        private void AddNewExcel_clicked()
        {
            Debug.Log($"添加excel");

            string path = EditorUtility.OpenFilePanel("选择Exce", ".", "xlsx");
            if (!string.IsNullOrEmpty(path))
            {

                string projPath = Application.dataPath;
                projPath = projPath.Substring(0, projPath.Length - 6);
                if (path.StartsWith(path))
                {
                    string name = Path.GetFileNameWithoutExtension(path);
                    string excelPath = path.Substring(projPath.Length, path.Length - projPath.Length);

                    ExcelVisualElement element = new ExcelVisualElement(ScrollView, InsertBtn_clicked, DeleteBtn_clicked, ProcessBtn_clicked);
                    element.SetData(new ExcelVisualElementParams() { ExcelName = name, ExcelPath = path });
                    excel.Add(element, element.Params);
                }


            }
        }


        /// <summary>
        /// 解析全部Excel
        /// </summary>
        private void ProcessAll_clicked()
        {
            Debug.Log($"解析所有excel");
        }


        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="excelVisual"></param>
        private void InsertBtn_clicked(ExcelVisualElement excelVisual)
        {

        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="excelVisual"></param>
        private void DeleteBtn_clicked(ExcelVisualElement excelVisual)
        {
            ScrollView.Remove(excelVisual.self);
            excel.Remove(excelVisual);
            excelVisual = null;
        }


        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="excelVisual"></param>
        private void ProcessBtn_clicked(ExcelVisualElement excelVisual)
        {

            if (!Directory.Exists(excelVisual.Params.ScriptFolder))
            {
                EditorUtility.DisplayDialog("解析失败", $"脚本生成路径不存在:{excelVisual.Params.ScriptFolder}", "OK");
                return;
            }

            if (!Directory.Exists(excelVisual.Params.AssetFolder))
            {
                EditorUtility.DisplayDialog("解析失败", $"数据生成路径不存在:{excelVisual.Params.AssetFolder}", "OK");
                return;
            }

            List<string> processExcels = new List<string>() { excelVisual.Params.ExcelPath };

            EditorPrefs.SetString("processExcels", string.Join("#", processExcels.ToArray()));

            var baseRow = new BaseRow() { TypeRow = TypeRow.value, DataFromRow = DataFromRow.value, FieldRow = FieldRow.value };

            if (impl.Process(baseRow, excelVisual.Params, true))
            {
                mToWriteAssets = true;
                AssetDatabase.Refresh();
            }
        }



        bool mToWriteAssets = false;
        void Update()
        {

            if (mToWriteAssets && !EditorApplication.isCompiling)
            {
                mToWriteAssets = false;
                string[] names = EditorPrefs.GetString("processExcels", "").Split('#');
                EditorPrefs.DeleteKey("processExcels");
                foreach (string name in names)
                {
                    if (string.IsNullOrEmpty(name)) continue;
                    var baseRow = new BaseRow() { TypeRow = TypeRow.value, DataFromRow = DataFromRow.value, FieldRow = FieldRow.value };
                    foreach (var item in excel)
                    {
                        if (item.Key.Params.ExcelPath.Equals(name))
                        {
                            impl.Process(baseRow, item.Value, false); break;
                        }
                    }
                }


            }
        }




        public class ExcelVisualElement
        {
            public VisualElement parent = null;
            public VisualElement self = null;

            public TextField NameSpace = null;
            private Label ExcelName = null;

            public Toggle UseHashString = null, PublicItemsGetter = null, HideaAssetProperties = null, CompressColorintoInteger = null, GenerateGetMethodIfPossoble = null, TreaUnknowTypesasEnum = null, IDorKeytoMuitiValues = null, GengrateToStringMethod = null;

            public Button ScriptFolder = null, AssetFolder = null, InsertBtn = null, DeleteBtn = null, ProcessBtn = null;

            public ExcelVisualElement(VisualElement parent, Action<ExcelVisualElement> insertCallback, Action<ExcelVisualElement> deleteCallback, Action<ExcelVisualElement> processCallback)
            {
                this.parent = parent;

                var item = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Toolkit/ProcessExcelWindow/ExcelItem.uxml");
                VisualElement cell = item.Instantiate();

                parent.Add(cell);

                this.self = cell;
                this.NameSpace = self.Q<TextField>(nameof(NameSpace));
                this.ScriptFolder = self.Q<Button>(nameof(ScriptFolder));
                this.AssetFolder = self.Q<Button>(nameof(AssetFolder));
                this.UseHashString = self.Q<Toggle>(nameof(UseHashString));
                this.PublicItemsGetter = self.Q<Toggle>(nameof(PublicItemsGetter));
                this.HideaAssetProperties = self.Q<Toggle>(nameof(HideaAssetProperties));
                this.CompressColorintoInteger = self.Q<Toggle>(nameof(CompressColorintoInteger));
                this.GenerateGetMethodIfPossoble = self.Q<Toggle>(nameof(GenerateGetMethodIfPossoble));
                this.TreaUnknowTypesasEnum = self.Q<Toggle>(nameof(TreaUnknowTypesasEnum));
                this.IDorKeytoMuitiValues = self.Q<Toggle>(nameof(IDorKeytoMuitiValues));
                this.GengrateToStringMethod = self.Q<Toggle>(nameof(GengrateToStringMethod));
                this.InsertBtn = self.Q<Button>(nameof(InsertBtn));
                this.DeleteBtn = self.Q<Button>(nameof(DeleteBtn));
                this.ProcessBtn = self.Q<Button>(nameof(ProcessBtn));
                this.ExcelName = self.Q<Label>(nameof(ExcelName));


                UseHashString.RegisterCallback<ChangeEvent<bool>>(UseHashStringRegisterCallback);
                PublicItemsGetter.RegisterCallback<ChangeEvent<bool>>(PublicItemsGetterRegisterCallback);
                HideaAssetProperties.RegisterCallback<ChangeEvent<bool>>(HideaAssetPropertiesRegisterCallback);
                CompressColorintoInteger.RegisterCallback<ChangeEvent<bool>>(CompressColorintoIntegerRegisterCallback);
                GenerateGetMethodIfPossoble.RegisterCallback<ChangeEvent<bool>>(GenerateGetMethodIfPossobleRegisterCallback);
                TreaUnknowTypesasEnum.RegisterCallback<ChangeEvent<bool>>(TreaUnknowTypesasEnumRegisterCallback);
                IDorKeytoMuitiValues.RegisterCallback<ChangeEvent<bool>>(IDorKeytoMuitiValuesRegisterCallback);
                GengrateToStringMethod.RegisterCallback<ChangeEvent<bool>>(GengrateToStringMethodRegisterCallback);


                ScriptFolder.clicked += ScriptDirectory_clicked;
                AssetFolder.clicked += AssetDirectory_clicked;

                InsertBtn.clicked += () => { insertCallback?.Invoke(this); };
                DeleteBtn.clicked += () => { deleteCallback?.Invoke(this); };
                ProcessBtn.clicked += () => { processCallback?.Invoke(this); };
            }




            /// <summary>选择脚本生成路径</summary>
            private void ScriptDirectory_clicked()
            {
                if (Params == null) return;

                string tmp = $"{Application.dataPath}/../HotFixAssembly/Scripts/Game/Data/Normal/ScriptableObject";
                string path = EditorUtility.OpenFolderPanel("脚本生成路径", Application.dataPath, string.Empty);
                if (!string.IsNullOrEmpty(path))
                {
                    Params.ScriptFolder = ScriptFolder.text = path;
                }
            }


            /// <summary>选择数据生成路径</summary>
            private void AssetDirectory_clicked()
            {
                if (Params == null) return;
                string path = EditorUtility.OpenFolderPanel("解析数据生成路径", $"{Application.dataPath}", string.Empty);
                if (!string.IsNullOrEmpty(path))
                {
                    Params.AssetFolder = AssetFolder.text = path;
                }
            }




            private void UseHashStringRegisterCallback(ChangeEvent<bool> value)
            {
                if (Params == null) return;
                Params.UseHashString = value.newValue;
            }


            private void PublicItemsGetterRegisterCallback(ChangeEvent<bool> value)
            {
                if (Params == null) return;
                Params.PublicItemsGetter = value.newValue;
            }


            private void HideaAssetPropertiesRegisterCallback(ChangeEvent<bool> value)
            {
                if (Params == null) return;
                Params.HideaAssetProperties = value.newValue;
            }


            private void CompressColorintoIntegerRegisterCallback(ChangeEvent<bool> value)
            {
                if (Params == null) return;
                Params.CompressColorintoInteger = value.newValue;
            }


            private void GenerateGetMethodIfPossobleRegisterCallback(ChangeEvent<bool> value)
            {
                if (Params == null) return;
                Params.GenerateGetMethodIfPossoble = value.newValue;
            }


            private void TreaUnknowTypesasEnumRegisterCallback(ChangeEvent<bool> value)
            {
                if (Params == null) return;
                Params.TreaUnknowTypesasEnum = value.newValue;
            }


            private void IDorKeytoMuitiValuesRegisterCallback(ChangeEvent<bool> value)
            {
                if (Params == null) return;
                Params.IDorKeytoMultiValues = value.newValue;
            }


            private void GengrateToStringMethodRegisterCallback(ChangeEvent<bool> value)
            {
                if (Params == null) return;
                Params.GengrateToStringMethod = value.newValue;
            }



            public ExcelVisualElementParams Params = null;

            public void SetData(ExcelVisualElementParams elementParams)
            {
                this.Params = elementParams;
                this.NameSpace.value = elementParams.NameSpace;
                this.ScriptFolder.text = elementParams.ScriptFolder;
                this.AssetFolder.text = elementParams.AssetFolder;
                this.UseHashString.value = elementParams.UseHashString;
                this.PublicItemsGetter.value = elementParams.PublicItemsGetter;
                this.HideaAssetProperties.value = elementParams.HideaAssetProperties;
                this.CompressColorintoInteger.value = elementParams.CompressColorintoInteger;
                this.GenerateGetMethodIfPossoble.value = elementParams.GenerateGetMethodIfPossoble;
                this.TreaUnknowTypesasEnum.value = elementParams.TreaUnknowTypesasEnum;
                this.IDorKeytoMuitiValues.value = elementParams.IDorKeytoMultiValues;
                this.GengrateToStringMethod.value = elementParams.GengrateToStringMethod;
                this.ExcelName.text = elementParams.ExcelName;
            }

        }


        [Serializable]
        public class ExcelToScriptableObjectCache
        {
            public BaseRow baseRow = null;

            public ExcelVisualElementParams[] excels = null;
        }




        [Serializable]

        public class BaseRow
        {
            public int FieldRow = 0;
            public int TypeRow = 1;
            public int DataFromRow = 2;
        }


        [Serializable]
        public class ExcelVisualElementParams
        {
            public string ExcelName = string.Empty, ExcelPath = string.Empty, NameSpace = "UGame.Remove", ScriptFolder = "Select", AssetFolder = "Select";


            public bool UseHashString = false;


            /// <summary>公开Item属性</summary>
            public bool PublicItemsGetter = false;


            /// <summary>inspector不显示属性变量</summary>
            public bool HideaAssetProperties = false;


            /// <summary>将颜色压缩到整数</summary>
            public bool CompressColorintoInteger = false;


            /// <summary>生成属性获取方法</summary>
            public bool GenerateGetMethodIfPossoble = true;


            /// <summary>字段设置为枚举</summary>
            public bool TreaUnknowTypesasEnum = false;


            /// <summary>id或者key多值</summary>
            public bool IDorKeytoMultiValues = false;


            /// <summary>重写ToString方法</summary>
            public bool GengrateToStringMethod = true;

        }




    }
}
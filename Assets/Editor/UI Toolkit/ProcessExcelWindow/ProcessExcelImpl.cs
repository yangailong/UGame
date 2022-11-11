using Excel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static UGame_Local_Editor.ProcessExcelWindow;

namespace UGame_Local_Editor
{
    public class ProcessExcelImpl
    {

        public bool Process(ExcelVisualElementParams excel, bool generateCode)
        {
            string className = Path.GetFileNameWithoutExtension(excel.ExcelPath);


            //检查文件名是否合法
            if (!Regex.IsMatch(className, @"^[A-Z][A-Za-z0-9_]*$"))
            {
                EditorUtility.DisplayDialog("Excel To ScriptableObject", "excel 文件名不合法，因为excel的名称是类名", "OK");
                return false;
            }


            Stream stream = null;
            int lastDot = excel.ExcelPath.LastIndexOf('.');
            string tempExcelName = $"{excel.ExcelPath.Substring(0, lastDot)}_temp{excel.ExcelPath.Substring(lastDot, excel.ExcelPath.Length - lastDot)}";


            try//检查文件是否被Excel应用占用
            {
                File.Copy(excel.ExcelPath, tempExcelName);
                stream = File.OpenRead(tempExcelName);
            }
            catch
            {
                EditorUtility.DisplayDialog("Excel To ScriptableObject", $"{className}打开中，请先关闭此Excel应用程序", "OK");
                File.Delete(tempExcelName);
                return false;
            }


            //读取Excel表数据
            IExcelDataReader reader = excel.ExcelPath.ToLower().EndsWith(".xls") ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet data = reader.AsDataSet();
            reader.Dispose();
            stream.Close();
            File.Delete(tempExcelName);


            //检查数据是否读取成功
            if (data == null)
            {
                EditorUtility.DisplayDialog("Excel To ScriptableObject", $"{className}读取失败，请检查", "OK");
                return false;
            }


            return true;
        }



    }


}

using System.IO;
using UnityEditor;
using UnityEngine;
namespace Excel2Charp
{
    public class CopyExcel2Project
    {
        public static void CopyExcelToProject()
        {
            string originPath = Path.Combine(Application.dataPath, "../../YG_Common/ExcelConfig/");
            string writePath = Path.Combine(Application.dataPath, "../Excels/");
            Directory.Delete(writePath, true);
            Directory.CreateDirectory(writePath);
            string[] fileStrs = Directory.GetFiles(originPath, "*.xlsx");
            for (int i = 0; i < fileStrs.Length; i++)
            {
                FileInfo fi = new FileInfo(fileStrs[i]);
                fi.CopyTo(writePath + fi.Name);
            }
        }

    }
}
using System.IO;
using UnityEngine;

public class DownDll
{

    public static byte[] PDBData(string pdbPath)
    {
        return FileToByte($"{Application.dataPath}/{pdbPath}");
    }


    public static byte[] DllData(string dllPath)
    {
        return FileToByte($"{Application.dataPath}/{dllPath}");
    }


    private static byte[] FileToByte(string path)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] byteArr = new byte[fs.Length];
                fs.Read(byteArr, 0, byteArr.Length);
                return byteArr;
            }
        }
        catch
        {
            return null;
        }

    }

}

using System.IO;
using UnityEngine;

public class DownDll
{
    public const string DllPath = "AddressableAssets/Remote_UnMapping/Dll/HotFixAssembly.dll";

    public const string PDBPath = "AddressableAssets/Remote_UnMapping/Dll/HotFixAssembly.pdb";

    public static byte[] PDBData()
    {
        return FileToByte($"{Application.dataPath}/{PDBPath}");
    }


    public static byte[] DllData()
    {
        return FileToByte($"{Application.dataPath}/{DllPath}");
    }


    public static byte[] FileToByte(string path)
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

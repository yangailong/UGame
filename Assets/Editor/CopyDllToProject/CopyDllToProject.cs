using UnityEngine;
using UnityEditor;
using System.IO;
/// <summary> 说明</summary>
public class CopyDllToProject : Editor
{
    [MenuItem("Tools/Dll/CopyDllToProject")]
    public static void Copy()
    {
        string originPath = $"{Application.dataPath}/../HotFixAssembly/bin/Debug/net6.0";
        string writePath = $"{Application.dataPath}/AddressableAssets/Remote/Dll";

        if (!Directory.Exists(originPath) || !Directory.Exists(writePath))
        {
            Debug.LogError($"no find path   {originPath} ---------  {writePath}");
            return;
        }

        Directory.Delete(writePath, true);
        Directory.CreateDirectory(writePath);

        var dll = new FileInfo($"{originPath}/HotFixAssembly.dll");
        var pdb = new FileInfo($"{originPath}/HotFixAssembly.pdb");

        dll.CopyTo($"{writePath}{dll.Name}");
        pdb.CopyTo($"{writePath}{pdb.Name}");

        dll.Refresh();
        pdb.Refresh();

    }


}


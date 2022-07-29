using UnityEngine;
using UnityEditor;
using System.IO;
/// <summary> 说明</summary>
public class CopyDllToProject : Editor
{
    [MenuItem("Tools/Dll/CopyDllToProject")]
    public static void Copy()
    {
        string dllFullName = "HotFixAssembly.dll";
        string pdbFullName = "HotFixAssembly.pdb";

        string originPath = $"{Application.dataPath}/../HotFixAssembly/bin/Debug/net6.0";
        string writePath = $"{Application.dataPath}/AddressableAssets/Remote/Dll/";

        if (!Directory.Exists(originPath))
        {
            Debug.LogError($"no find path   {originPath}");
            return;
        }

        var dll = new FileInfo($"{originPath}/{dllFullName}");
        var pdb = new FileInfo($"{originPath}/{pdbFullName}");

        dll.CopyTo($"{writePath}{dll.Name}", true);//覆盖
        pdb.CopyTo($"{writePath}{pdb.Name}", true);

        dll.Refresh();
        pdb.Refresh();

        AssetDatabase.Refresh();
    }


}


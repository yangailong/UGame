using UnityEngine;
using UnityEditor;
using System.IO;

namespace UGame_Local_Editor
{
    /// <summary> 说明</summary>
    public class DllToBytes : Editor
    {
        [MenuItem("Tools/Dll/DLLToBytes")]
        public static void DLLToBytes()
        {
            string dllFullName = "HotFixAssembly.dll";
            string pdbFullName = "HotFixAssembly.pdb";

            string originPath = $"{Application.dataPath}/AddressableAssets/Remote_UnMapper/Dll/Dll~";

            string writePath = $"{Application.dataPath}/AddressableAssets/Remote_UnMapper/Dll/";

            if (!Directory.Exists(originPath))
            {
                Debug.LogError($"no find path   {originPath}");
                return;
            }

            var dll = new FileInfo($"{originPath}/{dllFullName}");
            var pdb = new FileInfo($"{originPath}/{pdbFullName}");


            dll.Refresh();
            pdb.Refresh();

            var dllBytes = File.ReadAllBytes(dll.FullName);
            var pdbBytes = File.ReadAllBytes(pdb.FullName);


            foreach (string p in Directory.GetFileSystemEntries(writePath))
            {
                if (File.Exists(p))
                {
                    // 删除文件
                    File.Delete(p);
                }
            }


            //TODO...加密dll
            File.WriteAllBytes($"{writePath}{dllFullName}.bytes", dllBytes);

            File.WriteAllBytes($"{writePath}{pdbFullName}.bytes", pdbBytes);

            AssetDatabase.Refresh();
        }








    }
}


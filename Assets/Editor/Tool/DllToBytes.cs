using UnityEngine;
using UnityEditor;
using System.IO;
using UGame_Local;

namespace UGame_Local_Editor
{
    /// <summary>生成加密dll文件,文件后缀名追加.bytes</summary>
    public class DllToBytes : Editor
    {
        private static readonly string originPath = $"{Application.dataPath}/AddressableAssets/Remote_UnMapper/Dll/Dll~";

        private static readonly string writePath = $"{Application.dataPath}/AddressableAssets/Remote_UnMapper/Dll";


        private static readonly string dllFullName = "HotFixAssembly.dll";

        private static readonly string pdbFullName = "HotFixAssembly.pdb";


        [MenuItem("Tools/Dll/DLLToBytes")]
        public static void DLLToBytes()
        {
            if (!Directory.Exists(originPath))
            {
                throw new DirectoryNotFoundException($"the {originPath} path does not exist");
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
                    File.Delete(p);
                }
            }


            var uGame = AssetDatabase.LoadAssetAtPath<CfgUGame>($"Assets/UGame.asset");

            //加密dll
            var encrypt = CryptoManager.AesEncrypt(uGame.md5Key, dllBytes);

            File.WriteAllBytes($"{writePath}/{dllFullName}.bytes", encrypt);

            File.WriteAllBytes($"{writePath}/{pdbFullName}.bytes", pdbBytes);

            AssetDatabase.Refresh();
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]

        public static void RuntimeInitDllToBytes()
        {
            var dllCreationTim = File.GetLastWriteTime($"{originPath}/{dllFullName}");
            var bytesCreationTim = File.GetLastWriteTime($"{writePath}/{dllFullName}.bytes");

            //unity运行，检测最新的dll文件是否转成dll.bytes
            if (dllCreationTim > bytesCreationTim)
            {
                DLLToBytes();
            }
        }

    }
}


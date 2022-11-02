using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
namespace UGame_Local
{
    /// <summary> 说明</summary>
    public class AppMain : MonoBehaviour
    {
        //加载Dll

        public const string DllPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.dll.bytes";

        public const string PDBPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.pdb.bytes";

        public static AppDomain appDomain = null;

        public static HotFixAssembly hotFixAssembly = null;


        void Start()
        {
            appDomain = new AppDomain();

            hotFixAssembly = new HotFixAssembly(appDomain);

            var dll = Addressables.LoadAssetAsync<TextAsset>(DllPath).WaitForCompletion();
            var pdb = Addressables.LoadAssetAsync<TextAsset>(PDBPath).WaitForCompletion();

            hotFixAssembly.LoadAssembly(dll.bytes, pdb.bytes);


            hotFixAssembly.InitializeILRuntime();

            hotFixAssembly.CallRemoveRunGame("UGame_Remove.RunGame", "StartUp", null, null);

        }








    }

}


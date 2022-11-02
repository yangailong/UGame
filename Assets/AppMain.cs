using UnityEngine;
using UnityEngine.AddressableAssets;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
namespace UGame_Local
{
    /// <summary> 说明</summary>
    public class AppMain : MonoBehaviour
    {
        //加载Dll
        
        public const string DllPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.dlls";

        public const string PDBPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.pdb";


        public static AppDomain appDomain = null;

        public static HotFixAssembly hotFixAssembly = null;


        private void Awake()
        {

            appDomain = new AppDomain();

            hotFixAssembly = new HotFixAssembly(appDomain);

            byte[] dllData = Addressables.LoadAssetAsync<TextAsset>(DllPath).WaitForCompletion().bytes;

            byte[] pdbData = Addressables.LoadAssetAsync<TextAsset>(PDBPath).WaitForCompletion().bytes;


            hotFixAssembly.LoadAssembly(dllData, pdbData);


            hotFixAssembly.InitializeILRuntime();

            hotFixAssembly.CallRemoveRunGame("UGame_Remove.RunGame", "StartUp", null, null);

        }


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }




    }

}


using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
namespace UGame_Local
{
    /// <summary> 说明</summary>
    public class AppMain : MonoBehaviour
    {
        //加载Dll

        public const string DllPath = "AddressableAssets/Remote_UnMapper/Dll/Dll~/HotFixAssembly.dll";

        public const string PDBPath = "AddressableAssets/Remote_UnMapper/Dll/Dll~/HotFixAssembly.pdb";


        public  static AppDomain appDomain = null;

        public static HotFixAssembly hotFixAssembly = null;


        private void Awake()
        {

            appDomain = new AppDomain();

            hotFixAssembly = new HotFixAssembly(appDomain);


            byte[] dllData = DownDll.DllData(DllPath);

            byte[] pdbData = DownDll.PDBData(PDBPath);

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


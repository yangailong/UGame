using UnityEngine;
using UnityEngine.AddressableAssets;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local
{
    /// <summary> 说明</summary>
    public class UGame : MonoBehaviour
    {

        public string m_Key = "UGame.Prefs.Keys";

        public ILRuntimeJITFlags jITFlags = ILRuntimeJITFlags.None;

        public bool usePdb = true;


        public const string DllPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.dll.bytes";

        public const string PDBPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.pdb.bytes";

        public static AppDomain appDomain = null;

        public static HotFixAssembly hotFixAssembly = null;


        void Start()
        {
            appDomain = new AppDomain((int)jITFlags);

            hotFixAssembly = new HotFixAssembly(appDomain);

            var dll = Addressables.LoadAssetAsync<TextAsset>(DllPath).WaitForCompletion();

            var pdb = !usePdb ? null : Addressables.LoadAssetAsync<TextAsset>(PDBPath).WaitForCompletion();

            hotFixAssembly.LoadAssembly(dll.bytes, pdb.bytes);

            hotFixAssembly.InitializeILRuntime();

            hotFixAssembly.CallRemoveRunGame("UGame_Remove.RunGame", "StartUp", null, null);

        }
      
    }



   
}

public enum ILRuntimeJITFlags
{
    None = 0,

    JITOnDemand = 1,

    JITImmediately = 2,

    NoJIT = 4,

    ForceInline = 8,
}

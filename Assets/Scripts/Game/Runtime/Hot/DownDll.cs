using UnityEngine;
using UnityEngine.AddressableAssets;

public class DownDll
{

    public static byte[] PDBData(string pdbPath)
    {
        return Addressables.LoadAssetAsync<TextAsset>(pdbPath).WaitForCompletion().bytes;
    }


    public static byte[] DllData(string dllPath)
    {
        return Addressables.LoadAssetAsync<TextAsset>(dllPath).WaitForCompletion().bytes;
    }





}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

namespace UGame_Remove
{
    public class RunGame
    {
        public static void StartUp()
        {
            Debug.Log($"UGame_Remove StartUp");

            EnterPrepare();
        }


        private static void EnterPrepare()
        {
            //ResourceManager.LoadAssetMappingAsync<TextAsset>($"{AssetsMapping.needListenerAssetsRootPath}", null);
            //Addressables.LoadAssetAsync<GameObject>("");


        }


    }



}
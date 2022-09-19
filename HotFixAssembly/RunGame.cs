using System;
using System.Collections;
using UGame_Local;
using UnityEngine;
using UnityEngine.UI;

namespace UGame_Remove
{
    public class RunGame
    {

        public static void StartUp()
        {
            Debug.Log($"UGame_Remove StartUp");

            MonoBehaviourRuntime.Instance.StartCoroutine(Init());
        }


        private static IEnumerator Init()
        {
            string path = $"Assets/{AssetsMappingConst.needListenerAssetsRootPath}/{AssetsMappingConst.creatPath}";

            ResourceManager.LoadAssetAsync<TextAsset>(path, o =>
            {
                AssetsMapping.Initialize(o);
                UIManager.Init();
                CfgData.Init();
            });

            yield return null;






        }

    }
}

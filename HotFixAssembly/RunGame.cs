using System;
using UGame_Local;
using UnityEngine;

namespace UGame_Remove
{
    public class RunGame
    {

        public static void StartUp()
        {
            Debug.Log($"UGame_Remove StartUp");

            RunTime();
        }


        private static void RunTime()
        {
            InitMapping(EnterPrepare);
        }


        private static void InitMapping(Action enterPrepare)
        {
            ResourceManager.LoadAssetAsync<TextAsset>($"Assets/{AssetsMappingConst.needListenerAssetsRootPath}/{AssetsMappingConst.creatPath}", text =>
            {
                if (text == null || string.IsNullOrEmpty(text.text))
                {
                    Debug.LogError($"资源映射表下载失败...无法进入游戏");
                    return;
                }

                AssetsMapping.Initialize(text);

                enterPrepare?.Invoke();
            });
        }


        private static void EnterPrepare()
        {
            //TOOD....预加载资源

            ResourceManager.LoadSceneAsync("Login").Completed += p =>
            {
                UIManager.Init();
                CfgData.Init();
            };
        }


    }
}

using System.Collections;
using UGame_Local;
using UnityEngine;

namespace UGame_Remove
{
    public class RunGame
    {

        public static void StartUp()
        {
            Debug.Log($"UGame_Remove StartUp");

            CoroutineRunner.OverStartCoroutine(Init());
        }



        private static IEnumerator Init()
        {
            string path = $"Assets/{AssetsMapperConst.needListenerAssetsRootPath}/{AssetsMapperConst.creatPath}";

            //加载映射表txt
            ResourceManager.LoadAssetAsync<TextAsset>(path, o =>
            {
                AssetsMapper.Init(o);
                GlobalEvent.Init();
                AudioPlayManager.Init();

                //红点系统
                //网络系统

                UIManager.AsyncInit();
                CfgData.AsyncInit();
            });


            //CfgData和UIManager异步初始化完成
            while (!CfgData.AsyncInitComplete && !UIManager.AsyncInitComplete)
            {
                yield return new WaitForEndOfFrame();
            }

            //播放背景音
            ResourceManager.LoadAssetAsync<AudioClip>("Cyberworld", o =>
            {
                AudioPlayManager.PlayMusic2D(o, 0);
            });


        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
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

            //TODO.....
            //代码延迟执行功能

            //红点系统
        }



        private static IEnumerator Init()
        {
            string path = $"Assets/{AssetsMapperConst.needListenerAssetsRootPath}/{AssetsMapperConst.creatPath}";

            ResourceManager.LoadAssetAsync<TextAsset>(path, o =>
            {
                AssetsMapper.Init(o);
                GlobalEvent.Init();
                UIManager.Init();
                CfgData.Init();
                AudioPlayManager.Init();
            });


            yield return new WaitForSeconds(2f);


            ResourceManager.LoadAssetAsync<AudioClip>("Cyberworld", o =>
            {
                AudioPlayManager.PlayMusic2D(o, 0);
            });



        }

    }
}

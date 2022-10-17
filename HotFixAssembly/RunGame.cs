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


            Test.Test test = new Test.Test();


        }

    }
}

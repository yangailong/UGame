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

            TextAsset result = null;

            //加载映射表txt
            ResourceManager.LoadAssetAsync<TextAsset>(path, value => result = value);

            //等待映射表加载结束
            while (result == null)
            {
                yield return new WaitForEndOfFrame();
            }


            //初始化子系统
            AssetsMapper.Init(result);
            GlobalEvent.Init();
            AudioPlayManager.Init();

            UIManager.AsyncInit();
            CfgData.AsyncInit();

           // NetWebSocket.Instance.Open("", "", WebSocket4Net.WebSocketVersion.Rfc6455);


            // 等待子系统异步初始化完成
            while (!CfgData.AsyncInitComplete && !UIManager.AsyncInitComplete)
            {
                yield return new WaitForEndOfFrame();
            }

            UIManager.Open<DemoPanel>();

            Debug.Log($"子系统全部初始化完毕....");


        }


    }
}

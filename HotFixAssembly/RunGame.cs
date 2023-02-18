using System;
using UGame_Local;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace UGame_Remove
{
    public class RunGame
    {

        public static void StartUp()
        {
            Debug.Log($"UGame_Remove StartUp");
            InitAsync();
        }


        private static async void InitAsync()
        {
            string path = $"Assets/{AssetsMapperConst.needListenerAssetsRootPath}/{AssetsMapperConst.fullName}";
            AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(path);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new ArgumentNullException($"资源映射表加载失败：{handle.OperationException.Message}");
            }


            //初始化子系统
            AssetsMapper.Init(handle.Result);
            GlobalEvent.Init();
            AudioPlayManager.Init();
            ObjectPoolManager.Init();


            UIManager.InitAsync();
            CfgData.InitAsync();

            //网络连接
            //NetWebSocket.Open("ws://127.0.0.1:8088/ws", "", WebSocket4Net.WebSocketVersion.Rfc6455);


            // 等待子系统异步初始化完成
            //while (!CfgData.InitAsyncComplete || !UIManager.InitAsyncComplete)
            //{
            //    yield return new WaitForEndOfFrame();
            //}

            Debug.Log($"子系统全部初始化完毕....");


            NetWebSocket.Opened += (p1, p2) => { NetProxy.Instance.Register(); };
            NetWebSocket.Closed += (p1, p2) => { NetProxy.Instance.Unregister(); };

            AsyncOperationHandle<SceneInstance> operationHandle = Addressables.LoadSceneAsync(AssetsMapper.LoadPath("Login"));
            await operationHandle.Task;


            //打开Demo面板
            UIManager.Open<DemoPanel>();
        }

    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local
{
    /// <summary>UGame全局启动脚本,负责下载资源，启动热更程序</summary>

    [DisallowMultipleComponent]
    public class UGame : MonoBehaviour
    {
        /// <summary>UGame单例</summary>
        public static UGame Instance = null;


        /// <summary> 全局UGame配置</summary>
        [Tooltip("全局UGame配置"), FormerlySerializedAs("cfgUGame")]
        public CfgUGame cfgUGame = null;


        /// <summary>热更域</summary>
        public static AppDomain appDomain = null;


        /// <summary>加载热更程序</summary>
        public static HotFixAssembly hotFixAssembly = null;


        /// <summary>资源下载是否结束了</summary>
        private bool isDownLoadEnd = false;

        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            //开始下载资源
            StopAllCoroutines();
            StartCoroutine(DownLoadAssets());
        }



        /// <summary>
        /// 下载资源
        /// </summary>
        /// <returns></returns>
        IEnumerator DownLoadAssets()
        {
            //此处等一帧后执行
            yield return new WaitForEndOfFrame();

            //开始下载资源
            StartCoroutine(AssetsDownLoad.StartDownAsync(DownloadEnd));


            var samplePanel = GameObject.FindObjectOfType<SamplePanel>();

            samplePanel?.SetData("检查更新......", 0);

            //更新ui下载进度
            while (isDownLoadEnd)
            {
                if (AssetsDownLoad.DownLoadSize != 0)
                {
                    string text = "当前下载进度";
                    float currDownLoad = AssetsDownLoad.DownLoadPercent * (AssetsDownLoad.DownLoadSize / (1024 * 1024));//已下载size
                    text = $"{text}  {currDownLoad.ToString("0.00")} / {(AssetsDownLoad.DownLoadSize / (1024 * 1024)).ToString("0.00")} MB";//已下载size/总下载size
                    text = $"{text}  {Mathf.CeilToInt(AssetsDownLoad.DownLoadPercent * 100)}%";//下载百分比

                    samplePanel?.SetData(text, AssetsDownLoad.DownLoadPercent);
                }

                yield return new WaitForEndOfFrame();
            }

        }



        /// <summary>
        /// 下载结束
        /// </summary>
        /// <param name="success">是否下载成功</param>
        /// 下载成功,启动热更域
        /// 下载失败，结束失败说明
        private void DownloadEnd(bool success)
        {
            isDownLoadEnd = true;

            var samplePanel = GameObject.FindObjectOfType<SamplePanel>();

            samplePanel?.SetSliderValue(success ? 1f : 0);

            if (!success)//下载失败
            {
                FindObjectOfType<TipPanel>(true)?.Open("资源下载失败,请重新尝试!", onClickOk =>
                {
                    if (onClickOk)
                    {
                        isDownLoadEnd = false;

                        samplePanel?.SetData("检查更新......", 0);
                        StopAllCoroutines();
                        StartCoroutine(DownLoadAssets());
                    }
                    else
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }
                });
            }
            else
            {
                RunHotDll();
            }
        }


        /// <summary>
        /// 加载dll，启动热更程序
        /// </summary>
        private void RunHotDll()
        {
            //首先实例化ILRuntime的AppDomain，AppDomain是一个应用程序域，每个AppDomain都是一个独立的沙盒
            appDomain = new AppDomain((int)cfgUGame.jITFlags);

            hotFixAssembly = new HotFixAssembly(appDomain);

            string dllPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.dll.bytes";
            string pdbPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.pdb.bytes";

            var dll = Addressables.LoadAssetAsync<TextAsset>(dllPath).WaitForCompletion();

            var pdb = !cfgUGame.usePdb ? null : Addressables.LoadAssetAsync<TextAsset>(pdbPath).WaitForCompletion();

            //解密dll
            var dllByte = CryptoManager.AesDecrypt(cfgUGame.key, dll.bytes);

            hotFixAssembly.LoadAssembly(dllByte, pdb?.bytes);

            hotFixAssembly.InitializeILRuntime();


            hotFixAssembly.CallRemoveRunGame("UGame_Remove.RunGame", "StartUp", null, null);
        }


    }




}



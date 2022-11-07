using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local
{
    /// <summary> 说明</summary>
    public class UGame : MonoBehaviour
    {
        public CfgUGame CfgUGame = null;

        public const string DllPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.dll.bytes";

        public const string PDBPath = "Assets/AddressableAssets/Remote_UnMapper/Dll/HotFixAssembly.pdb.bytes";

        public static AppDomain appDomain = null;

        public static HotFixAssembly hotFixAssembly = null;


        private void Awake()
        {
            StopAllCoroutines();
            StartCoroutine(DownLoadAssets());
        }


        /// <summary>
        /// 下载资源
        /// </summary>
        /// <returns></returns>
        IEnumerator DownLoadAssets()
        {
            yield return new WaitForEndOfFrame();

            StartCoroutine(AssetsDownLoad.StartDownAsync(DownloadEnd));

            var startUpPanel = GameObject.FindObjectOfType<StartUpPanel>();

            startUpPanel?.SetData("检查更新......", 0);

            while (isDownLoadEnd)
            {
                if (AssetsDownLoad.DownLoadSize != 0)
                {
                    string text = "当前下载进度";
                    float currDownLoad = AssetsDownLoad.DownLoadPercent * (AssetsDownLoad.DownLoadSize / (1024 * 1024));//已下载size
                    text = $"{text}  {currDownLoad.ToString("0.00")} / {(AssetsDownLoad.DownLoadSize / (1024 * 1024)).ToString("0.00")} MB";//已下载size/总下载size
                    text = $"{text}  {Mathf.CeilToInt(AssetsDownLoad.DownLoadPercent * 100)}%";//下载百分比

                    startUpPanel?.SetData(text, AssetsDownLoad.DownLoadPercent);
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private bool isDownLoadEnd = false;

        private void DownloadEnd(bool success)
        {
            isDownLoadEnd = true;

            if (!success)//下载失败
            {
                FindObjectOfType<TipPanel>(true)?.Open("资源下载失败,请重新尝试!", onClickOk =>
                {
                    if (onClickOk)
                    {
                        isDownLoadEnd = false;

                        GameObject.FindObjectOfType<StartUpPanel>().SetData("检查更新......", 0);
                        StopAllCoroutines();
                        StartCoroutine(DownLoadAssets());

                        //注意： 如果多次下载失败，可以尝试清空缓存AssetsDownLoad.CleanBundleCache()
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
        /// 下载dll，启动热更程序
        /// </summary>
        private void RunHotDll()
        {
            appDomain = new AppDomain((int)CfgUGame.jITFlags);

            hotFixAssembly = new HotFixAssembly(appDomain);

            var dll = Addressables.LoadAssetAsync<TextAsset>(DllPath).WaitForCompletion();

            var pdb = !CfgUGame.usePdb ? null : Addressables.LoadAssetAsync<TextAsset>(PDBPath).WaitForCompletion();

            //解密
            var dllByte = CryptoManager.AesDecrypt(CfgUGame.m_Key, dll.bytes);

            hotFixAssembly.LoadAssembly(dllByte, pdb?.bytes);

            hotFixAssembly.InitializeILRuntime();

            hotFixAssembly.CallRemoveRunGame("UGame_Remove.RunGame", "StartUp", null, null);
        }


    }




}



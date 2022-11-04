using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UGame_Local
{
    /// <summary>
    /// 热更资源下载
    /// </summary>
    public class AssetsDownLoad
    {
        private static Action<bool> downloadEnd = null;

        private static AsyncOperationHandle downloadDependencies;//下载句柄

        private static DownLoadHandleInfoCarrier downLoadPercent = null;

        /// <summary>资源下载进度</summary>
        public static float DownLoadPercent
        {
            get
            {
                if (downLoadPercent == null) return 0;

                if (downLoadPercent.downLoadHandle.IsValid())
                {
                    downLoadPercent.precent = downloadDependencies.GetDownloadStatus().Percent;
                }
                else if (downLoadPercent.downLoadHandle.GetDownloadStatus().IsDone)
                {
                    downLoadPercent.precent = downLoadPercent.downLoadHandle.Status == AsyncOperationStatus.Succeeded ? 1f : 0;
                }
                return downLoadPercent.precent;
            }
        }

        /// <summary>下载资源大小</summary>
        public static float DownLoadSize => downLoadPercent == null ? 0 : downLoadPercent.downLoadSize;


        /// <summary>开始下载热更资源</summary>
        public static IEnumerator StartDownAsync(Action<bool> downloadEnd)
        {

#if UNITY_EDITOR
            //BuildScriptPackedPlayMode-->Use Existing Build
            if (!(UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.ActivePlayModeDataBuilder is
                  UnityEditor.AddressableAssets.Build.DataBuilders.BuildScriptPackedPlayMode))
            {
                Debug.Log("curr not Use Existing Build,not hot assets ");
                downloadEnd?.Invoke(true);
                yield break;
            }
#endif

            AssetsDownLoad.downloadEnd = downloadEnd;

            yield return DownAsync();

        }


        /// <summary>
        /// 删除包缓存中不再引用的所有AssetBundle
        /// </summary>
        public static void CleanBundleCache()
        {
            Addressables.CleanBundleCache().WaitForCompletion();
        }


        /// <summary>下载热更资源</summary>
        private static IEnumerator DownAsync()
        {
            Debug.Log("start down assets");

            Debug.Log($"assets down path:{AssetsRemoteLoadPath.Path}");

            downLoadPercent = null;

            //初始化Addressables
            AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync(true);
            yield return handle;

            //检查所有可更新的内容目录以获取新版本
            AsyncOperationHandle<List<string>> catalogs = Addressables.CheckForCatalogUpdates(false);
            yield return catalogs;

            if (catalogs.Result != null && catalogs.Result.Count > 0)
            {
                //更新指定的[catalogs.Result]目录
                var resourceLocator = Addressables.UpdateCatalogs(catalogs.Result, false);
                yield return resourceLocator;
                Addressables.Release(resourceLocator);
            }

            List<object> requestDownLoadKeys = new List<object>();

            foreach (var item in Addressables.ResourceLocators)
            {
                requestDownLoadKeys.AddRange(item.Keys);
            }

            AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(requestDownLoadKeys as IEnumerable);
            yield return getDownloadSize;

            bool result = true;

            Debug.Log($" need down load count: {requestDownLoadKeys.Count}  down load size：{(getDownloadSize.Result / (1024f * 1024f)).ToString("0.000")}MB");

            if (getDownloadSize.Result > 0)
            {
                //下载资源
                downloadDependencies = Addressables.DownloadDependenciesAsync(requestDownLoadKeys as IEnumerable, Addressables.MergeMode.Union, false);
                downLoadPercent = new DownLoadHandleInfoCarrier(downloadDependencies, getDownloadSize.Result);
                yield return downloadDependencies;

                result = downloadDependencies.Status == AsyncOperationStatus.Succeeded ? true : false;

                downLoadPercent.precent = downloadDependencies.Status == AsyncOperationStatus.Succeeded ? 1 : 0f;

                Debug.Log("<color=#00ff00>assets down load complete</color>");
                Addressables.Release(downloadDependencies);
            }


            AssetsDownLoad.downloadEnd?.Invoke(result);
            AssetsDownLoad.downloadEnd = null;
            downLoadPercent = null;

            Debug.Log($"down finish -->result:{result}");

            //释放操作句柄
            Addressables.Release(catalogs);
            Addressables.Release(getDownloadSize);


        }


        /// <summary>下载句柄信息</summary>
        private class DownLoadHandleInfoCarrier
        {
            public AsyncOperationHandle downLoadHandle;

            /// <summary>下载句柄信息</summary>
            public float precent = 0;

            /// <summary>需下载资源size</summary>
            public float downLoadSize = 0;

            public DownLoadHandleInfoCarrier(AsyncOperationHandle downLoadHandle, float downLoadSize)
            {
                this.downLoadHandle = downLoadHandle;
                this.precent = 0;
                this.downLoadSize = downLoadSize;
            }
        }

    }
}



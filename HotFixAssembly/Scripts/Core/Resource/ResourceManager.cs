﻿using System;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UGame_Remove
{
    public class ResourceManager
    {
        private static LoadAssetsImpl loadAssets = new LoadAssetsImpl(new LoadAddressImpl());


        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="assetName">资源名</param>
        /// <param name="callBack">加载结束后回调，返回该资源</param>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static void LoadAssetAsync<TObject>(string assetName, Action<TObject> callBack) where TObject : Object
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new ArgumentNullException($"{nameof(assetName)} is invalid");
            }

            if (callBack == null)
            {
                throw new ArgumentNullException($"{nameof(callBack)} is invalid");
            }


            string path = AssetsMapper.IsExit(assetName) ? AssetsMapper.LoadPath(assetName) : assetName;

            loadAssets.LoadAssetAsync<TObject>(path, callBack);
        }


        /// <summary>
        /// 加载多个资源
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="assetsName">资源名集合</param>
        /// <param name="callBack">加载结束后回调</param>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static void LoadAssetsAsync<TObject>(IEnumerable assetsName, Action<TObject> callBack) where TObject : Object
        {
            if (assetsName == null)
            {
                throw new ArgumentNullException($"{nameof(assetsName)} is invalid");
            }

            IEnumerable paths = AssetsMapper.LoadPaths(assetsName);

            loadAssets.LoadAssetsAsync<TObject>(paths, callBack);
        }


        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="loadMode"></param>
        /// <param name="activateOnLoad"></param>
        /// <param name="priority"></param>
        /// <returns>加载场景句柄</returns>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new ArgumentNullException($"{nameof(sceneName)} is invalid");
            }
            string path = AssetsMapper.LoadPath(sceneName);
            return loadAssets.LoadSceneAsync(path, loadMode, activateOnLoad, priority);
        }


        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void ClearCache()
        {
            loadAssets.ClearCache();
        }

    }
}

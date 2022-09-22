using System;
using System.Collections;
using UnityEngine;
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
        ///加载单个资源。注意：此资源是打成AssetBundle，记录在映射表中，所以加载只需要填写资源名;
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="assetsName">资源Name</param>
        /// <param name="callBack">加载结束后回调，返回该资源</param>
        public static void LoadAssetAsync<TObject>(string assetsName, Action<TObject> callBack) where TObject : Object
        {
            string path = AssetsMapper.IsExit(assetsName) ? AssetsMapper.LoadPath(assetsName) : assetsName;

            loadAssets.LoadAssetAsync<TObject>(path, callBack);
        }


        /// <summary>
        ///加载多个资源。注意：此资源是打成AssetBundle，记录在映射表中，所以加载只需要填写资源名;
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="assetsName">资源Name</param>
        /// <param name="callBack">加载结束后回调，返回该资源</param>
        public static void LoadAssetsAsync<TObject>(IEnumerable assetsName, Action<TObject> callBack) where TObject : Object
        {
            IEnumerable paths = AssetsMapper.LoadPaths(assetsName);

            loadAssets.LoadAssetsAsync<TObject>(paths, callBack);
        }


        /// <summary>
        /// 加载场景。注意：此资源是打成AssetBundle，记录在映射表中，所以加载只需要填写场景名;
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="loadMode"></param>
        /// <param name="activateOnLoad"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
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

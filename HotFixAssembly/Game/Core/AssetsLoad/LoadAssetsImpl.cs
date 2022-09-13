using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using UGame_Local;

namespace UGame_Remove
{
    /// <summary>
    /// 加载资源，首先判断缓存中是否存在该资源，如果没有，则加载，如果有，直接返回
    /// </summary>
    public class LoadAssetsImpl
    {
        /// <summary>资源缓存</summary>
        private Dictionary<string, Object> assetsCaches = new Dictionary<string, Object>();

        private LoadAddressImpl load = null;

        public LoadAssetsImpl(LoadAddressImpl load)
        {
            this.load = load;
        }


        /// <summary>加载单个资源</summary>
        public void LoadAssetAsync<TObject>(string path, Action<TObject> callBack) where TObject : Object
        {
            MonoBehaviourRuntime.Instance.StartCoroutine(DoLoadAsset<TObject>(path, callBack));
        }


        /// <summary>加载多个资源</summary>
        public void LoadAssetsAsync<TObject>(IEnumerable paths, Action<TObject> callBack) where TObject : Object
        {
            MonoBehaviourRuntime.Instance.StartCoroutine(DoLoadAssets(paths, callBack));
        }

        /// <summary>加载场景</summary>
        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(string name, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
            return load.LoadSceneAsync(name, loadMode, activateOnLoad, priority);
        }


        /// <summary>清空缓存</summary>
        public void ClearCache()
        {
            assetsCaches.Clear();
        }



        private IEnumerator DoLoadAsset<TObject>(string path, Action<TObject> callBack) where TObject : Object
        {
            if (assetsCaches.ContainsKey(path))
            {
                var asset = assetsCaches[path];

                callBack?.Invoke(asset as TObject);
            }
            else
            {
                Action<TObject> overCallback = asset =>
                {
                    if (asset != null)
                    {
                        if (!assetsCaches.ContainsKey(path))
                        {
                            assetsCaches.Add(path, asset);
                        }
                    }

                    callBack?.Invoke(asset);
                };

                MonoBehaviourRuntime.Instance.StartCoroutine(load.LoadAssetAsync<TObject>(path, overCallback));
            }

            yield return new WaitForEndOfFrame();
        }


        private IEnumerator DoLoadAssets<TObject>(IEnumerable paths, Action<TObject> callBack) where TObject : Object
        {
            List<string> againLoadPaths = new List<string>();

            foreach (string path in paths)
            {
                if (assetsCaches.ContainsKey(path))
                {
                    var asset = assetsCaches[path];

                    callBack?.Invoke(asset as TObject);
                }
                else
                {
                    againLoadPaths.Add(path);
                }
            }


            if (againLoadPaths.Count == 0) yield break;

            Action<TObject> overCallback = asset =>
            {
                // 防止闭包，重新接一下参数
                List<string> tmpAgainLoadPaths = againLoadPaths;
                Action<TObject> tmpResCallback = callBack;


                if (asset != null)
                {
                    string tmpPath = tmpAgainLoadPaths.Find(name => { return Path.GetFileNameWithoutExtension(name).Equals(asset.name); });

                    if (!assetsCaches.ContainsKey(tmpPath))
                    {
                        assetsCaches.Add(tmpPath, asset);
                    }
                }
                tmpResCallback?.Invoke(asset);
            };

            MonoBehaviourRuntime.Instance.StartCoroutine(load.LoadAssetsAsync<TObject>(againLoadPaths, overCallback));

            yield return new WaitForEndOfFrame();
        }
    }

}


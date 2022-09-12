using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGame_Remove
{
    public class LoadAssetsImpl
    {
        private LoadAddressImpl addressImpl=null;


        public LoadAssetsImpl(LoadAddressImpl addressImpl)
        {
            this.addressImpl=addressImpl;
        }

        /// <summary>资源缓存</summary>
        private Dictionary<string, Object> assetsCaches = new Dictionary<string, Object>();


        /// <summary>加载单个资源</summary>
        public void LoadAssetAsync<TObject>(string path, Action<TObject> callBack) where TObject : Object
        {
            MonoBehaviourRuntime.Instance.StartCoroutine(DoLoadAsset<TObject>(path, callBack));
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

                MonoBehaviourRuntime.Instance.StartCoroutine(addressImpl.LoadAssetAsync<TObject>(path, overCallback));
            }

            yield return new WaitForEndOfFrame();
        }


        /// <summary>清空缓存</summary>
        public void ClearCache()
        {
            assetsCaches.Clear();
        }



    }
}

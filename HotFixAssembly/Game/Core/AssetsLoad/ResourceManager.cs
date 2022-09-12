using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UGame_Remove
{
    public class ResourceManager
    {
        private LoadAssetsImpl loadAssetsImpl = new LoadAssetsImpl(new LoadAddressImpl());

        public static void LoadAssetMappingAsync<TObject>(string path, Action<TObject> callBack) where TObject : Object
        {
            //AssetsMapping.InitializeAsync(callBack);
        }





        /// <summary>清空缓存</summary>
        public static void ClearCache()
        {

        }


    }
}

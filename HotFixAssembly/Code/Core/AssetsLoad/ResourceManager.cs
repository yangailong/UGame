using System;
using Object = UnityEngine.Object;

namespace UGame_Remove
{
    public class ResourceManager
    {

        public static void LoadAssetMappingAsync<TObject>(string path, Action<TObject> callBack) where TObject : Object
        {
            //loadAssets.LoadAssetAsync<TObject>(path, callBack);
        }
    }

}

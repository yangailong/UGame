using System.Collections;
using UnityEngine;

namespace UGame_Remove
{
    public static class AssetsMapping
    {
        private static readonly string needListenerAssetsRootPath = "AddressableAssets/Remote";
        private static readonly string creatPath = $"AssetsMapping.txt";
        private static readonly char namePathSplit = '	';


        /// <summary>资源映射数据  key:资源名    value:资源路径 </summary>
        private static Dictionary<string, string> mapping = new Dictionary<string, string>();
        public static Dictionary<string, string> Mapping => mapping;


        /// <summary>获取assetsName路径 </summary>
        public static string LoadPath(string assetsName)
        {
            string path = string.Empty;
            if (!mapping.TryGetValue(assetsName, out path))
            {
                Debug.LogError($"AssetsMapping can't find ->{assetsName}<-");
            }

            path = $"Assets/{needListenerAssetsRootPath}/{path.TrimStart().TrimEnd()}";

            return path.Trim();
        }


        /// <summary>获取多个assetsName路径 </summary>
        public static IEnumerable LoadPaths(IEnumerable assetsName)
        {
            List<string> paths = new List<string>();
            foreach (string name in assetsName)
            {
                paths.Add(LoadPath(name));
            }
            return paths;
        }


        /// <summary>是否存在该资源</summary>
        public static bool IsExit(string assetsName)
        {
            return mapping.ContainsKey(assetsName);
        }


        /// <summary>异步初始化</summary>
        public static void InitializeAsync(Action<bool> callback)
        {
            LoadAssetMappingAsync(callback);
        }


        /// <summary>加载读取映射表</summary>
        private static void LoadAssetMappingAsync(Action<bool> callback)
        {
            string path = $"Assets/{needListenerAssetsRootPath}/{creatPath}";
            ResourceManager.LoadAssetMappingAsync<TextAsset>(path, o =>
            {
                if (o == null || string.IsNullOrEmpty(o.text))
                {
                    callback?.Invoke(false);
                    return;
                }

                string tmpContent = o.text.TrimEnd('\n');
                string[] allLine = tmpContent.Split('\n');
                foreach (string line in allLine)
                {
                    string[] lineData = line.Split(namePathSplit);
                    mapping.Add(lineData[0], lineData[1]);
                }
                Debug.Log($"mapping table count: {mapping.Count}");
                callback?.Invoke(true);
            });
        }

    }
}

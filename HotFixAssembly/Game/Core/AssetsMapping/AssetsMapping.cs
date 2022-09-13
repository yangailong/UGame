using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGame_Local;

namespace UGame_Remove
{
    public static class AssetsMapping
    {
        /// <summary>资源映射数据  key:资源名    value:资源路径 </summary>
        private static Dictionary<string, string> mapping = new Dictionary<string, string>();

        public static Dictionary<string, string> Mapping => mapping;


        /// <summary>获取assetsName路径 </summary>
        public static string LoadPath(string assetsName)
        {
            if (!mapping.TryGetValue(assetsName, out string path))
            {
                Debug.LogError($"AssetsMapping can't find ->{assetsName}<-");

                return assetsName;
            }

            path = $"Assets/{AssetsMappingConst.needListenerAssetsRootPath}/{path}";

            return path?.Trim();
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


        /// <summary>初始化映射表</summary>
        public static void Initialize(TextAsset textAsset)
        {
            if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
            {
                Debug.LogError($"mapping cannot be empty");
                return;
            }

            mapping.Clear();
            string tmpContent = textAsset.text.TrimEnd('\n');
            string[] allLine = tmpContent.Split('\n');
            foreach (string line in allLine)
            {
                string[] lineData = line.Split(AssetsMappingConst.namePathSplit);
                mapping.Add(lineData[0], lineData[1]);
            }

            Debug.Log("Mapping table count:" + mapping.Count);
        }



    }
}

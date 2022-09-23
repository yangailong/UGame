using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGame_Local;
using System;

namespace UGame_Remove
{
    public static class AssetsMapper
    {
        private static Dictionary<string, string> mapper = new Dictionary<string, string>();

        /// <summary>资源映射数据  key:资源名    value:资源路径 </summary>
        public static Dictionary<string, string> Mapper => mapper;


        /// <summary>
        /// 初始化资源映射表
        /// </summary>
        /// <param name="textAsset">资源映射表txt</param>
        public static void Init(TextAsset textAsset)
        {
            if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
            {
                throw new ArgumentNullException($"{nameof(textAsset)} cannot be empty ");
            }

            mapper.Clear();

            string tmpContent = textAsset.text.TrimEnd('\n');
            string[] allLine = tmpContent.Split('\n');
            foreach (string line in allLine)
            {
                string[] lineData = line.Split(AssetsMapperConst.namePathSplit);
                mapper.Add(lineData[0], lineData[1]);
            }

            Debug.Log($"mapper table count:{mapper.Count}");
        }


        /// <summary>
        /// 是否存在该资源
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <returns></returns>
        public static bool IsExit(string assetsName)
        {
            return mapper.ContainsKey(assetsName);
        }


        /// <summary>
        /// 获取assetsName路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <returns></returns>
        public static string LoadPath(string assetsName)
        {
            if (!mapper.TryGetValue(assetsName, out string path))
            {
                throw new ArgumentException($"AssetsMapper can't find ->{assetsName}<-");
            }

            path = $"Assets/{AssetsMapperConst.needListenerAssetsRootPath}/{path}";

            return path?.Trim();
        }


        /// <summary>
        /// 获取多个assetsName路径
        /// </summary>
        /// <param name="assetsName">资源集合</param>
        /// <returns></returns>
        public static IEnumerable LoadPaths(IEnumerable assetsName)
        {
            List<string> paths = new List<string>();

            foreach (string name in assetsName)
            {
                paths.Add(LoadPath(name));
            }
            return paths;
        }


    }
}

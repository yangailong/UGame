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


        /// <summary>
        /// 资源映射数据  key:资源名    value:资源路径
        /// </summary>
        public static Dictionary<string, string> Mapper => mapper;


        /// <summary>
        /// 初始化资源映射表
        /// </summary>
        /// <param name="assetsMapper">资源映射表</param>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static void Init(TextAsset assetsMapper)
        {
            if (assetsMapper == null || string.IsNullOrEmpty(assetsMapper.text))
            {
                throw new ArgumentNullException($"{nameof(assetsMapper)} is invalid");
            }

            mapper.Clear();

            string tmpContent = assetsMapper.text.TrimEnd('\n');
            string[] allLine = tmpContent.Split('\n');
            foreach (string line in allLine)
            {
                string[] lineData = line.Split(AssetsMapperConst.namePathSplit);
                mapper.Add(lineData[0], lineData[1]);
            }

            Debug.Log($"mapper table count:{mapper.Count}");
        }


        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="assetName">要检查的资源名</param>
        /// <returns>资源是否存在</returns>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static bool IsExit(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new ArgumentNullException($"{nameof(assetName)} is invalid ");
            }

            return mapper.ContainsKey(assetName);
        }


        /// <summary>
        /// 获取assetsName路径
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <returns>资源路径</returns>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        /// <exception cref="ArgumentException">未查找到该资源</exception>
        public static string LoadPath(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new ArgumentNullException($"{nameof(assetName)} is invalid ");
            }

            if (!mapper.TryGetValue(assetName, out string path))
            {
                throw new ArgumentException($"AssetsMapper can't find ->{assetName}<-");
            }

            path = $"Assets/{AssetsMapperConst.needListenerAssetsRootPath}/{path}";

            return path?.Trim();
        }


        /// <summary>
        /// 获取多个assetsName路径
        /// </summary>
        /// <param name="assetsName">要获取的资源名集合</param>
        /// <returns>资源路径集合</returns>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static IEnumerable LoadPaths(IEnumerable assetsName)
        {
            if (assetsName == null)
            {
                throw new ArgumentNullException($"{nameof(assetsName)} is invalid");
            }

            List<string> paths = new List<string>();

            foreach (string name in assetsName)
            {
                paths.Add(LoadPath(name));
            }
            return paths;
        }


    }
}

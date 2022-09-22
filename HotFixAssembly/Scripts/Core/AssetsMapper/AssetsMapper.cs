﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGame_Local;
using System;

namespace UGame_Remove
{
    public static class AssetsMapper
    {
        private static Dictionary<string, string> mapper = null;

        /// <summary>资源映射数据  key:资源名    value:资源路径 </summary>
        public static Dictionary<string, string> Mapper => mapper;


        /// <summary>
        /// 初始化资源映射表
        /// </summary>
        /// <param name="textAsset">资源映射表txt</param>
        public static void Initialize(TextAsset textAsset)
        {
            if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
            {
                Debug.LogError($"mapping cannot be empty");
            }

            mapper = new Dictionary<string, string>();

            string tmpContent = textAsset.text.TrimEnd('\n');
            string[] allLine = tmpContent.Split('\n');
            foreach (string line in allLine)
            {
                string[] lineData = line.Split(AssetsMappingConst.namePathSplit);
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

            path = $"Assets/{AssetsMappingConst.needListenerAssetsRootPath}/{path}";

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

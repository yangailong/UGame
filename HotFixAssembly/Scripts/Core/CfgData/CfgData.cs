using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UGame_Local;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UGame_Remove
{
    public class CfgData
    {

        private static Dictionary<string, ScriptableObject> valuePairs = null;


        /// <summary>
        /// 异步初始化是否完成
        /// true：完成
        /// false：未完成
        /// </summary>
        public static bool InitAsyncComplete { get; set; } = false;


        /// <summary>
        /// 异步初始化·读取配置数据
        /// </summary>
        public static async void InitAsync()
        {
            valuePairs = new Dictionary<string, ScriptableObject>();

            
            Assembly assembly = typeof(UGame).Assembly;

            Predicate<Type> typeMatch = type => type.BaseType == typeof(ScriptableObject) && !string.IsNullOrEmpty(type.Namespace) && type.Namespace.Equals("UGame_Local_Out_CfgData");

            var arr = assembly.GetTypes().ToList().FindAll(typeMatch);

            string[] assetsName = arr.Select(type => type.Name).ToArray();

            foreach (string assetName in assetsName)
            {
                Debug.Log($"配置表Name: {assetName}");
            }

            AsyncOperationHandle<IList<ScriptableObject>> handle = Addressables.LoadAssetsAsync<ScriptableObject>(assetsName, null);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new ArgumentNullException($"配置表加载失败：{handle.OperationException.Message}");
            }

            IList<ScriptableObject> loadedAssets = handle.Result;
            foreach (ScriptableObject asset in loadedAssets)
            {
                if (!valuePairs.ContainsKey(asset.name))
                {
                    valuePairs.Add(asset.name, asset);

                    Debug.Log($"CfgData {asset.name} load success");

                    if (valuePairs.Count == assetsName.Length)
                    {
                        InitAsyncComplete = true;

                        Debug.Log($"{nameof(CfgData)} Async Init Complete...All Count :{valuePairs.Count}");
                    }
                }
            }
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">要获取的数据类型</typeparam>
        /// <returns>要获得数据</returns>
        public static T GetValue<T>() where T : ScriptableObject
        {
            if (!valuePairs.TryGetValue(typeof(T).Name, out var value))
            {
                Debug.LogError($"The {typeof(T).Name} data does not exist");
            }
            return value as T;
        }




    }
}

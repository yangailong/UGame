using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UGame_Local;
using UnityEngine;

namespace UGame_Remove
{
    public class CfgData
    {
        private static Dictionary<string, ScriptableObject> valuePairs = null;


        public static void Init()
        {
            valuePairs = new Dictionary<string, ScriptableObject>();

            Assembly assembly = typeof(AppMain).Assembly;

            Predicate<Type> typeMatch = type => type.BaseType == typeof(ScriptableObject) && !string.IsNullOrEmpty(type.Namespace) && type.Namespace.Equals("UGame_Local_Out_CfgData");

            var arr = assembly.GetTypes().ToList().FindAll(typeMatch);

            string[] assetsName = arr.Select(type => type.Name).ToArray();


            ResourceManager.LoadAssetsAsync<ScriptableObject>(assetsName, o =>
            {
                if (!valuePairs.ContainsKey(o.name))
                {
                    valuePairs.Add(o.name, o);

                    Debug.Log($"count:{valuePairs.Count}");
                }
            });
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

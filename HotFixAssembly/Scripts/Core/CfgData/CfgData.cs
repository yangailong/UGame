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

            var arr = assembly.GetTypes().ToList().FindAll(t => t.Namespace.Equals("UGame_Local_CfgData"));
            foreach (var item in arr)
            {
                Debug.LogError($"name:{item.Name}");
            }
        }



        public T GetValue<T>() where T : ScriptableObject
        {
            if (!valuePairs.TryGetValue(typeof(T).Name, out var value))
            {
                Debug.LogError($"The {typeof(T).Name} data does not exist");
            }
            return value as T;
        }

    }
}

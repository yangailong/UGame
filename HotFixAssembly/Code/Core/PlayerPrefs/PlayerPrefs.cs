using System;
using System.Text;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace UGame_Remove
{
    public class PlayerPrefs
    {
        private const string SaverKeys = "UGame.Core.Saver.Keys";

        private static readonly string defaultEncryptKey = ""; //InitJEngine.Instance.key;

        public static void SaveString(string key,string value)
        {
            var s=UnityEngine.PlayerPrefs.GetString(SaverKeys, defaultEncryptKey);
        }


        private static void AddJSaverKeys(string key)
        {
            var s = UnityEngine.PlayerPrefs.GetString(SaverKeys, key);
            if (!s.Split(',').ToList().Contains(key))
            {
                var sb = new StringBuilder(s);
                sb.Append($",{key}");
                s = sb.ToString();
            }
            UnityEngine.PlayerPrefs.SetString(SaverKeys, s);
        }


        public static string SaveAsString<T>(string dataName, T val, string encryptKey = null)
        {
            if (String.IsNullOrEmpty(encryptKey))
            {
                encryptKey = defaultEncryptKey;
            }
            if (encryptKey.Length != 16)
            {
                var ex = new Exception("encryptKey needs to be 16 characters!");
                Debug.LogError($"[JSaver] 错误：{ex.Message}, {ex.Data["StackTrace"]}");
                return null;
            }
            string strData = val.ToString();
            var result = CryptoMgr.EncryptStr(strData, encryptKey);

            UnityEngine.PlayerPrefs.SetString(dataName, result);
            AddJSaverKeys(dataName);
            return result;
        }
    }
}

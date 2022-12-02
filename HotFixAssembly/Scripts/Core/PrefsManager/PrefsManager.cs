using System;
using UGame_Local;
using UnityEngine;

namespace UGame_Remove
{
    /// <summary>
    /// 本地数据持久化保存与读取
    /// </summary>
    public class PrefsManager
    {

        private static string Key=> UGame.Instance.cfgUGame.key;


        /// <summary>
        /// 是否有该键
        /// </summary>
        /// <param name="key">要检查的key</param>
        /// <returns></returns>
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }


        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="key">数据对应的唯一key</param>
        /// <param name="value">要写入的数据</param>
        public static void SetString(string key, string value)
        {
            var result = CryptoManager.EncryptStr(Key, value);
            PlayerPrefs.SetString(key, result);
        }


        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="key">数据对应的唯一key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue = null)
        {
            string decrypt = PlayerPrefs.GetString(key, defaultValue);

            if (!HasKey(key)) return defaultValue;

            try
            {
                return CryptoManager.DecryptStr(Key, decrypt);
            }
            catch (Exception e)
            {
                Debug.LogError($"can not decrypt:{key}, error details:{e.Message},local data:{decrypt}");
                return defaultValue;
            }
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key">数据对应的唯一key</param>
        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }


        /// <summary>
        ///删除所有数据
        /// </summary>
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }


    }
}

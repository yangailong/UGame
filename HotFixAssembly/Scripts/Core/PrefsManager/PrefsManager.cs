using System;
using UGame_Local;
using UnityEngine;

namespace UGame_Remove
{
    public class PrefsManager
    {

        private static string Key=UGame.Instance.cfgUGame.key;


        public static bool HasKey(string key)
        {

            return PlayerPrefs.HasKey(key);
        }


        public static void SetString(string key, string value)
        {

            var result = CryptoManager.EncryptStr(Key, value);

            PlayerPrefs.SetString(key, result);
        }


        public static string GetString(string key, string defaultValue = null)
        {
            string decrypt = PlayerPrefs.GetString(key, defaultValue);

            try
            {
                if (!HasKey(key)) return defaultValue;

                var result = CryptoManager.DecryptStr(Key, decrypt);

                return result;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("can not decrypt:{0}, error details:{1},local data:{2}", key, e.Message, decrypt);

                return defaultValue;
            }
        }


        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }


        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }


    }
}

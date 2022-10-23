using System;
using UnityEngine;

namespace UGame_Remove
{
    public class PrefsManager
    {

        private static readonly string defaultEncryptKey = "UGame.Core.Prefs.Keys";


        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }


        public static void SetString(string key, string value)
        {
            var result = CrypManager.EncryptStr(defaultEncryptKey, value);
            PlayerPrefs.SetString(key, result);
        }


        public static string GetString(string key, string defaultValue = null)
        {
            string decrypt = PlayerPrefs.GetString(key, defaultValue);

            try
            {
                decrypt = decrypt.Replace('-', '+').Replace('_', '/').PadRight(4 * ((decrypt.Length + 3) / 4), '=');

                var result = CrypManager.DecryptStr(defaultEncryptKey, decrypt);

                Debug.Log($" key:{key} PlayerPrefs.GetString:{decrypt}");

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

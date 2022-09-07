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


        public static string? GetString(string key, string? defaultValue = null)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogError($"does not exist key:{key}");
                return defaultValue;
            }


            var data = PlayerPrefs.GetString(key, defaultValue);

            try
            {
                var result = CrypManager.DecryptStr(defaultEncryptKey, data);
                return result;
            }
            catch (Exception e)
            {

                Debug.LogError($"can not decrypt:{key}, error details:{e.Message}  local data:{data}");

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

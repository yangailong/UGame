using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame_Remove
{
    public class PlayerPrefs
    {
        private const string JSaverKeys = "JEngine.Core.JSaver.Keys";

        private static readonly string defaultEncryptKey = ""; //InitJEngine.Instance.key;

        private static void AddJSaverKeys(string key)
        {

        }

        public static string SaveAsString<T>(string dataName, T val, string encryptKey = null)
        {
            return string.Empty;
        }


        public static byte[] SaveAsProtobufBytes<T>(string dataName, T val, string encryptKey = null) where T : class
        {

            return null;
        }


        public static string SaveAsJSON<T>(string dataName, T val, string encryptKey = null)
        {
            return string.Empty;
        }

    }
}

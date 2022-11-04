using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace UGame_Remove
{
    public class WebRequest
    {
        public string url;

        public string param;

        public string result;

        public IEnumerator PosSend(Dictionary<string, string> headers)
        {
            url = $"{url}?{param}";

            using (UnityWebRequest www = UnityWebRequest.Post(url, "Pos"))
            {
                foreach (KeyValuePair<string, string> kvp in headers)
                {
                    www.SetRequestHeader(kvp.Key, kvp.Value);
                }

                www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(param));

                yield return www.SendWebRequest();

                result = www.downloadHandler.text;

                if (string.IsNullOrEmpty(result))
                {
                    Debug.LogError($"pos 服务器返回消息为空，无法解析！");

                    yield break;
                }
            }
        }


        public IEnumerator GetSend(Dictionary<string, string> headers)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                foreach (KeyValuePair<string, string> kvp in headers)
                {
                    www.SetRequestHeader(kvp.Key, kvp.Value);
                }

                //if (!string.IsNullOrEmpty(param))
                //{
                //    webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(param));
                //}

                www.timeout = 50;

                yield return www.SendWebRequest();

                result = www.downloadHandler.text;

                if (string.IsNullOrEmpty(result))
                {
                    Debug.LogError($"get 服务器返回消息为空，无法解析！");

                    yield break;
                }
            }
        }


    }
}

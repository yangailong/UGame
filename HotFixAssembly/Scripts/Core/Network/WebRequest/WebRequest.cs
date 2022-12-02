using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace UGame_Remove
{
    public class WebRequest
    {
        public string url = string.Empty;

        public string message = string.Empty;

        public string result = string.Empty;


        public WebRequest() { }


        public WebRequest(string url, string message)
        {
            this.url = url;
            this.message = message;
        }


        public IEnumerator PosSend(Dictionary<string, string> headers)
        {
            url = $"{url}?{message}";

            using (UnityWebRequest www = UnityWebRequest.Post(url, "Pos"))
            {
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> kvp in headers)
                    {
                        www.SetRequestHeader(kvp.Key, kvp.Value);
                    }
                }

                www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(message));

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
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> kvp in headers)
                    {
                        www.SetRequestHeader(kvp.Key, kvp.Value);
                    }
                }

                //if (!string.IsNullOrEmpty(param))
                //{
                //    webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(param));
                //}

                www.timeout = 30;

                Debug.Log($"C2S:url:{url}  message:{message}");

                yield return www.SendWebRequest();

                result = www.downloadHandler.text;

                Debug.Log($"S2C:result:{result}");

                if (string.IsNullOrEmpty(result))
                {
                    Debug.LogError($"get 服务器返回消息为空，无法解析！");

                    yield break;
                }
            }
        }


    }
}

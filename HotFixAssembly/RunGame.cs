using UnityEngine;

namespace UGame_Remove
{
    public class RunGame
    {
        public static void StartUp()
        {
            Debug.Log($"UGame_Remove StartUp");

            EnterPrepare();
        }


        private static void EnterPrepare()
        {
            //ResourceManager.LoadAssetMappingAsync<TextAsset>($"{AssetsMapping.needListenerAssetsRootPath}", null);
            //Addressables.LoadAssetAsync<GameObject>("");

            UGame_Local.MonoBehaviourRuntime.Instance.StartCoroutine(Coroutine());


        }


        static System.Collections.IEnumerator Coroutine()
        {
            Debug.Log("开始协程,t=" + Time.time);
            yield return new WaitForSeconds(3);
            Debug.Log("等待了3秒,t=" + Time.time);
        }

    }



}
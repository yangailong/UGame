using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UGame_Local
{
    /// <summary>启动场景</summary>
    public class StartUp : MonoBehaviour
    {
        private Slider m_Slider = null;

        private void Awake()
        {
            m_Slider = GetComponentInChildren<Slider>();
        }


        private void Start()
        {
            StartCoroutine(DownLoadAssets());
        }


        private IEnumerator DownLoadAssets()
        {
            StartCoroutine(AssetsDownLoad.StartDownAsync(AssetsDownLoadCallback));
            yield return null;
            //TODO...下载说明
        }


        private void AssetsDownLoadCallback(bool succeeded)
        {
            if (!succeeded)
            {
                //TODO...重新下载

                StartCoroutine(DownLoadAssets());
                return;
            }

            new GameObject(typeof(AppMain).Name).AddComponent<AppMain>();
        }




    }

}
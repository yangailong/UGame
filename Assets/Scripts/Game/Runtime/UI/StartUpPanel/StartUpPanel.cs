using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace UGame_Local
{
    /// <summary>启动场景</summary>
    public class StartUpPanel : MonoBehaviour
    {
        private Slider m_Slider = null;
        private Text m_Text = null;

        private void Awake()
        {
            m_Slider = GetComponentInChildren<Slider>();
            m_Text = GetComponentInChildren<Text>();
        }


        private void OnEnable()
        {
            m_Slider.interactable = false;
            m_Slider.value = 0;
            m_Text.text = "检查更新......";
            isCallback = false;
        }


        private void Start()
        {
            StopAllCoroutines();
            StartCoroutine(DownLoadAssets());
        }


        private IEnumerator DownLoadAssets()
        {
            StartCoroutine(AssetsDownLoad.StartDownAsync(AssetsDownLoadCallback));

            yield return new WaitForEndOfFrame();

            while (!isCallback)
            {
                if (AssetsDownLoad.DownLoadSize != 0)
                {
                    m_Text.text = "当前下载进度";
                    float currDownLoad = AssetsDownLoad.DownLoadPercent * (AssetsDownLoad.DownLoadSize / (1024 * 1024));//已下载size
                    m_Text.text = $"{m_Text.text}  {currDownLoad.ToString("0.00")} / {(AssetsDownLoad.DownLoadSize / (1024 * 1024)).ToString("0.00")} MB";//已下载size/总下载size
                    m_Text.text = $"{m_Text.text}  {Mathf.CeilToInt(AssetsDownLoad.DownLoadPercent * 100)}%";//下载百分比
                }

                m_Slider.value = AssetsDownLoad.DownLoadPercent;

                yield return new WaitForEndOfFrame();
            }

        }


        private bool isCallback = false;
        private void AssetsDownLoadCallback(bool succeeded)
        {
            isCallback = true;
            if (!succeeded)//下载失败
            {
                Action<bool> downLoadFail = (b) =>
                {
                    if (b)
                    {
                        OnEnable();
                        StopAllCoroutines();
                        StartCoroutine(DownLoadAssets());
                    }
                    else
                    {
                        Application.Quit();
                    }
                };


                FindObjectOfType<TipPanel>()?.SetData("资源下载失败,请重新尝试!", downLoadFail);

            }
            else//下载成功
            {
                new GameObject($"{typeof(AppMain).Name}").AddComponent<AppMain>();

            }
        }



    }

   

}
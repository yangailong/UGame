using System;
using UnityEngine;
using UnityEngine.UI;
namespace UGame_Local
{
    public class TipPanel : MonoBehaviour
    {
        private Text m_Text = null;

        private Button m_CancelBtn = null, m_OkBtn = null;

        private Action<bool> OnClickCallback = null;

        private void Awake()
        {
            m_Text = transform.Find("Content/Text").GetComponent<Text>();
            m_CancelBtn = transform.Find("Content/CancelBtn").GetComponent<Button>();
            m_OkBtn = transform.Find("Content/OkBtn").GetComponent<Button>();
        }


        private void OnEnable()
        {
            m_CancelBtn?.onClick.AddListener(OnClickCancelBtn);
            m_OkBtn?.onClick.AddListener(OnClickOkBtn);
        }


        private void OnDisable()
        {
            m_CancelBtn?.onClick.RemoveAllListeners();
            m_OkBtn?.onClick.RemoveAllListeners();
        }


        public void OnClickCancelBtn()
        {
            OnClickCallback?.Invoke(false);
            gameObject.SetActive(false);
        }


        public void OnClickOkBtn()
        {
            OnClickCallback?.Invoke(false);
            gameObject.SetActive(false);
        }


        public void SetData(string message, Action<bool> onClickCallabck)
        {
            gameObject.SetActive(true);
            Awake();
            m_Text.text = message;
            OnClickCallback = onClickCallabck;
        }


    }
}

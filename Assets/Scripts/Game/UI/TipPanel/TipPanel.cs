using System;
using UnityEngine;
using UnityEngine.UI;

namespace UGame_Local
{
    public class TipPanel : MonoBehaviour
    {
        private Text m_Text = null;

        private Button m_CancelBtn = null, m_OkBtn = null, m_MaskBtn = null;

        private Action<bool> OnClickBtnCallback = null;


        private void GetComponent()
        {
            m_Text = transform.Find("Content/m_Text").GetComponent<Text>();

            m_CancelBtn = transform.Find("Content/m_CancelBtn").GetComponent<Button>();

            m_OkBtn = transform.Find("Content/m_OkBtn").GetComponent<Button>();

            m_MaskBtn = transform.Find("BG/m_MaskBtn").GetComponent<Button>();
        }


        private void OnEnable()
        {
            m_CancelBtn?.onClick.AddListener(OnClickCancelBtn);
            m_OkBtn?.onClick.AddListener(OnClickOkBtn);
            m_MaskBtn.onClick.AddListener(OnClickMaskBtn);
        }


        private void OnDisable()
        {
            m_CancelBtn?.onClick.RemoveAllListeners();
            m_OkBtn?.onClick.RemoveAllListeners();
            m_MaskBtn.onClick.RemoveAllListeners();

            OnClickBtnCallback = null;
        }


        public void OnClickCancelBtn()
        {
            OnClickBtnCallback?.Invoke(false);
            gameObject.SetActive(false);
            OnClickBtnCallback = null;
        }


        public void OnClickOkBtn()
        {
            OnClickBtnCallback?.Invoke(true);
            gameObject.SetActive(false);
            OnClickBtnCallback = null;
        }


        public void OnClickMaskBtn()
        {
            OnClickOkBtn();
        }


        public void Open(string message, Action<bool> onClickBtnCallabck)
        {
            GetComponent();
            gameObject.SetActive(true);
            m_Text.text = message;
            OnClickBtnCallback = onClickBtnCallabck;
        }


    }
}

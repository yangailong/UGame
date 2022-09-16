using JEngine.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _26Key
{

    public class AudioButtonClickComponent : MonoBehaviour
    {

        public string m_AudioName = "";

        public float m_Volume = 1f;


        private Button _button = null;
        private Button Button
        {
            get
            {
                if (_button == null)
                {
                    _button = GetComponent<Button>();
                }
                return _button;
            }
        }


        void Awake()
        {
            Button.onClick.AddListener(OnClick);
        }


        void OnDestroy()
        {
            Button.onClick.RemoveListener(OnClick);
        }



        private void OnClick()
        {
            if (!string.IsNullOrEmpty(m_AudioName))
            {
                AudioPlayManager.PlaySFX2D(m_AudioName, m_Volume);
            }
            else
            {
                Log.PrintError("不存在音频文件：" + m_AudioName);
            }
        }
    }
}
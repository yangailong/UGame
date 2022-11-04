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


        public void SetSliderValue(float value)
        {
            m_Slider.value = value;
        }


        public void SetText(string text)
        {
            m_Text.text = text;
        }


        public void SetData(string text,float sliderValue)
        {
            SetText(text);
            SetSliderValue(sliderValue);
        }

    }



}
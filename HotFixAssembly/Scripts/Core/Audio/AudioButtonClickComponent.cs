using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UGame_Remove
{
    public class AudioButtonClickComponent : MonoBehaviour
    {

        //[FormerlySerializedAs("m_AudioClip")]
        private AudioClip m_AudioClip = null;


      //  [FormerlySerializedAs("m_Volume")]
        private float m_Volume = 1f;


        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }


        void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveListener(OnClick);
        }


        private void OnClick()
        {
            AudioPlayManager.PlaySFX2D(m_AudioClip, m_Volume);
        }
    }
}
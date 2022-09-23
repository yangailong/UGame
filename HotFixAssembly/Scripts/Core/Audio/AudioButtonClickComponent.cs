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


        private void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(OnPointerClick);
        }


        private void OnDisTable()
        {
            GetComponent<Button>().onClick.RemoveListener(OnPointerClick);
        }


        private void OnPointerClick()
        {
            AudioPlayManager.PlaySFX2D(m_AudioClip, m_Volume);
        }


    }
}
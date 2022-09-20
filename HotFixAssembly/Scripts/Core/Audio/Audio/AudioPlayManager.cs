using System;
using UnityEngine;

namespace UGame_Remove
{
    public class AudioPlayManager : MonoBehaviour
    {
        public static Audio2DPlayer a2DPlayer = null;

        public static Audio3DPlayer a3DPlayer = null;


        public static void Init()
        {
            GameObject go = new GameObject("[AudioManager]");
            DontDestroyOnLoad(go);

            a2DPlayer = new Audio2DPlayer(go.AddComponent<AudioPlayManager>());
            a3DPlayer = new Audio3DPlayer(go.AddComponent<AudioPlayManager>());


            TotleVolume = float.Parse(PrefsManager.GetString("TotleVolume", "1"));
            MusicVolume = float.Parse(PrefsManager.GetString("MusicVolume", "1"));
            SFXVolume = float.Parse(PrefsManager.GetString("SFXVolume", "1"));
        }


        private static float totleVolume = 1f;
        public static float TotleVolume
        {
            get { return totleVolume; }
            set
            {
                totleVolume = Mathf.Clamp01(value);
                // SetMusicVolume();
                //SetSFXVolume();

            }
        }

        private static float musicVolume = 1f;


        public static float MusicVolume
        {
            get
            {
                return musicVolume;
            }

            set
            {
                musicVolume = Mathf.Clamp01(value);
                //SetMusicVolume();
            }
        }

        private static float sfxVolume = 1f;
        public static float SFXVolume
        {
            get
            {
                return sfxVolume;
            }

            set
            {
                sfxVolume = Mathf.Clamp01(value);
                //SetSFXVolume();
            }
        }


        private static void SetMusicVolume()
        {
            a2DPlayer.SetMusicVolume(totleVolume * musicVolume);
            a3DPlayer.SetMusicVolume(totleVolume * musicVolume);
        }

        private static void SetSFXVolume()
        {
            a2DPlayer.SetSFXVolume(totleVolume * sfxVolume);
            a3DPlayer.SetSFXVolume(totleVolume * sfxVolume);
        }

        public static void SaveVolume()
        {
            PrefsManager.SetString("TotleVolume", $"{TotleVolume}");
            PrefsManager.SetString("MusicVolume", $"{MusicVolume}");
            PrefsManager.SetString("SFXVolume", $"{SFXVolume}");
        }



    }
}

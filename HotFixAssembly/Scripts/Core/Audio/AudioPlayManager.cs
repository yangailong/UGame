using System;
using UnityEngine;

namespace UGame_Remove
{
    public class AudioPlayManager : MonoBehaviour
    {

        public static Audio2DPlayer a2DPlayer { get; private set; } = null;


        public static Audio3DPlayer a3DPlayer { get; private set; } = null;

        private static float totleVolume = 1f;

        private static float musicVolume = 1f;

        private static float sfxVolume = 1f;


        public static void Init()
        {
            var go = new GameObject($"[{typeof(AudioPlayManager).Name}]").AddComponent<AudioPlayManager>();

            DontDestroyOnLoad(go);

            a2DPlayer = new Audio2DPlayer(go);
            a3DPlayer = new Audio3DPlayer(go);


            TotleVolume = float.Parse(PrefsManager.GetString("TotleVolume", "1"));

            MusicVolume = float.Parse(PrefsManager.GetString("MusicVolume", "1"));

            SFXVolume = float.Parse(PrefsManager.GetString("SFXVolume", "1"));
        }


        public static float TotleVolume
        {
            get => totleVolume;

            set
            {
                totleVolume = Mathf.Clamp01(value);
                SetMusicVolume();
                SetSFXVolume();
            }
        }


        public static float MusicVolume
        {
            get => musicVolume;
           
            set
            {
                musicVolume = Mathf.Clamp01(value);
                SetMusicVolume();
            }
        }


        public static float SFXVolume
        {
            get => sfxVolume;

            set
            {
                sfxVolume = Mathf.Clamp01(value);
                SetSFXVolume();
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


        public static AudioAsset PlayMusic2D(AudioClip audioClip, int channel, float volumeScale = 1, bool isLoop = true, float delay = 0f)
        {
            return a2DPlayer.PlayMusic2D(channel, audioClip, isLoop, volumeScale, delay);
        }


        public static void PauseMusic2D(int channel, bool isPause)
        {
            a2DPlayer.PauseMusic2D(channel, isPause);
        }


        public static void PauseMusicAll2D(bool isPause)
        {
            a2DPlayer.PauseMusicAll2D(isPause);
        }


        public static void StopMusic2D(int channel)
        {
            a2DPlayer.StopMusic2D(channel);
        }


        public static void StopMusicAll2D()
        {
            a2DPlayer.StopMusicAll2D();
        }


        public static void PlaySFX2D(AudioClip audioClip, float volumeScale = 1f, float delay = 0f, float pitch = 1)
        {
            a2DPlayer.PlaySFX2D(audioClip, volumeScale, delay, pitch);
        }


        public static void PauseSFXAll2D(bool isPause)
        {
            a2DPlayer.PauseSFXAll2D(isPause);
        }


        public static AudioAsset PlayMusic3D(GameObject owner, AudioClip audioClip, int channel = 0, float volumeScale = 1, bool isLoop = true, float delay = 0f)
        {
            return a3DPlayer.PlayMusic3D(owner, audioClip, channel, isLoop, volumeScale, delay);
        }


        public static void PauseMusic3D(GameObject owner, int channel, bool isPause)
        {
            a3DPlayer.PauseMusic3D(owner, channel, isPause);
        }


        public static void PauseMusicAll3D(bool isPause)
        {
            a3DPlayer.PauseMusicAll3D(isPause);
        }


        public static void StopMusic3D(GameObject owner, int channel)
        {
            a3DPlayer.StopMusic3D(owner, channel);
        }


        public static void StopMusicOneAll3D(GameObject owner)
        {
            a3DPlayer.StopMusicOneAll3D(owner);
        }


        public static void StopMusicAll3D()
        {
            a3DPlayer.StopMusicAll3D();
        }


        public static void DestroyMusic3D(GameObject owner)
        {
            a3DPlayer.DestroyMusic3D(owner);
        }


        public static void DestroyMusicAll3D()
        {
            a3DPlayer.DestroyMusicAll3D();
        }


        public static void PlaySFX3D(GameObject owner, AudioClip audioClip, float delay = 0f, float volumeScale = 1f)
        {
            a3DPlayer.PlaySFX3D(owner, audioClip, volumeScale, delay);
        }


        public static void PlaySFX3D(Vector3 position, string name, float delay = 0f, float volumeScale = 1)
        {
            //TODO...
            //a3DPlayer.PlaySFX(position, name, volumeScale, delay);
        }


        public static void PauseSFXAll3D(bool isPause)
        {
            a3DPlayer.PauseSFXAll3D(isPause);
        }


        public static void DestroySFX3D(GameObject owner)
        {
            a3DPlayer.DestroySFX3D(owner);
        }


        public static void DestroySFXAll3D()
        {
            a3DPlayer.DestroySFXAll3D();
        }


        void Update()
        {
            a2DPlayer.ClearDirtyAudioAsset();
            a3DPlayer.ClearDirtyAudioAsset();
        }

    }
}

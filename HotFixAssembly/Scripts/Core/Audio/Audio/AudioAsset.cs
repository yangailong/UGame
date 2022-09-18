using System;
using UnityEngine;

namespace UGame_Remove
{
    public class AudioAsset
    {
        /// <summary>AudioSource</summary>
        public AudioSource audioSource = null;


        /// <summary>AudioSource 类型</summary>
        public AudioSourceType audioSourceType = AudioSourceType.Music;


        /// <summary>播放通道</summary>
        public int musicChannel = 0;

        private float totleVolume = 1;

        /// <summary>播放通道</summary>
        private float volumeScale = 1f;


        /// <summary>音量</summary>
        public float Volume
        {
            get => audioSource.volume;
            set => audioSource.volume = value;
        }


        /// <summary>总音量</summary>
        public float TotleVolume
        {
            get => totleVolume;
            set
            {
                totleVolume = value;
                Volume = totleVolume * volumeScale;
            }
        }


        /// <summary>相对于总音量当前当前AudioSource的音量缩放 Volume=TotleVolume * volumeScale</summary>
        public float VolumeScale
        {
            get => volumeScale;
            set
            {
                volumeScale = Mathf.Clamp01(value);
                ResetVolume();
            }
        }


        /// <summary>实际音量恢复到当前的最大</summary>
        public void ResetVolume() => Volume = TotleVolume * volumeScale;


        /// <summary>是否在播放</summary>
        public bool IsPlaying => audioSource.isPlaying;


        /// <summary>播放状态</summary>
        public AudioPlayState playState
        {
            get => playState;
            set => playState = value;
        }


        /// <summary>Audio Source Name</summary>
        public string Name => audioSource?.name;



        public void Play(float delay = 0)
        {
            if (audioSource?.clip != null)
            {
                audioSource.time = 0;
                audioSource.PlayDelayed(delay);
                playState = AudioPlayState.Playing;
            }
        }


        public void Pause()
        {
            if (audioSource?.clip != null && audioSource.isPlaying)
            {
                audioSource.Pause();
                playState = AudioPlayState.Pause;
            }
        }


        public void Stop()
        {
            audioSource?.Stop();
            playState = AudioPlayState.Stop;
        }


        public void Reset()
        {
            audioSource.pitch = 1;
        }




    }
}

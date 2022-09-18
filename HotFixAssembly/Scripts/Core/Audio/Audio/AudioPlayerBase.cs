using System.Collections.Generic;
using UnityEngine;

namespace UGame_Remove
{
    public class AudioPlayerBase
    {
        protected MonoBehaviour mono;

        public AudioPlayerBase(MonoBehaviour mono)
        {
            this.mono = mono;
        }


        private Queue<AudioAsset> audioAssetsPool = new Queue<AudioAsset>();


        private float musicVolume = 1f;

        public float MusicVolume { get => musicVolume; set => musicVolume = value; }


        private float sfxVolume = 1f;
        public float SFXVolume { get => sfxVolume; set => sfxVolume = value; }


        public AudioAsset CreateAudioAsset(GameObject go, bool is3D, AudioSourceType sourceType)
        {
            AudioAsset audioAsset = null;
            if (audioAssetsPool.Count > 0)
            {
                audioAsset = audioAssetsPool.Dequeue();
                audioAsset.Reset();
            }
            else
            {
                audioAsset = new AudioAsset();
                var tmpGo = new GameObject($"{sourceType}__source_{go.transform.childCount}");
                tmpGo.transform.SetParent(go.transform);
                audioAsset.audioSource = tmpGo.GetComponent<AudioSource>();
            }

            audioAsset.audioSource.spatialBlend = is3D ? 1 : 0;
            audioAsset.audioSourceType = sourceType;

            //TODO...该地方我认为只有一个voium，不应该有musicVolume和sfxVolume的区别
            audioAsset.TotleVolume = sourceType == AudioSourceType.Music ? musicVolume : sfxVolume;
            return audioAsset;
        }


        public void DestroyAudioAsset(AudioAsset audioAsset)
        {
            audioAsset.audioSource.clip = null;
            audioAssetsPool.Enqueue(audioAsset);
        }


        public void Play(AudioAsset audioAsset, AudioClip audioClip, bool isLoop = true, float volumeScale = 1, float delay = 0f, float pitch = 1)
        {
            audioAsset.audioSource.clip = audioClip;
            audioAsset.audioSource.loop = isLoop;
            audioAsset.audioSource.pitch = pitch;
            audioAsset.VolumeScale = volumeScale;
            audioAsset.Play();
        }


        public void Pause(AudioAsset audioAsset)
        {
            audioAsset.Pause();
        }


        public void Stop(AudioAsset audioAsset)
        {
            audioAsset.Stop();
        }



    }

}

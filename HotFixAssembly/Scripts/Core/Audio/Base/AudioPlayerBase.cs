using System.Collections.Generic;
using UnityEngine;

namespace UGame_Remove
{
    public class AudioPlayerBase
    {
        protected MonoBehaviour mono = null;

        private Queue<AudioAsset> assetsPool = null;

        public float musicVolume { get; set; } = 1f;


        public float sfxVolume { get; set; } = 1f;


        public AudioPlayerBase(MonoBehaviour mono)
        {
            this.mono = mono;

            this.assetsPool = new Queue<AudioAsset>();
        }


        public void Play(AudioAsset audioAsset, AudioClip audioClip, bool isLoop = true, float volumeScale = 1, float delay = 0f, float pitch = 1)
        {
            audioAsset.audioSource.clip = audioClip;
            audioAsset.audioSource.loop = isLoop;
            audioAsset.audioSource.pitch = pitch;
            audioAsset.VolumeScale = volumeScale;
            audioAsset.Play();
        }


        public AudioAsset CreateAudioAsset(GameObject go, bool is3D, AudioSourceType sourceType)
        {
            AudioAsset audioAsset = null;
            if (assetsPool.Count > 0)
            {
                audioAsset = assetsPool.Dequeue();
                audioAsset.Reset();
            }
            else
            {
                audioAsset = new AudioAsset();
                var tmpGo = new GameObject($"{sourceType}_{go.transform.childCount}");
                tmpGo.transform.SetParent(go.transform);
                audioAsset.audioSource = tmpGo.AddComponent<AudioSource>();
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
            assetsPool.Enqueue(audioAsset);
        }


    }

}

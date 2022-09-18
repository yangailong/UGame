using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGame_Remove
{
    public class Audio2DPlayer : AudioPlayerBase
    {
        public Dictionary<int, AudioAsset> bgMusicDic = new Dictionary<int, AudioAsset>();

        public List<AudioAsset> sfxList = new List<AudioAsset>();

        public Audio2DPlayer(MonoBehaviour mono) : base(mono) { }


        public void SetMusicVolume(float volume)
        {
            base.MusicVolume = volume;

            foreach (var item in bgMusicDic.Values)
            {
                item.TotleVolume = volume;
            }
        }


        public void SetSFXVolume(float volume)
        {
            base.SFXVolume = volume;

            foreach (var item in sfxList)
            {
                item.TotleVolume = volume;
            }
        }


        public AudioAsset PlayMusic(int channel, AudioClip audioClip, bool isLoop = true, float volumeScale = 1, float delay = 0f)
        {
            AudioAsset audioAsset = null;

            if (bgMusicDic.ContainsKey(channel))
            {
                audioAsset = bgMusicDic[channel];
            }
            else
            {
                audioAsset = CreateAudioAsset(mono.gameObject, false, AudioSourceType.Music);
                bgMusicDic.Add(channel, audioAsset);
            }
            audioAsset.musicChannel = channel;

            base.Play(audioAsset, audioClip, isLoop, volumeScale, delay);

            return audioAsset;
        }


        public void PauseMusic(int channel)
        {
            if (bgMusicDic.ContainsKey(channel))
            {
                AudioAsset audioAsset = bgMusicDic[channel];
                base.Pause(audioAsset);
            }
        }


        public void PauseMusicAll()
        {
            foreach (var item in bgMusicDic.Keys)
            {
                PauseMusic(item);
            }
        }


        public void StopMusic(int channel)
        {
            if (bgMusicDic.TryGetValue(channel, out var audioAsset))
            {
                base.Stop(audioAsset);
            }
        }


        public void StopMusicAll()
        {
            foreach (var item in bgMusicDic.Keys)
            {
                StopMusic(item);
            }
        }


        public void PlaySFX(string name, float volumeScale = 1f, float delay = 0f, float pitch = 1)
        {
            
        }

        public void PauseSFXAll(bool isPause)
        {
            
        }
    }
}

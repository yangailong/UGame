using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGame_Remove
{
    public class Audio2DPlayer : AudioPlayerBase
    {
        public Dictionary<int, AudioAsset> bgMusicDic = null;

        public List<AudioAsset> sfxList =null;

        public Audio2DPlayer(MonoBehaviour mono) : base(mono)
        {
            bgMusicDic = new Dictionary<int, AudioAsset>();
            sfxList = new List<AudioAsset>();   
        }


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


        public void PlaySFX(AudioClip audioClip, float volumeScale = 1f, float delay = 0f, float pitch = 1)
        {
            AudioAsset audioAsset = CreateAudioAsset(mono.gameObject, false, AudioSourceType.SFX);
            sfxList.Add(audioAsset);
            base.Play(audioAsset, audioClip, false, volumeScale, delay, pitch);
        }


        public void PauseSFXAll(bool isPause)
        {
            foreach (var item in sfxList)
            {
                if (isPause)
                {
                    item.Pause();
                }
                else
                {
                    item.Play();
                }
            }
        }



        public void ClearDirtyAudioAsset()
        {
            var dirty = new List<AudioAsset>();

            foreach (var item in sfxList)
            {
                item.CheckState();
                if (item.PlayState == AudioPlayState.Stop)
                {
                    dirty.Add(item);
                }
            }


            for (int i = 0; i < dirty.Count; i++)
            {
                var item = dirty[i];
                DestroyAudioAsset(item);
                sfxList.Remove(item);
            }



            var channalDirty = new List<int>();
            foreach (var item in bgMusicDic)
            {
                item.Value.CheckState();
                if (item.Value.PlayState == AudioPlayState.Stop)
                {
                    channalDirty.Add(item.Key);
                }
            }


            foreach (var item in channalDirty)
            {
                DestroyAudioAsset(bgMusicDic[item]);
                bgMusicDic.Remove(item);
            }

            channalDirty.Clear();
        }


    }
}

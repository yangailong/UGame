using System.Collections.Generic;
using UnityEngine;

namespace UGame_Remove
{
    public class Audio2DPlayer : AudioPlayerBase
    {
        public Dictionary<int, AudioAsset> musics { get; private set; } = null;

        public List<AudioAsset> sfxs { get; private set; } = null;


        public Audio2DPlayer(MonoBehaviour mono) : base(mono)
        {
            musics = new Dictionary<int, AudioAsset>();
            sfxs = new List<AudioAsset>();
        }


        public void SetMusicVolume(float volume)
        {
            base.musicVolume = volume;

            foreach (var item in musics.Values)
            {
                item.TotleVolume = volume;
            }
        }


        public void SetSFXVolume(float volume)
        {
            base.sfxVolume = volume;

            foreach (var item in sfxs)
            {
                item.TotleVolume = volume;
            }
        }


        public AudioAsset PlayMusic2D(int channel, AudioClip audioClip, bool isLoop = true, float volumeScale = 1, float delay = 0f)
        {
            AudioAsset audioAsset = null;

            if (musics.ContainsKey(channel))
            {
                audioAsset = musics[channel];
            }
            else
            {
                audioAsset = base.CreateAudioAsset(mono.gameObject, false, AudioSourceType.Music);
                musics.Add(channel, audioAsset);
            }
            audioAsset.musicChannel = channel;

            base.Play(audioAsset, audioClip, isLoop, volumeScale, delay);

            return audioAsset;
        }


        public void PauseMusic2D(int channel, bool isPause)
        {
            if (musics.TryGetValue(channel, out var audioAsset))
            {
                if (isPause)
                {
                    audioAsset.Pause();
                }
                else
                {
                    audioAsset.Play();
                }
            }
        }


        public void PauseMusicAll2D(bool isPause)
        {
            foreach (var item in musics.Keys)
            {
                PauseMusic2D(item, isPause);
            }
        }


        public void StopMusic2D(int channel)
        {
            if (musics.TryGetValue(channel, out var audioAsset))
            {
                audioAsset.Stop();
            }
        }


        public void StopMusicAll2D()
        {
            foreach (var item in musics.Keys)
            {
                StopMusic2D(item);
            }
        }


        public void PlaySFX2D(AudioClip audioClip, float volumeScale = 1f, float delay = 0f, float pitch = 1)
        {
            AudioAsset audioAsset = CreateAudioAsset(mono.gameObject, false, AudioSourceType.SFX);
            sfxs.Add(audioAsset);
            base.Play(audioAsset, audioClip, false, volumeScale, delay, pitch);
        }


        public void PauseSFXAll2D(bool isPause)
        {
            foreach (var item in sfxs)
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

            foreach (var item in sfxs)
            {
                item.CheckAudioState();
                if (item.PlayState == AudioPlayState.Stop)
                {
                    dirty.Add(item);
                }
            }


            for (int i = 0; i < dirty.Count; i++)
            {
                var item = dirty[i];
                base.DestroyAudioAsset(item);
                sfxs.Remove(item);
            }



            var channalDirty = new List<int>();
            foreach (var item in musics)
            {
                item.Value.CheckAudioState();
                if (item.Value.PlayState == AudioPlayState.Stop)
                {
                    channalDirty.Add(item.Key);
                }
            }


            foreach (var item in channalDirty)
            {
                DestroyAudioAsset(musics[item]);
                musics.Remove(item);
            }

            channalDirty.Clear();
        }


    }
}

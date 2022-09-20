using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UGame_Remove
{
    public class Audio3DPlayer : AudioPlayerBase
    {

        public Dictionary<GameObject, Dictionary<int, AudioAsset>> bgMusicDic = null;

        public Dictionary<GameObject, List<AudioAsset>> sfxDic = null;


        public Audio3DPlayer(MonoBehaviour mono) : base(mono)
        {
            bgMusicDic = new Dictionary<GameObject, Dictionary<int, AudioAsset>>();
            sfxDic = new Dictionary<GameObject, List<AudioAsset>>();
        }


        public void SetMusicVolume(float volume)
        {
            base.MusicVolume = volume;
            foreach (var dics in bgMusicDic.Values)
            {
                foreach (var item in dics.Values)
                {
                    item.TotleVolume = volume;
                }
            }
        }


        public void SetSFXVolume(float volume)
        {
            base.SFXVolume = volume;

            foreach (var item in sfxDic.Values)
            {
                foreach (var cell in item)
                {
                    cell.TotleVolume = volume;
                }
            }
        }


        public AudioAsset Play(GameObject owner, AudioClip audioClip, int channel = 0, bool isLoop = true, float volumeScale = 1, float delay = 0f)
        {
            AudioAsset audioAsset = null;

            if (owner == null)
            {
                throw new ArgumentException($"cannot play 3d audioClip,owner is null");
            }

            if (!bgMusicDic.TryGetValue(owner, out var pairs))
            {
                pairs = new Dictionary<int, AudioAsset>();
                bgMusicDic.Add(owner, pairs);
            }

            if (!pairs.TryGetValue(channel, out audioAsset))
            {
                audioAsset = base.CreateAudioAsset(owner, true, AudioSourceType.Music);
                pairs.Add(channel, audioAsset);
            }

            base.Play(audioAsset, audioClip, isLoop, volumeScale, delay);

            return audioAsset;
        }


        public void Pause(GameObject owner, int channel, bool isPause)
        {
            if (owner == null)
            {
                throw new ArgumentException($"cannot play 3d audioClip,owner is null");
            }

            if (bgMusicDic.TryGetValue(owner, out var pairs))
            {
                if (pairs.TryGetValue(channel, out var audioAsset))
                {
                    base.Pause(audioAsset);
                }
            }
        }


        public void PauseAll(bool isPause)
        {
            foreach (var item in bgMusicDic.Keys)
            {
                foreach (var cell in bgMusicDic[item].Keys)
                {
                    Pause(item, cell, isPause);
                }
            }
        }


        public void Stop(GameObject owner, int channel)
        {
            if (bgMusicDic.TryGetValue(owner, out var pairs))
            {
                if (pairs.TryGetValue(channel, out var audioAsset))
                {
                    base.Stop(audioAsset);
                }
            }
        }


        public void StopSubAll(GameObject owner)
        {
            if (bgMusicDic.TryGetValue(owner, out var pairs))
            {
                foreach (var cell in pairs.Keys)
                {
                    Stop(owner, cell);
                }
            }
        }


        public void StopAll()
        {
            foreach (var item in bgMusicDic.Keys)
            {
                StopSubAll(item);
            }
        }


        public void Destroy(GameObject owner)
        {
            if (bgMusicDic.TryGetValue(owner, out var pairs))
            {
                StopSubAll(owner);

                foreach (var cell in pairs.Values)
                {
                    GameObject.Destroy(cell.audioSource);
                }
            }

            bgMusicDic.Remove(owner);
        }


        public void DestroyAll()
        {
            foreach (var item in bgMusicDic.Keys)
            {
                Destroy(item);
            }

            bgMusicDic.Clear();
        }


        public void PlaySFX(GameObject owner, AudioClip audioClip, float volumeScale = 1f, float delay = 0f)
        {
            AudioAsset audioAsset = base.CreateAudioAsset(owner, true, AudioSourceType.SFX);

            if (!sfxDic.TryGetValue(owner, out var audioAssets))
            {
                audioAssets = new List<AudioAsset>() { audioAsset };
                sfxDic.Add(owner, audioAssets);
            }

            base.Play(audioAsset, audioClip, false, volumeScale, delay);

            ClearSFXSubDirtyAudioAsset(owner);
        }


        public void PauseSFXAll(bool isPause)
        {
            foreach (var item in sfxDic.Values)
            {
                foreach (var cell in item)
                {
                    if (isPause)
                    {
                        cell.Pause();
                    }
                    else
                    {
                        cell.Play();
                    }
                }
            }
        }


        public void DestroySFX(GameObject owner)
        {
            if (sfxDic.TryGetValue(owner, out var audioAssets))
            {
                foreach (var item in audioAssets)
                {
                    GameObject.Destroy(item.audioSource);
                }

                audioAssets.Clear();

                sfxDic.Remove(owner);
            }
        }


        public void DestroySFXAll()
        {
            foreach (var item in sfxDic.Keys)
            {
                DestroySFX(item);
            }

            sfxDic.Clear();
        }


        private void ClearSFXSubDirtyAudioAsset(GameObject owner)
        {
            List<AudioAsset> dirty = new List<AudioAsset>();

            if (sfxDic.ContainsKey(owner))
            {
                var arr = sfxDic[owner];

                foreach (var item in arr)
                {
                    item.CheckState();

                    if (item.PlayState == AudioPlayState.Stop)
                    {
                        dirty.Add(item);
                    }
                }

                foreach (var item in dirty)
                {
                    GameObject.Destroy(item.audioSource);
                    base.DestroyAudioAsset(item);
                    arr.Remove(item);
                }

                dirty.Clear();
            }
        }


        public void ClearDirtyAudioAsset()
        {
            var dirty = new List<GameObject>();
            dirty.AddRange(bgMusicDic.Keys);

            foreach (var item in dirty)
            {
                if (item == null)
                {
                    bgMusicDic.Remove(item);
                }
            }


            foreach (var dic in bgMusicDic)
            {
                foreach (var item in dic.Value)
                {
                    if (item.Value.PlayState == AudioPlayState.Stop)
                    {
                        base.DestroyAudioAsset(bgMusicDic[dic.Key][item.Key]);
                        bgMusicDic[dic.Key].Remove(item.Key);

                        continue;
                    }
                }
            }


            if (sfxDic.Count > 0)
            {
                dirty.Clear();
                dirty.AddRange(sfxDic.Keys);
                for (int i = 0; i < dirty.Count; i++)
                {
                    if (dirty[i] == null)
                    {
                        sfxDic.Remove(dirty[i]);
                    }
                }
                foreach (var list in sfxDic)
                {
                    foreach (var item in list.Value)
                    {
                        item.CheckState();
                        if (item.PlayState == AudioPlayState.Stop)
                        {
                            base.DestroyAudioAsset(item);
                            sfxDic[list.Key].Remove(item);
                            break;
                        }
                    }
                }
            }
        }

    }
}

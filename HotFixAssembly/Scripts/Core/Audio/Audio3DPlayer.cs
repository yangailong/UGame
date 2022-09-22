using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UGame_Remove
{
    public class Audio3DPlayer : AudioPlayerBase
    {

        public Dictionary<GameObject, Dictionary<int, AudioAsset>> musics { get; private set; } = null;


        public Dictionary<GameObject, List<AudioAsset>> sfxs { get; private set; } = null;


        public Audio3DPlayer(MonoBehaviour mono) : base(mono)
        {
            musics = new Dictionary<GameObject, Dictionary<int, AudioAsset>>();
            sfxs = new Dictionary<GameObject, List<AudioAsset>>();
        }


        public void SetMusicVolume(float volume)
        {
            base.musicVolume = volume;
            foreach (var dics in musics.Values)
            {
                foreach (var item in dics.Values)
                {
                    item.TotleVolume = volume;
                }
            }
        }


        public void SetSFXVolume(float volume)
        {
            base.sfxVolume = volume;

            foreach (var item in sfxs.Values)
            {
                foreach (var cell in item)
                {
                    cell.TotleVolume = volume;
                }
            }
        }


        public AudioAsset PlayMusic3D(GameObject owner, AudioClip audioClip, int channel = 0, bool isLoop = true, float volumeScale = 1, float delay = 0f)
        {
            AudioAsset audioAsset = null;

            if (owner == null)
            {
                throw new ArgumentException($"cannot play 3d audioClip,owner is null");
            }

            if (!musics.TryGetValue(owner, out var pairs))
            {
                pairs = new Dictionary<int, AudioAsset>();
                musics.Add(owner, pairs);
            }

            if (!pairs.TryGetValue(channel, out audioAsset))
            {
                audioAsset = base.CreateAudioAsset(owner, true, AudioSourceType.Music);
                pairs.Add(channel, audioAsset);
            }

            base.Play(audioAsset, audioClip, isLoop, volumeScale, delay);

            return audioAsset;
        }


        public void PauseMusic3D(GameObject owner, int channel, bool isPause)
        {
            if (owner == null)
            {
                throw new ArgumentException($"cannot play 3d audioClip,owner is null");
            }

            if (musics.TryGetValue(owner, out var pairs))
            {
                if (pairs.TryGetValue(channel, out var audioAsset))
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
        }


        public void PauseMusicAll3D(bool isPause)
        {
            foreach (var item in musics.Keys)
            {
                foreach (var cell in musics[item].Keys)
                {
                    PauseMusic3D(item, cell, isPause);
                }
            }
        }


        public void StopMusic3D(GameObject owner, int channel)
        {
            if (musics.TryGetValue(owner, out var pairs))
            {
                if (pairs.TryGetValue(channel, out var audioAsset))
                {
                    audioAsset.Stop();
                }
            }
        }


        public void StopMusicOneAll3D(GameObject owner)
        {
            if (musics.TryGetValue(owner, out var pairs))
            {
                foreach (var cell in pairs.Keys)
                {
                    StopMusic3D(owner, cell);
                }
            }
        }


        public void StopMusicAll3D()
        {
            foreach (var item in musics.Keys)
            {
                StopMusicOneAll3D(item);
            }
        }


        public void DestroyMusic3D(GameObject owner)
        {
            if (musics.TryGetValue(owner, out var pairs))
            {
                StopMusicOneAll3D(owner);

                foreach (var cell in pairs.Values)
                {
                    GameObject.Destroy(cell.audioSource);
                }
            }

            musics.Remove(owner);
        }


        public void DestroyMusicAll3D()
        {
            foreach (var item in musics.Keys)
            {
                DestroyMusic3D(item);
            }

            musics.Clear();
        }


        public void PlaySFX3D(GameObject owner, AudioClip audioClip, float volumeScale = 1f, float delay = 0f)
        {
            AudioAsset audioAsset = base.CreateAudioAsset(owner, true, AudioSourceType.SFX);

            if (!sfxs.TryGetValue(owner, out var audioAssets))
            {
                audioAssets = new List<AudioAsset>() { audioAsset };
                sfxs.Add(owner, audioAssets);
            }

            base.Play(audioAsset, audioClip, false, volumeScale, delay);

            ClearSFXSubDirtyAudioAsset(owner);
        }


        public async void PlaySFX(Vector3 position, AudioClip audioClip, float volumeScale = 1f, float delay = 0f)
        {
            int millisecondsDelay = (int)(delay * 100);
            await Task.Delay(millisecondsDelay);
            AudioSource.PlayClipAtPoint(audioClip, position, AudioPlayManager.TotleVolume * AudioPlayManager.SFXVolume * volumeScale);
        }


        public void PauseSFXAll3D(bool isPause)
        {
            foreach (var item in sfxs.Values)
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


        public void DestroySFX3D(GameObject owner)
        {
            if (sfxs.TryGetValue(owner, out var audioAssets))
            {
                foreach (var item in audioAssets)
                {
                    GameObject.Destroy(item.audioSource);
                }

                audioAssets.Clear();

                sfxs.Remove(owner);
            }
        }


        public void DestroySFXAll3D()
        {
            foreach (var item in sfxs.Keys)
            {
                DestroySFX3D(item);
            }

            sfxs.Clear();
        }


        private void ClearSFXSubDirtyAudioAsset(GameObject owner)
        {
            List<AudioAsset> dirty = new List<AudioAsset>();

            if (sfxs.ContainsKey(owner))
            {
                var arr = sfxs[owner];

                foreach (var item in arr)
                {
                    item.CheckAudioState();

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
            dirty.AddRange(musics.Keys);

            foreach (var item in dirty)
            {
                if (item == null)
                {
                    musics.Remove(item);
                }
            }


            foreach (var dic in musics)
            {
                foreach (var item in dic.Value)
                {
                    if (item.Value.PlayState == AudioPlayState.Stop)
                    {
                        base.DestroyAudioAsset(musics[dic.Key][item.Key]);
                        musics[dic.Key].Remove(item.Key);

                        continue;
                    }
                }
            }


            if (sfxs.Count > 0)
            {
                dirty.Clear();
                dirty.AddRange(sfxs.Keys);
                for (int i = 0; i < dirty.Count; i++)
                {
                    if (dirty[i] == null)
                    {
                        sfxs.Remove(dirty[i]);
                    }
                }
                foreach (var list in sfxs)
                {
                    foreach (var item in list.Value)
                    {
                        item.CheckAudioState();
                        if (item.PlayState == AudioPlayState.Stop)
                        {
                            base.DestroyAudioAsset(item);
                            sfxs[list.Key].Remove(item);
                            break;
                        }
                    }
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
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

        public AudioAsset PlayMusic(GameObject owner, AudioClip audioClip, int channel = 0, bool isLoop = true, float volumeScale = 1, float delay = 0f)
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

            if (pairs.TryGetValue(channel, out audioAsset))
            {

            }


            return audioAsset;
        }

    }
}

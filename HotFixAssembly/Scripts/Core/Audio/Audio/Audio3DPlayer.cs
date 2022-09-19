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



    }
}

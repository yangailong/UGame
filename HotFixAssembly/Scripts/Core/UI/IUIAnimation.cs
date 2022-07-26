﻿using System.Collections;

namespace UGame_Remove
{
    interface IUIAnimation
    {

        /// <summary>
        /// 播放打开页面动画
        /// </summary>
        /// <param name="callBack">播放动画回调</param>
        /// <returns></returns>
        IEnumerator EnterAnim(UICallback callBack = null);


        /// <summary>
        /// 播放结束结束页面动画
        /// </summary>
        /// <param name="callBack">播放动画回调</param>
        /// <returns></returns>
        IEnumerator ExitAnim(UICallback callBack = null, params object[] param);


    }
}

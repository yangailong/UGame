using System;
using System.Collections;
using UnityEngine;

namespace UGame_Remove
{
    /// <summary>
    /// 此脚本是为了统一管理页面打开和结束动画
    /// 激活此脚本，页面动画就会播放，反之，则页面动画不播放
    /// </summary>
    public class UIAnimManager : MonoBehaviour
    {

        /// <summary>
        /// 播放页面打开动画
        /// </summary>
        /// <param name="panel">需要播放打开动画的页面</param>
        /// <returns></returns>
        public void StartEnterAnim(UIPanelBase panel)
        {
            if (panel is IUIEnterAnimation)
            {
                StartCoroutine(EnterAnim(panel));
            }
        }


        /// <summary>
        /// 播放页面结束动画
        /// </summary>
        /// <param name="panel">需要播放打开动画的页面</param>
        /// <param name="callback">播放动画回调</param>
        /// <returns></returns>
        public void StartExitAnim(UIPanelBase panel, Action callback)
        {
            if (panel is IUIExitAnimation)
            {
                StartCoroutine(ExitAnim(panel, callback));
            }
        }


        private IEnumerator EnterAnim(UIPanelBase panel)
        {
            yield return StartCoroutine((panel as IUIEnterAnimation).EnterAnim());
        }


        private IEnumerator ExitAnim(UIPanelBase panel, Action callback)
        {
            yield return (panel as IUIExitAnimation).ExitAnim();

            callback?.Invoke();
        }


    }
}

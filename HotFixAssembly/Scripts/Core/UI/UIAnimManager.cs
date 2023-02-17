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
        /// <param name="callBack">播放动画回调</param>
        public void StartEnterAnim(UIPanelBase panel, UICallback callBack)
        {
            if (panel is IUIEnterAnimation)
            {
                StartCoroutine((panel as IUIEnterAnimation).EnterAnim(callBack));
            }
        }


        /// <summary>
        /// 播放页面结束动画
        /// </summary>
        /// <param name="panel">需要播放结束动画的页面</param>
        /// <param name="callBack">播放动画回调</param>
        /// <param name="param">结束动画参数</param>
        public void StartExitAnim(UIPanelBase panel, UICallback callBack, params object[] param)
        {
            if (panel is IUIExitAnimation)
            {
                StartCoroutine((panel as IUIExitAnimation).ExitAnim(callBack, param));
            }
        }



    }
}

using UnityEngine;

namespace UGame_Remove
{
    public class UIAnimManager : MonoBehaviour
    {
        //开始调用进入动画
        public void StartEnterAnim(UIPanelBase panel, UICallback callBack, params object[] param)
        {
            StartCoroutine(panel.EnterAnim(EndEnterAnim, callBack, param));
        }


        //进入动画播放完毕回调
        public void EndEnterAnim(UIPanelBase panel, UICallback callBack, params object[] param)
        {
            panel.OnCompleteEnterAnim();

            callBack?.Invoke(panel, param);
        }


        //开始调用退出动画
        public void StartExitAnim(UIPanelBase panel, UICallback callBack, params object[] param)
        {
            StartCoroutine(panel.ExitAnim(EndExitAnim, callBack, param));
        }


        //退出动画播放完毕回调
        public void EndExitAnim(UIPanelBase panel, UICallback callBack, params object[] param)
        {
            panel.OnCompleteExitAnim();

            callBack?.Invoke(panel, param);
        }
    }
}

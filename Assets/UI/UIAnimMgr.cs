using UnityEngine;
using System;

public class UIAnimMgr : MonoBehaviour
{
    /// <summary>
    /// 开始调用入场动画
    /// </summary>
    /// <param name="UIbase">当前UI</param>
    /// <param name="callBack">入场回调</param>
    /// <param name="objs">传入的参数</param>
    public void StartEnterAnim(UIBase UIbase, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(UIbase.EnterAnim(EndEnterAnim, callBack, objs));
    }

    /// <summary>
    /// 结束入场动画
    /// </summary>
    /// <param name="UIbase">当前UI</param>
    /// <param name="callBack">出场回调</param>
    /// <param name="objs">传入的参数</param>
    public void EndEnterAnim(UIBase UIbase, UICallBack callBack, params object[] objs)
    {
        UIbase.OnCompleteEnterAnim();

        try
        {
            callBack?.Invoke(UIbase, objs);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    /// <summary>
    /// 出场动画
    /// </summary>
    /// <param name="UIbase">当前UI</param>
    /// <param name="callBack">出场回调</param>
    /// <param name="objs">传入的参数</param>
    public void StartExitAnim(UIBase UIbase, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(UIbase.ExitAnim(EndExitAnim, callBack, objs));
    }

    /// <summary>
    /// 结束出场动画
    /// </summary>
    /// <param name="UIbase">当前UI</param>
    /// <param name="callBack">出场回调</param>
    /// <param name="objs">传入的参数</param>
    public void EndExitAnim(UIBase UIbase, UICallBack callBack, params object[] objs)
    {
        UIbase.OnCompleteExitAnim();
        try
        {
            callBack?.Invoke(UIbase, objs);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

}


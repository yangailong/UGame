using System.Collections;
using UnityEngine;

public class UIBase : Base
{
    /// <summary>指定层级</summary>
    public UIType m_UIType = UIType.Two;

    /// <summary>指定BG</summary>
    public RectTransform m_BGMask;

    /// <summary>指定Content</summary>
    public RectTransform m_Content;


    /// <summary>
    /// 入场动画
    /// </summary>
    /// <param name="animComplete">动画完成回调 注意事项：只有回调了animComplete,才会调用OnCompleteEnterAnim</param>
    /// <param name="callBack">入场回调</param>
    /// <param name="objs">传入的参数</param>
    /// <returns></returns>
    public virtual IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        //默认无动画
        animComplete(this, callBack, objs);

        yield break;
    }

    /// <summary>
    /// 入场动画完成回调
    /// 注意事项：只有在EnterAnim回调了animComplete,才会调用OnCompleteExitAnim
    /// </summary>
    public virtual void OnCompleteEnterAnim() { }

    /// <summary>
    /// 出场动画
    /// </summary>
    /// <param name="animComplete">动画完成回调 注意事项：只有回调了animComplete,才会调用OnCompleteExitAnim</param>
    /// <param name="callBack">出场回调</param>
    /// <param name="objs">传入的参数</param>
    /// <returns></returns>
    public virtual IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        //默认无动画
        animComplete(this, callBack, objs);

        yield break;
    }

    /// <summary>
    /// 出场动画完成回调 
    /// 注意事项：只有在ExitAnim回调了animComplete,才会调用OnCompleteExitAnim
    /// </summary>
    public virtual void OnCompleteExitAnim() { }


    public override void OnUIDisable()
    {
        base.OnUIDisable();
    }
}


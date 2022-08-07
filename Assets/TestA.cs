using System.Collections;
using UnityEngine;

public class TestA : UIBase
{
    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        callBack?.Invoke(this, objs);

        yield return null;
        //return base.EnterAnim(animComplete, callBack, objs);
    }




}

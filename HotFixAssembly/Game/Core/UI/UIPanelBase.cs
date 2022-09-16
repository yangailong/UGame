using System.Collections;
using UnityEngine;

namespace UGame_Remove
{

    [UILayer(layer: UIPanelLayer.Normal)]

    public class UIPanelBase : MonoBehaviour, UICycle
    {

        public void OnUIAwake()
        {

        }


        public void OnUIStart()
        {

        }


        public void OnUIEnable()
        {

        }


        public void OnUIDisable()
        {

        }


        public void OnUIDestroy()
        {

        }


        public virtual IEnumerator EnterAnim(UIAnimCallback animCompleteCallback, UICallback callBack, params object[] param)
        {
            animCompleteCallback?.Invoke(this, callBack, param);

            yield break;
        }


        public virtual void OnCompleteEnterAnim()
        {

        }


        public virtual IEnumerator ExitAnim(UIAnimCallback animCompleteCallback, UICallback callBack, params object[] param)
        {
            animCompleteCallback?.Invoke(this, callBack, param);

            yield break;
        }


        public virtual void OnCompleteExitAnim()
        {

        }


    }
}


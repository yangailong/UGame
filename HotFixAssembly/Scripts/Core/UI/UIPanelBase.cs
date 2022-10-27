using System.Collections;
using UnityEngine;

namespace UGame_Remove
{

    public class UIPanelBase : MonoBehaviour, UICycle
    {

        public virtual void OnUIAwake()
        {

        }


        public virtual void OnUIStart()
        {

        }


        public virtual void OnUIEnable()
        {

        }


        public virtual void OnUIDisable()
        {

        }


        public virtual void OnUIDestroy()
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


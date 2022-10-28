using UnityEngine;

namespace UGame_Remove
{

    public class UIPanelBase : MonoBehaviour, UICycle
    {

        public object[] Params { set; private get; } = null;


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
        
    }
}


using UnityEngine;

namespace UGame_Remove
{

    public abstract class UIPanelBase : MonoBehaviour, UICycle
    {

        protected UIPanelBase() { }

        protected object[] message { set; get; }


        public void SetData(params object[] message)
        {
            this.message = message;
        }


        public virtual void OnUIAwake()
        {

        }


        public virtual void OnUIStart()
        {

        }


        public virtual void OnUIEnable()
        {
            gameObject.SetActive(true);
        }


        public virtual void OnUIDisable()
        {
            gameObject.SetActive(false);
        }


        public virtual void OnUIDestroy()
        {
            Destroy(gameObject);
        }

    }
}


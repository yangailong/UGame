using System.Collections;
using UGameRemove;
using UnityEngine;
using UnityEngine.UI;

namespace UGame_Remove
{

    [UILayer(layer: UIPanelLayer.GameUI)]
    public class DemoPanel : UIPanelBase, IUIAnimation
    {

        public override void OnUIAwake()
        {
            Debug.LogError($"OnUIAwake");

        }


        public override void OnUIStart()
        {
            Debug.LogError($"OnUIStart");
        }


        public override void OnUIEnable()
        {
            Debug.LogError($"OnUIEnable");
            transform.GetMountChind<Button>("m_Mask").onClick.AddListener(OnClickMaskBtn);
            transform.GetMountChind<Button>("m_CloseBtn").onClick.AddListener(OnClickMaskBtn);


            NetWebSocket.Instance.Register<S2C_Protoc>((int)MsgID.S2CDemo, S2CMessage);
        }



        public override void OnUIDisable()
        {
            transform.GetMountChind<Button>("m_Mask").onClick.RemoveAllListeners();

            NetWebSocket.Instance.Unregister((int)MsgID.S2CDemo);
        }


        public IEnumerator EnterAnim(UICallback callBack = null)
        {
            yield return new WaitForSeconds(2f);

            Debug.LogError($"打开页面开始动画完成....");

            callBack?.Invoke(this);
        }


        public IEnumerator ExitAnim(UICallback callBack = null, params object[] param)
        {
            yield return new WaitForSeconds(2f);

            Debug.LogError($"关闭页面结束动画完成....");

            callBack?.Invoke(this);
        }



        void OnClickMaskBtn()
        {
            Debug.LogError($"点击.....");

            //NetWebSocket.Instance.Send(0,);
        }

        public void S2CMessage(int id, S2C_Protoc message)
        {

        }

        public void C2SMessage()
        {
            C2S_Protoc c2S_Protoc = new C2S_Protoc();
            c2S_Protoc.Message = "请问你收到了没";

            NetWebSocket.Instance.Send((int)MsgID.C2SDemo, c2S_Protoc);
        }

    }
}

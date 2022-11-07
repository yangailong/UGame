using System.Collections;
using Test;
using UGameRemove;
using UnityEngine;
using UnityEngine.UI;

namespace UGame_Remove
{

    [UILayer(layer: UIPanelLayer.GameUI)]
    public class DemoPanel : UIPanelBase, IUIAnimation
    {

        public override object[] Params { set => base.Params = value; }


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

            NetWebSocket.Register<TestRes>((int)MsgID.S2CDemo, S2CMessage);
        }



        public override void OnUIDisable()
        {
            transform.GetMountChind<Button>("m_Mask").onClick.RemoveAllListeners();

            NetWebSocket.Unregister((int)MsgID.S2CDemo);
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

            //NetProxy.Instance.C2S_Demo();

            C2SMessage();
        }



        public void S2CMessage(int id, TestRes message)
        {
            Debug.Log($"接受到数据:{message.Name}");
            Debug.Log($"接受到数据:{message.Id}");
        }

        public void C2SMessage()
        {
            TestReq c2S_Protoc = new TestReq();
            c2S_Protoc.Name = "请问你收到了没";
            c2S_Protoc.Id = 1234;
            c2S_Protoc.Psd = "请问你收到了没";

            NetWebSocket.Send((int)MsgID.C2SDemo, c2S_Protoc);
        }

    }
}

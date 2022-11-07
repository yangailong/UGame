
using Test;
using UGameRemove;
using UnityEngine;

namespace UGame_Remove
{
    public partial class NetProxy
    {
        [NetProxy(NetProxyAttribute.AutonType.AutoRegister)]
        public void Register_Demo()
        {
            NetWebSocket.Register<TestRes>((int)MsgID.S2CDemo, S2CMessage);
        }


        [NetProxy(NetProxyAttribute.AutonType.AutoUnregister)]
        public void Unregister_Demo()
        {
            NetWebSocket.Unregister((int)MsgID.S2CDemo);
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


using UGameRemove;
using UnityEngine;

namespace UGame_Remove
{
    public partial class NetProxy
    {
        public void Register_Demo()
        {
            NetWebSocket.Register<S2C_Protoc>((int)MsgID.S2CDemo, S2C_Demo);
        }


        public void Unregister_Demo()
        {
            NetWebSocket.Unregister((int)MsgID.S2CDemo);
        }


        public void C2S_Demo()
        {
            C2S_Protoc c2S_Protoc = new C2S_Protoc();
            c2S_Protoc.Message = "请问你收到了没";

            NetWebSocket.Send((int)MsgID.C2SDemo, c2S_Protoc);
        }


        public void S2C_Demo(int arg1, S2C_Protoc arg2)
        {
            Debug.LogError($"收到服务器消息...");

            UIManager.Get<DemoPanel>().Params = new object[] { arg2 };
        }


    }
}

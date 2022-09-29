using Google.Protobuf;
using System;
using System.IO;

namespace UGame_Remove
{
    public delegate void MsgCallBack(int protocolId, MemoryStream receiveBuffer);
    public delegate void MsgCallBackWithT<T>(int id, T data) where T : IMessage, new();


    public class ProtocolAnalytical
    {
        protected int id;

        private MsgCallBack callback;

        public ProtocolAnalytical() { }

        public ProtocolAnalytical(int id, MsgCallBack callBack)
        {
            this.id = id;
            this.callback = callBack;
        }


        public virtual void AnalyzingContext(byte[] receiveBuffer, int startPos, int analyzingLength)
        {
            MemoryStream ms = new MemoryStream(receiveBuffer, startPos, analyzingLength);
            callback?.Invoke(id, ms);
            ms.Dispose();
        }


    }
}

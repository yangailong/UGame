using Google.Protobuf;
using System;
using System.IO;

namespace UGame_Remove
{

    public class WebSocketEventBase
    {
        protected int msgID;


        private Action<int, MemoryStream> callback;


        public WebSocketEventBase() { }


        public WebSocketEventBase(int id, Action<int, MemoryStream> callBack)
        {
            this.msgID = id;
            this.callback = callBack;
        }


        public virtual void AnalyzingContext(byte[] receiveBuffer, int startPos, int analyzingLength)
        {
            MemoryStream ms = new MemoryStream(receiveBuffer, startPos, analyzingLength);
            callback?.Invoke(msgID, ms);
            ms.Dispose();
        }


    }
}

using Google.Protobuf;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace UGame_Remove
{

    public delegate void MsgCallBackWithT<T>(int id, T data) where T : IMessage, new();


    public class WebSocketEvent<T> : WebSocketEventBase where T : IMessage, new()
    {
        private MsgCallBackWithT<T> callback;

        private Action<T, int> action;

        public WebSocketEvent(int id, MsgCallBackWithT<T> callback) : base()
        {
            this.msgID = id;
            this.callback = callback;
        }

        public override void AnalyzingContext(byte[] receiveBuffer, int startPos, int analyzingLength)
        {
            var msg = new T().Descriptor.Parser.ParseFrom(receiveBuffer, startPos, receiveBuffer.Length - startPos);
            Debug.Log($"recv megId:{msgID} message:{JsonConvert.SerializeObject(msg)}");
            callback?.Invoke(msgID, (T)msg);
        }
    }
}

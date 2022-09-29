using Google.Protobuf;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace UGame_Remove
{
    public class ProtocolAnalyticalWithT<T> : ProtocolAnalytical where T : IMessage, new()
    {
        private MsgCallBackWithT<T> callback;

        public ProtocolAnalyticalWithT(int id, MsgCallBackWithT<T> callback) : base()
        {
            this.id = id;
            this.callback = callback;
        }

        public override void AnalyzingContext(byte[] receiveBuffer, int startPos, int analyzingLength)
        {
            var msg = new T().Descriptor.Parser.ParseFrom(receiveBuffer, startPos, receiveBuffer.Length - startPos);
            Debug.Log($"recv megId:{id} message:{JsonConvert.SerializeObject(msg)}");
            callback?.Invoke(id, (T)msg);
        }
    }
}

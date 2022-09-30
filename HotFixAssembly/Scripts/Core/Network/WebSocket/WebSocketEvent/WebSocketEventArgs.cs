using Google.Protobuf;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace UGame_Remove
{

    public interface IWebSocketEvent
    {
        void Dispatch(byte[] receiveBuffer, int startPos);
    }


    public class WebSocketEventArgs<T> : IWebSocketEvent where T : IMessage, new()
    {
        private int id;

        private Action<int, T> callback;


        public WebSocketEventArgs(int id, Action<int, T> callback)
        {
            this.id = id;
            this.callback = callback;
        }


        public void Dispatch(byte[] receiveBuffer, int startPos)
        {
            var msg = new T().Descriptor.Parser.ParseFrom(receiveBuffer, startPos, receiveBuffer.Length - startPos);

            Debug.Log($"recv msgId: {id}, {JsonConvert.SerializeObject(msg)}");

            callback?.Invoke(id, (T)msg);
        }

    }

}


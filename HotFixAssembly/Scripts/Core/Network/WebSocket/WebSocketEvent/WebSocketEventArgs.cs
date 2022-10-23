using Google.Protobuf;
using System;
using System.IO;
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
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(receiveBuffer, startPos, receiveBuffer.Length - startPos);
                ms.Seek(0, SeekOrigin.Begin);
                MessageParser<T> parser = new MessageParser<T>(() => new T());
                var msg = parser.ParseFrom(ms);
                callback?.Invoke(id, msg);
            }

        }

    }

}


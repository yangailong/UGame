using System;
using Google.Protobuf;
using System.Collections.Generic;
namespace UGame_Remove
{
    public class WebSocketEventManager
    {
        private Dictionary<int, IWebSocketEvent> eventPairs = null;

        public WebSocketEventManager()
        {
            eventPairs = new Dictionary<int, IWebSocketEvent>();
        }


        public void Register<T>(int id, Action<int, T> callback) where T : IMessage, new()
        {
            if (!eventPairs.ContainsKey(id))
            {
                var socketEvent = new WebSocketEventArgs<T>(id, callback);

                eventPairs.Add(id, socketEvent);
            }
        }


        public void Unregister(int id)
        {
            if (eventPairs.ContainsKey(id))
            {
                eventPairs.Remove(id);
            }
        }


        public bool ContainsMsg(int id)
        {
            return eventPairs.ContainsKey(id);
        }


        public void Dispatch(int id, byte[] buffer)
        {
            if (eventPairs.TryGetValue(id, out var webSocketEvent))
            {
                webSocketEvent.Dispatch(buffer, 8);
            }
        }

    }
}

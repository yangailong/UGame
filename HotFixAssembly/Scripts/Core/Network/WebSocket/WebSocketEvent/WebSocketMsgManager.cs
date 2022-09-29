using Google.Protobuf;
using System.Collections.Generic;

namespace UGame_Remove
{
    public class WebSocketEventManager
    {

        private Dictionary<int, WebSocketEventBase>  eventPairs = null;


        public WebSocketEventManager()
        {
            eventPairs = new Dictionary<int, WebSocketEventBase>();
        }


        public void Register<T>(int id, MsgCallBackWithT<T> callback) where T : IMessage, new()
        {
            if (!eventPairs.ContainsKey(id))
            {
                WebSocketEvent<T> protocol = new WebSocketEvent<T>(id, callback);
                eventPairs.Add(id, protocol);
            }
        }


        public void Unregister<T>(int id, MsgCallBackWithT<T> callback) where T : IMessage, new()
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
            if (eventPairs.TryGetValue(id, out var protocol))
            {
                protocol.AnalyzingContext(buffer, 8, buffer.Length);
            }
        }


       


    }
}

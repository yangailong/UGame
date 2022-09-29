using Google.Protobuf;
using System.Collections.Generic;

namespace UGame_Remove
{
    public class SocketMessages
    {

        private Dictionary<int, ProtocolAnalytical> protocs = null;

        public SocketMessages()
        {
            protocs = new Dictionary<int, ProtocolAnalytical>();
        }


        public void Register<T>(int id, MsgCallBackWithT<T> callback) where T : IMessage, new()
        {
            if (!protocs.ContainsKey(id))
            {
                ProtocolAnalyticalWithT<T> protocol = new ProtocolAnalyticalWithT<T>(id, callback);
                protocs.Add(id, protocol);
            }
        }


        public void Unregister<T>(int id, MsgCallBackWithT<T> callback) where T : IMessage, new()
        {
            if (protocs.ContainsKey(id))
            {
                protocs.Remove(id);
            }
        }


        public bool HasProtocol(int id)
        {
            return protocs.ContainsKey(id);
        }


        public void Invoke(int id, byte[] buffer)
        {
            if (protocs.TryGetValue(id, out var protocol))
            {
                protocol.AnalyzingContext(buffer, 8, buffer.Length);
            }
        }


       


    }
}

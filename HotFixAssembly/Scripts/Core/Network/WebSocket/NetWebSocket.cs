using Google.Protobuf;
using System;
using System.IO;
using UnityEngine;
using WebSocket4Net;

namespace UGame_Remove
{
    public class NetWebSocket : MonoBehaviour
    {
        private static WebSocket m_WebSocket = null;
        private static WebSocketEvent webSocketEvent = null;

        public static event EventHandler Opened;
        public static event EventHandler Closed;
        public static event EventHandler<SuperSocket.ClientEngine.ErrorEventArgs> Error;
        public static event EventHandler<DataReceivedEventArgs> DataReceived;
        public static event EventHandler<MessageReceivedEventArgs> MessageReceived;


        public static void Init()
        {
            var go = new GameObject($"[{typeof(NetWebSocket).Name}]");
            go.AddComponent<NetWebSocket>();
            DontDestroyOnLoad(go);
        }


        public static void Open(string url, string subProtocol, WebSocketVersion socketVersion)
        {
            webSocketEvent = new WebSocketEvent();

            m_WebSocket = new WebSocket(url, subProtocol, socketVersion);
            m_WebSocket.EnableAutoSendPing = true;
            m_WebSocket.AutoSendPingInterval = 1;

            AddEvent();

            m_WebSocket.Open();
        }


        public static void Close()
        {
            m_WebSocket.Close();
            RemoveEvent();
            m_WebSocket.Dispose();
        }


        public static void Send(int id, IMessage msg)
        {
            Debug.Log($"send msgId: {id}, {JsonUtility.ToJson(msg)}");

            var buffer = Serialize(id, msg);
            m_WebSocket.Send(buffer, 0, buffer.Length);
        }


        public static void Register<T>(int id, Action<int, T> callback) where T : IMessage, new()
        {
            webSocketEvent.Register<T>(id, callback);
        }


        public static void Unregister(int id)
        {
            webSocketEvent.Unregister(id);
        }



        private static void AddEvent()
        {
            m_WebSocket.Opened += WebSocket_Opened;
            m_WebSocket.Closed += WebSocket_Closed;
            m_WebSocket.Error += M_WebSocket_Error;

            m_WebSocket.MessageReceived += WebSocket_MessageReceived;
            m_WebSocket.DataReceived += WebSocket_DataReceived;
        }


        private static void RemoveEvent()
        {
            m_WebSocket.Opened -= WebSocket_Opened;
            m_WebSocket.Closed -= WebSocket_Closed;
            m_WebSocket.Error -= M_WebSocket_Error;
            m_WebSocket.MessageReceived -= WebSocket_MessageReceived;
            m_WebSocket.DataReceived -= WebSocket_DataReceived;
        }


        private static void WebSocket_Opened(object sender, EventArgs e)
        {
            Debug.Log($"WebSocket_Opened args:{e}");
            Opened?.Invoke(sender, e);
            //开始 心跳
        }


        private static void WebSocket_Closed(object sender, EventArgs e)
        {
            Debug.Log($"WebSocket_Closed args:{e}");
            Closed?.Invoke(sender, e);
            //结束 心跳
        }


        private static void M_WebSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.Log($"M_WebSocket_Error args:{e}");
            Error?.Invoke(sender, e);
        }


        private static void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.Log($"WebSocket_MessageReceived args:{e}");
            MessageReceived?.Invoke(sender, e);
        }


        private static void WebSocket_DataReceived(object sender, DataReceivedEventArgs e)
        {
            DataReceived?.Invoke(sender, e);

            var buffer = e.Data;

            if (buffer == null || buffer.Length < 8)
            {
                throw new ArgumentException("message resoive failed");
            }

            var id = Bytes2Int(buffer, 4);

            if (webSocketEvent.ContainsMsg(id))
            {
                webSocketEvent.Dispatch(id, buffer);
            }
            else
            {
                throw new MissingMemberException($"收到一条未注册处理的消息  msgID：{id}");
            }

        }


        private static int Bytes2Int(byte[] bytes, int offset)
        {
            int value = 0;
            value = (int)((bytes[offset + 3] & 0xFF) | ((bytes[offset + 2] & 0xFF) << 8) | ((bytes[offset + 1] & 0xFF) << 16) | ((bytes[offset + 0] & 0xFF) << 24));
            return value;
        }


        private static byte[] Serialize(int msgId, IMessage msg)
        {
            using (var ms = new MemoryStream())
            {
                ms.Position = 8;
                msg.WriteTo(ms);
                ms.Position = 0;
                WriteNum(ms, (uint)ms.Length);
                WriteNum(ms, (uint)msgId);
                return ms.ToArray();
            }
        }


        private static void WriteNum(MemoryStream buffer, uint num)
        {
            var b0 = (byte)(num & 0xFF);
            var b1 = (byte)((num >> 8) & 0xFF);
            var b2 = (byte)((num >> 16) & 0xFF);
            var b3 = (byte)((num >> 24) & 0xFF);
            buffer.WriteByte(b3);
            buffer.WriteByte(b2);
            buffer.WriteByte(b1);
            buffer.WriteByte(b0);
        }


    }
}

using System;
using System.IO;
using UnityEngine;
using WebSocket4Net;
using Google.Protobuf;

namespace UGame_Remove
{
    public class NetWebSocket : ComponentSingleton<NetWebSocket>
    {
        private WebSocket m_WebSocket = null;

        private WebSocketEventManager m_SocketMessages = new WebSocketEventManager();

        public event EventHandler Opened;
        public event EventHandler Closed;
        public event EventHandler<SuperSocket.ClientEngine.ErrorEventArgs> Error;
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;


        void Awake()
        {
            this.Open("192.168.....", "", WebSocketVersion.Rfc6455);
        }


        void OnEnable()
        {
            m_WebSocket.Opened += WebSocket_Opened;
            m_WebSocket.Closed += WebSocket_Closed;
            m_WebSocket.Error += M_WebSocket_Error;
            m_WebSocket.MessageReceived += WebSocket_MessageReceived;
            m_WebSocket.DataReceived += WebSocket_DataReceived;
        }

      

        void OnDisable()
        {
            m_WebSocket.Opened -= WebSocket_Opened;
            m_WebSocket.Closed -= WebSocket_Closed;
            m_WebSocket.Error -= M_WebSocket_Error;
            m_WebSocket.MessageReceived -= WebSocket_MessageReceived;
            m_WebSocket.DataReceived -= WebSocket_DataReceived;
        }


        void OnDestory()
        {
            this.Close();
        }


        public void Open(string url, string subProtocol, WebSocketVersion socketVersion)
        {
            m_WebSocket = new WebSocket(url, subProtocol, socketVersion);

            m_WebSocket.EnableAutoSendPing = true;
            m_WebSocket.AutoSendPingInterval = 1;

            m_WebSocket.Open();
        }


        public void Close()
        {
            m_WebSocket.Close();

            m_WebSocket.Dispose();
        }


        public void Send(int id, IMessage msg)
        {
            Debug.Log($"send msgId: {id}, {JsonUtility.ToJson(msg)}");

            var buffer = Serialize(id, msg);
            m_WebSocket.Send(buffer, 0, buffer.Length);
        }


        private void WebSocket_Opened(object sender, EventArgs e)
        {
            Debug.Log($"WebSocket_Opened args:{e}");
            Opened?.Invoke(sender, e);
            //开始 心跳
        }


        private void WebSocket_Closed(object sender, EventArgs e)
        {
            Debug.Log($"WebSocket_Closed args:{e}");
            Closed?.Invoke(sender, e);

            //结束 心跳
        }


        private void M_WebSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.Log($"M_WebSocket_Error args:{e}");
            Error?.Invoke(sender, e);
        }



        private void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.Log($"WebSocket_MessageReceived args:{e}");
            MessageReceived?.Invoke(sender, e);
        }


        private void WebSocket_DataReceived(object sender, DataReceivedEventArgs e)
        {
            DataReceived?.Invoke(sender, e);

            var buffer = e.Data;

            if (buffer == null || buffer.Length < 8)
            {
                throw new ArgumentException("message resoive failed");
            }

            var id = Bytes2Int(buffer, 4);

            if (m_SocketMessages.ContainsMsg(id))
            {
                m_SocketMessages.Dispatch(id, buffer);
            }
            else
            {
                throw new MissingMemberException($"收到一条未注册处理的消息  msgID：{id}");
            }

        }


        private int Bytes2Int(byte[] bytes, int offset)
        {
            int value = 0;
            value = (int)((bytes[offset + 3] & 0xFF) | ((bytes[offset + 2] & 0xFF) << 8) | ((bytes[offset + 1] & 0xFF) << 16) | ((bytes[offset + 0] & 0xFF) << 24));
            return value;
        }


        private byte[] Serialize(int msgId, IMessage msg)
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


        private void WriteNum(MemoryStream buffer, uint num)
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

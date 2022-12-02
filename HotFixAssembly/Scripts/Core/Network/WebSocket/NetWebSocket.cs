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

        public static event EventHandler Opened = null;
        public static event EventHandler Closed = null;
        public static event EventHandler<SuperSocket.ClientEngine.ErrorEventArgs> Error = null;
        public static event EventHandler<DataReceivedEventArgs> DataReceived = null;
        public static event EventHandler<MessageReceivedEventArgs> MessageReceived = null;


        public static void Open(string url, string subProtocol, WebSocketVersion socketVersion)
        {
            webSocketEvent = new WebSocketEvent();

            m_WebSocket = new WebSocket(url, subProtocol, socketVersion);

            m_WebSocket.EnableAutoSendPing = true;
            m_WebSocket.AutoSendPingInterval = 1;

            m_WebSocket.Opened += WebSocket_Opened;
            m_WebSocket.Closed += WebSocket_Closed;
            m_WebSocket.Error += M_WebSocket_Error;

            m_WebSocket.MessageReceived += WebSocket_MessageReceived;
            m_WebSocket.DataReceived += WebSocket_DataReceived;


            m_WebSocket.Open();
        }


        public static void Close()
        {
            m_WebSocket.Close();
            m_WebSocket.Dispose();


            m_WebSocket.Opened -= WebSocket_Opened;
            m_WebSocket.Closed -= WebSocket_Closed;
            m_WebSocket.Error -= M_WebSocket_Error;
            m_WebSocket.MessageReceived -= WebSocket_MessageReceived;
            m_WebSocket.DataReceived -= WebSocket_DataReceived;
        }


        public static void Send(int id, IMessage msg)
        {
            if (m_WebSocket == null)
            {
                throw new MethodAccessException("WebSocket 不能为空，请先执行 WebSocket.Open 方法，确保WebSocket不为空");
            }

            if (msg == null)
            {
                throw new ArgumentException($"{nameof(msg)} 发送参数不能为空");
            }

            Debug.Log($"Send msg  id:{id},  msg：{JsonUtility.ToJson(msg)}");

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
            Debug.Log($"接收到数据：{buffer.Length}");


            //将数据放到MemoryStream中
            using (MemoryStream ms = new MemoryStream(buffer))
            using (BinaryReader br = new BinaryReader(ms))
            {
                //获取id，读取一个int类型的长度数据4字节
                int id = br.ReadInt32();

                //msg 数据
                byte[] data = br.ReadBytes(buffer.Length - (int)ms.Position);

                if (webSocketEvent.ContainsMsg(id))
                {
                    webSocketEvent.Dispatch(id, data);
                }
                else
                {
                    throw new MissingMemberException($"收到一条未注册处理的消息  msgID：{id}");
                }
            }
        }


        private static byte[] Serialize(int msgId, IMessage msg)
        {
            using (var ms = new MemoryStream())
            {
                ms.Position = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(msgId);
                    if (msg != null)
                    {
                        msg.WriteTo(ms);
                    }
                }

                return ms.ToArray();
            }




        }

    }
}

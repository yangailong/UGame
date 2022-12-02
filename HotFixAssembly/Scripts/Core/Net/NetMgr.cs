using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
using UnityEngine;
using JEngine.Core;
using JEngine.Event;
using M26Key.Protobuf;

//using ProtoData;

namespace _26Key
{
    public sealed class NetMgr : MonoBehaviour
    {
        public static NetMgr Instance { get { return MonoSingleton<NetMgr>.Instance; } }


        public delegate void MsgCallBack(int protocolId, MemoryStream receiveBuffer);
        public delegate void MsgCallBackWithT<T>(MsgCmd protofile, T data);


        private Dictionary<int, ProtocolAnalytical> m_dic = new Dictionary<int, ProtocolAnalytical>();

        private List<Action> actionList = new List<Action>();



        private SocketClient m_socketClient;


        public void InitSocket()
        {

            m_socketClient = new SocketClient();
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            m_socketClient?.OnUpdate();

            while (actionList.Count > 0)
            {
                var action = actionList[0];
                actionList.RemoveAt(0);
                if (action != null)
                {
                    action();
                }
            }
        }


        /// <summary>
        /// 开始连接到TCP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="OnConnectComplete"></param>
        public void BeginConnect(string ip, int port)
        {
            if (m_socketClient == null) return;
            m_socketClient.BeginConnect(ip, port);
        }


        /// <summary>
        /// 发送TCP数据
        /// </summary>
        /// <param name="ctype"></param> 
        /// <param name="data"></param>
        public void Send(MsgCmd ctype, IMessage data)
        {
            if (m_socketClient == null) return;
            m_socketClient.Send(ctype, data);
        }



        void OnDestroy()
        {
            if (m_socketClient != null)
                m_socketClient.Close(false);
        }


        //注册
        public void Register<T>(MsgCmd commondId, MsgCallBackWithT<T> callback) where T : IMessage, new()
        {
            int msgId = (int)commondId;
            if (m_dic.ContainsKey(msgId) == false)
            {
                ProtocolAnalyticalWithT<T> protocolAnalytical = new ProtocolAnalyticalWithT<T>(msgId, callback);
                m_dic.Add(msgId, protocolAnalytical);
            }
        }


        //注销
        public void Unregister<T>(MsgCmd commondId, MsgCallBackWithT<T> callback)
        {
            int msgId = (int)commondId;
            if (m_dic.ContainsKey(msgId))
            {
                m_dic.Remove(msgId);
            }
        }



        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="eventArgs"></param>
        public void OnDataReceived(CmdMsg eventArgs)
        {
            var buffer = eventArgs.body;

            if (m_dic.ContainsKey(eventArgs.id))
            {
                ProtocolAnalytical _protocolAnalytical = m_dic[eventArgs.id];
                if (_protocolAnalytical != null)
                {
                    actionList.Add(() =>
                    {
                        _protocolAnalytical.AnalyzingContext(buffer);
                    });
                }
                Log.Print($" MsgCmd:  {(MsgCmd)eventArgs.id}");
            }
            else
            {

                Log.PrintError("注意：收到一条MsgId为" + (MsgCmd)eventArgs.id + "的消息，但没有侦听解析处理");
            }
        }




        public class ProtocolAnalytical
        {
            protected int protocolId;
            private MsgCallBack callBack;

            public ProtocolAnalytical() { }

            public ProtocolAnalytical(int protocolId, MsgCallBack callBack)
            {
                this.protocolId = protocolId;
                this.callBack = callBack;
            }


            public virtual void AnalyzingContext(byte[] receiveBuffer)
            {
                MemoryStream s = new MemoryStream(receiveBuffer);
                callBack(protocolId, s);
                s.Dispose();
            }


        }

        private class ProtocolAnalyticalWithT<T> : ProtocolAnalytical where T : IMessage, new()
        {
            private MsgCallBackWithT<T> callback;
            public ProtocolAnalyticalWithT(int protocolId, MsgCallBackWithT<T> callback) : base()
            {
                this.protocolId = protocolId;
                this.callback = callback;
            }
            public override void AnalyzingContext(byte[] receiveBuffer)
            {
                var msg = _26Key.ProtobufEncodeTools.ProtobufDeserialize<T>(receiveBuffer);
                callback((MsgCmd)protocolId, (T)msg);
            }

        }
    }
}

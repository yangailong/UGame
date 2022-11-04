using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _26Key
{
    public class NetManager : MonoBehaviour
    {
        private NetManager()
        {
        }
        public delegate void MsgCallBack(int protocolId, MemoryStream receiveBuffer);
        public delegate void MsgCallBackWithT<T>(MsgCmd protofile, T data);



        private Dictionary<int, ProtocolAnalytical> dict = new Dictionary<int, ProtocolAnalytical>();

        private static NetManager _instance;
        public static NetManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject netObj = new GameObject("NetManager");
                    _instance = netObj.AddComponent<NetManager>();
                    GameObject.DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }



        private List<Action> actionList = new List<Action>();
        private void Update()
        {
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



        public void Register<T>(MsgCmd commondId, MsgCallBackWithT<T> callback)
        {
            int msgId = (int)commondId;
            if (dict.ContainsKey(msgId) == false)
            {
                ProtocolAnalyticalWithT<T> protocolAnalytical = new ProtocolAnalyticalWithT<T>(msgId, callback);
                dict.Add(msgId, protocolAnalytical);
            }
        }


        public void Unregister<T>(MsgCmd commondId, MsgCallBackWithT<T> callback)
        {
            int msgId = (int)commondId;
            if (dict.ContainsKey(msgId))
            {
                dict.Remove(msgId);
            }
        }



        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="eventArgs"></param>
        public void OnDataReceived(_26Key.CmdMsg eventArgs)
        {
            var buffer = eventArgs.body;

            if (dict.ContainsKey(eventArgs.id))
            {
                ProtocolAnalytical _protocolAnalytical = dict[eventArgs.id];
                if (_protocolAnalytical != null)
                {
                    actionList.Add(() =>
                    {
                        _protocolAnalytical.AnalyzingContext(buffer);
                    });
                }
            }
            else
            {
                Debug.Log("注意：收到一条MsgId为" + (MsgCmd)eventArgs.id + "的消息，但没有侦听解析处理");
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

        private class ProtocolAnalyticalWithT<T> : ProtocolAnalytical
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
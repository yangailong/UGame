using JEngine.Core;
using JEngine.Event;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;
using M26Key.Protobuf;
//using ProtoData;
using UnityEngine;

namespace _26Key
{
    /**
	 * 客户端连接
	 */
    public class SocketClient
    {
        public class SocketArgs
        {
            public SocketState state;
        }
        public enum SocketState
        {
            /// <summary>
            /// 空闲
            /// </summary>
            Idle,
            /// <summary>
            /// 连接
            /// </summary>
            Connect,
            /// <summary>
            /// 通讯
            /// </summary>
            Communicate,
            /// <summary>
            /// 重连
            /// </summary>
            Reconnect,
            /// <summary>
            /// 关闭
            /// </summary>
            Close,
        }

        private SocketState m_CurrentState = SocketState.Idle;//当前套接字状态

        /// <summary>
        /// 当前连接的IP 
        /// </summary>
        public string CurrentIP;
        /// <summary>
        /// 当前连接的端口
        /// </summary>
        public int CurrentPort;
        /// <summary>
        /// //当前网络状态
        /// </summary>
        public NetworkReachability CurrentNetType = NetworkReachability.NotReachable;
        /// <summary>
        /// //接收缓冲区
        /// </summary>
        private byte[] m_receiveBuffer = new byte[RECEIVE_DATA_LENGTH];
        /// <summary>
        /// 接收的真实的数据
        /// </summary>
        private byte[] m_reveiveData = null;
        /// <summary>
        /// 接收的真实数据的包尺寸
        /// </summary>
        private int m_receiveDataLength = 0;
        /// <summary>
        /// 接收的缓冲区的总长度
        /// </summary>
        private const int RECEIVE_DATA_LENGTH = 8192;
        /// <summary>
        /// 已经接收到的数据长度
        /// </summary>
        private int m_receivedLength = 0;
        /// <summary>
        /// //接受消息队列
        /// </summary>
        private Queue<CmdMsg> m_receiveQueue = new Queue<CmdMsg>();
        /// <summary>
        /// //发送消息队列
        /// </summary>
        private Queue<byte[]> m_QueueSendData = new Queue<byte[]>();
        /// <summary>
        /// //客户端套接字
        /// </summary>
        public Socket m_tcpSocket = null;
        public HeartBeat m_lastHeart;
        /// <summary>
        /// 接收线程
        /// </summary>
        public Thread m_receiveThread = null;

        public bool m_isActiveClose;
        public bool Connected
        {
            get
            {
                return (m_CurrentState == SocketState.Communicate || m_CurrentState == SocketState.Connect);
            }
        }

        private Dictionary<SocketState, SocketStateBase> m_SocketStateDic;

        public SocketClient()
        {

            m_SocketStateDic = new Dictionary<SocketState, SocketStateBase>();
            m_SocketStateDic.Add(SocketState.Idle, new SocketIdleState(this));
            m_SocketStateDic.Add(SocketState.Connect, new SocketConnectState(this));
            m_SocketStateDic.Add(SocketState.Communicate, new SocketCommunicateState(this));
            m_SocketStateDic.Add(SocketState.Close, new SocketCloseState(this));
            m_SocketStateDic.Add(SocketState.Reconnect, new SocketReconnectState(this));
        }

        /// <summary>
        /// 开始连接
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <param name="onConnectedCallBack">连接完成回调</param>
        public void BeginConnect(string ip, int port)
        {
            if (m_CurrentState != SocketState.Idle)
            {
                return;
            }
            CurrentIP = ip;
            CurrentPort = port;
            ChangeState(SocketState.Connect);
        }

        public void OnUpdate()
        {
            if (Connected == false)
            {
                return;
            }
            lock (m_receiveQueue)
            {
                while (m_receiveQueue.Count > 0)
                {
                    CmdMsg msg = m_receiveQueue.Dequeue();
                    if (msg.id == (ushort)MsgCmd.HeartBeat)
                    {
                        m_lastHeart = ProtobufEncodeTools.ProtobufDeserialize<HeartBeat>(msg.body);
                    }
                    else
                    {
                        NetMgr.Instance.OnDataReceived(msg);
                    }

                }
            }
            m_SocketStateDic[m_CurrentState].OnUpdate();
        }

        #region ReceiveMassage 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public void ReceiveMessage()
        {
            m_receiveThread = new Thread(ReceiveHandleThread);
            m_receiveThread.Start();
        }
        /// <summary>
        /// 接收数据工作线程
        /// </summary>
        void ReceiveHandleThread()
        {
            if (Connected == false)
            {
                return;
            }

            while (true)
            {
                if (m_tcpSocket == null || m_tcpSocket.Connected == false)
                {
                    break;
                }

                try
                {
                    int recv_len = 0;
                    if (m_receivedLength < RECEIVE_DATA_LENGTH)
                    {
                        recv_len = m_tcpSocket.Receive(m_receiveBuffer, m_receivedLength, RECEIVE_DATA_LENGTH - m_receivedLength, SocketFlags.None);
                    }
                    else
                    {
                        if (m_reveiveData == null)
                        {
                            int pkgSize;
                            int head_size;
                            SocketNetPackageTools.ReadHeadr(m_receiveBuffer, m_receivedLength, out pkgSize, out head_size);
                            m_receiveDataLength = pkgSize;
                            m_reveiveData = new byte[pkgSize];
                            Array.Copy(m_receiveBuffer, 0, m_reveiveData, 0, m_receivedLength);
                        }
                        recv_len = m_tcpSocket.Receive(m_reveiveData, m_receiveDataLength - m_receivedLength, SocketFlags.None);
                    }

                    if (recv_len > 0)
                    {
                        m_receivedLength += recv_len;
                        OnReceiveData();
                    }
                }
                catch (System.Exception e)
                {
                    Log.Print(e.ToString());
                    ChangeState(SocketState.Close);
                    break;
                }
            }
        }
        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        void OnReceiveData()
        {
            byte[] pkgData = (m_reveiveData != null) ? m_reveiveData : m_receiveBuffer;
            while (m_receivedLength > 0)
            {
                int pkgSize = 0;
                int headSize = 0;

                if (!SocketNetPackageTools.ReadHeadr(pkgData, m_receivedLength, out pkgSize, out headSize))
                {
                    break;
                }

                if (m_receivedLength < pkgSize)
                {
                    break;
                }

                int raw_data_start = headSize;
                int raw_data_len = pkgSize - headSize;

                OnAddReceiveQueue(pkgData, raw_data_start, raw_data_len);

                if (m_receivedLength > pkgSize)
                {
                    m_receiveBuffer = new byte[RECEIVE_DATA_LENGTH];
                    Array.Copy(pkgData, pkgSize, m_receiveBuffer, 0, m_receivedLength - pkgSize);
                    pkgData = m_receiveBuffer;
                }

                m_receivedLength -= pkgSize;

                if (m_receivedLength == 0 && m_reveiveData != null)
                {
                    m_reveiveData = null;
                    m_receiveDataLength = 0;
                }
            }
        }
        /// <summary>
        /// 将接收到的数据添加到接收队列
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="data_len"></param>
        void OnAddReceiveQueue(byte[] data, int start, int data_len)
        {
            CmdMsg msg = SocketNetPackageTools.ParsePackage(data, start, data_len);
            if (msg != null)
            {
                lock (m_receiveQueue)
                {
                    m_receiveQueue.Enqueue(msg);
                }
            }
        }
        #endregion

        #region SendMsg 发送信息
        /// <summary>
        /// 真正的发送信息
        /// </summary>
        /// <param name="data"></param>
        private void SendMsg(byte[] data)
        {
            if (m_tcpSocket == null) return;

            m_tcpSocket.BeginSend(data, 0, data.Length, SocketFlags.None, OnSendCall, m_tcpSocket);

        }
        private void OnSendCall(IAsyncResult iar)
        {
            try
            {
                Socket client = (Socket)iar.AsyncState;
                client.EndSend(iar);
            }
            catch (System.Exception e)
            {
                Log.PrintError(e.ToString());
                ChangeState(SocketState.Close);
            }
        }
        #endregion
        #region OnCheckSendMassageQueueCallBack 检查队列回调
        /// <summary>
        /// 检查队列回调
        /// </summary>
        private void OnCheckSendMassageQueueCallBack()
        {
            lock (m_QueueSendData)
            {
                if (m_QueueSendData.Count > 0)
                {
                    lock (m_QueueSendData)
                    {
                        SendMsg(m_QueueSendData.Dequeue());
                    }
                }
            }
        }
        #endregion
        #region Send 客户端调用发送（加入到发送队列）
        /// <summary>
        /// 客户端调用发送（加入到发送队列）
        /// </summary>
        /// <param name="data"></param>
        public void Send(MsgCmd ctype, IMessage body)
        {
            if (m_tcpSocket == null || !m_tcpSocket.Connected)
            {
                Log.Print("tcp==null");
                return;
            }

            if (ctype != MsgCmd.HeartBeat)
            {
                Log.Print($"MsgCmd:  {ctype}");
            }

            byte[] pSendMassage = MakeDatas((ushort)ctype, body);
            if (pSendMassage == null)
            {
                Log.PrintError("发送的协议包是空的");
                return;
            }
            lock (m_QueueSendData)
            {
                m_QueueSendData.Enqueue(pSendMassage);
            }
            OnCheckSendMassageQueueCallBack();
        }
        #endregion

        byte[] MakeDatas(ushort ctype, IMessage body)
        {
            //stype 目前仅仅是一个占位符，不表示实际的服务器模块
            byte[] sendData = SocketNetPackageTools.MakePackage(1, ctype, body);
            if (sendData == null) return null;
            byte[] tcp_pkg = SocketNetPackageTools.MakePackage(sendData);
            return tcp_pkg;
        }


        #region Close 关闭套接字
        /// <summary>
        /// 关闭套接字
        /// </summary>
        public void Close(bool isActive = true)
        {
            m_isActiveClose = isActive;
            ChangeState(SocketState.Close);
        }
        #endregion
        #region ClearMessageQueue 清空消息队列
        /// <summary>
        /// 清空消息队列
        /// </summary>
        public void ClearMessageQueue()
        {
            m_QueueSendData.Clear();
            m_receiveQueue.Clear();
        }
        #endregion

        public void ChangeState(SocketState state)
        {
            if (m_CurrentState == state) return;

            if (m_SocketStateDic.ContainsKey(state))
            {
                if (m_CurrentState != SocketState.Idle)
                {
                    m_SocketStateDic[m_CurrentState].OnExit();
                }
                Log.Print("网络状态切换到" + state.ToString());
                m_CurrentState = state;

                m_SocketStateDic[m_CurrentState].OnEnter();
            }
        }
    }
}

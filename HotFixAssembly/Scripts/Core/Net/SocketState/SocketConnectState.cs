using JEngine.Event;
using System;
using System.Net.Sockets;
using JEngine.Core;
using UnityEngine;

namespace _26Key
{
    /**
	 * 连接状态
	 */
    public class SocketConnectState : SocketStateBase
    {

        private bool m_isConnectSuccess = false;

        private bool m_isConnectComplete = false;
        public SocketConnectState(SocketClient socketClient) : base(socketClient)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            Log.Print("进入到连接状态");
            m_isConnectSuccess = false;
            m_isConnectComplete = false;
            m_SocketClient.CurrentNetType = Application.internetReachability;

            m_SocketClient.ClearMessageQueue();
            BeginConnect();
        }
        public override void OnUpdate()
        {
            if (m_isConnectComplete)
            {
                if (m_isConnectSuccess)
                {
                    Log.Print("切换到通信");
                    m_SocketClient.ReceiveMessage();
                    ChangeState(SocketClient.SocketState.Communicate);
                }

                m_isConnectComplete = false;
            }
        }

        #region BeginConnect 异步连接
        /// <summary>
        /// 异步连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="onConnectedCallBack"></param>
        public void BeginConnect()
        {
            if (m_SocketClient.m_tcpSocket != null && m_SocketClient.m_tcpSocket.Connected)
            {
                Log.PrintError("重复连接？？？？？？？？？");
                m_SocketClient.Close(false);
                return;
            }

            string ip = m_SocketClient.CurrentIP;

            m_SocketClient.m_tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            try
            {
                Log.Print(string.Format("连接{0}:{1}", ip, m_SocketClient.CurrentPort.ToString()));
                m_SocketClient.m_tcpSocket.BeginConnect(ip, m_SocketClient.CurrentPort, ConnectCallBack, m_SocketClient.m_tcpSocket);
            }
            catch (Exception e)
            {
                Log.PrintError(e);
                m_isConnectComplete = true;
                m_isConnectSuccess = false;
            }
        }
        #endregion

        #region ConnectCallBack 连接回调
        /// <summary>
        /// 连接回调
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallBack(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                client.EndConnect(ar);
                m_SocketClient.ClearMessageQueue();
                Log.Print(string.Format("连接成功"));
                m_isConnectSuccess = true;
            }
            catch (Exception e)
            {
                m_isConnectSuccess = false;
                Log.PrintError("Socket连接失败!" + e.Message);
                ChangeState(SocketClient.SocketState.Close);
            }
            finally
            {
                m_isConnectComplete = true;
            }
        }
        #endregion

    }
}

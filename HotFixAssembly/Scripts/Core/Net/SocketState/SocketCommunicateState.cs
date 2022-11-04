
//using ProtoData;

using JEngine.Core;
using M26Key.Protobuf;
using UnityEngine;

namespace _26Key
{
    /**
	 * 通讯状态
	 */
    public class SocketCommunicateState : SocketStateBase
    {
        /// <summary>
        /// 上一次发送心跳包时间
        /// </summary>
        private float m_PrevSendHeartTime = 0.0f;
        /// <summary>
        /// 发送心跳间隔
        /// </summary>
        private const float SEND_HEART_BEAT_SPACE = 15f;
        /// <summary>
        /// 上一次接收心跳包时间
        /// </summary>
        private float m_PrevReceiveHeartTime = 0.0f;
        /// <summary>
        /// 心跳超时时间
        /// </summary>
        private const float HEART_BEAT_OVER_TIME = 30f;
        public SocketCommunicateState(SocketClient socketClient) : base(socketClient)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();
            EventMgr.Post(EventType.AuthResetLogin);
            m_SocketClient.m_lastHeart = null;
            m_PrevSendHeartTime = 0f;
            m_PrevReceiveHeartTime = Time.realtimeSinceStartup;
        }

        public override void OnUpdate()
        {

            if (Time.realtimeSinceStartup - m_PrevSendHeartTime > SEND_HEART_BEAT_SPACE)
            {
                m_PrevSendHeartTime = Time.realtimeSinceStartup;
                ClientSendHeart();
            }

            if (Time.realtimeSinceStartup - m_PrevReceiveHeartTime > HEART_BEAT_OVER_TIME)
            {
                Log.PrintWarning("心跳超时，断开连接");
                ChangeState(SocketClient.SocketState.Reconnect);
            }

            if (m_SocketClient.m_lastHeart != null)
            {
                long sendTime = m_SocketClient.m_lastHeart.CliTime;
                long serverTime = m_SocketClient.m_lastHeart.SvrTime;
                m_PrevReceiveHeartTime = Time.realtimeSinceStartup;
                long localTime = TimeUtil.GetTimestampMS();
                long fps = (localTime - sendTime) / 2;
                //Log.Print("FPS:" + fps.ToString() + "毫秒");
                m_SocketClient.m_lastHeart = null;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            m_SocketClient.m_lastHeart = null;
        }

        #region ClientSendHeart 客户端发送心跳包
        /// <summary>
        /// 客户端发送心跳包
        /// </summary>
        private void ClientSendHeart()
        {
            HeartBeat proto = new HeartBeat();
            proto.CliTime = TimeUtil.GetTimestampMS();
            m_SocketClient.Send(MsgCmd.HeartBeat, proto);
        }
        #endregion
    }
}

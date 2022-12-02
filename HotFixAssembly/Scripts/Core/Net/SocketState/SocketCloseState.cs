using JEngine.Core;
using JEngine.Event;
using JEngine.UI.UIKit;

namespace _26Key
{
    /**
	 * 关闭状态
	 */
    public class SocketCloseState : SocketStateBase
    {
        public SocketCloseState(SocketClient socketClient) : base(socketClient)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();

            if (m_SocketClient.m_receiveThread != null)
            {
                m_SocketClient.m_receiveThread.Interrupt();
                //m_SocketClient.m_receiveThread.Abort();
                m_SocketClient.m_receiveThread = null;
            }
            if (m_SocketClient != null && m_SocketClient.m_tcpSocket != null)
            {
                m_SocketClient.m_tcpSocket.Close();
                m_SocketClient.m_tcpSocket = null;
                m_SocketClient.ClearMessageQueue();
            }
            //如果之前有账户数据，那么直接显示连接网络窗口，手动连接后自动登录
            EventMgr.Post(EventType.ShowConnectNet);
            ChangeState(SocketClient.SocketState.Idle);
        }


        public override void OnExit()
        {
            base.OnExit();
        }

    }
}

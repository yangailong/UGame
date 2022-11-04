namespace _26Key
{
	/**
	 * 断线重连
	 */
	public class SocketReconnectState : SocketStateBase
	{
		public SocketReconnectState(SocketClient socketClient) : base(socketClient)
		{
		}


		public override void OnEnter()
		{
			base.OnEnter();
			if (m_SocketClient.m_receiveThread != null)
			{
				m_SocketClient.m_receiveThread.Interrupt();
				m_SocketClient.m_receiveThread.Abort();
				m_SocketClient.m_receiveThread = null;
			}
			if (m_SocketClient != null && m_SocketClient.m_tcpSocket != null)
			{
				m_SocketClient.m_tcpSocket.Close();
				m_SocketClient.m_tcpSocket = null;
				m_SocketClient.ClearMessageQueue();
			}
            ChangeState(SocketClient.SocketState.Connect);
		}
	}
}

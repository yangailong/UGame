namespace UGame_Remove
{
    /// <summary>网络错误</summary>
    public enum NetError
    {
        /// <summary>未知错误</summary>
        Unknown = 0,

        /// <summary>地址错误</summary>
        AddressError,

        /// <summary>Socket错误</summary>
        SocketError,

        /// <summary>连接错误</summary>
        ConnectError,

        /// <summary>发送错误</summary>
        SendError,

        /// <summary>接收错误</summary>
        ReceiveError,

        /// <summary>序列化错误</summary>
        SerializeError,

        /// <summary>反序列化消息包头错误</summary>
        DeserializePacketHeaderError,

        /// <summary>反序列化消息包错误</summary>
        DeserializePacketError,
    }
}

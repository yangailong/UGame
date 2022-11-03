
namespace _26Key
{
	public class CmdMsg
	{
		/// <summary>
		/// 服务号
		/// </summary>
		public ushort sType;
		/// <summary>
		/// 命令号·消息id
		/// </summary>
		public ushort id;

		public long utag;
		/// <summary>
		/// 消息包
		/// </summary>
		public byte[] body; // protobuf, utf8 string json byte;
	}


}

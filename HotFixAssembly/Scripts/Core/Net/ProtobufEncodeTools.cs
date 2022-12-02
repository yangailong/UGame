
using System.IO;
using Google.Protobuf;

namespace _26Key
{
    public class ProtobufEncodeTools
    {
		// 2 stype, 2 ctype, 4utag, msg--> body;
		private const int HEADER_SIZE = 8;

        /// <summary>
        /// Protobuf序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ProtobufSerializer(IMessage data)
        {
            using (MemoryStream rawOutput = new MemoryStream())
            {
                data.WriteTo(rawOutput);
                return rawOutput.ToArray();
            }
        }
        //public static byte[] ProtobufSerializer(object data)
        //{
        //	using (MemoryStream m = new MemoryStream())
        //	{
        //		byte[] buffer = null;
        //             // Serializer.Serialize(m, data);
        //		m.Position = 0;
        //		int length = (int)m.Length;
        //		buffer = new byte[length];
        //		m.Read(buffer, 0, length);
        //		return buffer;
        //	}
        //}
        /// <summary>
        /// Protobuf反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_data"></param>
        /// <returns></returns>
        public static T ProtobufDeserialize<T>(byte[] dataBytes) where T : IMessage, new()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(dataBytes, 0, dataBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                MessageParser<T> parser = new MessageParser<T>(() => new T());
                return parser.ParseFrom(ms);
            }
        }
        //public static T ProtobufDeserialize<T>(byte[] _data)
        //{
        //    using (MemoryStream m = new MemoryStream(_data))
        //    {
        //        return ProtoBuf.Serializer.Deserialize<T>(m);
        //    }
        //}
    }
}

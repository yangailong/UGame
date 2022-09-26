using JEngine.Core;
using System;
using Google.Protobuf;
using UnityEngine;

namespace _26Key
{
    public class SocketNetPackageTools
    {
        private const int LENGTH_HEADER_SIZE = 2;
        private const int HEADER_SIZE = 12;
        /// <summary>
        /// 制作协议包
        /// </summary>
        /// <param name="stype"></param>
        /// <param name="ctype"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] MakePackage(ushort stype, ushort ctype, IMessage msg)
        {
            int cmd_len = HEADER_SIZE;
            byte[] cmd_body = null;
            if (msg != null)
            {
                cmd_body = ProtobufEncodeTools.ProtobufSerializer(msg);
                cmd_len += cmd_body.Length;
            }
            byte[] cmd = new byte[cmd_len];
            // stype, ctype, utag(8保留), cmd_body
            WriteUShort(cmd, 0, stype);
            WriteUShort(cmd, 2, ctype);
            WriteULong(cmd, 4, 0L);
            //Debug.Log($"cmd:{ctype}   module:{stype}  bodyLen:{cmd_body.Length}");
            if (cmd_body != null)
            {
                WriteBytes(cmd, HEADER_SIZE, cmd_body);
            }
            return cmd;
        }
        /// <summary>
        /// 解析协议包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="cmd_len"></param>
        /// <returns></returns>
        public static CmdMsg ParsePackage(byte[] data, int start, int cmd_len)
        {
            CmdMsg msg = new CmdMsg();
            msg.sType = ReadUShort(data, start);
            msg.id = ReadUShort(data, start + 2);
           // msg.utag = ReadULong(data, start + 12);

            int body_len = cmd_len - HEADER_SIZE;
            msg.body = new byte[body_len];
            Array.Copy(data, start + HEADER_SIZE, msg.body, 0, body_len);
            return msg;
        }








        /// <summary>
        /// 组合消息长度和数据
        /// </summary>
        /// <param name="cmd_data"></param>
        /// <returns></returns>
        public static byte[] MakePackage(byte[] cmd_data)
        {
            int len = cmd_data.Length;
            if (len > 65535 - 2)
            {
                return null;
            }

            int cmd_len = len + LENGTH_HEADER_SIZE;
            byte[] cmd = new byte[cmd_len];
            WriteUShort(cmd, 0, (ushort)cmd_len);
            WriteBytes(cmd, LENGTH_HEADER_SIZE, cmd_data);

            return cmd;
        }

        public static bool ReadHeadr(byte[] data, int data_len, out int pkg_size, out int head_size)
        {
            pkg_size = 0;
            head_size = 0;

            if (data_len < 2)
            {
                return false;
            }

            head_size = 2;
            pkg_size = (data[0] | (data[1] << 8));

            return true;
        }

        #region 操作字节数组

        static void WriteUShort(byte[] buf, int offset, ushort value)
        {
            byte[] byte_value = BitConverter.GetBytes(value);
            // 小尾，还是大尾？BitConvert 系统是小尾还是大尾;
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(byte_value);
            }

            Array.Copy(byte_value, 0, buf, offset, byte_value.Length);

        } 
        static void WriteULong(byte[] buf, int offset, ulong value)
        {
            byte[] byte_value = BitConverter.GetBytes(value);
            // 小尾，还是大尾？BitConvert 系统是小尾还是大尾;
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(byte_value);
            }

            Array.Copy(byte_value, 0, buf, offset, byte_value.Length);
        }
        static void WriteUInt(byte[] buf, int offset, uint value)
        {
            byte[] byte_value = BitConverter.GetBytes(value);
            // 小尾，还是大尾？BitConvert 系统是小尾还是大尾;
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(byte_value);
            }

            Array.Copy(byte_value, 0, buf, offset, byte_value.Length);
        }

        static void WriteBytes(byte[] dst, int offset, byte[] value)
        {
            Array.Copy(value, 0, dst, offset, value.Length);
        }

        static ushort ReadUShort(byte[] data, int offset)
        {
            int ret = (data[offset] | (data[offset + 1] << 8));

            return (ushort)ret;
        } 
        static long ReadULong(byte[] data, int offset)
        {
            int ret = (data[offset] | (data[offset + 1] << 8));

            return (long)ret;
        }
        #endregion

    }
}


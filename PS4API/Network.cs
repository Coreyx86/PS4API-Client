using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace PS4API
{
    public class Network
    {
        public static Int16 ReadInt16FromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[2];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToInt16(tmp, 0);
        }
        public static Int32 ReadInt32FromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[4];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToInt32(tmp, 0);
        }
        public static Int64 ReadInt64FromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[8];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToInt64(tmp, 0);
        }
        public static UInt16 ReadUInt16FromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[2];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToUInt16(tmp, 0);
        }
        public static UInt32 ReadUInt32FromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[4];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToUInt32(tmp, 0);
        }
        public static UInt64 ReadUInt64FromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[8];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToUInt64(tmp, 0);
        }
        public static Single ReadFloatFromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[4];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToSingle(tmp, 0);
        }
        public static double ReadDoubleFromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[8];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToDouble(tmp, 0);
        }
        public static bool ReadBoolFromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[1];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return BitConverter.ToBoolean(tmp, 0);
        }
        public static string ReadStringFromServer(TcpClient tClient)
        {
            int StrLen = ReadInt32FromServer(tClient);
            Console.Write(StrLen.ToString());
            byte[] tmp = new byte[StrLen];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return System.Text.Encoding.ASCII.GetString(tmp);
        }
        public static byte ReadByteFromServer(TcpClient tClient)
        {
            byte[] tmp = new byte[1];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return tmp[0];
        }
        public static byte[] ReadBytesFromServer(TcpClient tClient)
        {
            int Len = ReadInt32FromServer(tClient);
            byte[] tmp = new byte[Len];
            while (tClient.Client.Available == 0) { }
            tClient.Client.Receive(tmp);
            return tmp;
        }

        public static void WriteIntToServer(TcpClient tClient, int msg)
        {
            byte[] tmp = BitConverter.GetBytes(msg);
            tClient.Client.Send(tmp);
        }

        public static void WriteBytesToServer(TcpClient tClient, byte[] msg)
        {
            tClient.Client.Send(msg);
        }
    }
}

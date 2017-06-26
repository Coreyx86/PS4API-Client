using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace PS4API
{
    public class PS4API
    {
        public PS4API()
        {
         }
        public static bool isConnected = false;
        public static TcpClient PS4APIClient;
        public static Int32 PID;


        public static void ConnectTarget(string ipAddress)
        {
            if (!isConnected)
            {
                try
                {
                    PS4APIClient = new TcpClient(ipAddress, 9023);
                    isConnected = true;

                }
                catch { Console.Write("[PS4API] - Failed to connect to client...\n"); }
            }
            else Console.Write("[PS4API] - Client is already connected...\n");
        }
        public static void DisconnectTarget()
        {
            if (isConnected)
            {
                try
                {
                    PS4APIClient.Client.Close();
                    PS4APIClient.Close();
                    isConnected = false;
                }
                catch (Exception ex) { Console.Write(ex.ToString()); }
            }
            else Console.Write("[PS4API] - Client was not connected...");
        }

        public static void AttachProcess(string targetProcess)
        {
            if (isConnected)
                PID = Commands.PS4API_GetPID(PS4APIClient, targetProcess);
            else Console.Write("[PS4API] - Not Connected to Client\n");
        }

        public static byte[] ReadMemory(uint address, int readLen)
        {
            if (isConnected)
            {
                return Commands.PS4API_ReadMemory(PS4APIClient, PID, address, readLen);
            }
            else return null;
        }
        public static void SetMemory(uint address, byte[] memory)
        {
            if (isConnected)
            {
                Commands.PS4API_WriteMemory(PS4APIClient, PID, address, memory);
            }
            else Console.Write("[PS4API] - Client must be connected before writing memory\n");
        }


        public static class Extension
        {
            public static bool ReadBool(uint address)
            {
                byte[] tmp = ReadMemory(address, 1);
                return tmp[0] != 0;
            }

            public static short ReadInt16(uint address)
            {
                byte[] tmp = ReadMemory(address, 2);
                return BitConverter.ToInt16(tmp, 0);
            }
            public static int ReadInt32(uint address)
            {
                byte[] tmp = ReadMemory(address, 4);
                return BitConverter.ToInt32(tmp, 0);
            }
            public static long ReadInt64(uint address)
            {
                byte[] tmp = ReadMemory(address, 8);
                return BitConverter.ToInt64(tmp, 0);
            }
            public static ushort ReadUInt16(uint address)
            {
                byte[] tmp = ReadMemory(address, 2);
                return BitConverter.ToUInt16(tmp, 0);
            }
            public static uint ReadUInt32(uint address)
            {
                byte[] tmp = ReadMemory(address, 4);
                return BitConverter.ToUInt32(tmp, 0);
            }
            public static ulong ReadUInt64(uint address)
            {
                byte[] tmp = ReadMemory(address, 8);
                return BitConverter.ToUInt64(tmp, 0);
            }
            public static byte ReadByte(uint address)
            {
                byte[] tmp = ReadMemory(address, 1);
                return tmp[0];
            }
            //This is a redudant function only used to help people migrate projects from PS3API to PS4API a little easier
            public static byte[] ReadBytes(uint address, int len)
            {
                return ReadMemory(address, len);
            }
            public static float ReadFloat(uint address)
            {
                byte[] tmp = ReadMemory(address, 4);
                return BitConverter.ToSingle(tmp, 0);
            }
            public static double ReadDouble(uint address)
            {
                byte[] tmp = ReadMemory(address, 8);
                return BitConverter.ToDouble(tmp, 0);
            }

            //Credits to FM|T for this function, straight outta PS3API
            public static string ReadString(uint address)
            {
                int block = 40;
                int addOffset = 0;
                string str = "";
                repeat:
                byte[] buffer = ReadBytes(address + (uint)addOffset, block);
                str += Encoding.UTF8.GetString(buffer);
                addOffset += block;
                if (str.Contains('\0'))
                {
                    int index = str.IndexOf('\0');
                    string final = str.Substring(0, index);
                    str = String.Empty;
                    return final;
                }
                else
                    goto repeat;
            }

            public static void WriteBool(uint address, bool mem)
            {
                byte[] buff = new byte[1];
                buff[0] = mem ? (byte)1 : (byte)0;
                SetMemory(address, buff);
            }
            public static void WriteInt16(uint address, short mem)
            {
                byte[] buff = BitConverter.GetBytes(mem);
                SetMemory(address, buff);
            }
            public static void WriteInt32(uint address, int mem)
            {
                byte[] buff = BitConverter.GetBytes(mem);
                SetMemory(address, buff);
            }
            public static void WriteInt64(uint address, long mem)
            {
                byte[] buff = BitConverter.GetBytes(mem);
                SetMemory(address, buff);
            }
            public static void WriteUInt16(uint address, ushort mem)
            {
                byte[] buff = BitConverter.GetBytes(mem);
                SetMemory(address, buff);
            }
            public static void WriteUInt32(uint address, uint mem)
            {
                byte[] buff = BitConverter.GetBytes(mem);
                SetMemory(address, buff);
            }
            public static void WriteUInt64(uint address, ulong mem)
            {
                byte[] buff = BitConverter.GetBytes(mem);
                SetMemory(address, buff);
            }
            public static void WriteByte(uint address, byte mem)
            {
                byte[] buff = new byte[] { mem };
                SetMemory(address, buff);
            }
            //Redudant function for transitioning
            public static void WriteBytes(uint address, byte[] mem)
            {
                SetMemory(address, mem);
            }
            public static void WriteFloat(uint address, float mem)
            {
                byte[] buff = BitConverter.GetBytes(mem);
                SetMemory(address, buff);
            }
            public static void WriteDouble(uint address, double mem)
            {
                byte[] buff = BitConverter.GetBytes(mem);
                SetMemory(address, buff);
            }
            public static void WriteString(uint address, string mem)
            {
                byte[] buff = Encoding.UTF8.GetBytes(mem);
                Array.Resize(ref buff, buff.Length + 1);
                SetMemory(address, buff);
            }
        }
    }
}

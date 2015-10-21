using System;
using Grpc.Core;
using rpc = HookKeylogger.ServerProxy.Rpc;

namespace ServerProxy
{
    /// <summary>
    /// This program is a proxy to the actual server, it handels caching the
    /// data to be sent, choosing the best channel to send the data over and
    /// when to send the data.
    /// </summary>
    class Proxy
    {
        static void Main(string[] args)
        {
            int port = 4567;
            Server server = new Server
            {
                Services = { rpc.ServerProxy.BindService(new rpc.ServerProxyImpl()) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };

            server.Start();
            Console.WriteLine("ServerProxy listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync();
        }
    }
}

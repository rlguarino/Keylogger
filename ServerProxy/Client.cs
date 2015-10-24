using Grpc.Core;
using System;

namespace HookKeylogger.UploadProxy
{
    public class UploadProxyClient 
    {
        readonly Channel channel;
        readonly Rpc.UploadProxy.IUploadProxyClient client;

        public UploadProxyClient() : this("127.0.0.1", 4567) { }
        public UploadProxyClient(string addr, int port)
        {
            this.channel = new Channel(addr, port, Credentials.Insecure);
            this.client = Rpc.UploadProxy.NewClient(channel);
        }

        public void SendCi(AggergationServer.Types.CI ci)
        {
            Rpc.SendRequest sr = new Rpc.SendRequest();
            sr.Ci = ci;
            try
            {
                Rpc.SendResponse resp = this.client.SendCi(sr);
                if (resp.Status == Rpc.SendResponse.Types.Status.ERROR){
                    Console.WriteLine("Error sending to proxy");
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed" + e);
                throw;
            }
        }

        public void close()
        {
            this.channel.ShutdownAsync().Wait();
        }
    }
}

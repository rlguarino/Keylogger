using System;
using System.Threading.Tasks;
using Grpc.Core;
using rpc = HookKeylogger.ServerProxy.Rpc;

namespace HookKeylogger.ServerProxy.Rpc
{
    /// <summary>
    /// Implementation of the ServerProxy rpc listener library
    /// </summary>
    public class ServerProxyImpl : rpc.ServerProxy.IServerProxy
    {
        public Task<rpc.SendResponse> SendCi(rpc.SendRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}

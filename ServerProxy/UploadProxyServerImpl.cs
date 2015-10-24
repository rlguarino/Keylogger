using System;
using System.Threading.Tasks;
using Grpc.Core;
using rpc = HookKeylogger.UploadProxy.Rpc;

namespace HookKeylogger.UploadProxy.Rpc
{
    /// <summary>
    /// Implementation of the ServerProxy rpc listener library
    /// </summary>
    public class UploadProxyImpl : rpc.UploadProxy.IUploadProxy
    {
        public Task<rpc.SendResponse> SendCi(rpc.SendRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}

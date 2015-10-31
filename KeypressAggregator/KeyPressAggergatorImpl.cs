using Grpc.Core;
using HookKeylogger.Base;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace KeypressAggregator
{
    class KeyPressAggergatorImpl : HookKeylogger.Base.KerPressAggergator.IKerPressAggergator
    {
        ConcurrentQueue<KeyPress> q;

        public KeyPressAggergatorImpl(ConcurrentQueue<KeyPress> q)
        {
            this.q = q;
        }

        public Task<PutKeyPressResponse> PutKeyPress(KeyPress request, ServerCallContext context)
        {
            this.q.Enqueue(request);
            return Task.FromResult(new PutKeyPressResponse());
        }
    }
}

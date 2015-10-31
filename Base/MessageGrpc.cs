// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Base/message.proto
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace HookKeylogger.Base {
  public static class KerPressAggergator
  {
    static readonly string __ServiceName = "base.KerPressAggergator";

    static readonly Marshaller<global::HookKeylogger.Base.KeyPress> __Marshaller_KeyPress = Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::HookKeylogger.Base.KeyPress.Parser.ParseFrom);
    static readonly Marshaller<global::HookKeylogger.Base.PutKeyPressResponse> __Marshaller_PutKeyPressResponse = Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::HookKeylogger.Base.PutKeyPressResponse.Parser.ParseFrom);

    static readonly Method<global::HookKeylogger.Base.KeyPress, global::HookKeylogger.Base.PutKeyPressResponse> __Method_PutKeyPress = new Method<global::HookKeylogger.Base.KeyPress, global::HookKeylogger.Base.PutKeyPressResponse>(
        MethodType.Unary,
        __ServiceName,
        "PutKeyPress",
        __Marshaller_KeyPress,
        __Marshaller_PutKeyPressResponse);

    // service descriptor
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::HookKeylogger.Base.Message.Descriptor.Services[0]; }
    }

    // client interface
    public interface IKerPressAggergatorClient
    {
      global::HookKeylogger.Base.PutKeyPressResponse PutKeyPress(global::HookKeylogger.Base.KeyPress request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));
      global::HookKeylogger.Base.PutKeyPressResponse PutKeyPress(global::HookKeylogger.Base.KeyPress request, CallOptions options);
      AsyncUnaryCall<global::HookKeylogger.Base.PutKeyPressResponse> PutKeyPressAsync(global::HookKeylogger.Base.KeyPress request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken));
      AsyncUnaryCall<global::HookKeylogger.Base.PutKeyPressResponse> PutKeyPressAsync(global::HookKeylogger.Base.KeyPress request, CallOptions options);
    }

    // server-side interface
    public interface IKerPressAggergator
    {
      Task<global::HookKeylogger.Base.PutKeyPressResponse> PutKeyPress(global::HookKeylogger.Base.KeyPress request, ServerCallContext context);
    }

    // client stub
    public class KerPressAggergatorClient : ClientBase, IKerPressAggergatorClient
    {
      public KerPressAggergatorClient(Channel channel) : base(channel)
      {
      }
      public global::HookKeylogger.Base.PutKeyPressResponse PutKeyPress(global::HookKeylogger.Base.KeyPress request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        var call = CreateCall(__Method_PutKeyPress, new CallOptions(headers, deadline, cancellationToken));
        return Calls.BlockingUnaryCall(call, request);
      }
      public global::HookKeylogger.Base.PutKeyPressResponse PutKeyPress(global::HookKeylogger.Base.KeyPress request, CallOptions options)
      {
        var call = CreateCall(__Method_PutKeyPress, options);
        return Calls.BlockingUnaryCall(call, request);
      }
      public AsyncUnaryCall<global::HookKeylogger.Base.PutKeyPressResponse> PutKeyPressAsync(global::HookKeylogger.Base.KeyPress request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        var call = CreateCall(__Method_PutKeyPress, new CallOptions(headers, deadline, cancellationToken));
        return Calls.AsyncUnaryCall(call, request);
      }
      public AsyncUnaryCall<global::HookKeylogger.Base.PutKeyPressResponse> PutKeyPressAsync(global::HookKeylogger.Base.KeyPress request, CallOptions options)
      {
        var call = CreateCall(__Method_PutKeyPress, options);
        return Calls.AsyncUnaryCall(call, request);
      }
    }

    // creates service definition that can be registered with a server
    public static ServerServiceDefinition BindService(IKerPressAggergator serviceImpl)
    {
      return ServerServiceDefinition.CreateBuilder(__ServiceName)
          .AddMethod(__Method_PutKeyPress, serviceImpl.PutKeyPress).Build();
    }

    // creates a new client
    public static KerPressAggergatorClient NewClient(Channel channel)
    {
      return new KerPressAggergatorClient(channel);
    }

  }
}
#endregion

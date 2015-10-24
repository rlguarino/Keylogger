// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Server/messages.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace HookKeylogger.AggergationServer.Types {

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public static partial class Messages {

    #region Descriptor
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static Messages() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVTZXJ2ZXIvbWVzc2FnZXMucHJvdG8SBXR5cGVzIiAKAkNJEgwKBHR5cGUY", 
            "ASABKAkSDAoEZGF0YRgCIAEoCUIoqgIlSG9va0tleWxvZ2dlci5BZ2dlcmdh", 
            "dGlvblNlcnZlci5UeXBlc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.InternalBuildGeneratedFileFrom(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedCodeInfo(null, new pbr::GeneratedCodeInfo[] {
            new pbr::GeneratedCodeInfo(typeof(global::HookKeylogger.AggergationServer.Types.CI), new[]{ "Type", "Data" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class CI : pb::IMessage<CI> {
    private static readonly pb::MessageParser<CI> _parser = new pb::MessageParser<CI>(() => new CI());
    public static pb::MessageParser<CI> Parser { get { return _parser; } }

    public static pbr::MessageDescriptor Descriptor {
      get { return global::HookKeylogger.AggergationServer.Types.Messages.Descriptor.MessageTypes[0]; }
    }

    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    public CI() {
      OnConstruction();
    }

    partial void OnConstruction();

    public CI(CI other) : this() {
      type_ = other.type_;
      data_ = other.data_;
    }

    public CI Clone() {
      return new CI(this);
    }

    public const int TypeFieldNumber = 1;
    private string type_ = "";
    public string Type {
      get { return type_; }
      set {
        type_ = pb::Preconditions.CheckNotNull(value, "value");
      }
    }

    public const int DataFieldNumber = 2;
    private string data_ = "";
    public string Data {
      get { return data_; }
      set {
        data_ = pb::Preconditions.CheckNotNull(value, "value");
      }
    }

    public override bool Equals(object other) {
      return Equals(other as CI);
    }

    public bool Equals(CI other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Type != other.Type) return false;
      if (Data != other.Data) return false;
      return true;
    }

    public override int GetHashCode() {
      int hash = 1;
      if (Type.Length != 0) hash ^= Type.GetHashCode();
      if (Data.Length != 0) hash ^= Data.GetHashCode();
      return hash;
    }

    public override string ToString() {
      return pb::JsonFormatter.Default.Format(this);
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (Type.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Type);
      }
      if (Data.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Data);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (Type.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Type);
      }
      if (Data.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Data);
      }
      return size;
    }

    public void MergeFrom(CI other) {
      if (other == null) {
        return;
      }
      if (other.Type.Length != 0) {
        Type = other.Type;
      }
      if (other.Data.Length != 0) {
        Data = other.Data;
      }
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            Type = input.ReadString();
            break;
          }
          case 18: {
            Data = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
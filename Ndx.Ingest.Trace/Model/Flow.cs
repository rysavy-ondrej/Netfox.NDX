// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Flow.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ndx.Model {

  /// <summary>Holder for reflection information generated from Flow.proto</summary>
  public static partial class FlowReflection {

    #region Descriptor
    /// <summary>File descriptor for Flow.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static FlowReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgpGbG93LnByb3RvEgluZHgubW9kZWwiOgoGSXBGbG93EhUKDVNvdXJjZUFk",
            "ZHJlc3MYASABKAwSGQoRRGVzdGluYXRpb25BZHJlc3MYAiABKAwiWQoHVGNw",
            "RmxvdxIhCgZJcGZsb3cYASABKAsyES5uZHgubW9kZWwuSXBGbG93EhIKClNv",
            "dXJjZVBvcnQYAiABKAUSFwoPRGVzdGluYXRpb25Qb3J0GAMgASgFIlkKB1Vk",
            "cEZsb3cSIQoGSXBmbG93GAEgASgLMhEubmR4Lm1vZGVsLklwRmxvdxISCgpT",
            "b3VyY2VQb3J0GAIgASgFEhcKD0Rlc3RpbmF0aW9uUG9ydBgDIAEoBSo5Cg5J",
            "cFByb3RvY29sVHlwZRIKCgZIb3BvcHQQABIICgRJY21wEAESCAoESWdtcBAC",
            "EgcKA0dHUBADYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Ndx.Model.IpProtocolType), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.IpFlow), global::Ndx.Model.IpFlow.Parser, new[]{ "SourceAddress", "DestinationAdress" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.TcpFlow), global::Ndx.Model.TcpFlow.Parser, new[]{ "Ipflow", "SourcePort", "DestinationPort" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.UdpFlow), global::Ndx.Model.UdpFlow.Parser, new[]{ "Ipflow", "SourcePort", "DestinationPort" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum IpProtocolType {
    [pbr::OriginalName("Hopopt")] Hopopt = 0,
    [pbr::OriginalName("Icmp")] Icmp = 1,
    [pbr::OriginalName("Igmp")] Igmp = 2,
    [pbr::OriginalName("GGP")] Ggp = 3,
  }

  #endregion

  #region Messages
  public sealed partial class IpFlow : pb::IMessage<IpFlow> {
    private static readonly pb::MessageParser<IpFlow> _parser = new pb::MessageParser<IpFlow>(() => new IpFlow());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<IpFlow> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ndx.Model.FlowReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IpFlow() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IpFlow(IpFlow other) : this() {
      sourceAddress_ = other.sourceAddress_;
      destinationAdress_ = other.destinationAdress_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IpFlow Clone() {
      return new IpFlow(this);
    }

    /// <summary>Field number for the "SourceAddress" field.</summary>
    public const int SourceAddressFieldNumber = 1;
    private pb::ByteString sourceAddress_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString SourceAddress {
      get { return sourceAddress_; }
      set {
        sourceAddress_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "DestinationAdress" field.</summary>
    public const int DestinationAdressFieldNumber = 2;
    private pb::ByteString destinationAdress_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString DestinationAdress {
      get { return destinationAdress_; }
      set {
        destinationAdress_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as IpFlow);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(IpFlow other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SourceAddress != other.SourceAddress) return false;
      if (DestinationAdress != other.DestinationAdress) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (SourceAddress.Length != 0) hash ^= SourceAddress.GetHashCode();
      if (DestinationAdress.Length != 0) hash ^= DestinationAdress.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (SourceAddress.Length != 0) {
        output.WriteRawTag(10);
        output.WriteBytes(SourceAddress);
      }
      if (DestinationAdress.Length != 0) {
        output.WriteRawTag(18);
        output.WriteBytes(DestinationAdress);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (SourceAddress.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(SourceAddress);
      }
      if (DestinationAdress.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(DestinationAdress);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(IpFlow other) {
      if (other == null) {
        return;
      }
      if (other.SourceAddress.Length != 0) {
        SourceAddress = other.SourceAddress;
      }
      if (other.DestinationAdress.Length != 0) {
        DestinationAdress = other.DestinationAdress;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            SourceAddress = input.ReadBytes();
            break;
          }
          case 18: {
            DestinationAdress = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  public sealed partial class TcpFlow : pb::IMessage<TcpFlow> {
    private static readonly pb::MessageParser<TcpFlow> _parser = new pb::MessageParser<TcpFlow>(() => new TcpFlow());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<TcpFlow> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ndx.Model.FlowReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TcpFlow() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TcpFlow(TcpFlow other) : this() {
      Ipflow = other.ipflow_ != null ? other.Ipflow.Clone() : null;
      sourcePort_ = other.sourcePort_;
      destinationPort_ = other.destinationPort_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TcpFlow Clone() {
      return new TcpFlow(this);
    }

    /// <summary>Field number for the "Ipflow" field.</summary>
    public const int IpflowFieldNumber = 1;
    private global::Ndx.Model.IpFlow ipflow_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.IpFlow Ipflow {
      get { return ipflow_; }
      set {
        ipflow_ = value;
      }
    }

    /// <summary>Field number for the "SourcePort" field.</summary>
    public const int SourcePortFieldNumber = 2;
    private int sourcePort_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int SourcePort {
      get { return sourcePort_; }
      set {
        sourcePort_ = value;
      }
    }

    /// <summary>Field number for the "DestinationPort" field.</summary>
    public const int DestinationPortFieldNumber = 3;
    private int destinationPort_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DestinationPort {
      get { return destinationPort_; }
      set {
        destinationPort_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as TcpFlow);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(TcpFlow other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Ipflow, other.Ipflow)) return false;
      if (SourcePort != other.SourcePort) return false;
      if (DestinationPort != other.DestinationPort) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ipflow_ != null) hash ^= Ipflow.GetHashCode();
      if (SourcePort != 0) hash ^= SourcePort.GetHashCode();
      if (DestinationPort != 0) hash ^= DestinationPort.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ipflow_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Ipflow);
      }
      if (SourcePort != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(SourcePort);
      }
      if (DestinationPort != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(DestinationPort);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ipflow_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Ipflow);
      }
      if (SourcePort != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SourcePort);
      }
      if (DestinationPort != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DestinationPort);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(TcpFlow other) {
      if (other == null) {
        return;
      }
      if (other.ipflow_ != null) {
        if (ipflow_ == null) {
          ipflow_ = new global::Ndx.Model.IpFlow();
        }
        Ipflow.MergeFrom(other.Ipflow);
      }
      if (other.SourcePort != 0) {
        SourcePort = other.SourcePort;
      }
      if (other.DestinationPort != 0) {
        DestinationPort = other.DestinationPort;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            if (ipflow_ == null) {
              ipflow_ = new global::Ndx.Model.IpFlow();
            }
            input.ReadMessage(ipflow_);
            break;
          }
          case 16: {
            SourcePort = input.ReadInt32();
            break;
          }
          case 24: {
            DestinationPort = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class UdpFlow : pb::IMessage<UdpFlow> {
    private static readonly pb::MessageParser<UdpFlow> _parser = new pb::MessageParser<UdpFlow>(() => new UdpFlow());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UdpFlow> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ndx.Model.FlowReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UdpFlow() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UdpFlow(UdpFlow other) : this() {
      Ipflow = other.ipflow_ != null ? other.Ipflow.Clone() : null;
      sourcePort_ = other.sourcePort_;
      destinationPort_ = other.destinationPort_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UdpFlow Clone() {
      return new UdpFlow(this);
    }

    /// <summary>Field number for the "Ipflow" field.</summary>
    public const int IpflowFieldNumber = 1;
    private global::Ndx.Model.IpFlow ipflow_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.IpFlow Ipflow {
      get { return ipflow_; }
      set {
        ipflow_ = value;
      }
    }

    /// <summary>Field number for the "SourcePort" field.</summary>
    public const int SourcePortFieldNumber = 2;
    private int sourcePort_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int SourcePort {
      get { return sourcePort_; }
      set {
        sourcePort_ = value;
      }
    }

    /// <summary>Field number for the "DestinationPort" field.</summary>
    public const int DestinationPortFieldNumber = 3;
    private int destinationPort_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DestinationPort {
      get { return destinationPort_; }
      set {
        destinationPort_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UdpFlow);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UdpFlow other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Ipflow, other.Ipflow)) return false;
      if (SourcePort != other.SourcePort) return false;
      if (DestinationPort != other.DestinationPort) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ipflow_ != null) hash ^= Ipflow.GetHashCode();
      if (SourcePort != 0) hash ^= SourcePort.GetHashCode();
      if (DestinationPort != 0) hash ^= DestinationPort.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ipflow_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Ipflow);
      }
      if (SourcePort != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(SourcePort);
      }
      if (DestinationPort != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(DestinationPort);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ipflow_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Ipflow);
      }
      if (SourcePort != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SourcePort);
      }
      if (DestinationPort != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DestinationPort);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UdpFlow other) {
      if (other == null) {
        return;
      }
      if (other.ipflow_ != null) {
        if (ipflow_ == null) {
          ipflow_ = new global::Ndx.Model.IpFlow();
        }
        Ipflow.MergeFrom(other.Ipflow);
      }
      if (other.SourcePort != 0) {
        SourcePort = other.SourcePort;
      }
      if (other.DestinationPort != 0) {
        DestinationPort = other.DestinationPort;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            if (ipflow_ == null) {
              ipflow_ = new global::Ndx.Model.IpFlow();
            }
            input.ReadMessage(ipflow_);
            break;
          }
          case 16: {
            SourcePort = input.ReadInt32();
            break;
          }
          case 24: {
            DestinationPort = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code

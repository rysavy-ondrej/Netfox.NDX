// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: FlowModel.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ndx.Model {

  /// <summary>Holder for reflection information generated from FlowModel.proto</summary>
  public static partial class FlowModelReflection {

    #region Descriptor
    /// <summary>File descriptor for FlowModel.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static FlowModelReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg9GbG93TW9kZWwucHJvdG8SCW5keC5tb2RlbCKzAQoHRmxvd0tleRIhCgRU",
            "eXBlGAEgASgOMhMubmR4Lm1vZGVsLkZsb3dUeXBlEhAKCFByb3RvY29sGAIg",
            "ASgMEhUKDVNvdXJjZUFkZHJlc3MYICABKAwSGgoSRGVzdGluYXRpb25BZGRy",
            "ZXNzGCEgASgMEhYKDlNvdXJjZVNlbGVjdG9yGEAgASgMEhsKE0Rlc3RpbmF0",
            "aW9uU2VsZWN0b3IYQSABKAwSCwoDVGFnGFAgASgMKjoKD0Zsb3dPcmllbnRh",
            "dGlvbhINCglVbmRlZmluZWQQABIKCgZVcGZsb3cQARIMCghEb3duZmxvdxAC",
            "Kl4KCEZsb3dUeXBlEhEKDVVuZGVmaW5lZEZsb3cQABIPCgtOZXR3b3JrRmxv",
            "dxABEg0KCUV0aGVyRmxvdxACEgoKBklwRmxvdxAEEhMKD0FwcGxpY2F0aW9u",
            "RmxvdxAIQg8KDW9yZy5uZHgubW9kZWxiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Ndx.Model.FlowOrientation), typeof(global::Ndx.Model.FlowType), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.FlowKey), global::Ndx.Model.FlowKey.Parser, new[]{ "Type", "Protocol", "SourceAddress", "DestinationAddress", "SourceSelector", "DestinationSelector", "Tag" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  //// &lt;summary>
  ////	Specifies the flow orientation if known.
  //// &lt;/summary>
  /// </summary>
  public enum FlowOrientation {
    [pbr::OriginalName("Undefined")] Undefined = 0,
    [pbr::OriginalName("Upflow")] Upflow = 1,
    [pbr::OriginalName("Downflow")] Downflow = 2,
  }

  public enum FlowType {
    [pbr::OriginalName("UndefinedFlow")] UndefinedFlow = 0,
    [pbr::OriginalName("NetworkFlow")] NetworkFlow = 1,
    [pbr::OriginalName("EtherFlow")] EtherFlow = 2,
    [pbr::OriginalName("IpFlow")] IpFlow = 4,
    [pbr::OriginalName("ApplicationFlow")] ApplicationFlow = 8,
  }

  #endregion

  #region Messages
  /// <summary>
  //// &lt;summary>
  ////	FlowKey is a 5-tuple that identifies the data traffic flow.
  //// This class supports classical IP flows as well as non-ip flows.
  ////	&lt;/summary>
  //// &lt;remarks>
  //// According to ISO/OSI model, the general addressing scheme is defined as follows:
  //// N-address = ((N-1)-Address, N-selector)
  //// 
  //// This means that for identification of communication between two parties it would be sufficient 
  //// to consider a 5 tuple:
  ////
  //// (N-protocol, (N-1)-SourceAddress, N-SourceSelector, (N-1)-DestinationAddress, N-DestinationSelector)
  ////
  //// The reason for adding protocol is that the single protocol is used and thus it 
  //// does not have to be encoded in both source and destination selectors.
  ////
  //// NetworkFlow:
  //// The case for TCP flow:
  //// (TCP, IP-Address, TCP-port, IP-Address, TCP-Port)
  //// 
  //// The case for ICMP flow:
  //// (ICMP, IP-Address, ICMP-Type . ICMP-Code, IP-Address, ICMP-Type . ICMP-Code)
  ////
  //// EtherFlow:
  //// The case of ARP flow:
  ////	(ARP, MAC-Address, [], MAC-Address, [])
  ////	
  //// The case of BPDU:
  ////	(BPDU, MAC-Address, [], MAC-Address, [])
  //// Because both parites needs to communicate using the same protocol it is possible to define
  //// the N-address pair as the 5-tuple.
  ////
  //// But there may be other flow types that aggreagate several flows or are more specific because of application selectors:
  //// 
  ////	IpFlow:
  //// (IP, IP-Addrees, [], IP-Address, [])
  //// 
  //// ApplicationFlow uses Endpoint as the address and selector which is application specific:
  ////
  //// (SMB, IP-Address:Port, (TID, PID, MID), IP-Address:Port, (TID, PID, MID))
  ////
  //// What about VLANs? and tunnels?
  //// &lt;/remarks>
  /// </summary>
  public sealed partial class FlowKey : pb::IMessage<FlowKey> {
    private static readonly pb::MessageParser<FlowKey> _parser = new pb::MessageParser<FlowKey>(() => new FlowKey());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FlowKey> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ndx.Model.FlowModelReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FlowKey() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FlowKey(FlowKey other) : this() {
      type_ = other.type_;
      protocol_ = other.protocol_;
      sourceAddress_ = other.sourceAddress_;
      destinationAddress_ = other.destinationAddress_;
      sourceSelector_ = other.sourceSelector_;
      destinationSelector_ = other.destinationSelector_;
      tag_ = other.tag_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FlowKey Clone() {
      return new FlowKey(this);
    }

    /// <summary>Field number for the "Type" field.</summary>
    public const int TypeFieldNumber = 1;
    private global::Ndx.Model.FlowType type_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.FlowType Type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    /// <summary>Field number for the "Protocol" field.</summary>
    public const int ProtocolFieldNumber = 2;
    private pb::ByteString protocol_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Protocol {
      get { return protocol_; }
      set {
        protocol_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "SourceAddress" field.</summary>
    public const int SourceAddressFieldNumber = 32;
    private pb::ByteString sourceAddress_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString SourceAddress {
      get { return sourceAddress_; }
      set {
        sourceAddress_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "DestinationAddress" field.</summary>
    public const int DestinationAddressFieldNumber = 33;
    private pb::ByteString destinationAddress_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString DestinationAddress {
      get { return destinationAddress_; }
      set {
        destinationAddress_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "SourceSelector" field.</summary>
    public const int SourceSelectorFieldNumber = 64;
    private pb::ByteString sourceSelector_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString SourceSelector {
      get { return sourceSelector_; }
      set {
        sourceSelector_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "DestinationSelector" field.</summary>
    public const int DestinationSelectorFieldNumber = 65;
    private pb::ByteString destinationSelector_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString DestinationSelector {
      get { return destinationSelector_; }
      set {
        destinationSelector_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Tag" field.</summary>
    public const int TagFieldNumber = 80;
    private pb::ByteString tag_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Tag {
      get { return tag_; }
      set {
        tag_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FlowKey);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FlowKey other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Type != other.Type) return false;
      if (Protocol != other.Protocol) return false;
      if (SourceAddress != other.SourceAddress) return false;
      if (DestinationAddress != other.DestinationAddress) return false;
      if (SourceSelector != other.SourceSelector) return false;
      if (DestinationSelector != other.DestinationSelector) return false;
      if (Tag != other.Tag) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Type != 0) hash ^= Type.GetHashCode();
      if (Protocol.Length != 0) hash ^= Protocol.GetHashCode();
      if (SourceAddress.Length != 0) hash ^= SourceAddress.GetHashCode();
      if (DestinationAddress.Length != 0) hash ^= DestinationAddress.GetHashCode();
      if (SourceSelector.Length != 0) hash ^= SourceSelector.GetHashCode();
      if (DestinationSelector.Length != 0) hash ^= DestinationSelector.GetHashCode();
      if (Tag.Length != 0) hash ^= Tag.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Type != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Type);
      }
      if (Protocol.Length != 0) {
        output.WriteRawTag(18);
        output.WriteBytes(Protocol);
      }
      if (SourceAddress.Length != 0) {
        output.WriteRawTag(130, 2);
        output.WriteBytes(SourceAddress);
      }
      if (DestinationAddress.Length != 0) {
        output.WriteRawTag(138, 2);
        output.WriteBytes(DestinationAddress);
      }
      if (SourceSelector.Length != 0) {
        output.WriteRawTag(130, 4);
        output.WriteBytes(SourceSelector);
      }
      if (DestinationSelector.Length != 0) {
        output.WriteRawTag(138, 4);
        output.WriteBytes(DestinationSelector);
      }
      if (Tag.Length != 0) {
        output.WriteRawTag(130, 5);
        output.WriteBytes(Tag);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Type);
      }
      if (Protocol.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Protocol);
      }
      if (SourceAddress.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeBytesSize(SourceAddress);
      }
      if (DestinationAddress.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeBytesSize(DestinationAddress);
      }
      if (SourceSelector.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeBytesSize(SourceSelector);
      }
      if (DestinationSelector.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeBytesSize(DestinationSelector);
      }
      if (Tag.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeBytesSize(Tag);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FlowKey other) {
      if (other == null) {
        return;
      }
      if (other.Type != 0) {
        Type = other.Type;
      }
      if (other.Protocol.Length != 0) {
        Protocol = other.Protocol;
      }
      if (other.SourceAddress.Length != 0) {
        SourceAddress = other.SourceAddress;
      }
      if (other.DestinationAddress.Length != 0) {
        DestinationAddress = other.DestinationAddress;
      }
      if (other.SourceSelector.Length != 0) {
        SourceSelector = other.SourceSelector;
      }
      if (other.DestinationSelector.Length != 0) {
        DestinationSelector = other.DestinationSelector;
      }
      if (other.Tag.Length != 0) {
        Tag = other.Tag;
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
          case 8: {
            type_ = (global::Ndx.Model.FlowType) input.ReadEnum();
            break;
          }
          case 18: {
            Protocol = input.ReadBytes();
            break;
          }
          case 258: {
            SourceAddress = input.ReadBytes();
            break;
          }
          case 266: {
            DestinationAddress = input.ReadBytes();
            break;
          }
          case 514: {
            SourceSelector = input.ReadBytes();
            break;
          }
          case 522: {
            DestinationSelector = input.ReadBytes();
            break;
          }
          case 642: {
            Tag = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code

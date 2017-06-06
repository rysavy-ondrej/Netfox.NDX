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
            "Cg9GbG93TW9kZWwucHJvdG8SCW5keC5tb2RlbCKdAQoLTmV0d29ya0Zsb3cS",
            "LwoNQWRkcmVzc0ZhbWlseRgBIAEoDjIYLm5keC5tb2RlbC5BZGRyZXNzRmFt",
            "aWx5EisKCFByb3RvY29sGAIgASgOMhkubmR4Lm1vZGVsLklwUHJvdG9jb2xU",
            "eXBlEhUKDVNvdXJjZUFkZHJlc3MYAyABKAwSGQoRRGVzdGluYXRpb25BZHJl",
            "c3MYBCABKAwiYwoHVGNwRmxvdxIrCgtOZXR3b3JrRmxvdxgBIAEoCzIWLm5k",
            "eC5tb2RlbC5OZXR3b3JrRmxvdxISCgpTb3VyY2VQb3J0GAIgASgFEhcKD0Rl",
            "c3RpbmF0aW9uUG9ydBgDIAEoBSJjCgdVZHBGbG93EisKC05ldHdvcmtGbG93",
            "GAEgASgLMhYubmR4Lm1vZGVsLk5ldHdvcmtGbG93EhIKClNvdXJjZVBvcnQY",
            "AiABKAUSFwoPRGVzdGluYXRpb25Qb3J0GAMgASgFKo0CCg5JcFByb3RvY29s",
            "VHlwZRILCgdIT1BPUFRTEAASCAoESUNNUBABEggKBElHTVAQAhIICgRJUElQ",
            "EAQSBwoDVENQEAYSBwoDRUdQEAgSBwoDUFVQEAwSBwoDVURQEBESBwoDSURQ",
            "EBYSBgoCVFAQHRIICgRJUFY2ECkSCwoHUk9VVElORxArEgwKCEZSQUdNRU5U",
            "ECwSCAoEUlNWUBAuEgcKA0dSRRAvEgcKA0VTUBAyEgYKAkFIEDMSCgoGSUNN",
            "UFY2EDoSCAoETk9ORRA7EgsKB0RTVE9QVFMQPBIHCgNNVFAQXBIJCgVFTkNB",
            "UBBiEgcKA1BJTRBnEggKBENPTVAQbBIICgNSQVcQ/wEq9QIKDUFkZHJlc3NG",
            "YW1pbHkSDwoLVW5zcGVjaWZpZWQQABIICgRVbml4EAESEAoMSW50ZXJOZXR3",
            "b3JrEAISCwoHSW1wTGluaxADEgcKA1B1cBAEEgkKBUNoYW9zEAUSBwoDSXB4",
            "EAYSBwoDSXNvEAcSCAoERWNtYRAIEgsKB0RhdGFLaXQQCRIJCgVDY2l0dBAK",
            "EgcKA1NuYRALEgoKBkRlY05ldBAMEgwKCERhdGFMaW5rEA0SBwoDTGF0EA4S",
            "EAoMSHlwZXJDaGFubmVsEA8SDQoJQXBwbGVUYWxrEBASCwoHTmV0QmlvcxAR",
            "Eg0KCVZvaWNlVmlldxASEgsKB0ZpcmVGb3gQExIKCgZCYW55YW4QFRIHCgNB",
            "dG0QFhISCg5JbnRlck5ldHdvcmtWNhAXEgsKB0NsdXN0ZXIQGBINCglJZWVl",
            "MTI4NDQQGRIICgRJcmRhEBoSFAoQTmV0d29ya0Rlc2lnbmVycxAcEgcKA01h",
            "eBAdYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Ndx.Model.IpProtocolType), typeof(global::Ndx.Model.AddressFamily), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.NetworkFlow), global::Ndx.Model.NetworkFlow.Parser, new[]{ "AddressFamily", "Protocol", "SourceAddress", "DestinationAdress" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.TcpFlow), global::Ndx.Model.TcpFlow.Parser, new[]{ "NetworkFlow", "SourcePort", "DestinationPort" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.UdpFlow), global::Ndx.Model.UdpFlow.Parser, new[]{ "NetworkFlow", "SourcePort", "DestinationPort" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  //// &lt;summary> Dummy protocol for TCP. &lt;/summary>
  /// </summary>
  public enum IpProtocolType {
    /// <summary>
    //// &lt;summary> IPv6 Hop-by-Hop options. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("HOPOPTS")] Hopopts = 0,
    /// <summary>
    //// &lt;summary> Internet Control Message Protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("ICMP")] Icmp = 1,
    /// <summary>
    //// &lt;summary> Internet Group Management Protocol.&lt;/summary>
    /// </summary>
    [pbr::OriginalName("IGMP")] Igmp = 2,
    /// <summary>
    //// &lt;summary> IPIP tunnels (older KA9Q tunnels use 94). &lt;/summary>
    /// </summary>
    [pbr::OriginalName("IPIP")] Ipip = 4,
    /// <summary>
    //// &lt;summary> Transmission Control Protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("TCP")] Tcp = 6,
    /// <summary>
    //// &lt;summary> Exterior Gateway Protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("EGP")] Egp = 8,
    /// <summary>
    //// &lt;summary> PUP protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("PUP")] Pup = 12,
    /// <summary>
    //// &lt;summary> User Datagram Protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("UDP")] Udp = 17,
    /// <summary>
    //// &lt;summary> XNS IDP protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("IDP")] Idp = 22,
    /// <summary>
    //// &lt;summary> SO Transport Protocol Class 4. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("TP")] Tp = 29,
    /// <summary>
    //// &lt;summary> IPv6 header. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("IPV6")] Ipv6 = 41,
    /// <summary>
    //// &lt;summary> IPv6 routing header. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("ROUTING")] Routing = 43,
    /// <summary>
    //// &lt;summary> IPv6 fragmentation header. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("FRAGMENT")] Fragment = 44,
    /// <summary>
    //// &lt;summary> Reservation Protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("RSVP")] Rsvp = 46,
    /// <summary>
    //// &lt;summary> General Routing Encapsulation. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("GRE")] Gre = 47,
    /// <summary>
    //// &lt;summary> Encapsulating security payload. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("ESP")] Esp = 50,
    /// <summary>
    //// &lt;summary> Authentication header. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("AH")] Ah = 51,
    /// <summary>
    //// &lt;summary> ICMPv6. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("ICMPV6")] Icmpv6 = 58,
    /// <summary>
    //// &lt;summary> IPv6 no next header. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("NONE")] None = 59,
    /// <summary>
    //// &lt;summary> IPv6 destination options. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("DSTOPTS")] Dstopts = 60,
    /// <summary>
    //// &lt;summary> Multicast Transport Protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("MTP")] Mtp = 92,
    /// <summary>
    //// &lt;summary> Encapsulation Header. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("ENCAP")] Encap = 98,
    /// <summary>
    //// &lt;summary> Protocol Independent Multicast. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("PIM")] Pim = 103,
    /// <summary>
    //// &lt;summary> Compression Header Protocol. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("COMP")] Comp = 108,
    /// <summary>
    //// &lt;summary> Raw IP packets. &lt;/summary>
    /// </summary>
    [pbr::OriginalName("RAW")] Raw = 255,
  }

  public enum AddressFamily {
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Unspecified")] Unspecified = 0,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Unix")] Unix = 1,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("InterNetwork")] InterNetwork = 2,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("ImpLink")] ImpLink = 3,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Pup")] Pup = 4,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Chaos")] Chaos = 5,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Ipx")] Ipx = 6,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Iso")] Iso = 7,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Ecma")] Ecma = 8,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("DataKit")] DataKit = 9,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Ccitt")] Ccitt = 10,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Sna")] Sna = 11,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("DecNet")] DecNet = 12,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("DataLink")] DataLink = 13,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Lat")] Lat = 14,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("HyperChannel")] HyperChannel = 15,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("AppleTalk")] AppleTalk = 16,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("NetBios")] NetBios = 17,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("VoiceView")] VoiceView = 18,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("FireFox")] FireFox = 19,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Banyan")] Banyan = 21,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Atm")] Atm = 22,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("InterNetworkV6")] InterNetworkV6 = 23,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Cluster")] Cluster = 24,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Ieee12844")] Ieee12844 = 25,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Irda")] Irda = 26,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("NetworkDesigners")] NetworkDesigners = 28,
    /// <summary>
    //// &lt;devdoc>
    ////    &lt;para>[To be supplied.]&lt;/para>
    //// &lt;/devdoc>
    /// </summary>
    [pbr::OriginalName("Max")] Max = 29,
  }

  #endregion

  #region Messages
  public sealed partial class NetworkFlow : pb::IMessage<NetworkFlow> {
    private static readonly pb::MessageParser<NetworkFlow> _parser = new pb::MessageParser<NetworkFlow>(() => new NetworkFlow());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<NetworkFlow> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ndx.Model.FlowModelReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NetworkFlow() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NetworkFlow(NetworkFlow other) : this() {
      addressFamily_ = other.addressFamily_;
      protocol_ = other.protocol_;
      sourceAddress_ = other.sourceAddress_;
      destinationAdress_ = other.destinationAdress_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NetworkFlow Clone() {
      return new NetworkFlow(this);
    }

    /// <summary>Field number for the "AddressFamily" field.</summary>
    public const int AddressFamilyFieldNumber = 1;
    private global::Ndx.Model.AddressFamily addressFamily_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.AddressFamily AddressFamily {
      get { return addressFamily_; }
      set {
        addressFamily_ = value;
      }
    }

    /// <summary>Field number for the "Protocol" field.</summary>
    public const int ProtocolFieldNumber = 2;
    private global::Ndx.Model.IpProtocolType protocol_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.IpProtocolType Protocol {
      get { return protocol_; }
      set {
        protocol_ = value;
      }
    }

    /// <summary>Field number for the "SourceAddress" field.</summary>
    public const int SourceAddressFieldNumber = 3;
    private pb::ByteString sourceAddress_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString SourceAddress {
      get { return sourceAddress_; }
      set {
        sourceAddress_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "DestinationAdress" field.</summary>
    public const int DestinationAdressFieldNumber = 4;
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
      return Equals(other as NetworkFlow);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(NetworkFlow other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AddressFamily != other.AddressFamily) return false;
      if (Protocol != other.Protocol) return false;
      if (SourceAddress != other.SourceAddress) return false;
      if (DestinationAdress != other.DestinationAdress) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (AddressFamily != 0) hash ^= AddressFamily.GetHashCode();
      if (Protocol != 0) hash ^= Protocol.GetHashCode();
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
      if (AddressFamily != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) AddressFamily);
      }
      if (Protocol != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Protocol);
      }
      if (SourceAddress.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(SourceAddress);
      }
      if (DestinationAdress.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(DestinationAdress);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (AddressFamily != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) AddressFamily);
      }
      if (Protocol != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Protocol);
      }
      if (SourceAddress.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(SourceAddress);
      }
      if (DestinationAdress.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(DestinationAdress);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(NetworkFlow other) {
      if (other == null) {
        return;
      }
      if (other.AddressFamily != 0) {
        AddressFamily = other.AddressFamily;
      }
      if (other.Protocol != 0) {
        Protocol = other.Protocol;
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
          case 8: {
            addressFamily_ = (global::Ndx.Model.AddressFamily) input.ReadEnum();
            break;
          }
          case 16: {
            protocol_ = (global::Ndx.Model.IpProtocolType) input.ReadEnum();
            break;
          }
          case 26: {
            SourceAddress = input.ReadBytes();
            break;
          }
          case 34: {
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
      get { return global::Ndx.Model.FlowModelReflection.Descriptor.MessageTypes[1]; }
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
      NetworkFlow = other.networkFlow_ != null ? other.NetworkFlow.Clone() : null;
      sourcePort_ = other.sourcePort_;
      destinationPort_ = other.destinationPort_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TcpFlow Clone() {
      return new TcpFlow(this);
    }

    /// <summary>Field number for the "NetworkFlow" field.</summary>
    public const int NetworkFlowFieldNumber = 1;
    private global::Ndx.Model.NetworkFlow networkFlow_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.NetworkFlow NetworkFlow {
      get { return networkFlow_; }
      set {
        networkFlow_ = value;
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
      if (!object.Equals(NetworkFlow, other.NetworkFlow)) return false;
      if (SourcePort != other.SourcePort) return false;
      if (DestinationPort != other.DestinationPort) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (networkFlow_ != null) hash ^= NetworkFlow.GetHashCode();
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
      if (networkFlow_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(NetworkFlow);
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
      if (networkFlow_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(NetworkFlow);
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
      if (other.networkFlow_ != null) {
        if (networkFlow_ == null) {
          networkFlow_ = new global::Ndx.Model.NetworkFlow();
        }
        NetworkFlow.MergeFrom(other.NetworkFlow);
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
            if (networkFlow_ == null) {
              networkFlow_ = new global::Ndx.Model.NetworkFlow();
            }
            input.ReadMessage(networkFlow_);
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
      get { return global::Ndx.Model.FlowModelReflection.Descriptor.MessageTypes[2]; }
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
      NetworkFlow = other.networkFlow_ != null ? other.NetworkFlow.Clone() : null;
      sourcePort_ = other.sourcePort_;
      destinationPort_ = other.destinationPort_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UdpFlow Clone() {
      return new UdpFlow(this);
    }

    /// <summary>Field number for the "NetworkFlow" field.</summary>
    public const int NetworkFlowFieldNumber = 1;
    private global::Ndx.Model.NetworkFlow networkFlow_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.NetworkFlow NetworkFlow {
      get { return networkFlow_; }
      set {
        networkFlow_ = value;
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
      if (!object.Equals(NetworkFlow, other.NetworkFlow)) return false;
      if (SourcePort != other.SourcePort) return false;
      if (DestinationPort != other.DestinationPort) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (networkFlow_ != null) hash ^= NetworkFlow.GetHashCode();
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
      if (networkFlow_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(NetworkFlow);
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
      if (networkFlow_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(NetworkFlow);
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
      if (other.networkFlow_ != null) {
        if (networkFlow_ == null) {
          networkFlow_ = new global::Ndx.Model.NetworkFlow();
        }
        NetworkFlow.MergeFrom(other.NetworkFlow);
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
            if (networkFlow_ == null) {
              networkFlow_ = new global::Ndx.Model.NetworkFlow();
            }
            input.ReadMessage(networkFlow_);
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

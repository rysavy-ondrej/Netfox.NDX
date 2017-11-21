// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: packet-udp.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ndx.Decoders.Basic {

  /// <summary>Holder for reflection information generated from packet-udp.proto</summary>
  public static partial class PacketUdpReflection {

    #region Descriptor
    /// <summary>File descriptor for packet-udp.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PacketUdpReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChBwYWNrZXQtdWRwLnByb3RvEhJOZHguRGVjb2RlcnMuQmFzaWMioAMKA1Vk",
            "cBISCgpVZHBTcmNwb3J0GAEgASgNEhIKClVkcERzdHBvcnQYAiABKA0SDwoH",
            "VWRwUG9ydBgDIAEoDRIRCglVZHBTdHJlYW0YBCABKA0SEQoJVWRwTGVuZ3Ro",
            "GAUgASgNEhMKC1VkcENoZWNrc3VtGAYgASgNEh0KFVVkcENoZWNrc3VtQ2Fs",
            "Y3VsYXRlZBgHIAEoDRIZChFVZHBDaGVja3N1bVN0YXR1cxgIIAEoDRIVCg1V",
            "ZHBQcm9jU3JjdWlkGAkgASgNEhUKDVVkcFByb2NTcmNwaWQYCiABKA0SFwoP",
            "VWRwUHJvY1NyY3VuYW1lGAsgASgJEhUKDVVkcFByb2NTcmNjbWQYDCABKAkS",
            "FQoNVWRwUHJvY0RzdHVpZBgNIAEoDRIVCg1VZHBQcm9jRHN0cGlkGA4gASgN",
            "EhcKD1VkcFByb2NEc3R1bmFtZRgPIAEoCRIVCg1VZHBQcm9jRHN0Y21kGBAg",
            "ASgJEhIKClVkcFBkdVNpemUYESABKA0SGwoTVWRwQ2hlY2tzdW1Db3ZlcmFn",
            "ZRgSIAEoDWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Decoders.Basic.Udp), global::Ndx.Decoders.Basic.Udp.Parser, new[]{ "UdpSrcport", "UdpDstport", "UdpPort", "UdpStream", "UdpLength", "UdpChecksum", "UdpChecksumCalculated", "UdpChecksumStatus", "UdpProcSrcuid", "UdpProcSrcpid", "UdpProcSrcuname", "UdpProcSrccmd", "UdpProcDstuid", "UdpProcDstpid", "UdpProcDstuname", "UdpProcDstcmd", "UdpPduSize", "UdpChecksumCoverage" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Udp : pb::IMessage<Udp> {
    private static readonly pb::MessageParser<Udp> _parser = new pb::MessageParser<Udp>(() => new Udp());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Udp> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ndx.Decoders.Basic.PacketUdpReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Udp() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Udp(Udp other) : this() {
      udpSrcport_ = other.udpSrcport_;
      udpDstport_ = other.udpDstport_;
      udpPort_ = other.udpPort_;
      udpStream_ = other.udpStream_;
      udpLength_ = other.udpLength_;
      udpChecksum_ = other.udpChecksum_;
      udpChecksumCalculated_ = other.udpChecksumCalculated_;
      udpChecksumStatus_ = other.udpChecksumStatus_;
      udpProcSrcuid_ = other.udpProcSrcuid_;
      udpProcSrcpid_ = other.udpProcSrcpid_;
      udpProcSrcuname_ = other.udpProcSrcuname_;
      udpProcSrccmd_ = other.udpProcSrccmd_;
      udpProcDstuid_ = other.udpProcDstuid_;
      udpProcDstpid_ = other.udpProcDstpid_;
      udpProcDstuname_ = other.udpProcDstuname_;
      udpProcDstcmd_ = other.udpProcDstcmd_;
      udpPduSize_ = other.udpPduSize_;
      udpChecksumCoverage_ = other.udpChecksumCoverage_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Udp Clone() {
      return new Udp(this);
    }

    /// <summary>Field number for the "UdpSrcport" field.</summary>
    public const int UdpSrcportFieldNumber = 1;
    private uint udpSrcport_;
    /// <summary>
    /// Source Port ('udp_udp_srcport')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpSrcport {
      get { return udpSrcport_; }
      set {
        udpSrcport_ = value;
      }
    }

    /// <summary>Field number for the "UdpDstport" field.</summary>
    public const int UdpDstportFieldNumber = 2;
    private uint udpDstport_;
    /// <summary>
    /// Destination Port ('udp_udp_dstport')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpDstport {
      get { return udpDstport_; }
      set {
        udpDstport_ = value;
      }
    }

    /// <summary>Field number for the "UdpPort" field.</summary>
    public const int UdpPortFieldNumber = 3;
    private uint udpPort_;
    /// <summary>
    /// Source or Destination Port ('udp_udp_port')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpPort {
      get { return udpPort_; }
      set {
        udpPort_ = value;
      }
    }

    /// <summary>Field number for the "UdpStream" field.</summary>
    public const int UdpStreamFieldNumber = 4;
    private uint udpStream_;
    /// <summary>
    /// Stream index ('udp_udp_stream')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpStream {
      get { return udpStream_; }
      set {
        udpStream_ = value;
      }
    }

    /// <summary>Field number for the "UdpLength" field.</summary>
    public const int UdpLengthFieldNumber = 5;
    private uint udpLength_;
    /// <summary>
    /// Length ('udp_udp_length')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpLength {
      get { return udpLength_; }
      set {
        udpLength_ = value;
      }
    }

    /// <summary>Field number for the "UdpChecksum" field.</summary>
    public const int UdpChecksumFieldNumber = 6;
    private uint udpChecksum_;
    /// <summary>
    /// Checksum ('udp_udp_checksum')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpChecksum {
      get { return udpChecksum_; }
      set {
        udpChecksum_ = value;
      }
    }

    /// <summary>Field number for the "UdpChecksumCalculated" field.</summary>
    public const int UdpChecksumCalculatedFieldNumber = 7;
    private uint udpChecksumCalculated_;
    /// <summary>
    /// Calculated Checksum ('udp_udp_checksum_calculated')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpChecksumCalculated {
      get { return udpChecksumCalculated_; }
      set {
        udpChecksumCalculated_ = value;
      }
    }

    /// <summary>Field number for the "UdpChecksumStatus" field.</summary>
    public const int UdpChecksumStatusFieldNumber = 8;
    private uint udpChecksumStatus_;
    /// <summary>
    /// Checksum Status ('udp_checksum_udp_checksum_status')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpChecksumStatus {
      get { return udpChecksumStatus_; }
      set {
        udpChecksumStatus_ = value;
      }
    }

    /// <summary>Field number for the "UdpProcSrcuid" field.</summary>
    public const int UdpProcSrcuidFieldNumber = 9;
    private uint udpProcSrcuid_;
    /// <summary>
    /// Source process user ID ('udp_udp_proc_srcuid')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpProcSrcuid {
      get { return udpProcSrcuid_; }
      set {
        udpProcSrcuid_ = value;
      }
    }

    /// <summary>Field number for the "UdpProcSrcpid" field.</summary>
    public const int UdpProcSrcpidFieldNumber = 10;
    private uint udpProcSrcpid_;
    /// <summary>
    /// Source process ID ('udp_udp_proc_srcpid')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpProcSrcpid {
      get { return udpProcSrcpid_; }
      set {
        udpProcSrcpid_ = value;
      }
    }

    /// <summary>Field number for the "UdpProcSrcuname" field.</summary>
    public const int UdpProcSrcunameFieldNumber = 11;
    private string udpProcSrcuname_ = "";
    /// <summary>
    /// Source process user name ('udp_udp_proc_srcuname')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UdpProcSrcuname {
      get { return udpProcSrcuname_; }
      set {
        udpProcSrcuname_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "UdpProcSrccmd" field.</summary>
    public const int UdpProcSrccmdFieldNumber = 12;
    private string udpProcSrccmd_ = "";
    /// <summary>
    /// Source process name ('udp_udp_proc_srccmd')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UdpProcSrccmd {
      get { return udpProcSrccmd_; }
      set {
        udpProcSrccmd_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "UdpProcDstuid" field.</summary>
    public const int UdpProcDstuidFieldNumber = 13;
    private uint udpProcDstuid_;
    /// <summary>
    /// Destination process user ID ('udp_udp_proc_dstuid')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpProcDstuid {
      get { return udpProcDstuid_; }
      set {
        udpProcDstuid_ = value;
      }
    }

    /// <summary>Field number for the "UdpProcDstpid" field.</summary>
    public const int UdpProcDstpidFieldNumber = 14;
    private uint udpProcDstpid_;
    /// <summary>
    /// Destination process ID ('udp_udp_proc_dstpid')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpProcDstpid {
      get { return udpProcDstpid_; }
      set {
        udpProcDstpid_ = value;
      }
    }

    /// <summary>Field number for the "UdpProcDstuname" field.</summary>
    public const int UdpProcDstunameFieldNumber = 15;
    private string udpProcDstuname_ = "";
    /// <summary>
    /// Destination process user name ('udp_udp_proc_dstuname')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UdpProcDstuname {
      get { return udpProcDstuname_; }
      set {
        udpProcDstuname_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "UdpProcDstcmd" field.</summary>
    public const int UdpProcDstcmdFieldNumber = 16;
    private string udpProcDstcmd_ = "";
    /// <summary>
    /// Destination process name ('udp_udp_proc_dstcmd')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UdpProcDstcmd {
      get { return udpProcDstcmd_; }
      set {
        udpProcDstcmd_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "UdpPduSize" field.</summary>
    public const int UdpPduSizeFieldNumber = 17;
    private uint udpPduSize_;
    /// <summary>
    /// PDU Size ('udp_udp_pdu_size')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpPduSize {
      get { return udpPduSize_; }
      set {
        udpPduSize_ = value;
      }
    }

    /// <summary>Field number for the "UdpChecksumCoverage" field.</summary>
    public const int UdpChecksumCoverageFieldNumber = 18;
    private uint udpChecksumCoverage_;
    /// <summary>
    /// Checksum coverage ('udp_udp_checksum_coverage')
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint UdpChecksumCoverage {
      get { return udpChecksumCoverage_; }
      set {
        udpChecksumCoverage_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Udp);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Udp other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UdpSrcport != other.UdpSrcport) return false;
      if (UdpDstport != other.UdpDstport) return false;
      if (UdpPort != other.UdpPort) return false;
      if (UdpStream != other.UdpStream) return false;
      if (UdpLength != other.UdpLength) return false;
      if (UdpChecksum != other.UdpChecksum) return false;
      if (UdpChecksumCalculated != other.UdpChecksumCalculated) return false;
      if (UdpChecksumStatus != other.UdpChecksumStatus) return false;
      if (UdpProcSrcuid != other.UdpProcSrcuid) return false;
      if (UdpProcSrcpid != other.UdpProcSrcpid) return false;
      if (UdpProcSrcuname != other.UdpProcSrcuname) return false;
      if (UdpProcSrccmd != other.UdpProcSrccmd) return false;
      if (UdpProcDstuid != other.UdpProcDstuid) return false;
      if (UdpProcDstpid != other.UdpProcDstpid) return false;
      if (UdpProcDstuname != other.UdpProcDstuname) return false;
      if (UdpProcDstcmd != other.UdpProcDstcmd) return false;
      if (UdpPduSize != other.UdpPduSize) return false;
      if (UdpChecksumCoverage != other.UdpChecksumCoverage) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UdpSrcport != 0) hash ^= UdpSrcport.GetHashCode();
      if (UdpDstport != 0) hash ^= UdpDstport.GetHashCode();
      if (UdpPort != 0) hash ^= UdpPort.GetHashCode();
      if (UdpStream != 0) hash ^= UdpStream.GetHashCode();
      if (UdpLength != 0) hash ^= UdpLength.GetHashCode();
      if (UdpChecksum != 0) hash ^= UdpChecksum.GetHashCode();
      if (UdpChecksumCalculated != 0) hash ^= UdpChecksumCalculated.GetHashCode();
      if (UdpChecksumStatus != 0) hash ^= UdpChecksumStatus.GetHashCode();
      if (UdpProcSrcuid != 0) hash ^= UdpProcSrcuid.GetHashCode();
      if (UdpProcSrcpid != 0) hash ^= UdpProcSrcpid.GetHashCode();
      if (UdpProcSrcuname.Length != 0) hash ^= UdpProcSrcuname.GetHashCode();
      if (UdpProcSrccmd.Length != 0) hash ^= UdpProcSrccmd.GetHashCode();
      if (UdpProcDstuid != 0) hash ^= UdpProcDstuid.GetHashCode();
      if (UdpProcDstpid != 0) hash ^= UdpProcDstpid.GetHashCode();
      if (UdpProcDstuname.Length != 0) hash ^= UdpProcDstuname.GetHashCode();
      if (UdpProcDstcmd.Length != 0) hash ^= UdpProcDstcmd.GetHashCode();
      if (UdpPduSize != 0) hash ^= UdpPduSize.GetHashCode();
      if (UdpChecksumCoverage != 0) hash ^= UdpChecksumCoverage.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UdpSrcport != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(UdpSrcport);
      }
      if (UdpDstport != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(UdpDstport);
      }
      if (UdpPort != 0) {
        output.WriteRawTag(24);
        output.WriteUInt32(UdpPort);
      }
      if (UdpStream != 0) {
        output.WriteRawTag(32);
        output.WriteUInt32(UdpStream);
      }
      if (UdpLength != 0) {
        output.WriteRawTag(40);
        output.WriteUInt32(UdpLength);
      }
      if (UdpChecksum != 0) {
        output.WriteRawTag(48);
        output.WriteUInt32(UdpChecksum);
      }
      if (UdpChecksumCalculated != 0) {
        output.WriteRawTag(56);
        output.WriteUInt32(UdpChecksumCalculated);
      }
      if (UdpChecksumStatus != 0) {
        output.WriteRawTag(64);
        output.WriteUInt32(UdpChecksumStatus);
      }
      if (UdpProcSrcuid != 0) {
        output.WriteRawTag(72);
        output.WriteUInt32(UdpProcSrcuid);
      }
      if (UdpProcSrcpid != 0) {
        output.WriteRawTag(80);
        output.WriteUInt32(UdpProcSrcpid);
      }
      if (UdpProcSrcuname.Length != 0) {
        output.WriteRawTag(90);
        output.WriteString(UdpProcSrcuname);
      }
      if (UdpProcSrccmd.Length != 0) {
        output.WriteRawTag(98);
        output.WriteString(UdpProcSrccmd);
      }
      if (UdpProcDstuid != 0) {
        output.WriteRawTag(104);
        output.WriteUInt32(UdpProcDstuid);
      }
      if (UdpProcDstpid != 0) {
        output.WriteRawTag(112);
        output.WriteUInt32(UdpProcDstpid);
      }
      if (UdpProcDstuname.Length != 0) {
        output.WriteRawTag(122);
        output.WriteString(UdpProcDstuname);
      }
      if (UdpProcDstcmd.Length != 0) {
        output.WriteRawTag(130, 1);
        output.WriteString(UdpProcDstcmd);
      }
      if (UdpPduSize != 0) {
        output.WriteRawTag(136, 1);
        output.WriteUInt32(UdpPduSize);
      }
      if (UdpChecksumCoverage != 0) {
        output.WriteRawTag(144, 1);
        output.WriteUInt32(UdpChecksumCoverage);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UdpSrcport != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpSrcport);
      }
      if (UdpDstport != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpDstport);
      }
      if (UdpPort != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpPort);
      }
      if (UdpStream != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpStream);
      }
      if (UdpLength != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpLength);
      }
      if (UdpChecksum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpChecksum);
      }
      if (UdpChecksumCalculated != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpChecksumCalculated);
      }
      if (UdpChecksumStatus != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpChecksumStatus);
      }
      if (UdpProcSrcuid != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpProcSrcuid);
      }
      if (UdpProcSrcpid != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpProcSrcpid);
      }
      if (UdpProcSrcuname.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UdpProcSrcuname);
      }
      if (UdpProcSrccmd.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UdpProcSrccmd);
      }
      if (UdpProcDstuid != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpProcDstuid);
      }
      if (UdpProcDstpid != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(UdpProcDstpid);
      }
      if (UdpProcDstuname.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UdpProcDstuname);
      }
      if (UdpProcDstcmd.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeStringSize(UdpProcDstcmd);
      }
      if (UdpPduSize != 0) {
        size += 2 + pb::CodedOutputStream.ComputeUInt32Size(UdpPduSize);
      }
      if (UdpChecksumCoverage != 0) {
        size += 2 + pb::CodedOutputStream.ComputeUInt32Size(UdpChecksumCoverage);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Udp other) {
      if (other == null) {
        return;
      }
      if (other.UdpSrcport != 0) {
        UdpSrcport = other.UdpSrcport;
      }
      if (other.UdpDstport != 0) {
        UdpDstport = other.UdpDstport;
      }
      if (other.UdpPort != 0) {
        UdpPort = other.UdpPort;
      }
      if (other.UdpStream != 0) {
        UdpStream = other.UdpStream;
      }
      if (other.UdpLength != 0) {
        UdpLength = other.UdpLength;
      }
      if (other.UdpChecksum != 0) {
        UdpChecksum = other.UdpChecksum;
      }
      if (other.UdpChecksumCalculated != 0) {
        UdpChecksumCalculated = other.UdpChecksumCalculated;
      }
      if (other.UdpChecksumStatus != 0) {
        UdpChecksumStatus = other.UdpChecksumStatus;
      }
      if (other.UdpProcSrcuid != 0) {
        UdpProcSrcuid = other.UdpProcSrcuid;
      }
      if (other.UdpProcSrcpid != 0) {
        UdpProcSrcpid = other.UdpProcSrcpid;
      }
      if (other.UdpProcSrcuname.Length != 0) {
        UdpProcSrcuname = other.UdpProcSrcuname;
      }
      if (other.UdpProcSrccmd.Length != 0) {
        UdpProcSrccmd = other.UdpProcSrccmd;
      }
      if (other.UdpProcDstuid != 0) {
        UdpProcDstuid = other.UdpProcDstuid;
      }
      if (other.UdpProcDstpid != 0) {
        UdpProcDstpid = other.UdpProcDstpid;
      }
      if (other.UdpProcDstuname.Length != 0) {
        UdpProcDstuname = other.UdpProcDstuname;
      }
      if (other.UdpProcDstcmd.Length != 0) {
        UdpProcDstcmd = other.UdpProcDstcmd;
      }
      if (other.UdpPduSize != 0) {
        UdpPduSize = other.UdpPduSize;
      }
      if (other.UdpChecksumCoverage != 0) {
        UdpChecksumCoverage = other.UdpChecksumCoverage;
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
            UdpSrcport = input.ReadUInt32();
            break;
          }
          case 16: {
            UdpDstport = input.ReadUInt32();
            break;
          }
          case 24: {
            UdpPort = input.ReadUInt32();
            break;
          }
          case 32: {
            UdpStream = input.ReadUInt32();
            break;
          }
          case 40: {
            UdpLength = input.ReadUInt32();
            break;
          }
          case 48: {
            UdpChecksum = input.ReadUInt32();
            break;
          }
          case 56: {
            UdpChecksumCalculated = input.ReadUInt32();
            break;
          }
          case 64: {
            UdpChecksumStatus = input.ReadUInt32();
            break;
          }
          case 72: {
            UdpProcSrcuid = input.ReadUInt32();
            break;
          }
          case 80: {
            UdpProcSrcpid = input.ReadUInt32();
            break;
          }
          case 90: {
            UdpProcSrcuname = input.ReadString();
            break;
          }
          case 98: {
            UdpProcSrccmd = input.ReadString();
            break;
          }
          case 104: {
            UdpProcDstuid = input.ReadUInt32();
            break;
          }
          case 112: {
            UdpProcDstpid = input.ReadUInt32();
            break;
          }
          case 122: {
            UdpProcDstuname = input.ReadString();
            break;
          }
          case 130: {
            UdpProcDstcmd = input.ReadString();
            break;
          }
          case 136: {
            UdpPduSize = input.ReadUInt32();
            break;
          }
          case 144: {
            UdpChecksumCoverage = input.ReadUInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
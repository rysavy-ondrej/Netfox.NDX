// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: SSH.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ndx.Model {

  /// <summary>Holder for reflection information generated from SSH.proto</summary>
  public static partial class SSHReflection {

    #region Descriptor
    /// <summary>File descriptor for SSH.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SSHReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CglTU0gucHJvdG8SCW5keC5tb2RlbCKqAQoIU3NoRXZlbnQSEgoKVGltZU9m",
            "ZnNldBgBIAEoAxInCgRUeXBlGAIgASgOMhkubmR4Lm1vZGVsLlNzaE1lc3Nh",
            "Z2VUeXBlEicKBENvZGUYAyABKA4yGS5uZHgubW9kZWwuU3NoTWVzc2FnZUNv",
            "ZGUSIgoGU2VuZGVyGAQgASgOMhIubmR4Lm1vZGVsLlNzaFJvbGUSFAoMUGFj",
            "a2V0TGVuZ3RoGAUgASgFIm0KD1NzaENvbnZlcnNhdGlvbhISCgpFbmNyeXB0",
            "aW9uGAogASgJEgwKBEhNQUMYCyABKAkSEwoLQ29tcHJlc3Npb24YDCABKAkS",
            "IwoGRXZlbnRzGBQgAygLMhMubmR4Lm1vZGVsLlNzaEV2ZW50Kk0KDlNzaE1l",
            "c3NhZ2VUeXBlEg8KC1NzaFByb3RvY29sEAASEgoOU3NoS2V5RXhjaGFuZ2UQ",
            "ARIWChJTc2hFbmNyeXB0ZWRQYWNrZXQQAionCgdTc2hSb2xlEg0KCVNzaENs",
            "aWVudBAAEg0KCVNzaFNlcnZlchABKqUECg5Tc2hNZXNzYWdlQ29kZRIQCgxT",
            "U0hfTVNHX05PTkUQABIWChJTU0hfTVNHX0RJU0NPTk5FQ1QQARISCg5TU0hf",
            "TVNHX0lHTk9SRRACEhgKFFNTSF9NU0dfVU5JTVBMRU1FTlRFEAMSEQoNU1NI",
            "X01TR19ERUJVRxAEEhsKF1NTSF9NU0dfU0VSVklDRV9SRVFVRVNUEAUSGgoW",
            "U1NIX01TR19TRVJWSUNFX0FDQ0VQVBAGEhMKD1NTSF9NU0dfS0VYSU5JVBAU",
            "EhMKD1NTSF9NU0dfTkVXS0VZUxAVEhcKE1NTSF9NU0dfREZIX0tFWElOSVQQ",
            "HhIYChRTU0hfTVNHX0RGSF9LRVhSRVBMWRAfEhwKGFNTSF9NU0dfVVNFUkFV",
            "VEhfUkVRVUVTVBAyEhwKGFNTSF9NU0dfVVNFUkFVVEhfRkFJTFVSRRAzEhwK",
            "GFNTSF9NU0dfVVNFUkFVVEhfU1VDQ0VTUxA0EhsKF1NTSF9NU0dfVVNFUkFV",
            "VEhfQkFOTkVSEDUSIQodU1NIX01TR19VU0VSQVVUSF9JTkZPX1JFUVVFU1QQ",
            "PBIiCh5TU0hfTVNHX1VTRVJBVVRIX0lORk9fUkVTUE9OU0UQPRIaChZTU0hf",
            "TVNHX0dMT0JBTF9SRVFVRVNUEFASGwoXU1NIX01TR19SRVFVRVNUX1NVQ0NF",
            "U1MQURIbChdTU0hfTVNHX1JFUVVFU1RfRkFJTFVSRRBSYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Ndx.Model.SshMessageType), typeof(global::Ndx.Model.SshRole), typeof(global::Ndx.Model.SshMessageCode), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.SshEvent), global::Ndx.Model.SshEvent.Parser, new[]{ "TimeOffset", "Type", "Code", "Sender", "PacketLength" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ndx.Model.SshConversation), global::Ndx.Model.SshConversation.Parser, new[]{ "Encryption", "HMAC", "Compression", "Events" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum SshMessageType {
    [pbr::OriginalName("SshProtocol")] SshProtocol = 0,
    [pbr::OriginalName("SshKeyExchange")] SshKeyExchange = 1,
    [pbr::OriginalName("SshEncryptedPacket")] SshEncryptedPacket = 2,
  }

  public enum SshRole {
    [pbr::OriginalName("SshClient")] SshClient = 0,
    [pbr::OriginalName("SshServer")] SshServer = 1,
  }

  public enum SshMessageCode {
    [pbr::OriginalName("SSH_MSG_NONE")] SshMsgNone = 0,
    [pbr::OriginalName("SSH_MSG_DISCONNECT")] SshMsgDisconnect = 1,
    [pbr::OriginalName("SSH_MSG_IGNORE")] SshMsgIgnore = 2,
    [pbr::OriginalName("SSH_MSG_UNIMPLEMENTE")] SshMsgUnimplemente = 3,
    [pbr::OriginalName("SSH_MSG_DEBUG")] SshMsgDebug = 4,
    [pbr::OriginalName("SSH_MSG_SERVICE_REQUEST")] SshMsgServiceRequest = 5,
    [pbr::OriginalName("SSH_MSG_SERVICE_ACCEPT")] SshMsgServiceAccept = 6,
    [pbr::OriginalName("SSH_MSG_KEXINIT")] SshMsgKexinit = 20,
    [pbr::OriginalName("SSH_MSG_NEWKEYS")] SshMsgNewkeys = 21,
    [pbr::OriginalName("SSH_MSG_DFH_KEXINIT")] SshMsgDfhKexinit = 30,
    [pbr::OriginalName("SSH_MSG_DFH_KEXREPLY")] SshMsgDfhKexreply = 31,
    [pbr::OriginalName("SSH_MSG_USERAUTH_REQUEST")] SshMsgUserauthRequest = 50,
    [pbr::OriginalName("SSH_MSG_USERAUTH_FAILURE")] SshMsgUserauthFailure = 51,
    [pbr::OriginalName("SSH_MSG_USERAUTH_SUCCESS")] SshMsgUserauthSuccess = 52,
    [pbr::OriginalName("SSH_MSG_USERAUTH_BANNER")] SshMsgUserauthBanner = 53,
    [pbr::OriginalName("SSH_MSG_USERAUTH_INFO_REQUEST")] SshMsgUserauthInfoRequest = 60,
    [pbr::OriginalName("SSH_MSG_USERAUTH_INFO_RESPONSE")] SshMsgUserauthInfoResponse = 61,
    [pbr::OriginalName("SSH_MSG_GLOBAL_REQUEST")] SshMsgGlobalRequest = 80,
    [pbr::OriginalName("SSH_MSG_REQUEST_SUCCESS")] SshMsgRequestSuccess = 81,
    [pbr::OriginalName("SSH_MSG_REQUEST_FAILURE")] SshMsgRequestFailure = 82,
  }

  #endregion

  #region Messages
  public sealed partial class SshEvent : pb::IMessage<SshEvent> {
    private static readonly pb::MessageParser<SshEvent> _parser = new pb::MessageParser<SshEvent>(() => new SshEvent());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SshEvent> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ndx.Model.SSHReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SshEvent() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SshEvent(SshEvent other) : this() {
      timeOffset_ = other.timeOffset_;
      type_ = other.type_;
      code_ = other.code_;
      sender_ = other.sender_;
      packetLength_ = other.packetLength_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SshEvent Clone() {
      return new SshEvent(this);
    }

    /// <summary>Field number for the "TimeOffset" field.</summary>
    public const int TimeOffsetFieldNumber = 1;
    private long timeOffset_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long TimeOffset {
      get { return timeOffset_; }
      set {
        timeOffset_ = value;
      }
    }

    /// <summary>Field number for the "Type" field.</summary>
    public const int TypeFieldNumber = 2;
    private global::Ndx.Model.SshMessageType type_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.SshMessageType Type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    /// <summary>Field number for the "Code" field.</summary>
    public const int CodeFieldNumber = 3;
    private global::Ndx.Model.SshMessageCode code_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.SshMessageCode Code {
      get { return code_; }
      set {
        code_ = value;
      }
    }

    /// <summary>Field number for the "Sender" field.</summary>
    public const int SenderFieldNumber = 4;
    private global::Ndx.Model.SshRole sender_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Ndx.Model.SshRole Sender {
      get { return sender_; }
      set {
        sender_ = value;
      }
    }

    /// <summary>Field number for the "PacketLength" field.</summary>
    public const int PacketLengthFieldNumber = 5;
    private int packetLength_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int PacketLength {
      get { return packetLength_; }
      set {
        packetLength_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SshEvent);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SshEvent other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (TimeOffset != other.TimeOffset) return false;
      if (Type != other.Type) return false;
      if (Code != other.Code) return false;
      if (Sender != other.Sender) return false;
      if (PacketLength != other.PacketLength) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (TimeOffset != 0L) hash ^= TimeOffset.GetHashCode();
      if (Type != 0) hash ^= Type.GetHashCode();
      if (Code != 0) hash ^= Code.GetHashCode();
      if (Sender != 0) hash ^= Sender.GetHashCode();
      if (PacketLength != 0) hash ^= PacketLength.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (TimeOffset != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(TimeOffset);
      }
      if (Type != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Type);
      }
      if (Code != 0) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Code);
      }
      if (Sender != 0) {
        output.WriteRawTag(32);
        output.WriteEnum((int) Sender);
      }
      if (PacketLength != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(PacketLength);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (TimeOffset != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(TimeOffset);
      }
      if (Type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Type);
      }
      if (Code != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Code);
      }
      if (Sender != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Sender);
      }
      if (PacketLength != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(PacketLength);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SshEvent other) {
      if (other == null) {
        return;
      }
      if (other.TimeOffset != 0L) {
        TimeOffset = other.TimeOffset;
      }
      if (other.Type != 0) {
        Type = other.Type;
      }
      if (other.Code != 0) {
        Code = other.Code;
      }
      if (other.Sender != 0) {
        Sender = other.Sender;
      }
      if (other.PacketLength != 0) {
        PacketLength = other.PacketLength;
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
            TimeOffset = input.ReadInt64();
            break;
          }
          case 16: {
            type_ = (global::Ndx.Model.SshMessageType) input.ReadEnum();
            break;
          }
          case 24: {
            code_ = (global::Ndx.Model.SshMessageCode) input.ReadEnum();
            break;
          }
          case 32: {
            sender_ = (global::Ndx.Model.SshRole) input.ReadEnum();
            break;
          }
          case 40: {
            PacketLength = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class SshConversation : pb::IMessage<SshConversation> {
    private static readonly pb::MessageParser<SshConversation> _parser = new pb::MessageParser<SshConversation>(() => new SshConversation());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SshConversation> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ndx.Model.SSHReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SshConversation() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SshConversation(SshConversation other) : this() {
      encryption_ = other.encryption_;
      hMAC_ = other.hMAC_;
      compression_ = other.compression_;
      events_ = other.events_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SshConversation Clone() {
      return new SshConversation(this);
    }

    /// <summary>Field number for the "Encryption" field.</summary>
    public const int EncryptionFieldNumber = 10;
    private string encryption_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Encryption {
      get { return encryption_; }
      set {
        encryption_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "HMAC" field.</summary>
    public const int HMACFieldNumber = 11;
    private string hMAC_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string HMAC {
      get { return hMAC_; }
      set {
        hMAC_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Compression" field.</summary>
    public const int CompressionFieldNumber = 12;
    private string compression_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Compression {
      get { return compression_; }
      set {
        compression_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Events" field.</summary>
    public const int EventsFieldNumber = 20;
    private static readonly pb::FieldCodec<global::Ndx.Model.SshEvent> _repeated_events_codec
        = pb::FieldCodec.ForMessage(162, global::Ndx.Model.SshEvent.Parser);
    private readonly pbc::RepeatedField<global::Ndx.Model.SshEvent> events_ = new pbc::RepeatedField<global::Ndx.Model.SshEvent>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Ndx.Model.SshEvent> Events {
      get { return events_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SshConversation);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SshConversation other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Encryption != other.Encryption) return false;
      if (HMAC != other.HMAC) return false;
      if (Compression != other.Compression) return false;
      if(!events_.Equals(other.events_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Encryption.Length != 0) hash ^= Encryption.GetHashCode();
      if (HMAC.Length != 0) hash ^= HMAC.GetHashCode();
      if (Compression.Length != 0) hash ^= Compression.GetHashCode();
      hash ^= events_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Encryption.Length != 0) {
        output.WriteRawTag(82);
        output.WriteString(Encryption);
      }
      if (HMAC.Length != 0) {
        output.WriteRawTag(90);
        output.WriteString(HMAC);
      }
      if (Compression.Length != 0) {
        output.WriteRawTag(98);
        output.WriteString(Compression);
      }
      events_.WriteTo(output, _repeated_events_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Encryption.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Encryption);
      }
      if (HMAC.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(HMAC);
      }
      if (Compression.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Compression);
      }
      size += events_.CalculateSize(_repeated_events_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SshConversation other) {
      if (other == null) {
        return;
      }
      if (other.Encryption.Length != 0) {
        Encryption = other.Encryption;
      }
      if (other.HMAC.Length != 0) {
        HMAC = other.HMAC;
      }
      if (other.Compression.Length != 0) {
        Compression = other.Compression;
      }
      events_.Add(other.events_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 82: {
            Encryption = input.ReadString();
            break;
          }
          case 90: {
            HMAC = input.ReadString();
            break;
          }
          case 98: {
            Compression = input.ReadString();
            break;
          }
          case 162: {
            events_.AddEntriesFrom(input, _repeated_events_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
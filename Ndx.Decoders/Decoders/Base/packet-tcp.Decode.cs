using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Basic
{
  public sealed partial class Tcp
  {
    public static Tcp DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Tcp DecodeJson(JToken token)
    {
      var obj = new Tcp();
      {
        var val = token["tcp_tcp_srcport"];
        if (val != null) obj.TcpSrcport = default(UInt32);
      }
      {
        var val = token["tcp_tcp_dstport"];
        if (val != null) obj.TcpDstport = default(UInt32);
      }
      {
        var val = token["tcp_tcp_port"];
        if (val != null) obj.TcpPort = default(UInt32);
      }
      {
        var val = token["tcp_tcp_stream"];
        if (val != null) obj.TcpStream = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_seq"];
        if (val != null) obj.TcpSeq = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_nxtseq"];
        if (val != null) obj.TcpNxtseq = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_ack"];
        if (val != null) obj.TcpAck = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_hdr_len"];
        if (val != null) obj.TcpHdrLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_flags"];
        if (val != null) obj.TcpFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_flags_tcp_flags_res"];
        if (val != null) obj.TcpFlagsRes = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_ns"];
        if (val != null) obj.TcpFlagsNs = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_cwr"];
        if (val != null) obj.TcpFlagsCwr = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_ecn"];
        if (val != null) obj.TcpFlagsEcn = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_urg"];
        if (val != null) obj.TcpFlagsUrg = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_ack"];
        if (val != null) obj.TcpFlagsAck = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_push"];
        if (val != null) obj.TcpFlagsPush = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_reset"];
        if (val != null) obj.TcpFlagsReset = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_syn"];
        if (val != null) obj.TcpFlagsSyn = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_fin"];
        if (val != null) obj.TcpFlagsFin = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_str"];
        if (val != null) obj.TcpFlagsStr = val.Value<string>();
      }
      {
        var val = token["tcp_tcp_window_size_value"];
        if (val != null) obj.TcpWindowSizeValue = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_window_size"];
        if (val != null) obj.TcpWindowSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_window_size_scalefactor"];
        if (val != null) obj.TcpWindowSizeScalefactor = Convert.ToInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_checksum"];
        if (val != null) obj.TcpChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_checksum_tcp_checksum_status"];
        if (val != null) obj.TcpChecksumStatus = default(UInt32);
      }
      {
        var val = token["tcp_tcp_checksum_calculated"];
        if (val != null) obj.TcpChecksumCalculated = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_tcp_analysis"];
        if (val != null) obj.TcpAnalysis = default(Int32);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_flags"];
        if (val != null) obj.TcpAnalysisFlags = default(Int32);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_duplicate_ack"];
        if (val != null) obj.TcpAnalysisDuplicateAck = default(Int32);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_duplicate_ack_num"];
        if (val != null) obj.TcpAnalysisDuplicateAckNum = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_duplicate_ack_frame"];
        if (val != null) obj.TcpAnalysisDuplicateAckFrame = default(Int64);
      }
      {
        var val = token["tcp_tcp_continuation_to"];
        if (val != null) obj.TcpContinuationTo = default(Int64);
      }
      {
        var val = token["tcp_tcp_len"];
        if (val != null) obj.TcpLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_acks_frame"];
        if (val != null) obj.TcpAnalysisAcksFrame = default(Int64);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_bytes_in_flight"];
        if (val != null) obj.TcpAnalysisBytesInFlight = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_push_bytes_sent"];
        if (val != null) obj.TcpAnalysisPushBytesSent = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_ack_rtt"];
        if (val != null) obj.TcpAnalysisAckRtt = default(Int64);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_initial_rtt"];
        if (val != null) obj.TcpAnalysisInitialRtt = default(Int64);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_rto"];
        if (val != null) obj.TcpAnalysisRto = default(Int64);
      }
      {
        var val = token["tcp_analysis_tcp_analysis_rto_frame"];
        if (val != null) obj.TcpAnalysisRtoFrame = default(Int64);
      }
      {
        var val = token["tcp_tcp_urgent_pointer"];
        if (val != null) obj.TcpUrgentPointer = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_segment_tcp_segment_overlap"];
        if (val != null) obj.TcpSegmentOverlap = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_segment_overlap_tcp_segment_overlap_conflict"];
        if (val != null) obj.TcpSegmentOverlapConflict = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_segment_tcp_segment_multipletails"];
        if (val != null) obj.TcpSegmentMultipletails = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_segment_tcp_segment_toolongfragment"];
        if (val != null) obj.TcpSegmentToolongfragment = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_segment_tcp_segment_error"];
        if (val != null) obj.TcpSegmentError = default(Int64);
      }
      {
        var val = token["tcp_segment_tcp_segment_count"];
        if (val != null) obj.TcpSegmentCount = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_segment"];
        if (val != null) obj.TcpSegment = default(Int64);
      }
      {
        var val = token["tcp_tcp_segments"];
        if (val != null) obj.TcpSegments = default(Int32);
      }
      {
        var val = token["tcp_tcp_reassembled_in"];
        if (val != null) obj.TcpReassembledIn = default(Int64);
      }
      {
        var val = token["tcp_tcp_reassembled_length"];
        if (val != null) obj.TcpReassembledLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_reassembled_data"];
        if (val != null) obj.TcpReassembledData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_tcp_option_kind"];
        if (val != null) obj.TcpOptionKind = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_option_len"];
        if (val != null) obj.TcpOptionLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options"];
        if (val != null) obj.TcpOptions = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_options_tcp_options_mss_val"];
        if (val != null) obj.TcpOptionsMssVal = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_wscale_shift"];
        if (val != null) obj.TcpOptionsWscaleShift = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_wscale_multiplier"];
        if (val != null) obj.TcpOptionsWscaleMultiplier = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_experimental_data"];
        if (val != null) obj.TcpOptionsExperimentalData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_tcp_options_experimental_magic_number"];
        if (val != null) obj.TcpOptionsExperimentalMagicNumber = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_options_tcp_options_sack_le"];
        if (val != null) obj.TcpOptionsSackLe = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_options_tcp_options_sack_re"];
        if (val != null) obj.TcpOptionsSackRe = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_sack_count"];
        if (val != null) obj.TcpOptionsSackCount = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_options_tcp_options_echo_value"];
        if (val != null) obj.TcpOptionsEchoValue = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_timestamp_tsval"];
        if (val != null) obj.TcpOptionsTimestampTsval = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_timestamp_tsecr"];
        if (val != null) obj.TcpOptionsTimestampTsecr = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_subtype"];
        if (val != null) obj.TcpOptionsMptcpSubtype = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_version"];
        if (val != null) obj.TcpOptionsMptcpVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_reserved"];
        if (val != null) obj.TcpOptionsMptcpReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_tcp_options_mptcp_flags"];
        if (val != null) obj.TcpOptionsMptcpFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_tcp_options_mptcp_backup_flag"];
        if (val != null) obj.TcpOptionsMptcpBackupFlag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_checksumreq_flags"];
        if (val != null) obj.TcpOptionsMptcpChecksumreqFlags = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_extensibility_flag"];
        if (val != null) obj.TcpOptionsMptcpExtensibilityFlag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_sha1_flag"];
        if (val != null) obj.TcpOptionsMptcpSha1Flag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_datafin_flag"];
        if (val != null) obj.TcpOptionsMptcpDatafinFlag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_dseqn8_flag"];
        if (val != null) obj.TcpOptionsMptcpDseqn8Flag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_dseqnpresent_flag"];
        if (val != null) obj.TcpOptionsMptcpDseqnpresentFlag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_dataack8_flag"];
        if (val != null) obj.TcpOptionsMptcpDataack8Flag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_dataackpresent_flag"];
        if (val != null) obj.TcpOptionsMptcpDataackpresentFlag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_options_mptcp_reserved_tcp_options_mptcp_reserved_flag"];
        if (val != null) obj.TcpOptionsMptcpReservedFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_tcp_options_mptcp_addrid"];
        if (val != null) obj.TcpOptionsMptcpAddrid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_sendkey"];
        if (val != null) obj.TcpOptionsMptcpSendkey = Convert.ToUInt64(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_recvkey"];
        if (val != null) obj.TcpOptionsMptcpRecvkey = Convert.ToUInt64(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_recvtok"];
        if (val != null) obj.TcpOptionsMptcpRecvtok = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_sendrand"];
        if (val != null) obj.TcpOptionsMptcpSendrand = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_sendtrunchmac"];
        if (val != null) obj.TcpOptionsMptcpSendtrunchmac = Convert.ToUInt64(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_sendhmac"];
        if (val != null) obj.TcpOptionsMptcpSendhmac = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_tcp_options_mptcp_addaddrtrunchmac"];
        if (val != null) obj.TcpOptionsMptcpAddaddrtrunchmac = Convert.ToUInt64(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_rawdataack"];
        if (val != null) obj.TcpOptionsMptcpRawdataack = Convert.ToUInt64(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_rawdataseqno"];
        if (val != null) obj.TcpOptionsMptcpRawdataseqno = Convert.ToUInt64(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_subflowseqno"];
        if (val != null) obj.TcpOptionsMptcpSubflowseqno = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_datalvllen"];
        if (val != null) obj.TcpOptionsMptcpDatalvllen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_checksum"];
        if (val != null) obj.TcpOptionsMptcpChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_tcp_options_mptcp_ipver"];
        if (val != null) obj.TcpOptionsMptcpIpver = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_mptcp_ipv4"];
        if (val != null) obj.TcpOptionsMptcpIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["tcp_tcp_options_mptcp_ipv6"];
        if (val != null) obj.TcpOptionsMptcpIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["tcp_tcp_options_mptcp_port"];
        if (val != null) obj.TcpOptionsMptcpPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_options_tcp_options_cc_value"];
        if (val != null) obj.TcpOptionsCcValue = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_md5_digest"];
        if (val != null) obj.TcpOptionsMd5Digest = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_tcp_options_qs_rate"];
        if (val != null) obj.TcpOptionsQsRate = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_qs_ttl_diff"];
        if (val != null) obj.TcpOptionsQsTtlDiff = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_scps_vector"];
        if (val != null) obj.TcpOptionsScpsVector = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_options_scps_binding_tcp_options_scps_binding_id"];
        if (val != null) obj.TcpOptionsScpsBindingId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_options_scps_binding_tcp_options_scps_binding_len"];
        if (val != null) obj.TcpOptionsScpsBindingLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_snack_offset"];
        if (val != null) obj.TcpOptionsSnackOffset = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_snack_size"];
        if (val != null) obj.TcpOptionsSnackSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_snack_le"];
        if (val != null) obj.TcpOptionsSnackLe = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_snack_re"];
        if (val != null) obj.TcpOptionsSnackRe = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_scpsflags_bets"];
        if (val != null) obj.TcpOptionsScpsflagsBets = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_tcp_options_scpsflags_snack1"];
        if (val != null) obj.TcpOptionsScpsflagsSnack1 = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_tcp_options_scpsflags_snack2"];
        if (val != null) obj.TcpOptionsScpsflagsSnack2 = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_tcp_options_scpsflags_compress"];
        if (val != null) obj.TcpOptionsScpsflagsCompress = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_tcp_options_scpsflags_nlts"];
        if (val != null) obj.TcpOptionsScpsflagsNlts = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_tcp_options_scpsflags_reserved"];
        if (val != null) obj.TcpOptionsScpsflagsReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_scps_binding"];
        if (val != null) obj.TcpOptionsScpsBinding = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_options_tcp_options_user_to_granularity"];
        if (val != null) obj.TcpOptionsUserToGranularity = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_tcp_options_user_to_val"];
        if (val != null) obj.TcpOptionsUserToVal = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_type1"];
        if (val != null) obj.TcpOptionsRvbdProbeType1 = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_type2"];
        if (val != null) obj.TcpOptionsRvbdProbeType2 = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_version"];
        if (val != null) obj.TcpOptionsRvbdProbeVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_version_raw"];
        if (val != null) obj.TcpOptionsRvbdProbeVersionRaw = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_len"];
        if (val != null) obj.TcpOptionsRvbdProbeLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_prober"];
        if (val != null) obj.TcpOptionsRvbdProbeProber = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_proxy_ip"];
        if (val != null) obj.TcpOptionsRvbdProbeProxyIp = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_proxy_port"];
        if (val != null) obj.TcpOptionsRvbdProbeProxyPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_appli_ver"];
        if (val != null) obj.TcpOptionsRvbdProbeAppliVer = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_client_ip"];
        if (val != null) obj.TcpOptionsRvbdProbeClientIp = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_storeid"];
        if (val != null) obj.TcpOptionsRvbdProbeStoreid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_flags"];
        if (val != null) obj.TcpOptionsRvbdProbeFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_options_rvbd_probe_flags_tcp_options_rvbd_probe_flags_notcfe"];
        if (val != null) obj.TcpOptionsRvbdProbeFlagsNotcfe = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_probe_flags_tcp_options_rvbd_probe_flags_last"];
        if (val != null) obj.TcpOptionsRvbdProbeFlagsLast = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_probe_flags_tcp_options_rvbd_probe_flags_probe"];
        if (val != null) obj.TcpOptionsRvbdProbeFlagsProbe = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_probe_flags_tcp_options_rvbd_probe_flags_ssl"];
        if (val != null) obj.TcpOptionsRvbdProbeFlagsSsl = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_probe_flags_tcp_options_rvbd_probe_flags_server"];
        if (val != null) obj.TcpOptionsRvbdProbeFlagsServer = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_tcp_options_rvbd_trpy_flags"];
        if (val != null) obj.TcpOptionsRvbdTrpyFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_options_rvbd_trpy_flags_tcp_options_rvbd_trpy_flags_fw_rst_probe"];
        if (val != null) obj.TcpOptionsRvbdTrpyFlagsFwRstProbe = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_trpy_flags_tcp_options_rvbd_trpy_flags_fw_rst_inner"];
        if (val != null) obj.TcpOptionsRvbdTrpyFlagsFwRstInner = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_trpy_flags_tcp_options_rvbd_trpy_flags_fw_rst"];
        if (val != null) obj.TcpOptionsRvbdTrpyFlagsFwRst = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_trpy_flags_tcp_options_rvbd_trpy_flags_chksum"];
        if (val != null) obj.TcpOptionsRvbdTrpyFlagsChksum = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_trpy_flags_tcp_options_rvbd_trpy_flags_oob"];
        if (val != null) obj.TcpOptionsRvbdTrpyFlagsOob = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_options_rvbd_trpy_flags_tcp_options_rvbd_trpy_flags_mode"];
        if (val != null) obj.TcpOptionsRvbdTrpyFlagsMode = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_tcp_options_rvbd_trpy_src_ip"];
        if (val != null) obj.TcpOptionsRvbdTrpySrcIp = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["tcp_tcp_options_rvbd_trpy_dst_ip"];
        if (val != null) obj.TcpOptionsRvbdTrpyDstIp = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["tcp_tcp_options_rvbd_trpy_src_port"];
        if (val != null) obj.TcpOptionsRvbdTrpySrcPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_trpy_dst_port"];
        if (val != null) obj.TcpOptionsRvbdTrpyDstPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_rvbd_trpy_client_port"];
        if (val != null) obj.TcpOptionsRvbdTrpyClientPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_options_tfo_request"];
        if (val != null) obj.TcpOptionsTfoRequest = default(Int32);
      }
      {
        var val = token["tcp_tcp_options_tfo_cookie"];
        if (val != null) obj.TcpOptionsTfoCookie = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_tcp_pdu_time"];
        if (val != null) obj.TcpPduTime = default(Int64);
      }
      {
        var val = token["tcp_tcp_pdu_size"];
        if (val != null) obj.TcpPduSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_pdu_last_frame"];
        if (val != null) obj.TcpPduLastFrame = default(Int64);
      }
      {
        var val = token["tcp_tcp_time_relative"];
        if (val != null) obj.TcpTimeRelative = default(Int64);
      }
      {
        var val = token["tcp_tcp_time_delta"];
        if (val != null) obj.TcpTimeDelta = default(Int64);
      }
      {
        var val = token["tcp_tcp_proc_srcuid"];
        if (val != null) obj.TcpProcSrcuid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_proc_srcpid"];
        if (val != null) obj.TcpProcSrcpid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_proc_srcuname"];
        if (val != null) obj.TcpProcSrcuname = val.Value<string>();
      }
      {
        var val = token["tcp_tcp_proc_srccmd"];
        if (val != null) obj.TcpProcSrccmd = val.Value<string>();
      }
      {
        var val = token["tcp_tcp_proc_dstuid"];
        if (val != null) obj.TcpProcDstuid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_proc_dstpid"];
        if (val != null) obj.TcpProcDstpid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_proc_dstuname"];
        if (val != null) obj.TcpProcDstuname = val.Value<string>();
      }
      {
        var val = token["tcp_tcp_proc_dstcmd"];
        if (val != null) obj.TcpProcDstcmd = val.Value<string>();
      }
      {
        var val = token["tcp_tcp_segment_data"];
        if (val != null) obj.TcpSegmentData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_tcp_payload"];
        if (val != null) obj.TcpPayload = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_options_scps_binding_tcp_options_scps_binding_data"];
        if (val != null) obj.TcpOptionsScpsBindingData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["tcp_tcp_options_rvbd_probe_reserved"];
        if (val != null) obj.TcpOptionsRvbdProbeReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_tcp_fin_retransmission"];
        if (val != null) obj.TcpFinRetransmission = default(Int64);
      }
      {
        var val = token["tcp_tcp_reset_cause"];
        if (val != null) obj.TcpResetCause = val.Value<string>();
      }
      return obj;
    }

                    public static Google.Protobuf.ByteString StringToBytes(string str)
                    {
                        var bstrArr = str.Split(':');
                        var byteArray = new byte[bstrArr.Length];
                        for (int i = 0; i < bstrArr.Length; i++)
                        {
                            byteArray[i] = Convert.ToByte(bstrArr[i], 16);
                        }
                        return Google.Protobuf.ByteString.CopyFrom( byteArray );
                    }
                    
  }
}

using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Core
{
  public sealed partial class Igmp
  {
    public static Igmp DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Igmp DecodeJson(JToken token)
    {
      var obj = new Igmp();
      {
        var val = token["igmp_igmp_type"];
        if (val != null) obj.IgmpType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["igmp_igmp_reserved"];
        if (val != null) obj.IgmpReserved = StringToBytes(val.Value<string>());
      }
      {
        var val = token["igmp_igmp_version"];
        if (val != null) obj.IgmpVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_group_type"];
        if (val != null) obj.IgmpGroupType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_reply"];
        if (val != null) obj.IgmpReply = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_reply_igmp_reply_pending"];
        if (val != null) obj.IgmpReplyPending = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_checksum"];
        if (val != null) obj.IgmpChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["igmp_checksum_igmp_checksum_status"];
        if (val != null) obj.IgmpChecksumStatus = default(UInt32);
      }
      {
        var val = token["igmp_igmp_identifier"];
        if (val != null) obj.IgmpIdentifier = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_access_key"];
        if (val != null) obj.IgmpAccessKey = StringToBytes(val.Value<string>());
      }
      {
        var val = token["igmp_igmp_max_resp"];
        if (val != null) obj.IgmpMaxResp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_s"];
        if (val != null) obj.IgmpS = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["igmp_igmp_qrv"];
        if (val != null) obj.IgmpQrv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_qqic"];
        if (val != null) obj.IgmpQqic = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_num_src"];
        if (val != null) obj.IgmpNumSrc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_saddr"];
        if (val != null) obj.IgmpSaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["igmp_igmp_num_grp_recs"];
        if (val != null) obj.IgmpNumGrpRecs = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_record_type"];
        if (val != null) obj.IgmpRecordType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_aux_data_len"];
        if (val != null) obj.IgmpAuxDataLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_maddr"];
        if (val != null) obj.IgmpMaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["igmp_igmp_aux_data"];
        if (val != null) obj.IgmpAuxData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["igmp_igmp_data"];
        if (val != null) obj.IgmpData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["igmp_max_resp_igmp_max_resp_exp"];
        if (val != null) obj.IgmpMaxRespExp = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["igmp_max_resp_igmp_max_resp_mant"];
        if (val != null) obj.IgmpMaxRespMant = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["igmp_igmp_mtrace_max_hops"];
        if (val != null) obj.IgmpMtraceMaxHops = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_saddr"];
        if (val != null) obj.IgmpMtraceSaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["igmp_igmp_mtrace_raddr"];
        if (val != null) obj.IgmpMtraceRaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["igmp_igmp_mtrace_rspaddr"];
        if (val != null) obj.IgmpMtraceRspaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["igmp_igmp_mtrace_resp_ttl"];
        if (val != null) obj.IgmpMtraceRespTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_q_id"];
        if (val != null) obj.IgmpMtraceQId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_q_arrival"];
        if (val != null) obj.IgmpMtraceQArrival = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_q_inaddr"];
        if (val != null) obj.IgmpMtraceQInaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["igmp_igmp_mtrace_q_outaddr"];
        if (val != null) obj.IgmpMtraceQOutaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["igmp_igmp_mtrace_q_prevrtr"];
        if (val != null) obj.IgmpMtraceQPrevrtr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["igmp_igmp_mtrace_q_inpkt"];
        if (val != null) obj.IgmpMtraceQInpkt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_q_outpkt"];
        if (val != null) obj.IgmpMtraceQOutpkt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_q_total"];
        if (val != null) obj.IgmpMtraceQTotal = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_q_rtg_proto"];
        if (val != null) obj.IgmpMtraceQRtgProto = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_q_fwd_ttl"];
        if (val != null) obj.IgmpMtraceQFwdTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["igmp_igmp_mtrace_q_mbz"];
        if (val != null) obj.IgmpMtraceQMbz = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["igmp_igmp_mtrace_q_s"];
        if (val != null) obj.IgmpMtraceQS = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["igmp_igmp_mtrace_q_src_mask"];
        if (val != null) obj.IgmpMtraceQSrcMask = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["igmp_igmp_mtrace_q_fwd_code"];
        if (val != null) obj.IgmpMtraceQFwdCode = Convert.ToUInt32(val.Value<string>(), 16);
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

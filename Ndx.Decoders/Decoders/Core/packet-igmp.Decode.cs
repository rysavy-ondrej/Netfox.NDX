// This is file was generated by netdx on (2017-11-24 12:35:28 PM.
using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpType = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["igmp_igmp_reserved"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpReserved = StringToBytes(propValue); }
      }
      {
        var val = token["igmp_igmp_version"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpVersion = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_group_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpGroupType = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_reply"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpReply = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_reply_igmp_reply_pending"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpReplyPending = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_checksum"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpChecksum = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["igmp_checksum_igmp_checksum_status"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpChecksumStatus = default(UInt32); }
      }
      {
        var val = token["igmp_igmp_identifier"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpIdentifier = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_access_key"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpAccessKey = StringToBytes(propValue); }
      }
      {
        var val = token["igmp_igmp_max_resp"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMaxResp = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_s"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpS = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["igmp_igmp_qrv"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpQrv = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_qqic"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpQqic = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_num_src"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpNumSrc = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_saddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpSaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["igmp_igmp_num_grp_recs"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpNumGrpRecs = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_record_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpRecordType = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_aux_data_len"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpAuxDataLen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_maddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["igmp_igmp_aux_data"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpAuxData = StringToBytes(propValue); }
      }
      {
        var val = token["igmp_igmp_data"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpData = StringToBytes(propValue); }
      }
      {
        var val = token["igmp_max_resp_igmp_max_resp_exp"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMaxRespExp = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["igmp_max_resp_igmp_max_resp_mant"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMaxRespMant = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["igmp_igmp_mtrace_max_hops"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceMaxHops = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_saddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceSaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["igmp_igmp_mtrace_raddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceRaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["igmp_igmp_mtrace_rspaddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceRspaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["igmp_igmp_mtrace_resp_ttl"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceRespTtl = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_id"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQId = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_arrival"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQArrival = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_inaddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQInaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_outaddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQOutaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_prevrtr"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQPrevrtr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_inpkt"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQInpkt = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_outpkt"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQOutpkt = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_total"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQTotal = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_rtg_proto"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQRtgProto = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_fwd_ttl"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQFwdTtl = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_mbz"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQMbz = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_s"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQS = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_src_mask"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQSrcMask = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["igmp_igmp_mtrace_q_fwd_code"];
        if (val != null) { var propValue = val.Value<string>(); obj.IgmpMtraceQFwdCode = Convert.ToUInt32(propValue, 16); }
      }
      return obj;
    }
    public static Igmp DecodeJson(JsonTextReader reader)                        
    {                                                                                     
        if (reader.TokenType != JsonToken.StartObject) return null;                       
        var obj = new Igmp();                                                   
int openObjects = 0;
                    while (reader.TokenType != JsonToken.None)
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            openObjects++;
                        }
                        if (reader.TokenType == JsonToken.EndObject)
                        {
                            openObjects--;
                            if (openObjects == 0) break;
                        }
                        if (reader.TokenType == JsonToken.PropertyName)
                        {
                            string propName = (string)reader.Value;
                            reader.Read();
                            if (reader.TokenType != JsonToken.String) { continue; }
                            string propValue = (string)reader.Value;
                            SetField(obj, propName, propValue);
                        }

                        reader.Read();
                    }
                    reader.Read();
                    return obj;
                    }
                    
    static void SetField(Igmp obj, string propName, string propValue)           
    {                                                                                     
      switch (propName)                                                                   
      {                                                                                   
      case "igmp_igmp_type": obj.IgmpType = Convert.ToUInt32(propValue, 16); break;
      case "igmp_igmp_reserved": obj.IgmpReserved = StringToBytes(propValue); break;
      case "igmp_igmp_version": obj.IgmpVersion = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_group_type": obj.IgmpGroupType = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_reply": obj.IgmpReply = Convert.ToUInt32(propValue, 10); break;
      case "igmp_reply_igmp_reply_pending": obj.IgmpReplyPending = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_checksum": obj.IgmpChecksum = Convert.ToUInt32(propValue, 16); break;
      case "igmp_checksum_igmp_checksum_status": obj.IgmpChecksumStatus = default(UInt32); break;
      case "igmp_igmp_identifier": obj.IgmpIdentifier = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_access_key": obj.IgmpAccessKey = StringToBytes(propValue); break;
      case "igmp_igmp_max_resp": obj.IgmpMaxResp = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_s": obj.IgmpS = Convert.ToInt32(propValue, 10) != 0; break;
      case "igmp_igmp_qrv": obj.IgmpQrv = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_qqic": obj.IgmpQqic = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_num_src": obj.IgmpNumSrc = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_saddr": obj.IgmpSaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "igmp_igmp_num_grp_recs": obj.IgmpNumGrpRecs = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_record_type": obj.IgmpRecordType = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_aux_data_len": obj.IgmpAuxDataLen = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_maddr": obj.IgmpMaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "igmp_igmp_aux_data": obj.IgmpAuxData = StringToBytes(propValue); break;
      case "igmp_igmp_data": obj.IgmpData = StringToBytes(propValue); break;
      case "igmp_max_resp_igmp_max_resp_exp": obj.IgmpMaxRespExp = Convert.ToUInt32(propValue, 16); break;
      case "igmp_max_resp_igmp_max_resp_mant": obj.IgmpMaxRespMant = Convert.ToUInt32(propValue, 16); break;
      case "igmp_igmp_mtrace_max_hops": obj.IgmpMtraceMaxHops = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_saddr": obj.IgmpMtraceSaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "igmp_igmp_mtrace_raddr": obj.IgmpMtraceRaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "igmp_igmp_mtrace_rspaddr": obj.IgmpMtraceRspaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "igmp_igmp_mtrace_resp_ttl": obj.IgmpMtraceRespTtl = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_q_id": obj.IgmpMtraceQId = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_q_arrival": obj.IgmpMtraceQArrival = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_q_inaddr": obj.IgmpMtraceQInaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "igmp_igmp_mtrace_q_outaddr": obj.IgmpMtraceQOutaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "igmp_igmp_mtrace_q_prevrtr": obj.IgmpMtraceQPrevrtr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "igmp_igmp_mtrace_q_inpkt": obj.IgmpMtraceQInpkt = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_q_outpkt": obj.IgmpMtraceQOutpkt = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_q_total": obj.IgmpMtraceQTotal = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_q_rtg_proto": obj.IgmpMtraceQRtgProto = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_q_fwd_ttl": obj.IgmpMtraceQFwdTtl = Convert.ToUInt32(propValue, 10); break;
      case "igmp_igmp_mtrace_q_mbz": obj.IgmpMtraceQMbz = Convert.ToUInt32(propValue, 16); break;
      case "igmp_igmp_mtrace_q_s": obj.IgmpMtraceQS = Convert.ToUInt32(propValue, 16); break;
      case "igmp_igmp_mtrace_q_src_mask": obj.IgmpMtraceQSrcMask = Convert.ToUInt32(propValue, 16); break;
      case "igmp_igmp_mtrace_q_fwd_code": obj.IgmpMtraceQFwdCode = Convert.ToUInt32(propValue, 16); break;
      }
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

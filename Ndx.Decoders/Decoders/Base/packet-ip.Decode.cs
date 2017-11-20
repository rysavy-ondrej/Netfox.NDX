using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Basic
{
  public sealed partial class Ip
  {
    public static Ip DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Ip DecodeJson(JToken token)
    {
      var obj = new Ip();
      {
        var val = token["ip_ip_version"];
        if (val != null) obj.IpVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_hdr_len"];
        if (val != null) obj.IpHdrLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_dsfield"];
        if (val != null) obj.IpDsfield = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_dsfield_ip_dsfield_dscp"];
        if (val != null) obj.IpDsfieldDscp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_dsfield_ip_dsfield_ecn"];
        if (val != null) obj.IpDsfieldEcn = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_tos"];
        if (val != null) obj.IpTos = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_tos_ip_tos_precedence"];
        if (val != null) obj.IpTosPrecedence = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_tos_ip_tos_delay"];
        if (val != null) obj.IpTosDelay = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_tos_ip_tos_throughput"];
        if (val != null) obj.IpTosThroughput = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_tos_ip_tos_reliability"];
        if (val != null) obj.IpTosReliability = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_tos_ip_tos_cost"];
        if (val != null) obj.IpTosCost = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_ip_len"];
        if (val != null) obj.IpLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_id"];
        if (val != null) obj.IpId = default(UInt32);
      }
      {
        var val = token["ip_ip_dst"];
        if (val != null) obj.IpDst = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_dst_host"];
        if (val != null) obj.IpDstHost = val.Value<string>();
      }
      {
        var val = token["ip_ip_src"];
        if (val != null) obj.IpSrc = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_src_host"];
        if (val != null) obj.IpSrcHost = val.Value<string>();
      }
      {
        var val = token["ip_ip_addr"];
        if (val != null) obj.IpAddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_host"];
        if (val != null) obj.IpHost = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_country"];
        if (val != null) obj.IpGeoipCountry = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_city"];
        if (val != null) obj.IpGeoipCity = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_org"];
        if (val != null) obj.IpGeoipOrg = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_isp"];
        if (val != null) obj.IpGeoipIsp = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_asnum"];
        if (val != null) obj.IpGeoipAsnum = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_lat"];
        if (val != null) obj.IpGeoipLat = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ip_ip_geoip_lon"];
        if (val != null) obj.IpGeoipLon = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ip_ip_geoip_src_country"];
        if (val != null) obj.IpGeoipSrcCountry = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_src_city"];
        if (val != null) obj.IpGeoipSrcCity = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_src_org"];
        if (val != null) obj.IpGeoipSrcOrg = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_src_isp"];
        if (val != null) obj.IpGeoipSrcIsp = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_src_asnum"];
        if (val != null) obj.IpGeoipSrcAsnum = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_src_lat"];
        if (val != null) obj.IpGeoipSrcLat = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ip_ip_geoip_src_lon"];
        if (val != null) obj.IpGeoipSrcLon = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ip_ip_geoip_dst_country"];
        if (val != null) obj.IpGeoipDstCountry = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_dst_city"];
        if (val != null) obj.IpGeoipDstCity = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_dst_org"];
        if (val != null) obj.IpGeoipDstOrg = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_dst_isp"];
        if (val != null) obj.IpGeoipDstIsp = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_dst_asnum"];
        if (val != null) obj.IpGeoipDstAsnum = val.Value<string>();
      }
      {
        var val = token["ip_ip_geoip_dst_lat"];
        if (val != null) obj.IpGeoipDstLat = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ip_ip_geoip_dst_lon"];
        if (val != null) obj.IpGeoipDstLon = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ip_ip_flags"];
        if (val != null) obj.IpFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_flags_ip_flags_sf"];
        if (val != null) obj.IpFlagsSf = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_flags_ip_flags_rb"];
        if (val != null) obj.IpFlagsRb = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_flags_ip_flags_df"];
        if (val != null) obj.IpFlagsDf = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_flags_ip_flags_mf"];
        if (val != null) obj.IpFlagsMf = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_ip_frag_offset"];
        if (val != null) obj.IpFragOffset = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_ttl"];
        if (val != null) obj.IpTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_proto"];
        if (val != null) obj.IpProto = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_checksum"];
        if (val != null) obj.IpChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_checksum_calculated"];
        if (val != null) obj.IpChecksumCalculated = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_checksum_ip_checksum_status"];
        if (val != null) obj.IpChecksumStatus = default(UInt32);
      }
      {
        var val = token["ip_ip_opt_type"];
        if (val != null) obj.IpOptType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_opt_type_ip_opt_type_copy"];
        if (val != null) obj.IpOptTypeCopy = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_opt_type_ip_opt_type_class"];
        if (val != null) obj.IpOptTypeClass = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_opt_type_ip_opt_type_number"];
        if (val != null) obj.IpOptTypeNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_len"];
        if (val != null) obj.IpOptLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_ptr"];
        if (val != null) obj.IpOptPtr = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_sid"];
        if (val != null) obj.IpOptSid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_mtu"];
        if (val != null) obj.IpOptMtu = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_id_number"];
        if (val != null) obj.IpOptIdNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_ohc"];
        if (val != null) obj.IpOptOhc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_rhc"];
        if (val != null) obj.IpOptRhc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_originator"];
        if (val != null) obj.IpOptOriginator = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_opt_ra"];
        if (val != null) obj.IpOptRa = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_addr"];
        if (val != null) obj.IpOptAddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_opt_padding"];
        if (val != null) obj.IpOptPadding = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ip_ip_opt_qs_func"];
        if (val != null) obj.IpOptQsFunc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_qs_rate"];
        if (val != null) obj.IpOptQsRate = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_qs_ttl"];
        if (val != null) obj.IpOptQsTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_qs_ttl_diff"];
        if (val != null) obj.IpOptQsTtlDiff = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_qs_unused"];
        if (val != null) obj.IpOptQsUnused = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_qs_nonce"];
        if (val != null) obj.IpOptQsNonce = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_opt_qs_reserved"];
        if (val != null) obj.IpOptQsReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_opt_sec_rfc791_sec"];
        if (val != null) obj.IpOptSecRfc791Sec = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_opt_sec_rfc791_comp"];
        if (val != null) obj.IpOptSecRfc791Comp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_sec_rfc791_hr"];
        if (val != null) obj.IpOptSecRfc791Hr = val.Value<string>();
      }
      {
        var val = token["ip_ip_opt_sec_rfc791_tcc"];
        if (val != null) obj.IpOptSecRfc791Tcc = val.Value<string>();
      }
      {
        var val = token["ip_ip_opt_sec_cl"];
        if (val != null) obj.IpOptSecCl = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_opt_sec_prot_auth_flags"];
        if (val != null) obj.IpOptSecProtAuthFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_opt_sec_prot_auth_genser"];
        if (val != null) obj.IpOptSecProtAuthGenser = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_ip_opt_sec_prot_auth_siop_esi"];
        if (val != null) obj.IpOptSecProtAuthSiopEsi = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_ip_opt_sec_prot_auth_sci"];
        if (val != null) obj.IpOptSecProtAuthSci = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_ip_opt_sec_prot_auth_nsa"];
        if (val != null) obj.IpOptSecProtAuthNsa = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_ip_opt_sec_prot_auth_doe"];
        if (val != null) obj.IpOptSecProtAuthDoe = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_ip_opt_sec_prot_auth_unassigned"];
        if (val != null) obj.IpOptSecProtAuthUnassigned = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_opt_sec_prot_auth_fti"];
        if (val != null) obj.IpOptSecProtAuthFti = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_ip_opt_ext_sec_add_sec_info_format_code"];
        if (val != null) obj.IpOptExtSecAddSecInfoFormatCode = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_opt_ext_sec_add_sec_info"];
        if (val != null) obj.IpOptExtSecAddSecInfo = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ip_ip_rec_rt"];
        if (val != null) obj.IpRecRt = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_rec_rt_host"];
        if (val != null) obj.IpRecRtHost = val.Value<string>();
      }
      {
        var val = token["ip_ip_cur_rt"];
        if (val != null) obj.IpCurRt = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_cur_rt_host"];
        if (val != null) obj.IpCurRtHost = val.Value<string>();
      }
      {
        var val = token["ip_ip_src_rt"];
        if (val != null) obj.IpSrcRt = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_src_rt_host"];
        if (val != null) obj.IpSrcRtHost = val.Value<string>();
      }
      {
        var val = token["ip_ip_empty_rt"];
        if (val != null) obj.IpEmptyRt = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ip_ip_empty_rt_host"];
        if (val != null) obj.IpEmptyRtHost = val.Value<string>();
      }
      {
        var val = token["ip_ip_cipso_tag_type"];
        if (val != null) obj.IpCipsoTagType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_fragment_ip_fragment_overlap"];
        if (val != null) obj.IpFragmentOverlap = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_fragment_overlap_ip_fragment_overlap_conflict"];
        if (val != null) obj.IpFragmentOverlapConflict = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_fragment_ip_fragment_multipletails"];
        if (val != null) obj.IpFragmentMultipletails = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_fragment_ip_fragment_toolongfragment"];
        if (val != null) obj.IpFragmentToolongfragment = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ip_fragment_ip_fragment_error"];
        if (val != null) obj.IpFragmentError = default(Int64);
      }
      {
        var val = token["ip_fragment_ip_fragment_count"];
        if (val != null) obj.IpFragmentCount = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_fragment"];
        if (val != null) obj.IpFragment = default(Int64);
      }
      {
        var val = token["ip_ip_fragments"];
        if (val != null) obj.IpFragments = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ip_ip_reassembled_in"];
        if (val != null) obj.IpReassembledIn = default(Int64);
      }
      {
        var val = token["ip_ip_reassembled_length"];
        if (val != null) obj.IpReassembledLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_reassembled_data"];
        if (val != null) obj.IpReassembledData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ip_ip_cipso_doi"];
        if (val != null) obj.IpCipsoDoi = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_cipso_sensitivity_level"];
        if (val != null) obj.IpCipsoSensitivityLevel = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_cipso_categories"];
        if (val != null) obj.IpCipsoCategories = val.Value<string>();
      }
      {
        var val = token["ip_ip_cipso_tag_data"];
        if (val != null) obj.IpCipsoTagData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ip_ip_opt_overflow"];
        if (val != null) obj.IpOptOverflow = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_flag"];
        if (val != null) obj.IpOptFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ip_ip_opt_time_stamp"];
        if (val != null) obj.IpOptTimeStamp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ip_ip_opt_time_stamp_addr"];
        if (val != null) obj.IpOptTimeStampAddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
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

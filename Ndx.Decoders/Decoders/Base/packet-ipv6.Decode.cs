using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Base
{
  public sealed partial class Ipv6
  {
    public static Ipv6 DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Ipv6 DecodeJson(JToken token)
    {
      var obj = new Ipv6();
      {
        var val = token["ipv6_ipv6_version"];
        if (val != null) obj.Ipv6Version = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_tclass"];
        if (val != null) obj.Ipv6Tclass = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_tclass_ipv6_tclass_dscp"];
        if (val != null) obj.Ipv6TclassDscp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_tclass_ipv6_tclass_ecn"];
        if (val != null) obj.Ipv6TclassEcn = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_flow"];
        if (val != null) obj.Ipv6Flow = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_plen"];
        if (val != null) obj.Ipv6Plen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_nxt"];
        if (val != null) obj.Ipv6Nxt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_hlim"];
        if (val != null) obj.Ipv6Hlim = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_src"];
        if (val != null) obj.Ipv6Src = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_src_host"];
        if (val != null) obj.Ipv6SrcHost = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_src_sa_mac"];
        if (val != null) obj.Ipv6SrcSaMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_src_isatap_ipv4"];
        if (val != null) obj.Ipv6SrcIsatapIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_src_6to4_gw_ipv4"];
        if (val != null) obj.Ipv6Src6To4GwIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_src_6to4_sla_id"];
        if (val != null) obj.Ipv6Src6To4SlaId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_src_ts_ipv4"];
        if (val != null) obj.Ipv6SrcTsIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_src_tc_port"];
        if (val != null) obj.Ipv6SrcTcPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_src_tc_ipv4"];
        if (val != null) obj.Ipv6SrcTcIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_src_embed_ipv4"];
        if (val != null) obj.Ipv6SrcEmbedIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_dst"];
        if (val != null) obj.Ipv6Dst = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_dst_host"];
        if (val != null) obj.Ipv6DstHost = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_dst_sa_mac"];
        if (val != null) obj.Ipv6DstSaMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_dst_isatap_ipv4"];
        if (val != null) obj.Ipv6DstIsatapIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_dst_6to4_gw_ipv4"];
        if (val != null) obj.Ipv6Dst6To4GwIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_dst_6to4_sla_id"];
        if (val != null) obj.Ipv6Dst6To4SlaId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_dst_ts_ipv4"];
        if (val != null) obj.Ipv6DstTsIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_dst_tc_port"];
        if (val != null) obj.Ipv6DstTcPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_dst_tc_ipv4"];
        if (val != null) obj.Ipv6DstTcIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_dst_embed_ipv4"];
        if (val != null) obj.Ipv6DstEmbedIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_addr"];
        if (val != null) obj.Ipv6Addr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_host"];
        if (val != null) obj.Ipv6Host = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_sa_mac"];
        if (val != null) obj.Ipv6SaMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_isatap_ipv4"];
        if (val != null) obj.Ipv6IsatapIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_6to4_gw_ipv4"];
        if (val != null) obj.Ipv66To4GwIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_6to4_sla_id"];
        if (val != null) obj.Ipv66To4SlaId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_ts_ipv4"];
        if (val != null) obj.Ipv6TsIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_tc_port"];
        if (val != null) obj.Ipv6TcPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_tc_ipv4"];
        if (val != null) obj.Ipv6TcIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_embed_ipv4"];
        if (val != null) obj.Ipv6EmbedIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_geoip_country"];
        if (val != null) obj.Ipv6GeoipCountry = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_city"];
        if (val != null) obj.Ipv6GeoipCity = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_org"];
        if (val != null) obj.Ipv6GeoipOrg = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_isp"];
        if (val != null) obj.Ipv6GeoipIsp = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_asnum"];
        if (val != null) obj.Ipv6GeoipAsnum = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_lat"];
        if (val != null) obj.Ipv6GeoipLat = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_geoip_lon"];
        if (val != null) obj.Ipv6GeoipLon = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_geoip_src_country"];
        if (val != null) obj.Ipv6GeoipSrcCountry = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_src_city"];
        if (val != null) obj.Ipv6GeoipSrcCity = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_src_org"];
        if (val != null) obj.Ipv6GeoipSrcOrg = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_src_isp"];
        if (val != null) obj.Ipv6GeoipSrcIsp = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_src_asnum"];
        if (val != null) obj.Ipv6GeoipSrcAsnum = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_src_lat"];
        if (val != null) obj.Ipv6GeoipSrcLat = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_geoip_src_lon"];
        if (val != null) obj.Ipv6GeoipSrcLon = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_geoip_dst_country"];
        if (val != null) obj.Ipv6GeoipDstCountry = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_dst_city"];
        if (val != null) obj.Ipv6GeoipDstCity = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_dst_org"];
        if (val != null) obj.Ipv6GeoipDstOrg = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_dst_isp"];
        if (val != null) obj.Ipv6GeoipDstIsp = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_dst_asnum"];
        if (val != null) obj.Ipv6GeoipDstAsnum = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_geoip_dst_lat"];
        if (val != null) obj.Ipv6GeoipDstLat = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_geoip_dst_lon"];
        if (val != null) obj.Ipv6GeoipDstLon = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_opt"];
        if (val != null) obj.Ipv6Opt = default(Int32);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_type"];
        if (val != null) obj.Ipv6OptType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_opt_type_ipv6_opt_type_action"];
        if (val != null) obj.Ipv6OptTypeAction = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_type_ipv6_opt_type_change"];
        if (val != null) obj.Ipv6OptTypeChange = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_opt_type_ipv6_opt_type_rest"];
        if (val != null) obj.Ipv6OptTypeRest = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_length"];
        if (val != null) obj.Ipv6OptLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_pad1"];
        if (val != null) obj.Ipv6OptPad1 = default(Int32);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_padn"];
        if (val != null) obj.Ipv6OptPadn = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_opt_ipv6_opt_router_alert"];
        if (val != null) obj.Ipv6OptRouterAlert = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_tel"];
        if (val != null) obj.Ipv6OptTel = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_jumbo"];
        if (val != null) obj.Ipv6OptJumbo = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_calipso_doi"];
        if (val != null) obj.Ipv6OptCalipsoDoi = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_calipso_cmpt_length"];
        if (val != null) obj.Ipv6OptCalipsoCmptLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_calipso_sens_level"];
        if (val != null) obj.Ipv6OptCalipsoSensLevel = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_calipso_checksum"];
        if (val != null) obj.Ipv6OptCalipsoChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_opt_calipso_cmpt_bitmap"];
        if (val != null) obj.Ipv6OptCalipsoCmptBitmap = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_opt_smf_dpd_hash_bit"];
        if (val != null) obj.Ipv6OptSmfDpdHashBit = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_ipv6_opt_smf_dpd_tid_type"];
        if (val != null) obj.Ipv6OptSmfDpdTidType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_smf_dpd_tid_len"];
        if (val != null) obj.Ipv6OptSmfDpdTidLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_smf_dpd_tagger_id"];
        if (val != null) obj.Ipv6OptSmfDpdTaggerId = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_opt_smf_dpd_ident"];
        if (val != null) obj.Ipv6OptSmfDpdIdent = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_opt_smf_dpd_hav"];
        if (val != null) obj.Ipv6OptSmfDpdHav = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_opt_pdm_scale_dtlr"];
        if (val != null) obj.Ipv6OptPdmScaleDtlr = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_pdm_scale_dtls"];
        if (val != null) obj.Ipv6OptPdmScaleDtls = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_pdm_psn_this_pkt"];
        if (val != null) obj.Ipv6OptPdmPsnThisPkt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_pdm_psn_last_recv"];
        if (val != null) obj.Ipv6OptPdmPsnLastRecv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_pdm_delta_last_recv"];
        if (val != null) obj.Ipv6OptPdmDeltaLastRecv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_pdm_delta_last_sent"];
        if (val != null) obj.Ipv6OptPdmDeltaLastSent = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_qs_func"];
        if (val != null) obj.Ipv6OptQsFunc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_qs_rate"];
        if (val != null) obj.Ipv6OptQsRate = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_qs_ttl"];
        if (val != null) obj.Ipv6OptQsTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_qs_ttl_diff"];
        if (val != null) obj.Ipv6OptQsTtlDiff = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_qs_unused"];
        if (val != null) obj.Ipv6OptQsUnused = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_qs_nonce"];
        if (val != null) obj.Ipv6OptQsNonce = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_qs_reserved"];
        if (val != null) obj.Ipv6OptQsReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_opt_mipv6_home_address"];
        if (val != null) obj.Ipv6OptMipv6HomeAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_opt_rpl_flag"];
        if (val != null) obj.Ipv6OptRplFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_opt_rpl_flag_ipv6_opt_rpl_flag_o"];
        if (val != null) obj.Ipv6OptRplFlagO = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_opt_rpl_flag_ipv6_opt_rpl_flag_r"];
        if (val != null) obj.Ipv6OptRplFlagR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_opt_rpl_flag_ipv6_opt_rpl_flag_f"];
        if (val != null) obj.Ipv6OptRplFlagF = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_opt_rpl_flag_ipv6_opt_rpl_flag_rsv"];
        if (val != null) obj.Ipv6OptRplFlagRsv = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_opt_rpl_instance_id"];
        if (val != null) obj.Ipv6OptRplInstanceId = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_opt_rpl_sender_rank"];
        if (val != null) obj.Ipv6OptRplSenderRank = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_ilnp_nonce"];
        if (val != null) obj.Ipv6OptIlnpNonce = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_opt_lio_length"];
        if (val != null) obj.Ipv6OptLioLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_lio_line_id"];
        if (val != null) obj.Ipv6OptLioLineId = val.Value<string>();
      }
      {
        var val = token["ipv6_ipv6_opt_mpl_flag"];
        if (val != null) obj.Ipv6OptMplFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_opt_mpl_flag_ipv6_opt_mpl_flag_s"];
        if (val != null) obj.Ipv6OptMplFlagS = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_opt_mpl_flag_ipv6_opt_mpl_flag_m"];
        if (val != null) obj.Ipv6OptMplFlagM = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_opt_mpl_flag_ipv6_opt_mpl_flag_v"];
        if (val != null) obj.Ipv6OptMplFlagV = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_opt_mpl_flag_ipv6_opt_mpl_flag_rsv"];
        if (val != null) obj.Ipv6OptMplFlagRsv = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_opt_mpl_sequence"];
        if (val != null) obj.Ipv6OptMplSequence = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_opt_mpl_seed_id"];
        if (val != null) obj.Ipv6OptMplSeedId = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_opt_dff_flags"];
        if (val != null) obj.Ipv6OptDffFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_opt_dff_flag_ver"];
        if (val != null) obj.Ipv6OptDffFlagVer = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_opt_dff_flag_dup"];
        if (val != null) obj.Ipv6OptDffFlagDup = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_ipv6_opt_dff_flag_ret"];
        if (val != null) obj.Ipv6OptDffFlagRet = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_ipv6_opt_dff_flag_rsv"];
        if (val != null) obj.Ipv6OptDffFlagRsv = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_opt_dff_sequence_number"];
        if (val != null) obj.Ipv6OptDffSequenceNumber = default(UInt32);
      }
      {
        var val = token["ipv6_opt_ipv6_opt_experimental"];
        if (val != null) obj.Ipv6OptExperimental = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_opt_unknown_data"];
        if (val != null) obj.Ipv6OptUnknownData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_opt_ipv6_opt_unknown"];
        if (val != null) obj.Ipv6OptUnknown = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_fragment"];
        if (val != null) obj.Ipv6Fragment = default(Int64);
      }
      {
        var val = token["ipv6_fragment_ipv6_fragment_overlap"];
        if (val != null) obj.Ipv6FragmentOverlap = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_fragment_overlap_ipv6_fragment_overlap_conflict"];
        if (val != null) obj.Ipv6FragmentOverlapConflict = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_fragment_ipv6_fragment_multipletails"];
        if (val != null) obj.Ipv6FragmentMultipletails = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_fragment_ipv6_fragment_toolongfragment"];
        if (val != null) obj.Ipv6FragmentToolongfragment = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_fragment_ipv6_fragment_error"];
        if (val != null) obj.Ipv6FragmentError = default(Int64);
      }
      {
        var val = token["ipv6_fragment_ipv6_fragment_count"];
        if (val != null) obj.Ipv6FragmentCount = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_fragments"];
        if (val != null) obj.Ipv6Fragments = default(Int32);
      }
      {
        var val = token["ipv6_ipv6_reassembled_in"];
        if (val != null) obj.Ipv6ReassembledIn = default(Int64);
      }
      {
        var val = token["ipv6_ipv6_reassembled_length"];
        if (val != null) obj.Ipv6ReassembledLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_reassembled_data"];
        if (val != null) obj.Ipv6ReassembledData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_hopopts_nxt"];
        if (val != null) obj.Ipv6HopoptsNxt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_hopopts_len"];
        if (val != null) obj.Ipv6HopoptsLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_hopopts_len_oct"];
        if (val != null) obj.Ipv6HopoptsLenOct = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_dstopts_nxt"];
        if (val != null) obj.Ipv6DstoptsNxt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_dstopts_len"];
        if (val != null) obj.Ipv6DstoptsLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_dstopts_len_oct"];
        if (val != null) obj.Ipv6DstoptsLenOct = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_nxt"];
        if (val != null) obj.Ipv6RoutingNxt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_len"];
        if (val != null) obj.Ipv6RoutingLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_len_oct"];
        if (val != null) obj.Ipv6RoutingLenOct = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_type"];
        if (val != null) obj.Ipv6RoutingType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_segleft"];
        if (val != null) obj.Ipv6RoutingSegleft = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_unknown_data"];
        if (val != null) obj.Ipv6RoutingUnknownData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_routing_src_reserved"];
        if (val != null) obj.Ipv6RoutingSrcReserved = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_routing_src_addr"];
        if (val != null) obj.Ipv6RoutingSrcAddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_routing_mipv6_reserved"];
        if (val != null) obj.Ipv6RoutingMipv6Reserved = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_routing_mipv6_home_address"];
        if (val != null) obj.Ipv6RoutingMipv6HomeAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_routing_rpl_cmprI"];
        if (val != null) obj.Ipv6RoutingRplCmpri = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_rpl_cmprE"];
        if (val != null) obj.Ipv6RoutingRplCmpre = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_rpl_pad"];
        if (val != null) obj.Ipv6RoutingRplPad = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_rpl_reserved"];
        if (val != null) obj.Ipv6RoutingRplReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_rpl_addr_count"];
        if (val != null) obj.Ipv6RoutingRplAddrCount = Convert.ToInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_rpl_address"];
        if (val != null) obj.Ipv6RoutingRplAddress = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_routing_rpl_full_address"];
        if (val != null) obj.Ipv6RoutingRplFullAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_routing_srh_first_segment"];
        if (val != null) obj.Ipv6RoutingSrhFirstSegment = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_routing_srh_flags"];
        if (val != null) obj.Ipv6RoutingSrhFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_routing_srh_flag_unused1"];
        if (val != null) obj.Ipv6RoutingSrhFlagUnused1 = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_routing_srh_flag_p"];
        if (val != null) obj.Ipv6RoutingSrhFlagP = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_ipv6_routing_srh_flag_o"];
        if (val != null) obj.Ipv6RoutingSrhFlagO = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_ipv6_routing_srh_flag_a"];
        if (val != null) obj.Ipv6RoutingSrhFlagA = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_ipv6_routing_srh_flag_h"];
        if (val != null) obj.Ipv6RoutingSrhFlagH = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_ipv6_routing_srh_flag_unused2"];
        if (val != null) obj.Ipv6RoutingSrhFlagUnused2 = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_routing_srh_reserved"];
        if (val != null) obj.Ipv6RoutingSrhReserved = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ipv6_ipv6_routing_srh_addr"];
        if (val != null) obj.Ipv6RoutingSrhAddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["ipv6_ipv6_fraghdr_nxt"];
        if (val != null) obj.Ipv6FraghdrNxt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_fraghdr_reserved_octet"];
        if (val != null) obj.Ipv6FraghdrReservedOctet = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipv6_ipv6_fraghdr_offset"];
        if (val != null) obj.Ipv6FraghdrOffset = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_fraghdr_reserved_bits"];
        if (val != null) obj.Ipv6FraghdrReservedBits = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipv6_ipv6_fraghdr_more"];
        if (val != null) obj.Ipv6FraghdrMore = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipv6_ipv6_fraghdr_ident"];
        if (val != null) obj.Ipv6FraghdrIdent = Convert.ToUInt32(val.Value<string>(), 16);
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

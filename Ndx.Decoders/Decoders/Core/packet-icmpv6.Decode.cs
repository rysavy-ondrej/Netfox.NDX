using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Core
{
  public sealed partial class Icmpv6
  {
    public static Icmpv6 DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Icmpv6 DecodeJson(JToken token)
    {
      var obj = new Icmpv6();
      {
        var val = token["icmpv6_icmpv6_type"];
        if (val != null) obj.Icmpv6Type = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_code"];
        if (val != null) obj.Icmpv6Code = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_checksum"];
        if (val != null) obj.Icmpv6Checksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_checksum_icmpv6_checksum_status"];
        if (val != null) obj.Icmpv6ChecksumStatus = default(UInt32);
      }
      {
        var val = token["icmpv6_icmpv6_reserved"];
        if (val != null) obj.Icmpv6Reserved = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_data"];
        if (val != null) obj.Icmpv6Data = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_unknown_data"];
        if (val != null) obj.Icmpv6UnknownData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_mtu"];
        if (val != null) obj.Icmpv6Mtu = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_pointer"];
        if (val != null) obj.Icmpv6Pointer = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_echo_identifier"];
        if (val != null) obj.Icmpv6EchoIdentifier = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_echo_sequence_number"];
        if (val != null) obj.Icmpv6EchoSequenceNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_nonce"];
        if (val != null) obj.Icmpv6Nonce = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_nd_ra_cur_hop_limit"];
        if (val != null) obj.Icmpv6NdRaCurHopLimit = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_nd_ra_flag"];
        if (val != null) obj.Icmpv6NdRaFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_nd_ra_flag_icmpv6_nd_ra_flag_m"];
        if (val != null) obj.Icmpv6NdRaFlagM = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_nd_ra_flag_icmpv6_nd_ra_flag_o"];
        if (val != null) obj.Icmpv6NdRaFlagO = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_nd_ra_flag_icmpv6_nd_ra_flag_h"];
        if (val != null) obj.Icmpv6NdRaFlagH = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_nd_ra_flag_icmpv6_nd_ra_flag_prf"];
        if (val != null) obj.Icmpv6NdRaFlagPrf = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_nd_ra_flag_icmpv6_nd_ra_flag_p"];
        if (val != null) obj.Icmpv6NdRaFlagP = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_nd_ra_flag_icmpv6_nd_ra_flag_rsv"];
        if (val != null) obj.Icmpv6NdRaFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_nd_ra_router_lifetime"];
        if (val != null) obj.Icmpv6NdRaRouterLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_nd_ra_reachable_time"];
        if (val != null) obj.Icmpv6NdRaReachableTime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_nd_ra_retrans_timer"];
        if (val != null) obj.Icmpv6NdRaRetransTimer = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_nd_ns_target_address"];
        if (val != null) obj.Icmpv6NdNsTargetAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_nd_na_flag"];
        if (val != null) obj.Icmpv6NdNaFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_nd_na_flag_icmpv6_nd_na_flag_r"];
        if (val != null) obj.Icmpv6NdNaFlagR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_nd_na_flag_icmpv6_nd_na_flag_s"];
        if (val != null) obj.Icmpv6NdNaFlagS = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_nd_na_flag_icmpv6_nd_na_flag_o"];
        if (val != null) obj.Icmpv6NdNaFlagO = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_nd_na_flag_icmpv6_nd_na_flag_rsv"];
        if (val != null) obj.Icmpv6NdNaFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_nd_na_target_address"];
        if (val != null) obj.Icmpv6NdNaTargetAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_nd_rd_target_address"];
        if (val != null) obj.Icmpv6NdRdTargetAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rd_na_destination_address"];
        if (val != null) obj.Icmpv6RdNaDestinationAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_opt"];
        if (val != null) obj.Icmpv6Opt = default(Int32);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_type"];
        if (val != null) obj.Icmpv6OptType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_length"];
        if (val != null) obj.Icmpv6OptLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_reserved"];
        if (val != null) obj.Icmpv6OptReserved = default(Int32);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_padding"];
        if (val != null) obj.Icmpv6OptPadding = default(Int32);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_linkaddr"];
        if (val != null) obj.Icmpv6OptLinkaddr = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_src_linkaddr"];
        if (val != null) obj.Icmpv6OptSrcLinkaddr = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_target_linkaddr"];
        if (val != null) obj.Icmpv6OptTargetLinkaddr = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_linkaddr_eui64"];
        if (val != null) obj.Icmpv6OptLinkaddrEui64 = default(ByteString);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_src_linkaddr_eui64"];
        if (val != null) obj.Icmpv6OptSrcLinkaddrEui64 = default(ByteString);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_target_linkaddr_eui64"];
        if (val != null) obj.Icmpv6OptTargetLinkaddrEui64 = default(ByteString);
      }
      {
        var val = token["icmpv6_opt_prefix_icmpv6_opt_prefix_length"];
        if (val != null) obj.Icmpv6OptPrefixLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_prefix_icmpv6_opt_prefix_flag"];
        if (val != null) obj.Icmpv6OptPrefixFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_opt_prefix_flag_icmpv6_opt_prefix_flag_l"];
        if (val != null) obj.Icmpv6OptPrefixFlagL = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_prefix_flag_icmpv6_opt_prefix_flag_a"];
        if (val != null) obj.Icmpv6OptPrefixFlagA = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_prefix_flag_icmpv6_opt_prefix_flag_r"];
        if (val != null) obj.Icmpv6OptPrefixFlagR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_prefix_flag_icmpv6_opt_prefix_flag_reserved"];
        if (val != null) obj.Icmpv6OptPrefixFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_prefix_icmpv6_opt_prefix_valid_lifetime"];
        if (val != null) obj.Icmpv6OptPrefixValidLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_prefix_icmpv6_opt_prefix_preferred_lifetime"];
        if (val != null) obj.Icmpv6OptPrefixPreferredLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_prefix"];
        if (val != null) obj.Icmpv6OptPrefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_opt_cga_icmpv6_opt_cga_pad_length"];
        if (val != null) obj.Icmpv6OptCgaPadLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_cga"];
        if (val != null) obj.Icmpv6OptCga = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_cga_icmpv6_opt_cga_modifier"];
        if (val != null) obj.Icmpv6OptCgaModifier = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_cga_icmpv6_opt_cga_subnet_prefix"];
        if (val != null) obj.Icmpv6OptCgaSubnetPrefix = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_cga_icmpv6_opt_cga_count"];
        if (val != null) obj.Icmpv6OptCgaCount = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_cga_icmpv6_opt_cga_ext_type"];
        if (val != null) obj.Icmpv6OptCgaExtType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_cga_icmpv6_opt_cga_ext_length"];
        if (val != null) obj.Icmpv6OptCgaExtLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_cga_icmpv6_opt_cga_ext_data"];
        if (val != null) obj.Icmpv6OptCgaExtData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_opt_rsa_key_hash"];
        if (val != null) obj.Icmpv6OptRsaKeyHash = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_digital_signature_padding"];
        if (val != null) obj.Icmpv6OptDigitalSignaturePadding = default(Int32);
      }
      {
        var val = token["icmpv6_icmpv6_opt_ps_key_hash"];
        if (val != null) obj.Icmpv6OptPsKeyHash = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_timestamp"];
        if (val != null) obj.Icmpv6OptTimestamp = default(Int64);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_nonce"];
        if (val != null) obj.Icmpv6OptNonce = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_certificate_padding"];
        if (val != null) obj.Icmpv6OptCertificatePadding = default(Int32);
      }
      {
        var val = token["icmpv6_icmpv6_opt_ipa_option_code"];
        if (val != null) obj.Icmpv6OptIpaOptionCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_ipa_prefix_len"];
        if (val != null) obj.Icmpv6OptIpaPrefixLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_ipa_ipv6_address"];
        if (val != null) obj.Icmpv6OptIpaIpv6Address = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_opt_nrpi_option_code"];
        if (val != null) obj.Icmpv6OptNrpiOptionCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_nrpi_prefix_len"];
        if (val != null) obj.Icmpv6OptNrpiPrefixLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_nrpi_prefix"];
        if (val != null) obj.Icmpv6OptNrpiPrefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_opt_lla_option_code"];
        if (val != null) obj.Icmpv6OptLlaOptionCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_lla_bytes"];
        if (val != null) obj.Icmpv6OptLlaBytes = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_opt_naack_option_code"];
        if (val != null) obj.Icmpv6OptNaackOptionCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_naack_status"];
        if (val != null) obj.Icmpv6OptNaackStatus = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_naack_supplied_ncoa"];
        if (val != null) obj.Icmpv6OptNaackSuppliedNcoa = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_opt_map_distance"];
        if (val != null) obj.Icmpv6OptMapDistance = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_map_preference"];
        if (val != null) obj.Icmpv6OptMapPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_map_flag"];
        if (val != null) obj.Icmpv6OptMapFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_opt_map_flag_icmpv6_opt_map_flag_r"];
        if (val != null) obj.Icmpv6OptMapFlagR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_map_flag_icmpv6_opt_map_flag_reserved"];
        if (val != null) obj.Icmpv6OptMapFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_map_valid_lifetime"];
        if (val != null) obj.Icmpv6OptMapValidLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_map_global_address"];
        if (val != null) obj.Icmpv6OptMapGlobalAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_opt_route_info_flag"];
        if (val != null) obj.Icmpv6OptRouteInfoFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_opt_route_info_flag_icmpv6_opt_route_info_flag_route_preference"];
        if (val != null) obj.Icmpv6OptRouteInfoFlagRoutePreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_route_info_flag_icmpv6_opt_route_info_flag_reserved"];
        if (val != null) obj.Icmpv6OptRouteInfoFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_route_lifetime"];
        if (val != null) obj.Icmpv6OptRouteLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_name_type"];
        if (val != null) obj.Icmpv6OptNameType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_name_x501"];
        if (val != null) obj.Icmpv6OptNameX501 = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_name_type_icmpv6_opt_name_type_fqdn"];
        if (val != null) obj.Icmpv6OptNameTypeFqdn = val.Value<string>();
      }
      {
        var val = token["icmpv6_icmpv6_send_identifier"];
        if (val != null) obj.Icmpv6SendIdentifier = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_send_all_components"];
        if (val != null) obj.Icmpv6SendAllComponents = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_send_component"];
        if (val != null) obj.Icmpv6SendComponent = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_x509_Name"];
        if (val != null) obj.Icmpv6X509Name = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_x509_Certificate"];
        if (val != null) obj.Icmpv6X509Certificate = default(Int32);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_redirected_packet"];
        if (val != null) obj.Icmpv6OptRedirectedPacket = default(Int32);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_mtu"];
        if (val != null) obj.Icmpv6OptMtu = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_nbma_shortcut_limit"];
        if (val != null) obj.Icmpv6OptNbmaShortcutLimit = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_advertisement_interval"];
        if (val != null) obj.Icmpv6OptAdvertisementInterval = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_home_agent_preference"];
        if (val != null) obj.Icmpv6OptHomeAgentPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_home_agent_lifetime"];
        if (val != null) obj.Icmpv6OptHomeAgentLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_ipv6_address"];
        if (val != null) obj.Icmpv6OptIpv6Address = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_opt_rdnss_icmpv6_opt_rdnss_lifetime"];
        if (val != null) obj.Icmpv6OptRdnssLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_rdnss"];
        if (val != null) obj.Icmpv6OptRdnss = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_efo"];
        if (val != null) obj.Icmpv6OptEfo = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_opt_efo_icmpv6_opt_efo_m"];
        if (val != null) obj.Icmpv6OptEfoM = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_efo_icmpv6_opt_efo_o"];
        if (val != null) obj.Icmpv6OptEfoO = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_efo_icmpv6_opt_efo_h"];
        if (val != null) obj.Icmpv6OptEfoH = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_efo_icmpv6_opt_efo_prf"];
        if (val != null) obj.Icmpv6OptEfoPrf = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_efo_icmpv6_opt_efo_p"];
        if (val != null) obj.Icmpv6OptEfoP = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_efo_icmpv6_opt_efo_rsv"];
        if (val != null) obj.Icmpv6OptEfoRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_hkr_pad_length"];
        if (val != null) obj.Icmpv6OptHkrPadLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_hkr_at"];
        if (val != null) obj.Icmpv6OptHkrAt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_hkr_reserved"];
        if (val != null) obj.Icmpv6OptHkrReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_hkr_encryption_public_key"];
        if (val != null) obj.Icmpv6OptHkrEncryptionPublicKey = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_opt_hkr_padding"];
        if (val != null) obj.Icmpv6OptHkrPadding = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_opt_hkr_lifetime"];
        if (val != null) obj.Icmpv6OptHkrLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_hkr_encrypted_handover_key"];
        if (val != null) obj.Icmpv6OptHkrEncryptedHandoverKey = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_opt_hai_option_code"];
        if (val != null) obj.Icmpv6OptHaiOptionCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_hai_length"];
        if (val != null) obj.Icmpv6OptHaiLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_hai_value"];
        if (val != null) obj.Icmpv6OptHaiValue = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_opt_mn_option_code"];
        if (val != null) obj.Icmpv6OptMnOptionCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_mn_length"];
        if (val != null) obj.Icmpv6OptMnLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_mn_value"];
        if (val != null) obj.Icmpv6OptMnValue = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_opt_dnssl_icmpv6_opt_dnssl_lifetime"];
        if (val != null) obj.Icmpv6OptDnsslLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_dnssl"];
        if (val != null) obj.Icmpv6OptDnssl = val.Value<string>();
      }
      {
        var val = token["icmpv6_icmpv6_opt_aro_status"];
        if (val != null) obj.Icmpv6OptAroStatus = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_aro_registration_lifetime"];
        if (val != null) obj.Icmpv6OptAroRegistrationLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_aro_eui64"];
        if (val != null) obj.Icmpv6OptAroEui64 = default(ByteString);
      }
      {
        var val = token["icmpv6_icmpv6_opt_6co_context_length"];
        if (val != null) obj.Icmpv6Opt6CoContextLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_6co_flag"];
        if (val != null) obj.Icmpv6Opt6CoFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_opt_6co_flag_icmpv6_opt_6co_flag_c"];
        if (val != null) obj.Icmpv6Opt6CoFlagC = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_opt_6co_flag_icmpv6_opt_6co_flag_cid"];
        if (val != null) obj.Icmpv6Opt6CoFlagCid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_opt_6co_flag_icmpv6_opt_6co_flag_reserved"];
        if (val != null) obj.Icmpv6Opt6CoFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_6co_valid_lifetime"];
        if (val != null) obj.Icmpv6Opt6CoValidLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_6co_context_prefix"];
        if (val != null) obj.Icmpv6Opt6CoContextPrefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_opt_abro_version_low"];
        if (val != null) obj.Icmpv6OptAbroVersionLow = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_abro_version_high"];
        if (val != null) obj.Icmpv6OptAbroVersionHigh = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_abro_valid_lifetime"];
        if (val != null) obj.Icmpv6OptAbroValidLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_opt_abro_6lbr_address"];
        if (val != null) obj.Icmpv6OptAbro6LbrAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_opt_6cio_unassigned1"];
        if (val != null) obj.Icmpv6Opt6CioUnassigned1 = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_opt_6cio_flag_g"];
        if (val != null) obj.Icmpv6Opt6CioFlagG = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_opt_6cio_unassigned2"];
        if (val != null) obj.Icmpv6Opt6CioUnassigned2 = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_opt_icmpv6_opt_captive_portal"];
        if (val != null) obj.Icmpv6OptCaptivePortal = val.Value<string>();
      }
      {
        var val = token["icmpv6_icmpv6_mld_maximum_response_delay"];
        if (val != null) obj.Icmpv6MldMaximumResponseDelay = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mld_multicast_address"];
        if (val != null) obj.Icmpv6MldMulticastAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rr_sequence_number"];
        if (val != null) obj.Icmpv6RrSequenceNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rr_segment_number"];
        if (val != null) obj.Icmpv6RrSegmentNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rr_flag"];
        if (val != null) obj.Icmpv6RrFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rr_flag_icmpv6_rr_flag_t"];
        if (val != null) obj.Icmpv6RrFlagT = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_flag_icmpv6_rr_flag_r"];
        if (val != null) obj.Icmpv6RrFlagR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_flag_icmpv6_rr_flag_a"];
        if (val != null) obj.Icmpv6RrFlagA = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_flag_icmpv6_rr_flag_s"];
        if (val != null) obj.Icmpv6RrFlagS = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_flag_icmpv6_rr_flag_p"];
        if (val != null) obj.Icmpv6RrFlagP = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_flag_icmpv6_rr_flag_rsv"];
        if (val != null) obj.Icmpv6RrFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rr_maxdelay"];
        if (val != null) obj.Icmpv6RrMaxdelay = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rr_pco_mp"];
        if (val != null) obj.Icmpv6RrPcoMp = default(Int32);
      }
      {
        var val = token["icmpv6_rr_pco_mp_icmpv6_rr_pco_mp_opcode"];
        if (val != null) obj.Icmpv6RrPcoMpOpcode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_mp_icmpv6_rr_pco_mp_oplength"];
        if (val != null) obj.Icmpv6RrPcoMpOplength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_mp_icmpv6_rr_pco_mp_ordinal"];
        if (val != null) obj.Icmpv6RrPcoMpOrdinal = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rr_pco_mp_icmpv6_rr_pco_mp_matchlen"];
        if (val != null) obj.Icmpv6RrPcoMpMatchlen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_mp_icmpv6_rr_pco_mp_minlen"];
        if (val != null) obj.Icmpv6RrPcoMpMinlen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_mp_icmpv6_rr_pco_mp_maxlen"];
        if (val != null) obj.Icmpv6RrPcoMpMaxlen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_mp_icmpv6_rr_pco_mp_matchprefix"];
        if (val != null) obj.Icmpv6RrPcoMpMatchprefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rr_pco_up"];
        if (val != null) obj.Icmpv6RrPcoUp = default(Int32);
      }
      {
        var val = token["icmpv6_rr_pco_up_icmpv6_rr_pco_up_uselen"];
        if (val != null) obj.Icmpv6RrPcoUpUselen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_up_icmpv6_rr_pco_up_keeplen"];
        if (val != null) obj.Icmpv6RrPcoUpKeeplen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_up_icmpv6_rr_pco_up_flagmask"];
        if (val != null) obj.Icmpv6RrPcoUpFlagmask = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rr_pco_up_flagmask_icmpv6_rr_pco_up_flagmask_l"];
        if (val != null) obj.Icmpv6RrPcoUpFlagmaskL = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_pco_up_flagmask_icmpv6_rr_pco_up_flagmask_a"];
        if (val != null) obj.Icmpv6RrPcoUpFlagmaskA = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_pco_up_flagmask_icmpv6_rr_pco_up_flagmask_reserved"];
        if (val != null) obj.Icmpv6RrPcoUpFlagmaskReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_up_icmpv6_rr_pco_up_raflags"];
        if (val != null) obj.Icmpv6RrPcoUpRaflags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rr_pco_up_icmpv6_rr_pco_up_validlifetime"];
        if (val != null) obj.Icmpv6RrPcoUpValidlifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_up_icmpv6_rr_pco_up_preferredlifetime"];
        if (val != null) obj.Icmpv6RrPcoUpPreferredlifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_up_icmpv6_rr_pco_up_flag"];
        if (val != null) obj.Icmpv6RrPcoUpFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rr_pco_up_flag_icmpv6_rr_pco_up_flag_v"];
        if (val != null) obj.Icmpv6RrPcoUpFlagV = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_pco_up_flag_icmpv6_rr_pco_up_flag_p"];
        if (val != null) obj.Icmpv6RrPcoUpFlagP = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_pco_up_flag_icmpv6_rr_pco_up_flag_reserved"];
        if (val != null) obj.Icmpv6RrPcoUpFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_pco_up_icmpv6_rr_pco_up_useprefix"];
        if (val != null) obj.Icmpv6RrPcoUpUseprefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rr_rm"];
        if (val != null) obj.Icmpv6RrRm = default(Int32);
      }
      {
        var val = token["icmpv6_rr_rm_icmpv6_rr_rm_flag"];
        if (val != null) obj.Icmpv6RrRmFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rr_rm_flag_icmpv6_rr_rm_flag_b"];
        if (val != null) obj.Icmpv6RrRmFlagB = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_rm_flag_icmpv6_rr_rm_flag_f"];
        if (val != null) obj.Icmpv6RrRmFlagF = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rr_rm_flag_icmpv6_rr_rm_flag_reserved"];
        if (val != null) obj.Icmpv6RrRmFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_rm_icmpv6_rr_rm_ordinal"];
        if (val != null) obj.Icmpv6RrRmOrdinal = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rr_rm_icmpv6_rr_rm_matchedlen"];
        if (val != null) obj.Icmpv6RrRmMatchedlen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_rm_icmpv6_rr_rm_interfaceindex"];
        if (val != null) obj.Icmpv6RrRmInterfaceindex = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rr_rm_icmpv6_rr_rm_matchedprefix"];
        if (val != null) obj.Icmpv6RrRmMatchedprefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_mip6_identifier"];
        if (val != null) obj.Icmpv6Mip6Identifier = default(UInt32);
      }
      {
        var val = token["icmpv6_icmpv6_mip6_home_agent_address"];
        if (val != null) obj.Icmpv6Mip6HomeAgentAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_mip6_flag"];
        if (val != null) obj.Icmpv6Mip6Flag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_mip6_flag_icmpv6_mip6_flag_m"];
        if (val != null) obj.Icmpv6Mip6FlagM = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_mip6_flag_icmpv6_mip6_flag_o"];
        if (val != null) obj.Icmpv6Mip6FlagO = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_mip6_flag_icmpv6_mip6_flag_rsv"];
        if (val != null) obj.Icmpv6Mip6FlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mld_maximum_response_code"];
        if (val != null) obj.Icmpv6MldMaximumResponseCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mld_flag"];
        if (val != null) obj.Icmpv6MldFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_mld_flag_icmpv6_mld_flag_s"];
        if (val != null) obj.Icmpv6MldFlagS = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_mld_flag_icmpv6_mld_flag_qrv"];
        if (val != null) obj.Icmpv6MldFlagQrv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_mld_flag_icmpv6_mld_flag_reserved"];
        if (val != null) obj.Icmpv6MldFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mld_qqi"];
        if (val != null) obj.Icmpv6MldQqi = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mld_nb_sources"];
        if (val != null) obj.Icmpv6MldNbSources = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mld_source_address"];
        if (val != null) obj.Icmpv6MldSourceAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_mldr_nb_mcast_records"];
        if (val != null) obj.Icmpv6MldrNbMcastRecords = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mldr_mar"];
        if (val != null) obj.Icmpv6MldrMar = default(Int32);
      }
      {
        var val = token["icmpv6_mldr_mar_icmpv6_mldr_mar_record_type"];
        if (val != null) obj.Icmpv6MldrMarRecordType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_mldr_mar_icmpv6_mldr_mar_aux_data_len"];
        if (val != null) obj.Icmpv6MldrMarAuxDataLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_mldr_mar_icmpv6_mldr_mar_nb_sources"];
        if (val != null) obj.Icmpv6MldrMarNbSources = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_mldr_mar_icmpv6_mldr_mar_multicast_address"];
        if (val != null) obj.Icmpv6MldrMarMulticastAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_mldr_mar_icmpv6_mldr_mar_source_address"];
        if (val != null) obj.Icmpv6MldrMarSourceAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_mldr_mar_icmpv6_mldr_mar_auxiliary_data"];
        if (val != null) obj.Icmpv6MldrMarAuxiliaryData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_fmip6_subtype"];
        if (val != null) obj.Icmpv6Fmip6Subtype = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_fmip6_hi_flag"];
        if (val != null) obj.Icmpv6Fmip6HiFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_fmip6_hi_flag_icmpv6_fmip6_hi_flag_s"];
        if (val != null) obj.Icmpv6Fmip6HiFlagS = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_fmip6_hi_flag_icmpv6_fmip6_hi_flag_a"];
        if (val != null) obj.Icmpv6Fmip6HiFlagA = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_fmip6_hi_flag_icmpv6_fmip6_hi_flag_reserved"];
        if (val != null) obj.Icmpv6Fmip6HiFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_fmip6_identifier"];
        if (val != null) obj.Icmpv6Fmip6Identifier = default(UInt32);
      }
      {
        var val = token["icmpv6_icmpv6_mcast_ra_query_interval"];
        if (val != null) obj.Icmpv6McastRaQueryInterval = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mcast_ra_robustness_variable"];
        if (val != null) obj.Icmpv6McastRaRobustnessVariable = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_ni_qtype"];
        if (val != null) obj.Icmpv6NiQtype = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_ni_flag"];
        if (val != null) obj.Icmpv6NiFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_ni_flag_icmpv6_ni_flag_g"];
        if (val != null) obj.Icmpv6NiFlagG = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_ni_flag_icmpv6_ni_flag_s"];
        if (val != null) obj.Icmpv6NiFlagS = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_ni_flag_icmpv6_ni_flag_l"];
        if (val != null) obj.Icmpv6NiFlagL = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_ni_flag_icmpv6_ni_flag_c"];
        if (val != null) obj.Icmpv6NiFlagC = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_ni_flag_icmpv6_ni_flag_a"];
        if (val != null) obj.Icmpv6NiFlagA = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_ni_flag_icmpv6_ni_flag_t"];
        if (val != null) obj.Icmpv6NiFlagT = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_ni_flag_icmpv6_ni_flag_rsv"];
        if (val != null) obj.Icmpv6NiFlagRsv = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_ni_nonce"];
        if (val != null) obj.Icmpv6NiNonce = Convert.ToUInt64(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_ni_query_subject_ipv6"];
        if (val != null) obj.Icmpv6NiQuerySubjectIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_ni_query_subject_fqdn"];
        if (val != null) obj.Icmpv6NiQuerySubjectFqdn = val.Value<string>();
      }
      {
        var val = token["icmpv6_icmpv6_ni_query_subject_ipv4"];
        if (val != null) obj.Icmpv6NiQuerySubjectIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_ni_reply_node_ttl"];
        if (val != null) obj.Icmpv6NiReplyNodeTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_ni_reply_node_name"];
        if (val != null) obj.Icmpv6NiReplyNodeName = val.Value<string>();
      }
      {
        var val = token["icmpv6_icmpv6_ni_reply_node_address"];
        if (val != null) obj.Icmpv6NiReplyNodeAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_ni_reply_ipv4_address"];
        if (val != null) obj.Icmpv6NiReplyIpv4Address = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dis_flags"];
        if (val != null) obj.Icmpv6RplDisFlags = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dio_instance"];
        if (val != null) obj.Icmpv6RplDioInstance = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dio_version"];
        if (val != null) obj.Icmpv6RplDioVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dio_rank"];
        if (val != null) obj.Icmpv6RplDioRank = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dio_flag"];
        if (val != null) obj.Icmpv6RplDioFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_dio_flag_icmpv6_rpl_dio_flag_g"];
        if (val != null) obj.Icmpv6RplDioFlagG = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_dio_flag_icmpv6_rpl_dio_flag_0"];
        if (val != null) obj.Icmpv6RplDioFlag0 = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_dio_flag_icmpv6_rpl_dio_flag_mop"];
        if (val != null) obj.Icmpv6RplDioFlagMop = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_dio_flag_icmpv6_rpl_dio_flag_preference"];
        if (val != null) obj.Icmpv6RplDioFlagPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dio_dtsn"];
        if (val != null) obj.Icmpv6RplDioDtsn = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dio_dagid"];
        if (val != null) obj.Icmpv6RplDioDagid = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dao_instance"];
        if (val != null) obj.Icmpv6RplDaoInstance = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dao_flag"];
        if (val != null) obj.Icmpv6RplDaoFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_dao_flag_icmpv6_rpl_dao_flag_k"];
        if (val != null) obj.Icmpv6RplDaoFlagK = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_dao_flag_icmpv6_rpl_dao_flag_d"];
        if (val != null) obj.Icmpv6RplDaoFlagD = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_dao_flag_icmpv6_rpl_dao_flag_rsv"];
        if (val != null) obj.Icmpv6RplDaoFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dao_sequence"];
        if (val != null) obj.Icmpv6RplDaoSequence = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_dao_dodagid"];
        if (val != null) obj.Icmpv6RplDaoDodagid = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_daoack_instance"];
        if (val != null) obj.Icmpv6RplDaoackInstance = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_daoack_flag"];
        if (val != null) obj.Icmpv6RplDaoackFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_daoack_flag_icmpv6_rpl_daoack_flag_d"];
        if (val != null) obj.Icmpv6RplDaoackFlagD = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_daoack_flag_icmpv6_rpl_daoack_flag_rsv"];
        if (val != null) obj.Icmpv6RplDaoackFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_daoack_sequence"];
        if (val != null) obj.Icmpv6RplDaoackSequence = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_daoack_status"];
        if (val != null) obj.Icmpv6RplDaoackStatus = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_daoack_dodagid"];
        if (val != null) obj.Icmpv6RplDaoackDodagid = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_cc_instance"];
        if (val != null) obj.Icmpv6RplCcInstance = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_cc_flag"];
        if (val != null) obj.Icmpv6RplCcFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_cc_flag_icmpv6_rpl_cc_flag_r"];
        if (val != null) obj.Icmpv6RplCcFlagR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_cc_flag_icmpv6_rpl_cc_flag_rsv"];
        if (val != null) obj.Icmpv6RplCcFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_cc_nonce"];
        if (val != null) obj.Icmpv6RplCcNonce = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_cc_dodagid"];
        if (val != null) obj.Icmpv6RplCcDodagid = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_cc_destination_counter"];
        if (val != null) obj.Icmpv6RplCcDestinationCounter = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_secure_flag"];
        if (val != null) obj.Icmpv6RplSecureFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_secure_flag_icmpv6_rpl_secure_flag_t"];
        if (val != null) obj.Icmpv6RplSecureFlagT = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_secure_flag_icmpv6_rpl_secure_flag_rsv"];
        if (val != null) obj.Icmpv6RplSecureFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_secure_algorithm"];
        if (val != null) obj.Icmpv6RplSecureAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_secure_kim"];
        if (val != null) obj.Icmpv6RplSecureKim = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_secure_lvl"];
        if (val != null) obj.Icmpv6RplSecureLvl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_secure_rsv"];
        if (val != null) obj.Icmpv6RplSecureRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_secure_counter"];
        if (val != null) obj.Icmpv6RplSecureCounter = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_secure_key_source"];
        if (val != null) obj.Icmpv6RplSecureKeySource = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_secure_key_index"];
        if (val != null) obj.Icmpv6RplSecureKeyIndex = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_type"];
        if (val != null) obj.Icmpv6RplOptType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_length"];
        if (val != null) obj.Icmpv6RplOptLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_reserved"];
        if (val != null) obj.Icmpv6RplOptReserved = default(Int32);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_padn"];
        if (val != null) obj.Icmpv6RplOptPadn = default(Int32);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_type"];
        if (val != null) obj.Icmpv6RplOptMetricType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_flags"];
        if (val != null) obj.Icmpv6RplOptMetricFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_reserved"];
        if (val != null) obj.Icmpv6RplOptMetricReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_flag_p"];
        if (val != null) obj.Icmpv6RplOptMetricFlagP = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_flag_c"];
        if (val != null) obj.Icmpv6RplOptMetricFlagC = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_flag_o"];
        if (val != null) obj.Icmpv6RplOptMetricFlagO = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_flag_r"];
        if (val != null) obj.Icmpv6RplOptMetricFlagR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_flag_a"];
        if (val != null) obj.Icmpv6RplOptMetricFlagA = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_prec"];
        if (val != null) obj.Icmpv6RplOptMetricPrec = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_length"];
        if (val != null) obj.Icmpv6RplOptMetricLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_nsa_object"];
        if (val != null) obj.Icmpv6RplOptMetricNsaObject = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_nsa_object_icmpv6_rpl_opt_metric_nsa_object_reserved"];
        if (val != null) obj.Icmpv6RplOptMetricNsaObjectReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_nsa_object_icmpv6_rpl_opt_metric_nsa_object_flags"];
        if (val != null) obj.Icmpv6RplOptMetricNsaObjectFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_nsa_object_flag_a"];
        if (val != null) obj.Icmpv6RplOptMetricNsaObjectFlagA = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_nsa_object_flag_o"];
        if (val != null) obj.Icmpv6RplOptMetricNsaObjectFlagO = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_ne_object"];
        if (val != null) obj.Icmpv6RplOptMetricNeObject = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_ne_object_icmpv6_rpl_opt_metric_ne_object_flags"];
        if (val != null) obj.Icmpv6RplOptMetricNeObjectFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_ne_object_flag_i"];
        if (val != null) obj.Icmpv6RplOptMetricNeObjectFlagI = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_metric_ne_object_icmpv6_rpl_opt_metric_ne_object_type"];
        if (val != null) obj.Icmpv6RplOptMetricNeObjectType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_ne_object_flag_e"];
        if (val != null) obj.Icmpv6RplOptMetricNeObjectFlagE = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_metric_ne_object_icmpv6_rpl_opt_metric_ne_object_energy"];
        if (val != null) obj.Icmpv6RplOptMetricNeObjectEnergy = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_hp_object"];
        if (val != null) obj.Icmpv6RplOptMetricHpObject = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_hp_object_icmpv6_rpl_opt_metric_hp_object_reserved"];
        if (val != null) obj.Icmpv6RplOptMetricHpObjectReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_hp_object_icmpv6_rpl_opt_metric_hp_object_flags"];
        if (val != null) obj.Icmpv6RplOptMetricHpObjectFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_hp_object_icmpv6_rpl_opt_metric_hp_object_hp"];
        if (val != null) obj.Icmpv6RplOptMetricHpObjectHp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_lt_object_lt"];
        if (val != null) obj.Icmpv6RplOptMetricLtObjectLt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_ll_object_ll"];
        if (val != null) obj.Icmpv6RplOptMetricLlObjectLl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_lql_object"];
        if (val != null) obj.Icmpv6RplOptMetricLqlObject = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_lql_object_icmpv6_rpl_opt_metric_lql_object_res"];
        if (val != null) obj.Icmpv6RplOptMetricLqlObjectRes = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_lql_object_icmpv6_rpl_opt_metric_lql_object_val"];
        if (val != null) obj.Icmpv6RplOptMetricLqlObjectVal = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_lql_object_icmpv6_rpl_opt_metric_lql_object_counter"];
        if (val != null) obj.Icmpv6RplOptMetricLqlObjectCounter = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_etx_object_etx"];
        if (val != null) obj.Icmpv6RplOptMetricEtxObjectEtx = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_lc_object"];
        if (val != null) obj.Icmpv6RplOptMetricLcObject = default(Int32);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_lc_object_icmpv6_rpl_opt_metric_lc_object_res"];
        if (val != null) obj.Icmpv6RplOptMetricLcObjectRes = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_lc_object_icmpv6_rpl_opt_metric_lc_object_lc"];
        if (val != null) obj.Icmpv6RplOptMetricLcObjectLc = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_lc_object_icmpv6_rpl_opt_metric_lc_object_counter"];
        if (val != null) obj.Icmpv6RplOptMetricLcObjectCounter = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rpl_opt_metric_lc_object_icmpv6_rpl_opt_metric_lc_object_reserved"];
        if (val != null) obj.Icmpv6RplOptMetricLcObjectReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_metric_lc_object_flag_i"];
        if (val != null) obj.Icmpv6RplOptMetricLcObjectFlagI = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_route_prefix_length"];
        if (val != null) obj.Icmpv6RplOptRoutePrefixLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_route_flag"];
        if (val != null) obj.Icmpv6RplOptRouteFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_route_pref"];
        if (val != null) obj.Icmpv6RplOptRoutePref = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_route_reserved"];
        if (val != null) obj.Icmpv6RplOptRouteReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_route_lifetime"];
        if (val != null) obj.Icmpv6RplOptRouteLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_route_prefix"];
        if (val != null) obj.Icmpv6RplOptRoutePrefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_flag"];
        if (val != null) obj.Icmpv6RplOptConfigFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_reserved"];
        if (val != null) obj.Icmpv6RplOptConfigReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_auth"];
        if (val != null) obj.Icmpv6RplOptConfigAuth = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_pcs"];
        if (val != null) obj.Icmpv6RplOptConfigPcs = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_interval_double"];
        if (val != null) obj.Icmpv6RplOptConfigIntervalDouble = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_interval_min"];
        if (val != null) obj.Icmpv6RplOptConfigIntervalMin = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_redundancy"];
        if (val != null) obj.Icmpv6RplOptConfigRedundancy = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_max_rank_inc"];
        if (val != null) obj.Icmpv6RplOptConfigMaxRankInc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_min_hop_rank_inc"];
        if (val != null) obj.Icmpv6RplOptConfigMinHopRankInc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_ocp"];
        if (val != null) obj.Icmpv6RplOptConfigOcp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_rsv"];
        if (val != null) obj.Icmpv6RplOptConfigRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_def_lifetime"];
        if (val != null) obj.Icmpv6RplOptConfigDefLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_config_lifetime_unit"];
        if (val != null) obj.Icmpv6RplOptConfigLifetimeUnit = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_target_flag"];
        if (val != null) obj.Icmpv6RplOptTargetFlag = default(Int32);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_target_prefix_length"];
        if (val != null) obj.Icmpv6RplOptTargetPrefixLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_target_prefix"];
        if (val != null) obj.Icmpv6RplOptTargetPrefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_transit_flag"];
        if (val != null) obj.Icmpv6RplOptTransitFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_transit_flag_icmpv6_rpl_opt_transit_flag_e"];
        if (val != null) obj.Icmpv6RplOptTransitFlagE = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_transit_flag_icmpv6_rpl_opt_transit_flag_rsv"];
        if (val != null) obj.Icmpv6RplOptTransitFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_transit_pathctl"];
        if (val != null) obj.Icmpv6RplOptTransitPathctl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_transit_pathseq"];
        if (val != null) obj.Icmpv6RplOptTransitPathseq = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_transit_pathlifetime"];
        if (val != null) obj.Icmpv6RplOptTransitPathlifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_transit_parent"];
        if (val != null) obj.Icmpv6RplOptTransitParent = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_solicited_instance"];
        if (val != null) obj.Icmpv6RplOptSolicitedInstance = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_solicited_flag"];
        if (val != null) obj.Icmpv6RplOptSolicitedFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_solicited_flag_icmpv6_rpl_opt_solicited_flag_v"];
        if (val != null) obj.Icmpv6RplOptSolicitedFlagV = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_solicited_flag_icmpv6_rpl_opt_solicited_flag_i"];
        if (val != null) obj.Icmpv6RplOptSolicitedFlagI = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_solicited_flag_icmpv6_rpl_opt_solicited_flag_d"];
        if (val != null) obj.Icmpv6RplOptSolicitedFlagD = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_solicited_flag_icmpv6_rpl_opt_solicited_flag_rsv"];
        if (val != null) obj.Icmpv6RplOptSolicitedFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_solicited_dodagid"];
        if (val != null) obj.Icmpv6RplOptSolicitedDodagid = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_solicited_version"];
        if (val != null) obj.Icmpv6RplOptSolicitedVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rpl_opt_prefix_icmpv6_rpl_opt_prefix_length"];
        if (val != null) obj.Icmpv6RplOptPrefixLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rpl_opt_prefix_icmpv6_rpl_opt_prefix_flag"];
        if (val != null) obj.Icmpv6RplOptPrefixFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_opt_prefix_flag_icmpv6_rpl_opt_prefix_flag_l"];
        if (val != null) obj.Icmpv6RplOptPrefixFlagL = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_config_flag_icmpv6_rpl_opt_config_flag_a"];
        if (val != null) obj.Icmpv6RplOptConfigFlagA = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_config_flag_icmpv6_rpl_opt_config_flag_r"];
        if (val != null) obj.Icmpv6RplOptConfigFlagR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_config_flag_icmpv6_rpl_opt_config_flag_rsv"];
        if (val != null) obj.Icmpv6RplOptConfigFlagRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rpl_opt_prefix_icmpv6_rpl_opt_prefix_valid_lifetime"];
        if (val != null) obj.Icmpv6RplOptPrefixValidLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rpl_opt_prefix_icmpv6_rpl_opt_prefix_preferred_lifetime"];
        if (val != null) obj.Icmpv6RplOptPrefixPreferredLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_prefix"];
        if (val != null) obj.Icmpv6RplOptPrefix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_targetdesc_descriptor"];
        if (val != null) obj.Icmpv6RplOptTargetdescDescriptor = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_routediscovery_flag"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryFlag = default(Int32);
      }
      {
        var val = token["icmpv6_rpl_opt_routediscovery_flag_icmpv6_rpl_opt_routediscovery_flag_reply"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryFlagReply = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_routediscovery_flag_icmpv6_rpl_opt_routediscovery_flag_hopbyhop"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryFlagHopbyhop = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_opt_routediscovery_flag_icmpv6_rpl_opt_routediscovery_flag_numofroutes"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryFlagNumofroutes = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rpl_opt_routediscovery_flag_icmpv6_rpl_opt_routediscovery_flag_compr"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryFlagCompr = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_routediscovery_lifetime"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_routediscovery_maxrank"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryMaxrank = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_routediscovery_nh"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryNh = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_routediscovery_targetaddr"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryTargetaddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_routediscovery_addr_vec"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryAddrVec = default(Int32);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_opt_routediscovery_addrvec_addr"];
        if (val != null) obj.Icmpv6RplOptRoutediscoveryAddrvecAddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_p2p_dro_instance"];
        if (val != null) obj.Icmpv6RplP2PDroInstance = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_p2p_dro_version"];
        if (val != null) obj.Icmpv6RplP2PDroVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_p2p_dro_flag"];
        if (val != null) obj.Icmpv6RplP2PDroFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_p2p_dro_flag_icmpv6_rpl_p2p_dro_flag_stop"];
        if (val != null) obj.Icmpv6RplP2PDroFlagStop = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_p2p_dro_flag_icmpv6_rpl_p2p_dro_flag_ack"];
        if (val != null) obj.Icmpv6RplP2PDroFlagAck = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmpv6_rpl_p2p_dro_flag_icmpv6_rpl_p2p_dro_flag_seq"];
        if (val != null) obj.Icmpv6RplP2PDroFlagSeq = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rpl_p2p_dro_flag_icmpv6_rpl_p2p_dro_flag_reserved"];
        if (val != null) obj.Icmpv6RplP2PDroFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_rpl_p2p_dro_dagid"];
        if (val != null) obj.Icmpv6RplP2PDroDagid = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_rpl_p2p_droack_flag"];
        if (val != null) obj.Icmpv6RplP2PDroackFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_rpl_p2p_droack_flag_icmpv6_rpl_p2p_droack_flag_seq"];
        if (val != null) obj.Icmpv6RplP2PDroackFlagSeq = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_rpl_p2p_droack_flag_icmpv6_rpl_p2p_droack_flag_reserved"];
        if (val != null) obj.Icmpv6RplP2PDroackFlagReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_ilnp_nb_locs"];
        if (val != null) obj.Icmpv6IlnpNbLocs = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_ilnp_nb_locator"];
        if (val != null) obj.Icmpv6IlnpNbLocator = Convert.ToUInt64(val.Value<string>(), 16);
      }
      {
        var val = token["icmpv6_icmpv6_ilnp_nb_preference"];
        if (val != null) obj.Icmpv6IlnpNbPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_ilnp_nb_lifetime"];
        if (val != null) obj.Icmpv6IlnpNbLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_6lowpannd_da_status"];
        if (val != null) obj.Icmpv66LowpanndDaStatus = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_6lowpannd_da_rsv"];
        if (val != null) obj.Icmpv66LowpanndDaRsv = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_6lowpannd_da_lifetime"];
        if (val != null) obj.Icmpv66LowpanndDaLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_6lowpannd_da_eui64"];
        if (val != null) obj.Icmpv66LowpanndDaEui64 = default(ByteString);
      }
      {
        var val = token["icmpv6_icmpv6_6lowpannd_da_reg_addr"];
        if (val != null) obj.Icmpv66LowpanndDaRegAddr = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmpv6_icmpv6_resp_in"];
        if (val != null) obj.Icmpv6RespIn = default(Int64);
      }
      {
        var val = token["icmpv6_icmpv6_no_resp"];
        if (val != null) obj.Icmpv6NoResp = default(Int32);
      }
      {
        var val = token["icmpv6_icmpv6_resp_to"];
        if (val != null) obj.Icmpv6RespTo = default(Int64);
      }
      {
        var val = token["icmpv6_icmpv6_resptime"];
        if (val != null) obj.Icmpv6Resptime = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["icmpv6_icmpv6_mpl_seed_info_min_sequence"];
        if (val != null) obj.Icmpv6MplSeedInfoMinSequence = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mpl_seed_info_bm_len"];
        if (val != null) obj.Icmpv6MplSeedInfoBmLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mpl_seed_info_s"];
        if (val != null) obj.Icmpv6MplSeedInfoS = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmpv6_icmpv6_mpl_seed_info_seed_id"];
        if (val != null) obj.Icmpv6MplSeedInfoSeedId = val.Value<string>();
      }
      {
        var val = token["icmpv6_icmpv6_mpl_seed_info_sequence"];
        if (val != null) obj.Icmpv6MplSeedInfoSequence = Convert.ToUInt32(val.Value<string>(), 10);
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

using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Core
{
  public sealed partial class Dns
  {
    public static Dns DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Dns DecodeJson(JToken token)
    {
      var obj = new Dns();
      {
        var val = token["dns_dns_flags"];
        if (val != null) obj.DnsFlags = default(ByteString);
      }
      {
        var val = token["dns_dns_id"];
        if (val != null) obj.DnsId = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_dns_response_in"];
        if (val != null) obj.DnsResponseIn = default(Int64);
      }
      {
        var val = token["dns_dns_response_to"];
        if (val != null) obj.DnsResponseTo = default(Int64);
      }
      {
        var val = token["dns_dns_time"];
        if (val != null) obj.DnsTime = default(Int64);
      }
      {
        var val = token["dns_dns_count_add_rr"];
        if (val != null) obj.DnsCountAddRr = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token[""];
        if (val != null) obj.DnsQry = default(ByteString);
      }
      {
        var val = token[""];
        if (val != null) obj.DnsResp = default(ByteString);
      }
      {
        var val = token["text_dns_srv_service"];
        if (val != null) obj.DnsSrvService = val.Value<string>();
      }
      {
        var val = token["text_dns_srv_proto"];
        if (val != null) obj.DnsSrvProto = val.Value<string>();
      }
      {
        var val = token["text_dns_srv_name"];
        if (val != null) obj.DnsSrvName = val.Value<string>();
      }
      {
        var val = token["text_dns_srv_priority"];
        if (val != null) obj.DnsSrvPriority = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_srv_weight"];
        if (val != null) obj.DnsSrvWeight = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_srv_port"];
        if (val != null) obj.DnsSrvPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_srv_target"];
        if (val != null) obj.DnsSrvTarget = val.Value<string>();
      }
      {
        var val = token["text_dns_naptr_order"];
        if (val != null) obj.DnsNaptrOrder = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_naptr_preference"];
        if (val != null) obj.DnsNaptrPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_naptr_flags_length"];
        if (val != null) obj.DnsNaptrFlagsLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_naptr_flags"];
        if (val != null) obj.DnsNaptrFlags = val.Value<string>();
      }
      {
        var val = token["text_dns_naptr_service_length"];
        if (val != null) obj.DnsNaptrServiceLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_naptr_service"];
        if (val != null) obj.DnsNaptrService = val.Value<string>();
      }
      {
        var val = token["text_dns_naptr_regex_length"];
        if (val != null) obj.DnsNaptrRegexLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_naptr_regex"];
        if (val != null) obj.DnsNaptrRegex = val.Value<string>();
      }
      {
        var val = token["text_dns_naptr_replacement_length"];
        if (val != null) obj.DnsNaptrReplacementLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_naptr_replacement"];
        if (val != null) obj.DnsNaptrReplacement = val.Value<string>();
      }
      {
        var val = token["text_dns_a"];
        if (val != null) obj.DnsA = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["text_dns_md"];
        if (val != null) obj.DnsMd = val.Value<string>();
      }
      {
        var val = token["text_dns_mf"];
        if (val != null) obj.DnsMf = val.Value<string>();
      }
      {
        var val = token["text_dns_mb"];
        if (val != null) obj.DnsMb = val.Value<string>();
      }
      {
        var val = token["text_dns_mg"];
        if (val != null) obj.DnsMg = val.Value<string>();
      }
      {
        var val = token["text_dns_mr"];
        if (val != null) obj.DnsMr = val.Value<string>();
      }
      {
        var val = token["dns_dns_null"];
        if (val != null) obj.DnsNull = StringToBytes(val.Value<string>());
      }
      {
        var val = token["text_dns_aaaa"];
        if (val != null) obj.DnsAaaa = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["text_dns_cname"];
        if (val != null) obj.DnsCname = val.Value<string>();
      }
      {
        var val = token["text_dns_rr_udp_payload_size"];
        if (val != null) obj.DnsRrUdpPayloadSize = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_dns_soa_mname"];
        if (val != null) obj.DnsSoaMname = val.Value<string>();
      }
      {
        var val = token["text_dns_soa_rname"];
        if (val != null) obj.DnsSoaRname = val.Value<string>();
      }
      {
        var val = token["text_dns_soa_serial_number"];
        if (val != null) obj.DnsSoaSerialNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_soa_refresh_interval"];
        if (val != null) obj.DnsSoaRefreshInterval = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_soa_retry_interval"];
        if (val != null) obj.DnsSoaRetryInterval = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_soa_expire_limit"];
        if (val != null) obj.DnsSoaExpireLimit = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_soa_mininum_ttl"];
        if (val != null) obj.DnsSoaMininumTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_ptr_domain_name"];
        if (val != null) obj.DnsPtrDomainName = val.Value<string>();
      }
      {
        var val = token["dns_dns_wks_address"];
        if (val != null) obj.DnsWksAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_wks_protocol"];
        if (val != null) obj.DnsWksProtocol = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_wks_bits"];
        if (val != null) obj.DnsWksBits = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_dns_hinfo_cpu_length"];
        if (val != null) obj.DnsHinfoCpuLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_hinfo_cpu"];
        if (val != null) obj.DnsHinfoCpu = val.Value<string>();
      }
      {
        var val = token["dns_dns_hinfo_os_length"];
        if (val != null) obj.DnsHinfoOsLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_hinfo_os"];
        if (val != null) obj.DnsHinfoOs = val.Value<string>();
      }
      {
        var val = token["dns_dns_minfo_r"];
        if (val != null) obj.DnsMinfoR = val.Value<string>();
      }
      {
        var val = token["dns_dns_minfo_e"];
        if (val != null) obj.DnsMinfoE = val.Value<string>();
      }
      {
        var val = token["text_dns_mx_preference"];
        if (val != null) obj.DnsMxPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_dns_mx_mail_exchange"];
        if (val != null) obj.DnsMxMailExchange = val.Value<string>();
      }
      {
        var val = token["dns_txt_dns_txt_length"];
        if (val != null) obj.DnsTxtLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_txt"];
        if (val != null) obj.DnsTxt = val.Value<string>();
      }
      {
        var val = token["dns_dns_openpgpkey"];
        if (val != null) obj.DnsOpenpgpkey = val.Value<string>();
      }
      {
        var val = token["dns_dns_csync_soa"];
        if (val != null) obj.DnsCsyncSoa = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_csync_flags"];
        if (val != null) obj.DnsCsyncFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_csync_flags_dns_csync_flags_immediate"];
        if (val != null) obj.DnsCsyncFlagsImmediate = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_csync_flags_dns_csync_flags_soaminimum"];
        if (val != null) obj.DnsCsyncFlagsSoaminimum = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dns_csync_type_bitmap"];
        if (val != null) obj.DnsCsyncTypeBitmap = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_spf_dns_spf_length"];
        if (val != null) obj.DnsSpfLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_spf"];
        if (val != null) obj.DnsSpf = val.Value<string>();
      }
      {
        var val = token["dns_ilnp_nid_dns_ilnp_nid_preference"];
        if (val != null) obj.DnsIlnpNidPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ilnp_nid"];
        if (val != null) obj.DnsIlnpNid = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_ilnp_l32_dns_ilnp_l32_preference"];
        if (val != null) obj.DnsIlnpL32Preference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ilnp_l32"];
        if (val != null) obj.DnsIlnpL32 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_ilnp_l64_dns_ilnp_l64_preference"];
        if (val != null) obj.DnsIlnpL64Preference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ilnp_l64"];
        if (val != null) obj.DnsIlnpL64 = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_ilnp_lp_dns_ilnp_lp_preference"];
        if (val != null) obj.DnsIlnpLpPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ilnp_lp"];
        if (val != null) obj.DnsIlnpLp = val.Value<string>();
      }
      {
        var val = token["dns_dns_eui48"];
        if (val != null) obj.DnsEui48 = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["dns_dns_eui64"];
        if (val != null) obj.DnsEui64 = default(ByteString);
      }
      {
        var val = token["dns_dns_rrsig_type_covered"];
        if (val != null) obj.DnsRrsigTypeCovered = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_rrsig_algorithm"];
        if (val != null) obj.DnsRrsigAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_rrsig_labels"];
        if (val != null) obj.DnsRrsigLabels = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_rrsig_original_ttl"];
        if (val != null) obj.DnsRrsigOriginalTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_rrsig_signature_expiration"];
        if (val != null) obj.DnsRrsigSignatureExpiration = default(Int64);
      }
      {
        var val = token["dns_dns_rrsig_signature_inception"];
        if (val != null) obj.DnsRrsigSignatureInception = default(Int64);
      }
      {
        var val = token["dns_dns_rrsig_key_tag"];
        if (val != null) obj.DnsRrsigKeyTag = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_rrsig_signers_name"];
        if (val != null) obj.DnsRrsigSignersName = val.Value<string>();
      }
      {
        var val = token["dns_dns_rrsig_signature"];
        if (val != null) obj.DnsRrsigSignature = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_dnskey_flags"];
        if (val != null) obj.DnsDnskeyFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_dnskey_flags_dns_dnskey_flags_zone_key"];
        if (val != null) obj.DnsDnskeyFlagsZoneKey = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dnskey_flags_dns_dnskey_flags_key_revoked"];
        if (val != null) obj.DnsDnskeyFlagsKeyRevoked = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dnskey_flags_dns_dnskey_flags_secure_entry_point"];
        if (val != null) obj.DnsDnskeyFlagsSecureEntryPoint = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dnskey_flags_dns_dnskey_flags_reserved"];
        if (val != null) obj.DnsDnskeyFlagsReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_dns_dnskey_protocol"];
        if (val != null) obj.DnsDnskeyProtocol = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_dnskey_algorithm"];
        if (val != null) obj.DnsDnskeyAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_dnskey_key_id"];
        if (val != null) obj.DnsDnskeyKeyId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_dnskey_public_key"];
        if (val != null) obj.DnsDnskeyPublicKey = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_key_flags"];
        if (val != null) obj.DnsKeyFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_key_flags_dns_key_flags_authentication"];
        if (val != null) obj.DnsKeyFlagsAuthentication = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_key_flags_dns_key_flags_confidentiality"];
        if (val != null) obj.DnsKeyFlagsConfidentiality = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_key_flags_dns_key_flags_required"];
        if (val != null) obj.DnsKeyFlagsRequired = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_key_flags_dns_key_flags_associated_user"];
        if (val != null) obj.DnsKeyFlagsAssociatedUser = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_key_flags_dns_key_flags_associated_named_entity"];
        if (val != null) obj.DnsKeyFlagsAssociatedNamedEntity = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_key_flags_dns_key_flags_ipsec"];
        if (val != null) obj.DnsKeyFlagsIpsec = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_key_flags_dns_key_flags_mime"];
        if (val != null) obj.DnsKeyFlagsMime = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_key_flags_dns_key_flags_signatory"];
        if (val != null) obj.DnsKeyFlagsSignatory = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_key_protocol"];
        if (val != null) obj.DnsKeyProtocol = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_key_algorithm"];
        if (val != null) obj.DnsKeyAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_key_key_id"];
        if (val != null) obj.DnsKeyKeyId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_key_public_key"];
        if (val != null) obj.DnsKeyPublicKey = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_px_preference"];
        if (val != null) obj.DnsPxPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_px_map822"];
        if (val != null) obj.DnsPxMap822 = val.Value<string>();
      }
      {
        var val = token["dns_dns_px_map400"];
        if (val != null) obj.DnsPxMap400 = val.Value<string>();
      }
      {
        var val = token["dns_dns_tkey_algo_name"];
        if (val != null) obj.DnsTkeyAlgoName = val.Value<string>();
      }
      {
        var val = token["dns_dns_tkey_signature_expiration"];
        if (val != null) obj.DnsTkeySignatureExpiration = default(Int64);
      }
      {
        var val = token["dns_dns_tkey_signature_inception"];
        if (val != null) obj.DnsTkeySignatureInception = default(Int64);
      }
      {
        var val = token["dns_dns_tkey_mode"];
        if (val != null) obj.DnsTkeyMode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tkey_error"];
        if (val != null) obj.DnsTkeyError = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tkey_key_size"];
        if (val != null) obj.DnsTkeyKeySize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tkey_key_data"];
        if (val != null) obj.DnsTkeyKeyData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_tkey_other_size"];
        if (val != null) obj.DnsTkeyOtherSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tkey_other_data"];
        if (val != null) obj.DnsTkeyOtherData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_ipseckey_gateway_precedence"];
        if (val != null) obj.DnsIpseckeyGatewayPrecedence = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ipseckey_gateway_algorithm"];
        if (val != null) obj.DnsIpseckeyGatewayAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ipseckey_gateway_type"];
        if (val != null) obj.DnsIpseckeyGatewayType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ipseckey_gateway_ipv4"];
        if (val != null) obj.DnsIpseckeyGatewayIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_ipseckey_gateway_ipv6"];
        if (val != null) obj.DnsIpseckeyGatewayIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_ipseckey_gateway_dns"];
        if (val != null) obj.DnsIpseckeyGatewayDns = val.Value<string>();
      }
      {
        var val = token["dns_dns_ipseckey_public_key"];
        if (val != null) obj.DnsIpseckeyPublicKey = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_xpf_ip_version"];
        if (val != null) obj.DnsXpfIpVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_xpf_protocol"];
        if (val != null) obj.DnsXpfProtocol = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_xpf_source_ipv4"];
        if (val != null) obj.DnsXpfSourceIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_xpf_destination_ipv4"];
        if (val != null) obj.DnsXpfDestinationIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_xpf_source_ipv6"];
        if (val != null) obj.DnsXpfSourceIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_xpf_destination_ipv6"];
        if (val != null) obj.DnsXpfDestinationIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_xpf_sport"];
        if (val != null) obj.DnsXpfSport = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_xpf_dport"];
        if (val != null) obj.DnsXpfDport = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_a6_prefix_len"];
        if (val != null) obj.DnsA6PrefixLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_a6_address_suffix"];
        if (val != null) obj.DnsA6AddressSuffix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_a6_prefix_name"];
        if (val != null) obj.DnsA6PrefixName = val.Value<string>();
      }
      {
        var val = token["dns_dns_dname"];
        if (val != null) obj.DnsDname = val.Value<string>();
      }
      {
        var val = token["dns_dns_loc_version"];
        if (val != null) obj.DnsLocVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_loc_size"];
        if (val != null) obj.DnsLocSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_loc_horizontal_precision"];
        if (val != null) obj.DnsLocHorizontalPrecision = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_loc_vertical_precision"];
        if (val != null) obj.DnsLocVerticalPrecision = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_loc_latitude"];
        if (val != null) obj.DnsLocLatitude = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_loc_longitude"];
        if (val != null) obj.DnsLocLongitude = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_loc_altitude"];
        if (val != null) obj.DnsLocAltitude = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_loc_unknown_data"];
        if (val != null) obj.DnsLocUnknownData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_nxt_next_domain_name"];
        if (val != null) obj.DnsNxtNextDomainName = val.Value<string>();
      }
      {
        var val = token["dns_dns_kx_preference"];
        if (val != null) obj.DnsKxPreference = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_kx_key_exchange"];
        if (val != null) obj.DnsKxKeyExchange = val.Value<string>();
      }
      {
        var val = token["dns_dns_cert_type"];
        if (val != null) obj.DnsCertType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_cert_key_tag"];
        if (val != null) obj.DnsCertKeyTag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_dns_cert_algorithm"];
        if (val != null) obj.DnsCertAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_cert_certificate"];
        if (val != null) obj.DnsCertCertificate = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_nsec_next_domain_name"];
        if (val != null) obj.DnsNsecNextDomainName = val.Value<string>();
      }
      {
        var val = token["text_dns_ns"];
        if (val != null) obj.DnsNs = val.Value<string>();
      }
      {
        var val = token["dns_dns_opt"];
        if (val != null) obj.DnsOpt = default(Int32);
      }
      {
        var val = token["dns_opt_dns_opt_code"];
        if (val != null) obj.DnsOptCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_opt_dns_opt_len"];
        if (val != null) obj.DnsOptLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_opt_dns_opt_data"];
        if (val != null) obj.DnsOptData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_opt_dns_opt_dau"];
        if (val != null) obj.DnsOptDau = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_opt_dns_opt_dhu"];
        if (val != null) obj.DnsOptDhu = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_opt_dns_opt_n3u"];
        if (val != null) obj.DnsOptN3u = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_opt_client_family"];
        if (val != null) obj.DnsOptClientFamily = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_opt_client_netmask"];
        if (val != null) obj.DnsOptClientNetmask = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_opt_client_scope"];
        if (val != null) obj.DnsOptClientScope = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_opt_client_addr"];
        if (val != null) obj.DnsOptClientAddr = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_opt_client_addr4"];
        if (val != null) obj.DnsOptClientAddr4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_opt_client_addr6"];
        if (val != null) obj.DnsOptClientAddr6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_opt_cookie_client"];
        if (val != null) obj.DnsOptCookieClient = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_opt_cookie_server"];
        if (val != null) obj.DnsOptCookieServer = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_opt_edns_tcp_keepalive_timeout"];
        if (val != null) obj.DnsOptEdnsTcpKeepaliveTimeout = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_opt_dns_opt_padding"];
        if (val != null) obj.DnsOptPadding = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_opt_chain_fqdn"];
        if (val != null) obj.DnsOptChainFqdn = val.Value<string>();
      }
      {
        var val = token["dns_dns_count_queries"];
        if (val != null) obj.DnsCountQueries = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_count_zones"];
        if (val != null) obj.DnsCountZones = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_count_answers"];
        if (val != null) obj.DnsCountAnswers = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_count_prerequisites"];
        if (val != null) obj.DnsCountPrerequisites = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_count_auth_rr"];
        if (val != null) obj.DnsCountAuthRr = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_count_updates"];
        if (val != null) obj.DnsCountUpdates = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_nsec3_algo"];
        if (val != null) obj.DnsNsec3Algo = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_nsec3_flags"];
        if (val != null) obj.DnsNsec3Flags = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_nsec3_flags_dns_nsec3_flags_opt_out"];
        if (val != null) obj.DnsNsec3FlagsOptOut = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dns_nsec3_iterations"];
        if (val != null) obj.DnsNsec3Iterations = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_nsec3_salt_length"];
        if (val != null) obj.DnsNsec3SaltLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_nsec3_salt_value"];
        if (val != null) obj.DnsNsec3SaltValue = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_nsec3_hash_length"];
        if (val != null) obj.DnsNsec3HashLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_nsec3_hash_value"];
        if (val != null) obj.DnsNsec3HashValue = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_tlsa_certificate_usage"];
        if (val != null) obj.DnsTlsaCertificateUsage = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tlsa_selector"];
        if (val != null) obj.DnsTlsaSelector = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tlsa_matching_type"];
        if (val != null) obj.DnsTlsaMatchingType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tlsa_certificate_association_data"];
        if (val != null) obj.DnsTlsaCertificateAssociationData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_tsig_algorithm_name"];
        if (val != null) obj.DnsTsigAlgorithmName = val.Value<string>();
      }
      {
        var val = token["dns_dns_tsig_time_signed"];
        if (val != null) obj.DnsTsigTimeSigned = default(Int64);
      }
      {
        var val = token["dns_dns_tsig_original_id"];
        if (val != null) obj.DnsTsigOriginalId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tsig_error"];
        if (val != null) obj.DnsTsigError = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tsig_fudge"];
        if (val != null) obj.DnsTsigFudge = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tsig_mac_size"];
        if (val != null) obj.DnsTsigMacSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tsig_other_len"];
        if (val != null) obj.DnsTsigOtherLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_tsig_mac"];
        if (val != null) obj.DnsTsigMac = default(Int32);
      }
      {
        var val = token["dns_dns_tsig_other_data"];
        if (val != null) obj.DnsTsigOtherData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_sshfp_algorithm"];
        if (val != null) obj.DnsSshfpAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_sshfp_fingerprint_dns_sshfp_fingerprint_type"];
        if (val != null) obj.DnsSshfpFingerprintType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_sshfp_fingerprint"];
        if (val != null) obj.DnsSshfpFingerprint = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_hip_hit_dns_hip_hit_length"];
        if (val != null) obj.DnsHipHitLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_hip_hit_pk_algo"];
        if (val != null) obj.DnsHipHitPkAlgo = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_hip_pk_dns_hip_pk_length"];
        if (val != null) obj.DnsHipPkLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_hip_hit"];
        if (val != null) obj.DnsHipHit = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_hip_pk"];
        if (val != null) obj.DnsHipPk = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_hip_rendezvous_server"];
        if (val != null) obj.DnsHipRendezvousServer = val.Value<string>();
      }
      {
        var val = token["dns_dns_dhcid_rdata"];
        if (val != null) obj.DnsDhcidRdata = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_ds_key_id"];
        if (val != null) obj.DnsDsKeyId = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_dns_ds_algorithm"];
        if (val != null) obj.DnsDsAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ds_digest_type"];
        if (val != null) obj.DnsDsDigestType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_ds_digest"];
        if (val != null) obj.DnsDsDigest = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_apl_address_family"];
        if (val != null) obj.DnsAplAddressFamily = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_apl_coded_prefix"];
        if (val != null) obj.DnsAplCodedPrefix = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_apl_negation"];
        if (val != null) obj.DnsAplNegation = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dns_apl_afdlength"];
        if (val != null) obj.DnsAplAfdlength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_apl_afdpart_ipv4"];
        if (val != null) obj.DnsAplAfdpartIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_apl_afdpart_ipv6"];
        if (val != null) obj.DnsAplAfdpartIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_apl_afdpart_data"];
        if (val != null) obj.DnsAplAfdpartData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_gpos_longitude_length"];
        if (val != null) obj.DnsGposLongitudeLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_gpos_longitude"];
        if (val != null) obj.DnsGposLongitude = val.Value<string>();
      }
      {
        var val = token["dns_dns_gpos_latitude_length"];
        if (val != null) obj.DnsGposLatitudeLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_gpos_latitude"];
        if (val != null) obj.DnsGposLatitude = val.Value<string>();
      }
      {
        var val = token["dns_dns_gpos_altitude_length"];
        if (val != null) obj.DnsGposAltitudeLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_gpos_altitude"];
        if (val != null) obj.DnsGposAltitude = val.Value<string>();
      }
      {
        var val = token["dns_dns_rp_mailbox"];
        if (val != null) obj.DnsRpMailbox = val.Value<string>();
      }
      {
        var val = token["dns_dns_rp_txt_rr"];
        if (val != null) obj.DnsRpTxtRr = val.Value<string>();
      }
      {
        var val = token["dns_dns_afsdb_subtype"];
        if (val != null) obj.DnsAfsdbSubtype = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_afsdb_hostname"];
        if (val != null) obj.DnsAfsdbHostname = val.Value<string>();
      }
      {
        var val = token["dns_dns_x25_length"];
        if (val != null) obj.DnsX25Length = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_x25_psdn_address"];
        if (val != null) obj.DnsX25PsdnAddress = val.Value<string>();
      }
      {
        var val = token["dns_dns_idsn_length"];
        if (val != null) obj.DnsIdsnLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_idsn_address"];
        if (val != null) obj.DnsIdsnAddress = val.Value<string>();
      }
      {
        var val = token["dns_dns_idsn_sa_length"];
        if (val != null) obj.DnsIdsnSaLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_idsn_sa_address"];
        if (val != null) obj.DnsIdsnSaAddress = val.Value<string>();
      }
      {
        var val = token["dns_dns_rt_subtype"];
        if (val != null) obj.DnsRtSubtype = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_rt_intermediate_host"];
        if (val != null) obj.DnsRtIntermediateHost = val.Value<string>();
      }
      {
        var val = token["dns_dns_nsap_rdata"];
        if (val != null) obj.DnsNsapRdata = StringToBytes(val.Value<string>());
      }
      {
        var val = token["dns_dns_nsap_ptr_owner"];
        if (val != null) obj.DnsNsapPtrOwner = val.Value<string>();
      }
      {
        var val = token["dns_dns_caa_flags"];
        if (val != null) obj.DnsCaaFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["dns_caa_flags_dns_caa_flags_issuer_critical"];
        if (val != null) obj.DnsCaaFlagsIssuerCritical = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dns_caa_issue"];
        if (val != null) obj.DnsCaaIssue = val.Value<string>();
      }
      {
        var val = token["dns_dns_caa_issuewild"];
        if (val != null) obj.DnsCaaIssuewild = val.Value<string>();
      }
      {
        var val = token["dns_dns_caa_iodef"];
        if (val != null) obj.DnsCaaIodef = val.Value<string>();
      }
      {
        var val = token["dns_dns_caa_unknown"];
        if (val != null) obj.DnsCaaUnknown = val.Value<string>();
      }
      {
        var val = token["dns_dns_caa_tag_length"];
        if (val != null) obj.DnsCaaTagLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_caa_tag"];
        if (val != null) obj.DnsCaaTag = val.Value<string>();
      }
      {
        var val = token["dns_dns_caa_value"];
        if (val != null) obj.DnsCaaValue = val.Value<string>();
      }
      {
        var val = token["dns_dns_wins_local_flag"];
        if (val != null) obj.DnsWinsLocalFlag = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dns_wins_lookup_timeout"];
        if (val != null) obj.DnsWinsLookupTimeout = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_wins_cache_timeout"];
        if (val != null) obj.DnsWinsCacheTimeout = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_wins_nb_wins_servers"];
        if (val != null) obj.DnsWinsNbWinsServers = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_wins_wins_server"];
        if (val != null) obj.DnsWinsWinsServer = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["dns_dns_winsr_local_flag"];
        if (val != null) obj.DnsWinsrLocalFlag = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["dns_dns_winsr_lookup_timeout"];
        if (val != null) obj.DnsWinsrLookupTimeout = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_winsr_cache_timeout"];
        if (val != null) obj.DnsWinsrCacheTimeout = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["dns_dns_winsr_name_result_domain"];
        if (val != null) obj.DnsWinsrNameResultDomain = val.Value<string>();
      }
      {
        var val = token["dns_dns_data"];
        if (val != null) obj.DnsData = StringToBytes(val.Value<string>());
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

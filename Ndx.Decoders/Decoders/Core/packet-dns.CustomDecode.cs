using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
using static Ndx.Decoders.Core.Dns.Types;
using System.Linq;

namespace Ndx.Decoders.Core
{
    public sealed partial class Dns
    {
        public static Dns DecodeJson(string jsonLine)
        {
            var jsonObject = CustomJsonDeserializer.Parse(jsonLine);
            return DecodeJson(jsonObject);
        }

        public static Dns DecodeJson(JToken token)
        {
            var obj = new Dns() { DnsFlags = new _DnsFlags() };
            _DnsQry[] dnsQry = null;
            _DnsResp[] dnsResp = null;



            foreach (var field in token)
            {
                var prop = (field as JProperty);
                if (prop == null) continue;
                var val = prop.Value;
                switch (prop.Name)
                {
                    case "dns_flags_dns_flags_response":
                        obj.DnsFlags.DnsFlagsResponse = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_opcode":
                        obj.DnsFlags.DnsFlagsOpcode = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_flags_dns_flags_authoritative":
                        obj.DnsFlags.DnsFlagsAuthoritative = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_conflict":
                        obj.DnsFlags.DnsFlagsConflict = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_truncated":
                        obj.DnsFlags.DnsFlagsTruncated = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_recdesired":
                        obj.DnsFlags.DnsFlagsRecdesired = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_tentative":
                        obj.DnsFlags.DnsFlagsTentative = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_recavail":
                        obj.DnsFlags.DnsFlagsRecavail = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_z":
                        obj.DnsFlags.DnsFlagsZ = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_authenticated":
                        obj.DnsFlags.DnsFlagsAuthenticated = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_checkdisable":
                        obj.DnsFlags.DnsFlagsCheckdisable = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_flags_dns_flags_rcode":
                        obj.DnsFlags.DnsFlagsRcode = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_id":
                        obj.DnsId = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "text_dns_qry_type":
                        SetArray(dnsQry, (r, t) => r.DnsQryType = t, x => Convert.ToUInt32(x, 10), val);
                        break;

                    case "text_dns_qry_class":
                        SetArray(dnsQry, (r, t) => r.DnsQryClass = t, x => Convert.ToUInt32(x, 16), val);
                        break;

                    case "text_dns_qry_name":
                        dnsQry = MakeArray((value) => new _DnsQry() { DnsQryName = value }, str => str, val);
                        break;

                    case "text_dns_qry_name_len":
                        SetArray(dnsQry, (r,t)=> r.DnsQryNameLen = t, x=> Convert.ToUInt32(x, 10), val);
                        break;

                    case "dns_dns_count_labels":
                        SetArray(dnsQry, (r, t) => r.DnsCountLabels = t, x => Convert.ToInt32(x, 10), val);
                        break;

                    case "text_dns_resp_type":
                        SetArray(dnsResp, (r, t) => r.DnsRespType = t, x => Convert.ToUInt32(x, 10), val);
                        break;

                    case "text_dns_resp_class":
                        SetArray(dnsResp, (r, t) => r.DnsRespClass = t, x => Convert.ToUInt32(x, 16), val);
                        break;

                    case "dns_dns_resp_cache_flush":
                        SetArray(dnsResp, (r, t) => r.DnsRespCacheFlush = t, x => Convert.ToInt32(x, 10) != 0, val);
                        break;

                    case "dns_dns_resp_ext_rcode":
                        SetArray(dnsResp, (r, t) => r.DnsRespExtRcode = t, x => Convert.ToUInt32(x, 16), val);
                        break;

                    case "dns_dns_resp_edns0_version":
                        SetArray(dnsResp, (r, t) => r.DnsRespEdns0Version = t, x => Convert.ToUInt32(x, 10), val);
                        break;

                    case "dns_dns_resp_z":
                        SetArray(dnsResp, (r, t) => r.DnsRespZ = t, x => Convert.ToUInt32(x, 16), val);
                        break;

                    case "dns_resp_z_dns_resp_z_do":
                        SetArray(dnsResp, (r, t) => r.DnsRespZDo = t, x => Convert.ToInt32(x, 10) != 0, val);
                        break;

                    case "dns_resp_z_dns_resp_z_reserved":
                        SetArray(dnsResp, (r, t) => r.DnsRespZReserved = t, x => Convert.ToUInt32(x, 16), val);
                        break;

                    case "text_dns_srv_service":
                        obj.DnsSrvService = val.Value<string>();
                        break;

                    case "text_dns_srv_proto":
                        obj.DnsSrvProto = val.Value<string>();
                        break;

                    case "text_dns_srv_name":
                        obj.DnsSrvName = val.Value<string>();
                        break;

                    case "text_dns_srv_priority":
                        obj.DnsSrvPriority = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_srv_weight":
                        obj.DnsSrvWeight = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_srv_port":
                        obj.DnsSrvPort = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_srv_target":
                        obj.DnsSrvTarget = val.Value<string>();
                        break;

                    case "text_dns_naptr_order":
                        obj.DnsNaptrOrder = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_naptr_preference":
                        obj.DnsNaptrPreference = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_naptr_flags_length":
                        obj.DnsNaptrFlagsLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_naptr_flags":
                        obj.DnsNaptrFlags = val.Value<string>();
                        break;

                    case "text_dns_naptr_service_length":
                        obj.DnsNaptrServiceLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_naptr_service":
                        obj.DnsNaptrService = val.Value<string>();
                        break;

                    case "text_dns_naptr_regex_length":
                        obj.DnsNaptrRegexLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_naptr_regex":
                        obj.DnsNaptrRegex = val.Value<string>();
                        break;

                    case "text_dns_naptr_replacement_length":
                        obj.DnsNaptrReplacementLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_naptr_replacement":
                        obj.DnsNaptrReplacement = val.Value<string>();
                        break;

                    case "text_dns_resp_name":
                        dnsResp = MakeArray((value) => new _DnsResp() { DnsRespName = value }, str => str, val);
                        break;

                    case "text_dns_resp_ttl":
                        SetArray(dnsResp, (source, value) => source.DnsRespTtl = value, str=>Convert.ToInt32(str, 10),  val);
                        break;

                    case "text_dns_resp_len":
                        SetArray(dnsResp, (source, value) => source.DnsRespLen = value, str => Convert.ToUInt32(str, 10), val);
                        break;

                    case "text_dns_a":
                        obj.DnsA = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "text_dns_md":
                        obj.DnsMd = val.Value<string>();
                        break;

                    case "text_dns_mf":
                        obj.DnsMf = val.Value<string>();
                        break;

                    case "text_dns_mb":
                        obj.DnsMb = val.Value<string>();
                        break;

                    case "text_dns_mg":
                        obj.DnsMg = val.Value<string>();
                        break;

                    case "text_dns_mr":
                        obj.DnsMr = val.Value<string>();
                        break;

                    case "dns_dns_null":
                        obj.DnsNull = StringToBytes(val.Value<string>());
                        break;

                    case "text_dns_aaaa":
                        obj.DnsAaaa = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "text_dns_cname":
                        obj.DnsCname = val.Value<string>();
                        break;

                    case "text_dns_rr_udp_payload_size":
                        obj.DnsRrUdpPayloadSize = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_dns_soa_mname":
                        obj.DnsSoaMname = val.Value<string>();
                        break;

                    case "text_dns_soa_rname":
                        obj.DnsSoaRname = val.Value<string>();
                        break;

                    case "text_dns_soa_serial_number":
                        obj.DnsSoaSerialNumber = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_soa_refresh_interval":
                        obj.DnsSoaRefreshInterval = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_soa_retry_interval":
                        obj.DnsSoaRetryInterval = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_soa_expire_limit":
                        obj.DnsSoaExpireLimit = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_soa_mininum_ttl":
                        obj.DnsSoaMininumTtl = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_ptr_domain_name":
                        obj.DnsPtrDomainName = val.Value<string>();
                        break;

                    case "dns_dns_wks_address":
                        obj.DnsWksAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_wks_protocol":
                        obj.DnsWksProtocol = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_wks_bits":
                        obj.DnsWksBits = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_dns_hinfo_cpu_length":
                        obj.DnsHinfoCpuLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_hinfo_cpu":
                        obj.DnsHinfoCpu = val.Value<string>();
                        break;

                    case "dns_dns_hinfo_os_length":
                        obj.DnsHinfoOsLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_hinfo_os":
                        obj.DnsHinfoOs = val.Value<string>();
                        break;

                    case "dns_dns_minfo_r":
                        obj.DnsMinfoR = val.Value<string>();
                        break;

                    case "dns_dns_minfo_e":
                        obj.DnsMinfoE = val.Value<string>();
                        break;

                    case "text_dns_mx_preference":
                        obj.DnsMxPreference = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "text_dns_mx_mail_exchange":
                        obj.DnsMxMailExchange = val.Value<string>();
                        break;

                    case "dns_txt_dns_txt_length":
                        obj.DnsTxtLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_txt":
                        obj.DnsTxt = val.Value<string>();
                        break;

                    case "dns_dns_openpgpkey":
                        obj.DnsOpenpgpkey = val.Value<string>();
                        break;

                    case "dns_dns_csync_soa":
                        obj.DnsCsyncSoa = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_csync_flags":
                        obj.DnsCsyncFlags = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_csync_flags_dns_csync_flags_immediate":
                        obj.DnsCsyncFlagsImmediate = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_csync_flags_dns_csync_flags_soaminimum":
                        obj.DnsCsyncFlagsSoaminimum = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dns_csync_type_bitmap":
                        obj.DnsCsyncTypeBitmap = StringToBytes(val.Value<string>());
                        break;

                    case "dns_spf_dns_spf_length":
                        obj.DnsSpfLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_spf":
                        obj.DnsSpf = val.Value<string>();
                        break;

                    case "dns_ilnp_nid_dns_ilnp_nid_preference":
                        obj.DnsIlnpNidPreference = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ilnp_nid":
                        obj.DnsIlnpNid = StringToBytes(val.Value<string>());
                        break;

                    case "dns_ilnp_l32_dns_ilnp_l32_preference":
                        obj.DnsIlnpL32Preference = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ilnp_l32":
                        obj.DnsIlnpL32 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_ilnp_l64_dns_ilnp_l64_preference":
                        obj.DnsIlnpL64Preference = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ilnp_l64":
                        obj.DnsIlnpL64 = StringToBytes(val.Value<string>());
                        break;

                    case "dns_ilnp_lp_dns_ilnp_lp_preference":
                        obj.DnsIlnpLpPreference = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ilnp_lp":
                        obj.DnsIlnpLp = val.Value<string>();
                        break;

                    case "dns_dns_eui48":
                        obj.DnsEui48 = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':', '-')).GetAddressBytes());
                        break;

                    case "dns_dns_eui64":
                        obj.DnsEui64 = default(ByteString);
                        break;

                    case "dns_dns_rrsig_type_covered":
                        obj.DnsRrsigTypeCovered = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_rrsig_algorithm":
                        obj.DnsRrsigAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_rrsig_labels":
                        obj.DnsRrsigLabels = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_rrsig_original_ttl":
                        obj.DnsRrsigOriginalTtl = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_rrsig_signature_expiration":
                        obj.DnsRrsigSignatureExpiration = default(Int64);
                        break;

                    case "dns_dns_rrsig_signature_inception":
                        obj.DnsRrsigSignatureInception = default(Int64);
                        break;

                    case "dns_dns_rrsig_key_tag":
                        obj.DnsRrsigKeyTag = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_rrsig_signers_name":
                        obj.DnsRrsigSignersName = val.Value<string>();
                        break;

                    case "dns_dns_rrsig_signature":
                        obj.DnsRrsigSignature = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_dnskey_flags":
                        obj.DnsDnskeyFlags = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_dnskey_flags_dns_dnskey_flags_zone_key":
                        obj.DnsDnskeyFlagsZoneKey = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dnskey_flags_dns_dnskey_flags_key_revoked":
                        obj.DnsDnskeyFlagsKeyRevoked = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dnskey_flags_dns_dnskey_flags_secure_entry_point":
                        obj.DnsDnskeyFlagsSecureEntryPoint = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dnskey_flags_dns_dnskey_flags_reserved":
                        obj.DnsDnskeyFlagsReserved = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_dns_dnskey_protocol":
                        obj.DnsDnskeyProtocol = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_dnskey_algorithm":
                        obj.DnsDnskeyAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_dnskey_key_id":
                        obj.DnsDnskeyKeyId = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_dnskey_public_key":
                        obj.DnsDnskeyPublicKey = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_key_flags":
                        obj.DnsKeyFlags = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_key_flags_dns_key_flags_authentication":
                        obj.DnsKeyFlagsAuthentication = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_key_flags_dns_key_flags_confidentiality":
                        obj.DnsKeyFlagsConfidentiality = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_key_flags_dns_key_flags_required":
                        obj.DnsKeyFlagsRequired = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_key_flags_dns_key_flags_associated_user":
                        obj.DnsKeyFlagsAssociatedUser = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_key_flags_dns_key_flags_associated_named_entity":
                        obj.DnsKeyFlagsAssociatedNamedEntity = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_key_flags_dns_key_flags_ipsec":
                        obj.DnsKeyFlagsIpsec = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_key_flags_dns_key_flags_mime":
                        obj.DnsKeyFlagsMime = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_key_flags_dns_key_flags_signatory":
                        obj.DnsKeyFlagsSignatory = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_key_protocol":
                        obj.DnsKeyProtocol = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_key_algorithm":
                        obj.DnsKeyAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_key_key_id":
                        obj.DnsKeyKeyId = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_key_public_key":
                        obj.DnsKeyPublicKey = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_px_preference":
                        obj.DnsPxPreference = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_px_map822":
                        obj.DnsPxMap822 = val.Value<string>();
                        break;

                    case "dns_dns_px_map400":
                        obj.DnsPxMap400 = val.Value<string>();
                        break;

                    case "dns_dns_tkey_algo_name":
                        obj.DnsTkeyAlgoName = val.Value<string>();
                        break;

                    case "dns_dns_tkey_signature_expiration":
                        obj.DnsTkeySignatureExpiration = default(Int64);
                        break;

                    case "dns_dns_tkey_signature_inception":
                        obj.DnsTkeySignatureInception = default(Int64);
                        break;

                    case "dns_dns_tkey_mode":
                        obj.DnsTkeyMode = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tkey_error":
                        obj.DnsTkeyError = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tkey_key_size":
                        obj.DnsTkeyKeySize = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tkey_key_data":
                        obj.DnsTkeyKeyData = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_tkey_other_size":
                        obj.DnsTkeyOtherSize = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tkey_other_data":
                        obj.DnsTkeyOtherData = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_ipseckey_gateway_precedence":
                        obj.DnsIpseckeyGatewayPrecedence = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ipseckey_gateway_algorithm":
                        obj.DnsIpseckeyGatewayAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ipseckey_gateway_type":
                        obj.DnsIpseckeyGatewayType = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ipseckey_gateway_ipv4":
                        obj.DnsIpseckeyGatewayIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_ipseckey_gateway_ipv6":
                        obj.DnsIpseckeyGatewayIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_ipseckey_gateway_dns":
                        obj.DnsIpseckeyGatewayDns = val.Value<string>();
                        break;

                    case "dns_dns_ipseckey_public_key":
                        obj.DnsIpseckeyPublicKey = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_xpf_ip_version":
                        obj.DnsXpfIpVersion = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_xpf_protocol":
                        obj.DnsXpfProtocol = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_xpf_source_ipv4":
                        obj.DnsXpfSourceIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_xpf_destination_ipv4":
                        obj.DnsXpfDestinationIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_xpf_source_ipv6":
                        obj.DnsXpfSourceIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_xpf_destination_ipv6":
                        obj.DnsXpfDestinationIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_xpf_sport":
                        obj.DnsXpfSport = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_xpf_dport":
                        obj.DnsXpfDport = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_a6_prefix_len":
                        obj.DnsA6PrefixLen = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_a6_address_suffix":
                        obj.DnsA6AddressSuffix = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_a6_prefix_name":
                        obj.DnsA6PrefixName = val.Value<string>();
                        break;

                    case "dns_dns_dname":
                        obj.DnsDname = val.Value<string>();
                        break;

                    case "dns_dns_loc_version":
                        obj.DnsLocVersion = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_loc_size":
                        obj.DnsLocSize = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_loc_horizontal_precision":
                        obj.DnsLocHorizontalPrecision = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_loc_vertical_precision":
                        obj.DnsLocVerticalPrecision = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_loc_latitude":
                        obj.DnsLocLatitude = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_loc_longitude":
                        obj.DnsLocLongitude = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_loc_altitude":
                        obj.DnsLocAltitude = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_loc_unknown_data":
                        obj.DnsLocUnknownData = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_nxt_next_domain_name":
                        obj.DnsNxtNextDomainName = val.Value<string>();
                        break;

                    case "dns_dns_kx_preference":
                        obj.DnsKxPreference = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_kx_key_exchange":
                        obj.DnsKxKeyExchange = val.Value<string>();
                        break;

                    case "dns_dns_cert_type":
                        obj.DnsCertType = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_cert_key_tag":
                        obj.DnsCertKeyTag = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_dns_cert_algorithm":
                        obj.DnsCertAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_cert_certificate":
                        obj.DnsCertCertificate = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_nsec_next_domain_name":
                        obj.DnsNsecNextDomainName = val.Value<string>();
                        break;

                    case "text_dns_ns":
                        obj.DnsNs = val.Value<string>();
                        break;

                    case "dns_dns_opt":
                        obj.DnsOpt = default(Int32);
                        break;

                    case "dns_opt_dns_opt_code":
                        obj.DnsOptCode = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_opt_dns_opt_len":
                        obj.DnsOptLen = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_opt_dns_opt_data":
                        obj.DnsOptData = StringToBytes(val.Value<string>());
                        break;

                    case "dns_opt_dns_opt_dau":
                        obj.DnsOptDau = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_opt_dns_opt_dhu":
                        obj.DnsOptDhu = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_opt_dns_opt_n3u":
                        obj.DnsOptN3U = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_opt_client_family":
                        obj.DnsOptClientFamily = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_opt_client_netmask":
                        obj.DnsOptClientNetmask = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_opt_client_scope":
                        obj.DnsOptClientScope = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_opt_client_addr":
                        obj.DnsOptClientAddr = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_opt_client_addr4":
                        obj.DnsOptClientAddr4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_opt_client_addr6":
                        obj.DnsOptClientAddr6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_opt_cookie_client":
                        obj.DnsOptCookieClient = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_opt_cookie_server":
                        obj.DnsOptCookieServer = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_opt_edns_tcp_keepalive_timeout":
                        obj.DnsOptEdnsTcpKeepaliveTimeout = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_opt_dns_opt_padding":
                        obj.DnsOptPadding = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_opt_chain_fqdn":
                        obj.DnsOptChainFqdn = val.Value<string>();
                        break;

                    case "dns_dns_count_queries":
                        obj.DnsCountQueries = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_count_zones":
                        obj.DnsCountZones = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_count_answers":
                        obj.DnsCountAnswers = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_count_prerequisites":
                        obj.DnsCountPrerequisites = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_count_auth_rr":
                        obj.DnsCountAuthRr = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_count_updates":
                        obj.DnsCountUpdates = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_nsec3_algo":
                        obj.DnsNsec3Algo = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_nsec3_flags":
                        obj.DnsNsec3Flags = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_nsec3_flags_dns_nsec3_flags_opt_out":
                        obj.DnsNsec3FlagsOptOut = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dns_nsec3_iterations":
                        obj.DnsNsec3Iterations = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_nsec3_salt_length":
                        obj.DnsNsec3SaltLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_nsec3_salt_value":
                        obj.DnsNsec3SaltValue = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_nsec3_hash_length":
                        obj.DnsNsec3HashLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_nsec3_hash_value":
                        obj.DnsNsec3HashValue = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_tlsa_certificate_usage":
                        obj.DnsTlsaCertificateUsage = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tlsa_selector":
                        obj.DnsTlsaSelector = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tlsa_matching_type":
                        obj.DnsTlsaMatchingType = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tlsa_certificate_association_data":
                        obj.DnsTlsaCertificateAssociationData = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_tsig_algorithm_name":
                        obj.DnsTsigAlgorithmName = val.Value<string>();
                        break;

                    case "dns_dns_tsig_time_signed":
                        obj.DnsTsigTimeSigned = default(Int64);
                        break;

                    case "dns_dns_tsig_original_id":
                        obj.DnsTsigOriginalId = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tsig_error":
                        obj.DnsTsigError = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tsig_fudge":
                        obj.DnsTsigFudge = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tsig_mac_size":
                        obj.DnsTsigMacSize = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tsig_other_len":
                        obj.DnsTsigOtherLen = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_tsig_mac":
                        obj.DnsTsigMac = default(Int32);
                        break;

                    case "dns_dns_tsig_other_data":
                        obj.DnsTsigOtherData = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_response_in":
                        obj.DnsResponseIn = default(Int64);
                        break;

                    case "dns_dns_response_to":
                        obj.DnsResponseTo = default(Int64);
                        break;

                    case "dns_dns_time":
                        obj.DnsTime = default(Int64);
                        break;

                    case "dns_dns_count_add_rr":
                        obj.DnsCountAddRr = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_sshfp_algorithm":
                        obj.DnsSshfpAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_sshfp_fingerprint_dns_sshfp_fingerprint_type":
                        obj.DnsSshfpFingerprintType = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_sshfp_fingerprint":
                        obj.DnsSshfpFingerprint = StringToBytes(val.Value<string>());
                        break;

                    case "dns_hip_hit_dns_hip_hit_length":
                        obj.DnsHipHitLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_hip_hit_pk_algo":
                        obj.DnsHipHitPkAlgo = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_hip_pk_dns_hip_pk_length":
                        obj.DnsHipPkLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_hip_hit":
                        obj.DnsHipHit = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_hip_pk":
                        obj.DnsHipPk = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_hip_rendezvous_server":
                        obj.DnsHipRendezvousServer = val.Value<string>();
                        break;

                    case "dns_dns_dhcid_rdata":
                        obj.DnsDhcidRdata = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_ds_key_id":
                        obj.DnsDsKeyId = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_dns_ds_algorithm":
                        obj.DnsDsAlgorithm = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ds_digest_type":
                        obj.DnsDsDigestType = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_ds_digest":
                        obj.DnsDsDigest = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_apl_address_family":
                        obj.DnsAplAddressFamily = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_apl_coded_prefix":
                        obj.DnsAplCodedPrefix = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_apl_negation":
                        obj.DnsAplNegation = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dns_apl_afdlength":
                        obj.DnsAplAfdlength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_apl_afdpart_ipv4":
                        obj.DnsAplAfdpartIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_apl_afdpart_ipv6":
                        obj.DnsAplAfdpartIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_apl_afdpart_data":
                        obj.DnsAplAfdpartData = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_gpos_longitude_length":
                        obj.DnsGposLongitudeLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_gpos_longitude":
                        obj.DnsGposLongitude = val.Value<string>();
                        break;

                    case "dns_dns_gpos_latitude_length":
                        obj.DnsGposLatitudeLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_gpos_latitude":
                        obj.DnsGposLatitude = val.Value<string>();
                        break;

                    case "dns_dns_gpos_altitude_length":
                        obj.DnsGposAltitudeLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_gpos_altitude":
                        obj.DnsGposAltitude = val.Value<string>();
                        break;

                    case "dns_dns_rp_mailbox":
                        obj.DnsRpMailbox = val.Value<string>();
                        break;

                    case "dns_dns_rp_txt_rr":
                        obj.DnsRpTxtRr = val.Value<string>();
                        break;

                    case "dns_dns_afsdb_subtype":
                        obj.DnsAfsdbSubtype = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_afsdb_hostname":
                        obj.DnsAfsdbHostname = val.Value<string>();
                        break;

                    case "dns_dns_x25_length":
                        obj.DnsX25Length = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_x25_psdn_address":
                        obj.DnsX25PsdnAddress = val.Value<string>();
                        break;

                    case "dns_dns_idsn_length":
                        obj.DnsIdsnLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_idsn_address":
                        obj.DnsIdsnAddress = val.Value<string>();
                        break;

                    case "dns_dns_idsn_sa_length":
                        obj.DnsIdsnSaLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_idsn_sa_address":
                        obj.DnsIdsnSaAddress = val.Value<string>();
                        break;

                    case "dns_dns_rt_subtype":
                        obj.DnsRtSubtype = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_rt_intermediate_host":
                        obj.DnsRtIntermediateHost = val.Value<string>();
                        break;

                    case "dns_dns_nsap_rdata":
                        obj.DnsNsapRdata = StringToBytes(val.Value<string>());
                        break;

                    case "dns_dns_nsap_ptr_owner":
                        obj.DnsNsapPtrOwner = val.Value<string>();
                        break;

                    case "dns_dns_caa_flags":
                        obj.DnsCaaFlags = Convert.ToUInt32(val.Value<string>(), 16);
                        break;

                    case "dns_caa_flags_dns_caa_flags_issuer_critical":
                        obj.DnsCaaFlagsIssuerCritical = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dns_caa_issue":
                        obj.DnsCaaIssue = val.Value<string>();
                        break;

                    case "dns_dns_caa_issuewild":
                        obj.DnsCaaIssuewild = val.Value<string>();
                        break;

                    case "dns_dns_caa_iodef":
                        obj.DnsCaaIodef = val.Value<string>();
                        break;

                    case "dns_dns_caa_unknown":
                        obj.DnsCaaUnknown = val.Value<string>();
                        break;

                    case "dns_dns_caa_tag_length":
                        obj.DnsCaaTagLength = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_caa_tag":
                        obj.DnsCaaTag = val.Value<string>();
                        break;

                    case "dns_dns_caa_value":
                        obj.DnsCaaValue = val.Value<string>();
                        break;

                    case "dns_dns_wins_local_flag":
                        obj.DnsWinsLocalFlag = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dns_wins_lookup_timeout":
                        obj.DnsWinsLookupTimeout = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_wins_cache_timeout":
                        obj.DnsWinsCacheTimeout = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_wins_nb_wins_servers":
                        obj.DnsWinsNbWinsServers = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_wins_wins_server":
                        obj.DnsWinsWinsServer = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
                        break;

                    case "dns_dns_winsr_local_flag":
                        obj.DnsWinsrLocalFlag = Convert.ToInt32(val.Value<string>(), 10) != 0;
                        break;

                    case "dns_dns_winsr_lookup_timeout":
                        obj.DnsWinsrLookupTimeout = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_winsr_cache_timeout":
                        obj.DnsWinsrCacheTimeout = Convert.ToUInt32(val.Value<string>(), 10);
                        break;

                    case "dns_dns_winsr_name_result_domain":
                        obj.DnsWinsrNameResultDomain = val.Value<string>();
                        break;

                    case "dns_dns_data":
                        obj.DnsData = StringToBytes(val.Value<string>());
                        break;
                }
            }
            if (dnsQry != null) obj.DnsQry.Add(dnsQry);
            if (dnsResp != null) obj.DnsResp.Add(dnsResp);
            return obj;
        }

        private static R[] MakeArray<R,T>(Func<T, R> constructor, Func<string, T> converter, JToken val) 
        {
            if (val is JArray)
            {
                return(val as JArray).Select(x => converter(x.Value<string>())).Select(constructor).ToArray();
            }
            else
            {
                return new R[] { constructor(converter(val.Value<string>()))};
            }
        }

        private static void SetArray<R,T>(R[] source, Action<R, T> setter, Func<string,T> converter, JToken values)
        {
            if (values is JArray)
            {
                int i = 0;
                foreach (var x in (values as JArray).Values<string>())
                {
                    setter(source[i], converter(x));
                    i++;
                }
            }
            else
            {
                setter(source[0], converter(values.Value<string>()));
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
            return Google.Protobuf.ByteString.CopyFrom(byteArray);
        }
    }
}
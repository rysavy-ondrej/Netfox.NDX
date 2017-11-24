using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
using static Ndx.Decoders.Core.Dns.Types;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Ndx.Decoders.Core
{
    public sealed partial class Dns
    {
        public static Dns DecodeJson(string jsonLine)
        {
            using (var reader = new JsonTextReader(new StringReader(jsonLine)))
            {
                return DecodeJson(reader);
            }

        }


        public static Dns DecodeJson(JsonTextReader reader)
        {
            if (reader.TokenType != JsonToken.StartObject) return null;
            var obj = new Dns() { DnsFlags = new _DnsFlags() };
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

        static void SetField(Dns obj, string propName, string propValue)
        {
            switch (propName)
            {
                case "dns_flags_dns_flags_response":
                    obj.DnsFlags.DnsFlagsResponse = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_opcode":
                    obj.DnsFlags.DnsFlagsOpcode = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_flags_dns_flags_authoritative":
                    obj.DnsFlags.DnsFlagsAuthoritative = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_conflict":
                    obj.DnsFlags.DnsFlagsConflict = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_truncated":
                    obj.DnsFlags.DnsFlagsTruncated = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_recdesired":
                    obj.DnsFlags.DnsFlagsRecdesired = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_tentative":
                    obj.DnsFlags.DnsFlagsTentative = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_recavail":
                    obj.DnsFlags.DnsFlagsRecavail = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_z":
                    obj.DnsFlags.DnsFlagsZ = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_authenticated":
                    obj.DnsFlags.DnsFlagsAuthenticated = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_checkdisable":
                    obj.DnsFlags.DnsFlagsCheckdisable = Convert.ToInt32(propValue, 10) != 0;
                    break;

                case "dns_flags_dns_flags_rcode":
                    obj.DnsFlags.DnsFlagsRcode = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_id":
                    obj.DnsId = Convert.ToUInt32(propValue, 16);
                    break;

                case "text_dns_qry_type":
                    obj.DnsQry.Last().DnsQryType = Convert.ToUInt32(propValue, 10);
                    break;
                case "text_dns_qry_class":
                    obj.DnsQry.Last().DnsQryClass = Convert.ToUInt32(propValue, 16);
                    break;

                case "text_dns_qry_name":
                    obj.DnsQry.Add(new _DnsQry() { DnsQryName = propValue });
                    break;

                case "text_dns_qry_name_len":
                    obj.DnsQry.Last().DnsQryNameLen = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_count_labels":
                    obj.DnsQry.Last().DnsCountLabels = Convert.ToInt32(propValue, 10);
                    break;

                case "text_dns_resp_type":
                    obj.DnsResp.Last().DnsRespType = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_resp_class":
                    obj.DnsResp.Last().DnsRespClass = Convert.ToUInt32(propValue, 16);
                    break;

                case "dns_dns_resp_cache_flush":
                    obj.DnsResp.Last().DnsRespCacheFlush = Convert.ToInt32(propValue, 10) != 0;;
                    break;

                case "dns_dns_resp_ext_rcode":
                    obj.DnsResp.Last().DnsRespClass = Convert.ToUInt32(propValue, 16);
                    break;

                case "dns_dns_resp_edns0_version":
                    obj.DnsResp.Last().DnsRespEdns0Version = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_resp_z":
                    obj.DnsResp.Last().DnsRespZ = Convert.ToUInt32(propValue, 16);
                    break;

                case "dns_resp_z_dns_resp_z_do":
                    obj.DnsResp.Last().DnsRespZDo = Convert.ToInt32(propValue, 10) != 0; ;
                    break;

                case "dns_resp_z_dns_resp_z_reserved":
                    obj.DnsResp.Last().DnsRespZReserved = Convert.ToUInt32(propValue, 16);
                    break;

                case "text_dns_srv_service":
                    obj.DnsResp.Last().DnsSrvService = propValue;
                    break;

                case "text_dns_srv_proto":
                    obj.DnsResp.Last().DnsSrvProto = propValue;
                    break;

                case "text_dns_srv_name":
                    obj.DnsResp.Last().DnsSrvName = propValue;
                    break;

                case "text_dns_srv_priority":
                    obj.DnsResp.Last().DnsSrvPriority = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_srv_weight":
                    obj.DnsResp.Last().DnsSrvWeight = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_srv_port":
                    obj.DnsResp.Last().DnsSrvPort = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_srv_target":
                    obj.DnsResp.Last().DnsSrvTarget = propValue;
                    break;

                case "text_dns_naptr_order":
                    obj.DnsResp.Last().DnsNaptrOrder = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_naptr_preference":
                    obj.DnsResp.Last().DnsNaptrPreference = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_naptr_flags_length":
                    obj.DnsResp.Last().DnsNaptrFlagsLength = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_naptr_flags":
                    obj.DnsResp.Last().DnsNaptrFlags = propValue;
                    break;

                case "text_dns_naptr_service_length":
                    obj.DnsResp.Last().DnsNaptrServiceLength = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_naptr_service":
                    obj.DnsResp.Last().DnsNaptrService = propValue;
                    break;

                case "text_dns_naptr_regex_length":
                    obj.DnsResp.Last().DnsNaptrRegexLength = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_naptr_regex":
                    obj.DnsResp.Last().DnsNaptrRegex = propValue;
                    break;

                case "text_dns_naptr_replacement_length":
                    obj.DnsResp.Last().DnsNaptrReplacementLength = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_naptr_replacement":
                    obj.DnsResp.Last().DnsNaptrReplacement = propValue;
                    break;

                case "text_dns_resp_name":
                    obj.DnsResp.Add(new _DnsResp() { DnsRespName = propValue });
                    break;

                case "text_dns_resp_ttl":
                    obj.DnsResp.Last().DnsRespTtl = Convert.ToInt32(propValue, 10);
                    break;

                case "text_dns_resp_len":
                    obj.DnsResp.Last().DnsRespLen = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_a":
                    obj.DnsResp.Last().DnsA = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes());
                    break;

                case "text_dns_md":
                    obj.DnsResp.Last().DnsMd = propValue;
                    break;

                case "text_dns_mf":
                    obj.DnsResp.Last().DnsMf = propValue;
                    break;

                case "text_dns_mb":
                    obj.DnsResp.Last().DnsMb = propValue;
                    break;

                case "text_dns_mg":
                    obj.DnsResp.Last().DnsMg = propValue;
                    break;

                case "text_dns_mr":
                    obj.DnsResp.Last().DnsMr = propValue;
                    break;

                case "dns_dns_null":
                    obj.DnsResp.Last().DnsNull = StringToBytes(propValue);
                    break;

                case "text_dns_aaaa":
                    obj.DnsResp.Last().DnsAaaa = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes());
                    break;

                case "text_dns_cname":
                    obj.DnsResp.Last().DnsCname = propValue;
                    break;

                case "text_dns_rr_udp_payload_size":
                    obj.DnsResp.Last().DnsRrUdpPayloadSize = Convert.ToUInt32(propValue, 16);
                    break;

                case "dns_dns_soa_mname":
                    obj.DnsResp.Last().DnsSoaMname = propValue;
                    break;

                case "text_dns_soa_rname":
                    obj.DnsResp.Last().DnsSoaRname = propValue;
                    break;

                case "text_dns_soa_serial_number":
                    obj.DnsResp.Last().DnsSoaSerialNumber = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_soa_refresh_interval":
                    obj.DnsResp.Last().DnsSoaRefreshInterval = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_soa_retry_interval":
                    obj.DnsResp.Last().DnsSoaRetryInterval = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_soa_expire_limit":
                    obj.DnsResp.Last().DnsSoaExpireLimit = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_soa_mininum_ttl":
                    obj.DnsResp.Last().DnsSoaMininumTtl = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_ptr_domain_name":
                    obj.DnsResp.Last().DnsPtrDomainName = propValue;
                    break;

               

                case "text_dns_mx_preference":
                    obj.DnsResp.Last().DnsMxPreference = Convert.ToUInt32(propValue, 10);
                    break;

                case "text_dns_mx_mail_exchange":
                    obj.DnsResp.Last().DnsMxMailExchange = propValue;
                    break;

                case "dns_txt_dns_txt_length":
                    obj.DnsResp.Last().DnsTxtLength = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_txt":
                    obj.DnsResp.Last().DnsTxt = propValue;
                    break;

              
               
              
                case "text_dns_ns":
                    obj.DnsResp.Last().DnsNs = propValue;
                    break;

               

                case "dns_dns_count_queries":
                    obj.DnsCountQueries = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_count_zones":
                    obj.DnsCountZones = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_count_answers":
                    obj.DnsCountAnswers = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_count_prerequisites":
                    obj.DnsCountPrerequisites = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_count_auth_rr":
                    obj.DnsCountAuthRr = Convert.ToUInt32(propValue, 10);
                    break;

                case "dns_dns_count_updates":
                    obj.DnsCountUpdates = Convert.ToUInt32(propValue, 10);
                    break;

               
                case "dns_dns_response_in":
                    obj.DnsResponseIn = default(Int64);
                    break;

                case "dns_dns_response_to":
                    obj.DnsResponseTo = default(Int64);
                    break;

                case "dns_dns_time":
                    obj.DnsTime = Convert.ToSingle(propValue);
                    break;

                case "dns_dns_count_add_rr":
                    obj.DnsCountAddRr = Convert.ToUInt32(propValue, 10);
                    break;
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
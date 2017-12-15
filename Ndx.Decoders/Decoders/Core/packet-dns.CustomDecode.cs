using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
using static Ndx.Decoders.Core.Dns.Types;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Google.Protobuf.Collections;

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
                    SetRepeated(obj.DnsResp, x => x.DnsRespType = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_resp_class":
                    SetRepeated(obj.DnsResp, x => x.DnsRespClass = Convert.ToUInt32(propValue, 16));
                    break;

                case "dns_dns_resp_cache_flush":
                    SetRepeated(obj.DnsResp, x => x.DnsRespCacheFlush = Convert.ToInt32(propValue, 10) != 0);
                    break;

                case "dns_dns_resp_ext_rcode":
                    SetRepeated(obj.DnsResp, x => x.DnsRespClass = Convert.ToUInt32(propValue, 16));
                    break;

                case "dns_dns_resp_edns0_version":
                    SetRepeated(obj.DnsResp, x => x.DnsRespEdns0Version = Convert.ToUInt32(propValue, 10));
                    break;

                case "dns_dns_resp_z":
                    SetRepeated(obj.DnsResp, x => x.DnsRespZ = Convert.ToUInt32(propValue, 16));
                    break;

                case "dns_resp_z_dns_resp_z_do":
                    SetRepeated(obj.DnsResp, x => x.DnsRespZDo = Convert.ToInt32(propValue, 10) != 0);
                    break;

                case "dns_resp_z_dns_resp_z_reserved":
                    SetRepeated(obj.DnsResp, x => x.DnsRespZReserved = Convert.ToUInt32(propValue, 16));
                    break;

                case "text_dns_srv_service":
                    SetRepeated(obj.DnsResp, x => x.DnsSrvService = propValue);
                    break;

                case "text_dns_srv_proto":
                    SetRepeated(obj.DnsResp, x => x.DnsSrvProto = propValue);
                    break;

                case "text_dns_srv_name":
                    SetRepeated(obj.DnsResp, x => x.DnsSrvName = propValue);
                    break;

                case "text_dns_srv_priority":
                    SetRepeated(obj.DnsResp, x => x.DnsSrvPriority = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_srv_weight":
                    SetRepeated(obj.DnsResp, x => x.DnsSrvWeight = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_srv_port":
                    SetRepeated(obj.DnsResp, x => x.DnsSrvPort = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_srv_target":
                    SetRepeated(obj.DnsResp, x => x.DnsSrvTarget = propValue);
                    break;

                case "text_dns_naptr_order":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrOrder = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_naptr_preference":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrPreference = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_naptr_flags_length":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrFlagsLength = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_naptr_flags":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrFlags = propValue);
                    break;

                case "text_dns_naptr_service_length":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrServiceLength = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_naptr_service":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrService = propValue);
                    break;

                case "text_dns_naptr_regex_length":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrRegexLength = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_naptr_regex":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrRegex = propValue);
                    break;

                case "text_dns_naptr_replacement_length":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrReplacementLength = Convert.ToUInt32(propValue, 10));
                    break;

                case "dns_dns_naptr_replacement":
                    SetRepeated(obj.DnsResp, x => x.DnsNaptrReplacement = propValue);
                    break;

                case "text_dns_resp_name":
                    obj.DnsResp.Add(new _DnsResp() { DnsRespName = propValue });
                    break;

                case "text_dns_resp_ttl":
                    SetRepeated(obj.DnsResp, x => x.DnsRespTtl = Convert.ToInt32(propValue, 10));                    
                    break;

                case "text_dns_resp_len":
                    SetRepeated(obj.DnsResp, x => x.DnsRespLen = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_a":
                    SetRepeated(obj.DnsResp, x => x.DnsA = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()));
                    break;

                case "text_dns_md":
                    SetRepeated(obj.DnsResp, x => x.DnsMd = propValue);
                    break;

                case "text_dns_mf":
                    SetRepeated(obj.DnsResp, x => x.DnsMf = propValue);
                    break;

                case "text_dns_mb":
                    SetRepeated(obj.DnsResp, x => x.DnsMb = propValue);
                    break;

                case "text_dns_mg":
                    SetRepeated(obj.DnsResp, x => x.DnsMg = propValue);
                    break;

                case "text_dns_mr":
                    SetRepeated(obj.DnsResp, x => x.DnsMr = propValue);
                    break;

                case "dns_dns_null":
                    SetRepeated(obj.DnsResp, x => x.DnsNull = StringToBytes(propValue));
                    break;

                case "text_dns_aaaa":
                    SetRepeated(obj.DnsResp, x => x.DnsAaaa = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()));
                    break;

                case "text_dns_cname":
                    SetRepeated(obj.DnsResp, x => x.DnsCname = propValue);
                    break;

                case "text_dns_rr_udp_payload_size":
                    SetRepeated(obj.DnsResp, x => x.DnsRrUdpPayloadSize = Convert.ToUInt32(propValue, 16));
                    break;

                case "dns_dns_soa_mname":
                    SetRepeated(obj.DnsResp, x => x.DnsSoaMname = propValue);
                    break;

                case "text_dns_soa_rname":
                    SetRepeated(obj.DnsResp, x => x.DnsSoaRname = propValue);
                    break;

                case "text_dns_soa_serial_number":
                    SetRepeated(obj.DnsResp, x => x.DnsSoaSerialNumber = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_soa_refresh_interval":
                    SetRepeated(obj.DnsResp, x => x.DnsSoaRefreshInterval = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_soa_retry_interval":
                    SetRepeated(obj.DnsResp, x => x.DnsSoaRetryInterval = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_soa_expire_limit":
                    SetRepeated(obj.DnsResp, x => x.DnsSoaExpireLimit = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_soa_mininum_ttl":
                    SetRepeated(obj.DnsResp, x => x.DnsSoaMininumTtl = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_ptr_domain_name":
                    SetRepeated(obj.DnsResp, x => x.DnsPtrDomainName = propValue);
                    break;

               

                case "text_dns_mx_preference":
                    SetRepeated(obj.DnsResp, x => x.DnsMxPreference = Convert.ToUInt32(propValue, 10));
                    break;

                case "text_dns_mx_mail_exchange":
                    SetRepeated(obj.DnsResp, x => x.DnsMxMailExchange = propValue);
                    break;

                case "dns_txt_dns_txt_length":
                    SetRepeated(obj.DnsResp, x => x.DnsTxtLength = Convert.ToUInt32(propValue, 10));
                    break;

                case "dns_dns_txt":
                    SetRepeated(obj.DnsResp, x => x.DnsTxt = propValue);
                    break;
                case "text_dns_ns":
                    SetRepeated(obj.DnsResp, x => x.DnsNs = propValue);
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

        private static void SetRepeated<T>(RepeatedField<T> rf, Action<T> set) where T : new()
        {
            var x = rf.LastOrDefault();
            if (x != null)
            {
                set(x);
            }
            else
            {
                x = new T();
                set(x);
                rf.Add(x);
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
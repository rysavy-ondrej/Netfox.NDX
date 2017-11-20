using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Core
{
  public sealed partial class Icmp
  {
    public static Icmp DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Icmp DecodeJson(JToken token)
    {
      var obj = new Icmp();
      {
        var val = token["icmp_icmp_type"];
        if (val != null) obj.IcmpType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_code"];
        if (val != null) obj.IcmpCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_checksum"];
        if (val != null) obj.IcmpChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmp_checksum_icmp_checksum_status"];
        if (val != null) obj.IcmpChecksumStatus = default(UInt32);
      }
      {
        var val = token["icmp_icmp_unused"];
        if (val != null) obj.IcmpUnused = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmp_icmp_reserved"];
        if (val != null) obj.IcmpReserved = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmp_icmp_ident"];
        if (val != null) obj.IcmpIdent = default(UInt32);
      }
      {
        var val = token["icmp_icmp_seq"];
        if (val != null) obj.IcmpSeq = default(UInt32);
      }
      {
        var val = token["icmp_icmp_seq_le"];
        if (val != null) obj.IcmpSeqLe = default(UInt32);
      }
      {
        var val = token["icmp_icmp_mtu"];
        if (val != null) obj.IcmpMtu = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_num_addrs"];
        if (val != null) obj.IcmpNumAddrs = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_addr_entry_size"];
        if (val != null) obj.IcmpAddrEntrySize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_lifetime"];
        if (val != null) obj.IcmpLifetime = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_pointer"];
        if (val != null) obj.IcmpPointer = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_router_address"];
        if (val != null) obj.IcmpRouterAddress = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmp_icmp_pref_level"];
        if (val != null) obj.IcmpPrefLevel = Convert.ToInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_originate_timestamp"];
        if (val != null) obj.IcmpOriginateTimestamp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_receive_timestamp"];
        if (val != null) obj.IcmpReceiveTimestamp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_transmit_timestamp"];
        if (val != null) obj.IcmpTransmitTimestamp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_address_mask"];
        if (val != null) obj.IcmpAddressMask = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmp_icmp_redir_gw"];
        if (val != null) obj.IcmpRedirGw = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmp_icmp_mip_type"];
        if (val != null) obj.IcmpMipType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_mip_length"];
        if (val != null) obj.IcmpMipLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_mip_prefixlength"];
        if (val != null) obj.IcmpMipPrefixlength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_mip_seq"];
        if (val != null) obj.IcmpMipSeq = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_mip_life"];
        if (val != null) obj.IcmpMipLife = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_mip_flags"];
        if (val != null) obj.IcmpMipFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmp_icmp_mip_r"];
        if (val != null) obj.IcmpMipR = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_b"];
        if (val != null) obj.IcmpMipB = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_h"];
        if (val != null) obj.IcmpMipH = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_f"];
        if (val != null) obj.IcmpMipF = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_m"];
        if (val != null) obj.IcmpMipM = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_g"];
        if (val != null) obj.IcmpMipG = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_v"];
        if (val != null) obj.IcmpMipV = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_rt"];
        if (val != null) obj.IcmpMipRt = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_u"];
        if (val != null) obj.IcmpMipU = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_x"];
        if (val != null) obj.IcmpMipX = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mip_reserved"];
        if (val != null) obj.IcmpMipReserved = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmp_icmp_mip_coa"];
        if (val != null) obj.IcmpMipCoa = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmp_icmp_mip_challenge"];
        if (val != null) obj.IcmpMipChallenge = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmp_icmp_mip_content"];
        if (val != null) obj.IcmpMipContent = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmp_icmp_ext"];
        if (val != null) obj.IcmpExt = default(Int32);
      }
      {
        var val = token["icmp_ext_icmp_ext_version"];
        if (val != null) obj.IcmpExtVersion = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_ext_icmp_ext_res"];
        if (val != null) obj.IcmpExtRes = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmp_ext_icmp_ext_checksum"];
        if (val != null) obj.IcmpExtChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["icmp_ext_checksum_icmp_ext_checksum_status"];
        if (val != null) obj.IcmpExtChecksumStatus = default(UInt32);
      }
      {
        var val = token["icmp_ext_icmp_ext_length"];
        if (val != null) obj.IcmpExtLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_ext_icmp_ext_class"];
        if (val != null) obj.IcmpExtClass = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_ext_icmp_ext_ctype"];
        if (val != null) obj.IcmpExtCtype = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_ext_icmp_ext_data"];
        if (val != null) obj.IcmpExtData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmp_icmp_mpls_label"];
        if (val != null) obj.IcmpMplsLabel = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_mpls_exp"];
        if (val != null) obj.IcmpMplsExp = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_mpls_s"];
        if (val != null) obj.IcmpMplsS = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_mpls_ttl"];
        if (val != null) obj.IcmpMplsTtl = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_mpls_data"];
        if (val != null) obj.IcmpMplsData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmp_icmp_resp_in"];
        if (val != null) obj.IcmpRespIn = default(Int64);
      }
      {
        var val = token["icmp_icmp_no_resp"];
        if (val != null) obj.IcmpNoResp = default(Int32);
      }
      {
        var val = token["icmp_icmp_resp_to"];
        if (val != null) obj.IcmpRespTo = default(Int64);
      }
      {
        var val = token["icmp_icmp_resptime"];
        if (val != null) obj.IcmpResptime = Convert.ToDouble(val.Value<string>());
      }
      {
        var val = token["icmp_icmp_data_time"];
        if (val != null) obj.IcmpDataTime = default(Int64);
      }
      {
        var val = token["icmp_icmp_data_time_relative"];
        if (val != null) obj.IcmpDataTimeRelative = default(Int64);
      }
      {
        var val = token["icmp_icmp_length"];
        if (val != null) obj.IcmpLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_length_icmp_length_original_datagram"];
        if (val != null) obj.IcmpLengthOriginalDatagram = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_int_info_role"];
        if (val != null) obj.IcmpIntInfoRole = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_int_info_reserved"];
        if (val != null) obj.IcmpIntInfoReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_int_info_ifindex"];
        if (val != null) obj.IcmpIntInfoIfindex = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_int_info_ipaddr"];
        if (val != null) obj.IcmpIntInfoIpaddr = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_int_info_name_present"];
        if (val != null) obj.IcmpIntInfoNamePresent = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_int_info_mtu"];
        if (val != null) obj.IcmpIntInfoMtu = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["icmp_icmp_int_info_index"];
        if (val != null) obj.IcmpIntInfoIndex = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_int_info_afi"];
        if (val != null) obj.IcmpIntInfoAfi = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_int_info_ipv4"];
        if (val != null) obj.IcmpIntInfoIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmp_icmp_int_info_ipv6"];
        if (val != null) obj.IcmpIntInfoIpv6 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["icmp_icmp_int_info_ipunknown"];
        if (val != null) obj.IcmpIntInfoIpunknown = StringToBytes(val.Value<string>());
      }
      {
        var val = token["icmp_icmp_int_info_name_length"];
        if (val != null) obj.IcmpIntInfoNameLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["icmp_icmp_int_info_name"];
        if (val != null) obj.IcmpIntInfoName = val.Value<string>();
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

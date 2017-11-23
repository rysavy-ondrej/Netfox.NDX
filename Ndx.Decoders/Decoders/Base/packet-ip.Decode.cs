using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Base
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
        var val = token["ip_checksum_ip_checksum_status"];
        if (val != null) obj.IpChecksumStatus = default(UInt32);
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

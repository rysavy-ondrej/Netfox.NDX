using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Base
{
  public sealed partial class Udp
  {
    public static Udp DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Udp DecodeJson(JToken token)
    {
      var obj = new Udp();
      {
        var val = token["udp_udp_srcport"];
        if (val != null) obj.UdpSrcport = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_dstport"];
        if (val != null) obj.UdpDstport = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_stream"];
        if (val != null) obj.UdpStream = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_length"];
        if (val != null) obj.UdpLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_checksum"];
        if (val != null) obj.UdpChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["udp_checksum_udp_checksum_status"];
        if (val != null) obj.UdpChecksumStatus = default(UInt32);
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

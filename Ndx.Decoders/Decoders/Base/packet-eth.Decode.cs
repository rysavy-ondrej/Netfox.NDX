using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Base
{
  public sealed partial class Eth
  {
    public static Eth DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Eth DecodeJson(JToken token)
    {
      var obj = new Eth();
      {
        var val = token["eth_eth_dst"];
        if (val != null) obj.EthDst = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["eth_dst_eth_dst_resolved"];
        if (val != null) obj.EthDstResolved = val.Value<string>();
      }
      {
        var val = token["eth_eth_src"];
        if (val != null) obj.EthSrc = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["eth_src_eth_src_resolved"];
        if (val != null) obj.EthSrcResolved = val.Value<string>();
      }
      {
        var val = token["eth_eth_len"];
        if (val != null) obj.EthLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["eth_eth_type"];
        if (val != null) obj.EthType = Convert.ToUInt32(val.Value<string>(), 16);
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

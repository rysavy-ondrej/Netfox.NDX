using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Core
{
  public sealed partial class Ipx
  {
    public static Ipx DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Ipx DecodeJson(JToken token)
    {
      var obj = new Ipx();
      {
        var val = token["ipx_ipx_checksum"];
        if (val != null) obj.IpxChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipx_ipx_src"];
        if (val != null) obj.IpxSrc = val.Value<string>();
      }
      {
        var val = token["ipx_ipx_dst"];
        if (val != null) obj.IpxDst = val.Value<string>();
      }
      {
        var val = token["ipx_ipx_addr"];
        if (val != null) obj.IpxAddr = val.Value<string>();
      }
      {
        var val = token["ipx_ipx_len"];
        if (val != null) obj.IpxLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipx_ipx_hops"];
        if (val != null) obj.IpxHops = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipx_ipx_packet_type"];
        if (val != null) obj.IpxPacketType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipx_dst_ipx_dst_net"];
        if (val != null) obj.IpxDstNet = default(ByteString);
      }
      {
        var val = token["ipx_dst_ipx_dst_node"];
        if (val != null) obj.IpxDstNode = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["ipx_dst_ipx_dst_socket"];
        if (val != null) obj.IpxDstSocket = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipx_src_ipx_src_net"];
        if (val != null) obj.IpxSrcNet = default(ByteString);
      }
      {
        var val = token["ipx_src_ipx_src_node"];
        if (val != null) obj.IpxSrcNode = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["ipx_src_ipx_src_socket"];
        if (val != null) obj.IpxSrcSocket = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipx_ipx_net"];
        if (val != null) obj.IpxNet = default(ByteString);
      }
      {
        var val = token["ipx_ipx_node"];
        if (val != null) obj.IpxNode = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["ipx_ipx_socket"];
        if (val != null) obj.IpxSocket = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipx_ipxrip_request"];
        if (val != null) obj.IpxripRequest = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipx_ipxrip_response"];
        if (val != null) obj.IpxripResponse = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipx_ipxrip_packet_type"];
        if (val != null) obj.IpxripPacketType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipx_ipxrip_route_vector"];
        if (val != null) obj.IpxripRouteVector = default(ByteString);
      }
      {
        var val = token["ipx_ipxrip_hops"];
        if (val != null) obj.IpxripHops = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipx_ipxrip_ticks"];
        if (val != null) obj.IpxripTicks = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipx_ipxsap_request"];
        if (val != null) obj.IpxsapRequest = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipx_ipxsap_response"];
        if (val != null) obj.IpxsapResponse = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ipx_ipxsap_packet_type"];
        if (val != null) obj.IpxsapPacketType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipx_ipxsap_server_name"];
        if (val != null) obj.IpxsapServerName = default(String);
      }
      {
        var val = token["ipx_ipxsap_server_type"];
        if (val != null) obj.IpxsapServerType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipx_ipxsap_network"];
        if (val != null) obj.IpxsapNetwork = default(ByteString);
      }
      {
        var val = token["ipx_ipxsap_node"];
        if (val != null) obj.IpxsapNode = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["ipx_ipxsap_socket"];
        if (val != null) obj.IpxsapSocket = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ipx_ipxsap_intermediate_networks"];
        if (val != null) obj.IpxsapIntermediateNetworks = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipx_ipxmsg_conn"];
        if (val != null) obj.IpxmsgConn = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ipx_ipxmsg_sigchar"];
        if (val != null) obj.IpxmsgSigchar = Convert.ToUInt32(val.Value<string>(), 16);
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

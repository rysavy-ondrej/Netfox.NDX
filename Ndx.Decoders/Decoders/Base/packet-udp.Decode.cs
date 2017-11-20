using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Basic
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
        var val = token["udp_udp_port"];
        if (val != null) obj.UdpPort = Convert.ToUInt32(val.Value<string>(), 10);
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
        var val = token["udp_udp_checksum_calculated"];
        if (val != null) obj.UdpChecksumCalculated = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["udp_checksum_udp_checksum_status"];
        if (val != null) obj.UdpChecksumStatus = default(UInt32);
      }
      {
        var val = token["udp_udp_proc_srcuid"];
        if (val != null) obj.UdpProcSrcuid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_proc_srcpid"];
        if (val != null) obj.UdpProcSrcpid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_proc_srcuname"];
        if (val != null) obj.UdpProcSrcuname = val.Value<string>();
      }
      {
        var val = token["udp_udp_proc_srccmd"];
        if (val != null) obj.UdpProcSrccmd = val.Value<string>();
      }
      {
        var val = token["udp_udp_proc_dstuid"];
        if (val != null) obj.UdpProcDstuid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_proc_dstpid"];
        if (val != null) obj.UdpProcDstpid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_proc_dstuname"];
        if (val != null) obj.UdpProcDstuname = val.Value<string>();
      }
      {
        var val = token["udp_udp_proc_dstcmd"];
        if (val != null) obj.UdpProcDstcmd = val.Value<string>();
      }
      {
        var val = token["udp_udp_pdu_size"];
        if (val != null) obj.UdpPduSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["udp_udp_checksum_coverage"];
        if (val != null) obj.UdpChecksumCoverage = Convert.ToUInt32(val.Value<string>(), 10);
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

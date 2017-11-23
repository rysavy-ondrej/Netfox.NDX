using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Base
{
  public sealed partial class Tcp
  {
    public static Tcp DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Tcp DecodeJson(JToken token)
    {
      var obj = new Tcp();
      {
        var val = token["tcp_tcp_srcport"];
        if (val != null) obj.TcpSrcport = default(UInt32);
      }
      {
        var val = token["tcp_tcp_dstport"];
        if (val != null) obj.TcpDstport = default(UInt32);
      }
      {
        var val = token["tcp_tcp_stream"];
        if (val != null) obj.TcpStream = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_seq"];
        if (val != null) obj.TcpSeq = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_nxtseq"];
        if (val != null) obj.TcpNxtseq = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_ack"];
        if (val != null) obj.TcpAck = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_hdr_len"];
        if (val != null) obj.TcpHdrLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_flags"];
        if (val != null) obj.TcpFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_flags_tcp_flags_res"];
        if (val != null) obj.TcpFlagsRes = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_ns"];
        if (val != null) obj.TcpFlagsNs = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_cwr"];
        if (val != null) obj.TcpFlagsCwr = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_ecn"];
        if (val != null) obj.TcpFlagsEcn = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_urg"];
        if (val != null) obj.TcpFlagsUrg = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_ack"];
        if (val != null) obj.TcpFlagsAck = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_push"];
        if (val != null) obj.TcpFlagsPush = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_reset"];
        if (val != null) obj.TcpFlagsReset = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_syn"];
        if (val != null) obj.TcpFlagsSyn = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_flags_tcp_flags_fin"];
        if (val != null) obj.TcpFlagsFin = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["tcp_tcp_window_size"];
        if (val != null) obj.TcpWindowSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_checksum"];
        if (val != null) obj.TcpChecksum = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["tcp_checksum_tcp_checksum_status"];
        if (val != null) obj.TcpChecksumStatus = default(UInt32);
      }
      {
        var val = token["tcp_tcp_len"];
        if (val != null) obj.TcpLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_urgent_pointer"];
        if (val != null) obj.TcpUrgentPointer = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["tcp_tcp_payload"];
        if (val != null) obj.TcpPayload = StringToBytes(val.Value<string>());
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

// This is file was generated by netdx on (2017-11-24 12:33:59 PM.
using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        if (val != null) { var propValue = val.Value<string>(); obj.TcpSrcport = default(UInt32); }
      }
      {
        var val = token["tcp_tcp_dstport"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpDstport = default(UInt32); }
      }
      {
        var val = token["tcp_tcp_stream"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpStream = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["tcp_tcp_seq"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpSeq = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["tcp_tcp_nxtseq"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpNxtseq = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["tcp_tcp_ack"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpAck = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["tcp_tcp_hdr_len"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpHdrLen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["tcp_tcp_flags"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlags = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["tcp_flags_tcp_flags_res"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsRes = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_ns"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsNs = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_cwr"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsCwr = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_ecn"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsEcn = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_urg"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsUrg = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_ack"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsAck = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_push"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsPush = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_reset"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsReset = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_syn"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsSyn = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_flags_tcp_flags_fin"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpFlagsFin = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["tcp_tcp_window_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpWindowSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["tcp_tcp_checksum"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpChecksum = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["tcp_checksum_tcp_checksum_status"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpChecksumStatus = default(UInt32); }
      }
      {
        var val = token["tcp_tcp_len"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpLen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["tcp_tcp_urgent_pointer"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpUrgentPointer = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["tcp_tcp_payload"];
        if (val != null) { var propValue = val.Value<string>(); obj.TcpPayload = StringToBytes(propValue); }
      }
      return obj;
    }
    public static Tcp DecodeJson(JsonTextReader reader)                        
    {                                                                                     
        if (reader.TokenType != JsonToken.StartObject) return null;                       
        var obj = new Tcp();                                                   
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
                    
    static void SetField(Tcp obj, string propName, string propValue)           
    {                                                                                     
      switch (propName)                                                                   
      {                                                                                   
      case "tcp_tcp_srcport": obj.TcpSrcport = default(UInt32); break;
      case "tcp_tcp_dstport": obj.TcpDstport = default(UInt32); break;
      case "tcp_tcp_stream": obj.TcpStream = Convert.ToUInt32(propValue, 10); break;
      case "tcp_tcp_seq": obj.TcpSeq = Convert.ToUInt32(propValue, 10); break;
      case "tcp_tcp_nxtseq": obj.TcpNxtseq = Convert.ToUInt32(propValue, 10); break;
      case "tcp_tcp_ack": obj.TcpAck = Convert.ToUInt32(propValue, 10); break;
      case "tcp_tcp_hdr_len": obj.TcpHdrLen = Convert.ToUInt32(propValue, 10); break;
      case "tcp_tcp_flags": obj.TcpFlags = Convert.ToUInt32(propValue, 16); break;
      case "tcp_flags_tcp_flags_res": obj.TcpFlagsRes = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_ns": obj.TcpFlagsNs = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_cwr": obj.TcpFlagsCwr = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_ecn": obj.TcpFlagsEcn = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_urg": obj.TcpFlagsUrg = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_ack": obj.TcpFlagsAck = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_push": obj.TcpFlagsPush = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_reset": obj.TcpFlagsReset = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_syn": obj.TcpFlagsSyn = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_flags_tcp_flags_fin": obj.TcpFlagsFin = Convert.ToInt32(propValue, 10) != 0; break;
      case "tcp_tcp_window_size": obj.TcpWindowSize = Convert.ToUInt32(propValue, 10); break;
      case "tcp_tcp_checksum": obj.TcpChecksum = Convert.ToUInt32(propValue, 16); break;
      case "tcp_checksum_tcp_checksum_status": obj.TcpChecksumStatus = default(UInt32); break;
      case "tcp_tcp_len": obj.TcpLen = Convert.ToUInt32(propValue, 10); break;
      case "tcp_tcp_urgent_pointer": obj.TcpUrgentPointer = Convert.ToUInt32(propValue, 10); break;
      case "tcp_tcp_payload": obj.TcpPayload = StringToBytes(propValue); break;
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
      return Google.Protobuf.ByteString.CopyFrom( byteArray );                
    }                                                                         

  }
}

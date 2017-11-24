// This is file was generated by netdx on (2017-11-24 11:58:07 AM.
using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        if (val != null) { var propValue = val.Value<string>(); obj.UdpSrcport = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["udp_udp_dstport"];
        if (val != null) { var propValue = val.Value<string>(); obj.UdpDstport = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["udp_udp_stream"];
        if (val != null) { var propValue = val.Value<string>(); obj.UdpStream = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["udp_udp_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.UdpLength = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["udp_udp_checksum"];
        if (val != null) { var propValue = val.Value<string>(); obj.UdpChecksum = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["udp_checksum_udp_checksum_status"];
        if (val != null) { var propValue = val.Value<string>(); obj.UdpChecksumStatus = default(UInt32); }
      }
      return obj;
    }
    public static Udp DecodeJson(JsonTextReader reader)                        
    {                                                                                     
        if (reader.TokenType != JsonToken.StartObject) return null;                       
        var obj = new Udp();                                                   
        reader.Read();                                                                    
        while (reader.TokenType != JsonToken.EndObject)                                   
        {                                                                                 
            if (reader.TokenType == JsonToken.PropertyName)                               
            {                                                                             
                string propName = (string)reader.Value;                                   
                reader.Read();                                                            
                if (reader.TokenType != JsonToken.String) { reader.Read(); continue; }    
                string propValue = (string)reader.Value;                                  
                SetField(obj, propName, propValue);                                       
            }                                                                             
            reader.Read();                                                                
        }                                                                                 
        reader.Read();                                                                    
        return obj;                                                                       
    }                                                                                     

    static void SetField(Udp obj, string propName, string propValue)           
    {                                                                                     
      switch (propName)                                                                   
      {                                                                                   
      case "udp_udp_srcport": obj.UdpSrcport = Convert.ToUInt32(propValue, 10); break;
      case "udp_udp_dstport": obj.UdpDstport = Convert.ToUInt32(propValue, 10); break;
      case "udp_udp_stream": obj.UdpStream = Convert.ToUInt32(propValue, 10); break;
      case "udp_udp_length": obj.UdpLength = Convert.ToUInt32(propValue, 10); break;
      case "udp_udp_checksum": obj.UdpChecksum = Convert.ToUInt32(propValue, 16); break;
      case "udp_checksum_udp_checksum_status": obj.UdpChecksumStatus = default(UInt32); break;
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

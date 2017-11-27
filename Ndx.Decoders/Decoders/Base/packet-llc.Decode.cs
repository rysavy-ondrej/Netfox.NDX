// This is file was generated by netdx on (2017-11-24 12:33:53 PM.
using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Ndx.Decoders.Base
{
  public sealed partial class Llc
  {
    public static Llc DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Llc DecodeJson(JToken token)
    {
      var obj = new Llc();
      {
        var val = token["llc_llc_dsap"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcDsap = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_dsap_llc_dsap_sap"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcDsapSap = default(UInt32); }
      }
      {
        var val = token["llc_dsap_llc_dsap_ig"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcDsapIg = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["llc_llc_ssap"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcSsap = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_ssap_llc_ssap_sap"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcSsapSap = default(UInt32); }
      }
      {
        var val = token["llc_ssap_llc_ssap_cr"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcSsapCr = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["llc_llc_control"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControl = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_control_llc_control_n_r"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControlNR = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["llc_control_llc_control_n_s"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControlNS = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["llc_control_llc_control_p"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControlP = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["llc_control_llc_control_f"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControlF = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["llc_control_llc_control_s_ftype"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControlSFtype = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_control_llc_control_u_modifier_cmd"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControlUModifierCmd = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_control_llc_control_u_modifier_resp"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControlUModifierResp = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_control_llc_control_ftype"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcControlFtype = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_llc_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcType = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_llc_oui"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcOui = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["llc_llc_pid"];
        if (val != null) { var propValue = val.Value<string>(); obj.LlcPid = Convert.ToUInt32(propValue, 16); }
      }
      return obj;
    }
    public static Llc DecodeJson(JsonTextReader reader)                        
    {                                                                                     
        if (reader.TokenType != JsonToken.StartObject) return null;                       
        var obj = new Llc();                                                   
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
                    
    static void SetField(Llc obj, string propName, string propValue)           
    {                                                                                     
      switch (propName)                                                                   
      {                                                                                   
      case "llc_llc_dsap": obj.LlcDsap = Convert.ToUInt32(propValue, 16); break;
      case "llc_dsap_llc_dsap_sap": obj.LlcDsapSap = default(UInt32); break;
      case "llc_dsap_llc_dsap_ig": obj.LlcDsapIg = Convert.ToInt32(propValue, 10) != 0; break;
      case "llc_llc_ssap": obj.LlcSsap = Convert.ToUInt32(propValue, 16); break;
      case "llc_ssap_llc_ssap_sap": obj.LlcSsapSap = default(UInt32); break;
      case "llc_ssap_llc_ssap_cr": obj.LlcSsapCr = Convert.ToInt32(propValue, 10) != 0; break;
      case "llc_llc_control": obj.LlcControl = Convert.ToUInt32(propValue, 16); break;
      case "llc_control_llc_control_n_r": obj.LlcControlNR = Convert.ToUInt32(propValue, 10); break;
      case "llc_control_llc_control_n_s": obj.LlcControlNS = Convert.ToUInt32(propValue, 10); break;
      case "llc_control_llc_control_p": obj.LlcControlP = Convert.ToInt32(propValue, 10) != 0; break;
      case "llc_control_llc_control_f": obj.LlcControlF = Convert.ToInt32(propValue, 10) != 0; break;
      case "llc_control_llc_control_s_ftype": obj.LlcControlSFtype = Convert.ToUInt32(propValue, 16); break;
      case "llc_control_llc_control_u_modifier_cmd": obj.LlcControlUModifierCmd = Convert.ToUInt32(propValue, 16); break;
      case "llc_control_llc_control_u_modifier_resp": obj.LlcControlUModifierResp = Convert.ToUInt32(propValue, 16); break;
      case "llc_control_llc_control_ftype": obj.LlcControlFtype = Convert.ToUInt32(propValue, 16); break;
      case "llc_llc_type": obj.LlcType = Convert.ToUInt32(propValue, 16); break;
      case "llc_llc_oui": obj.LlcOui = Convert.ToUInt32(propValue, 16); break;
      case "llc_llc_pid": obj.LlcPid = Convert.ToUInt32(propValue, 16); break;
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

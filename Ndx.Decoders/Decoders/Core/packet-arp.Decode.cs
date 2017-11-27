// This is file was generated by netdx on (2017-11-24 12:34:35 PM.
using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Ndx.Decoders.Core
{
  public sealed partial class Arp
  {
    public static Arp DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Arp DecodeJson(JToken token)
    {
      var obj = new Arp();
      {
        var val = token["arp_arp_hw_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpHwType = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_proto_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpProtoType = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["arp_arp_hw_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpHwSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_src_htype"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcHtype = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["arp_arp_src_hlen"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcHlen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_src_stype"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcStype = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["arp_arp_src_slen"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcSlen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_proto_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpProtoSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_opcode"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpOpcode = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_isgratuitous"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpIsgratuitous = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["arp_arp_src_pln"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcPln = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_dst_htype"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstHtype = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["arp_arp_dst_hlen"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstHlen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_dst_stype"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstStype = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["arp_arp_dst_slen"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstSlen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_dst_pln"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstPln = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_src_hw"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcHw = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_hw_mac"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcHwMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(propValue.ToUpperInvariant().Replace(':','-')).GetAddressBytes()); }
      }
      {
        var val = token["arp_arp_src_hw_ax25"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcHwAx25 = default(ByteString); }
      }
      {
        var val = token["arp_arp_src_atm_num_e164"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmNumE164 = propValue; }
      }
      {
        var val = token["arp_arp_src_atm_num_nsap"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmNumNsap = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_atm_subaddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmSubaddr = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_proto"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcProto = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_proto_ipv4"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcProtoIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["arp_arp_dst_hw"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstHw = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_dst_hw_mac"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstHwMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(propValue.ToUpperInvariant().Replace(':','-')).GetAddressBytes()); }
      }
      {
        var val = token["arp_arp_dst_hw_ax25"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstHwAx25 = default(ByteString); }
      }
      {
        var val = token["arp_arp_dst_atm_num_e164"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstAtmNumE164 = propValue; }
      }
      {
        var val = token["arp_arp_dst_atm_num_nsap"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstAtmNumNsap = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_dst_atm_subaddr"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstAtmSubaddr = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_dst_proto"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstProto = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_dst_proto_ipv4"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstProtoIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); }
      }
      {
        var val = token["arp_arp_dst_drarp_error_status"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDstDrarpErrorStatus = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_duplicate-address-frame"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpDuplicateAddressFrame = default(Int64); }
      }
      {
        var val = token["arp_arp_seconds-since-duplicate-address-frame"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSecondsSinceDuplicateAddressFrame = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["arp_arp_src_atm_data_country_code"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmDataCountryCode = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["arp_arp_src_atm_data_country_code_group"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmDataCountryCodeGroup = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["arp_arp_src_atm_high_order_dsp"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmHighOrderDsp = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_atm_end_system_identifier"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmEndSystemIdentifier = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_atm_selector"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmSelector = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["arp_arp_src_atm_international_code_designator"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmInternationalCodeDesignator = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["arp_arp_src_atm_international_code_designator_group"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmInternationalCodeDesignatorGroup = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["arp_arp_src_atm_e_164_isdn"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmE164Isdn = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_atm_e_164_isdn_group"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmE164IsdnGroup = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_atm_rest_of_address"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmRestOfAddress = StringToBytes(propValue); }
      }
      {
        var val = token["arp_arp_src_atm_afi"];
        if (val != null) { var propValue = val.Value<string>(); obj.ArpSrcAtmAfi = Convert.ToUInt32(propValue, 16); }
      }
      return obj;
    }
    public static Arp DecodeJson(JsonTextReader reader)                        
    {                                                                                     
        if (reader.TokenType != JsonToken.StartObject) return null;                       
        var obj = new Arp();                                                   
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
                    
    static void SetField(Arp obj, string propName, string propValue)           
    {                                                                                     
      switch (propName)                                                                   
      {                                                                                   
      case "arp_arp_hw_type": obj.ArpHwType = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_proto_type": obj.ArpProtoType = Convert.ToUInt32(propValue, 16); break;
      case "arp_arp_hw_size": obj.ArpHwSize = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_src_htype": obj.ArpSrcHtype = Convert.ToInt32(propValue, 10) != 0; break;
      case "arp_arp_src_hlen": obj.ArpSrcHlen = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_src_stype": obj.ArpSrcStype = Convert.ToInt32(propValue, 10) != 0; break;
      case "arp_arp_src_slen": obj.ArpSrcSlen = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_proto_size": obj.ArpProtoSize = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_opcode": obj.ArpOpcode = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_isgratuitous": obj.ArpIsgratuitous = Convert.ToInt32(propValue, 10) != 0; break;
      case "arp_arp_src_pln": obj.ArpSrcPln = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_dst_htype": obj.ArpDstHtype = Convert.ToInt32(propValue, 10) != 0; break;
      case "arp_arp_dst_hlen": obj.ArpDstHlen = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_dst_stype": obj.ArpDstStype = Convert.ToInt32(propValue, 10) != 0; break;
      case "arp_arp_dst_slen": obj.ArpDstSlen = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_dst_pln": obj.ArpDstPln = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_src_hw": obj.ArpSrcHw = StringToBytes(propValue); break;
      case "arp_arp_src_hw_mac": obj.ArpSrcHwMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(propValue.ToUpperInvariant().Replace(':','-')).GetAddressBytes()); break;
      case "arp_arp_src_hw_ax25": obj.ArpSrcHwAx25 = default(ByteString); break;
      case "arp_arp_src_atm_num_e164": obj.ArpSrcAtmNumE164 = propValue; break;
      case "arp_arp_src_atm_num_nsap": obj.ArpSrcAtmNumNsap = StringToBytes(propValue); break;
      case "arp_arp_src_atm_subaddr": obj.ArpSrcAtmSubaddr = StringToBytes(propValue); break;
      case "arp_arp_src_proto": obj.ArpSrcProto = StringToBytes(propValue); break;
      case "arp_arp_src_proto_ipv4": obj.ArpSrcProtoIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "arp_arp_dst_hw": obj.ArpDstHw = StringToBytes(propValue); break;
      case "arp_arp_dst_hw_mac": obj.ArpDstHwMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(propValue.ToUpperInvariant().Replace(':','-')).GetAddressBytes()); break;
      case "arp_arp_dst_hw_ax25": obj.ArpDstHwAx25 = default(ByteString); break;
      case "arp_arp_dst_atm_num_e164": obj.ArpDstAtmNumE164 = propValue; break;
      case "arp_arp_dst_atm_num_nsap": obj.ArpDstAtmNumNsap = StringToBytes(propValue); break;
      case "arp_arp_dst_atm_subaddr": obj.ArpDstAtmSubaddr = StringToBytes(propValue); break;
      case "arp_arp_dst_proto": obj.ArpDstProto = StringToBytes(propValue); break;
      case "arp_arp_dst_proto_ipv4": obj.ArpDstProtoIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(propValue).GetAddressBytes()); break;
      case "arp_arp_dst_drarp_error_status": obj.ArpDstDrarpErrorStatus = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_duplicate-address-frame": obj.ArpDuplicateAddressFrame = default(Int64); break;
      case "arp_arp_seconds-since-duplicate-address-frame": obj.ArpSecondsSinceDuplicateAddressFrame = Convert.ToUInt32(propValue, 10); break;
      case "arp_arp_src_atm_data_country_code": obj.ArpSrcAtmDataCountryCode = Convert.ToUInt32(propValue, 16); break;
      case "arp_arp_src_atm_data_country_code_group": obj.ArpSrcAtmDataCountryCodeGroup = Convert.ToUInt32(propValue, 16); break;
      case "arp_arp_src_atm_high_order_dsp": obj.ArpSrcAtmHighOrderDsp = StringToBytes(propValue); break;
      case "arp_arp_src_atm_end_system_identifier": obj.ArpSrcAtmEndSystemIdentifier = StringToBytes(propValue); break;
      case "arp_arp_src_atm_selector": obj.ArpSrcAtmSelector = Convert.ToUInt32(propValue, 16); break;
      case "arp_arp_src_atm_international_code_designator": obj.ArpSrcAtmInternationalCodeDesignator = Convert.ToUInt32(propValue, 16); break;
      case "arp_arp_src_atm_international_code_designator_group": obj.ArpSrcAtmInternationalCodeDesignatorGroup = Convert.ToUInt32(propValue, 16); break;
      case "arp_arp_src_atm_e_164_isdn": obj.ArpSrcAtmE164Isdn = StringToBytes(propValue); break;
      case "arp_arp_src_atm_e_164_isdn_group": obj.ArpSrcAtmE164IsdnGroup = StringToBytes(propValue); break;
      case "arp_arp_src_atm_rest_of_address": obj.ArpSrcAtmRestOfAddress = StringToBytes(propValue); break;
      case "arp_arp_src_atm_afi": obj.ArpSrcAtmAfi = Convert.ToUInt32(propValue, 16); break;
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

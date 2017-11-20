using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
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
        if (val != null) obj.ArpHwType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_proto_type"];
        if (val != null) obj.ArpProtoType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["arp_arp_hw_size"];
        if (val != null) obj.ArpHwSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_src_htype"];
        if (val != null) obj.ArpSrcHtype = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["arp_arp_src_hlen"];
        if (val != null) obj.ArpSrcHlen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_src_stype"];
        if (val != null) obj.ArpSrcStype = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["arp_arp_src_slen"];
        if (val != null) obj.ArpSrcSlen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_proto_size"];
        if (val != null) obj.ArpProtoSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_opcode"];
        if (val != null) obj.ArpOpcode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_isgratuitous"];
        if (val != null) obj.ArpIsgratuitous = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["arp_arp_src_pln"];
        if (val != null) obj.ArpSrcPln = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_dst_htype"];
        if (val != null) obj.ArpDstHtype = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["arp_arp_dst_hlen"];
        if (val != null) obj.ArpDstHlen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_dst_stype"];
        if (val != null) obj.ArpDstStype = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["arp_arp_dst_slen"];
        if (val != null) obj.ArpDstSlen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_dst_pln"];
        if (val != null) obj.ArpDstPln = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_src_hw"];
        if (val != null) obj.ArpSrcHw = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_hw_mac"];
        if (val != null) obj.ArpSrcHwMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["arp_arp_src_hw_ax25"];
        if (val != null) obj.ArpSrcHwAx25 = default(ByteString);
      }
      {
        var val = token["arp_arp_src_atm_num_e164"];
        if (val != null) obj.ArpSrcAtmNumE164 = val.Value<string>();
      }
      {
        var val = token["arp_arp_src_atm_num_nsap"];
        if (val != null) obj.ArpSrcAtmNumNsap = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_atm_subaddr"];
        if (val != null) obj.ArpSrcAtmSubaddr = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_proto"];
        if (val != null) obj.ArpSrcProto = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_proto_ipv4"];
        if (val != null) obj.ArpSrcProtoIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["arp_arp_dst_hw"];
        if (val != null) obj.ArpDstHw = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_dst_hw_mac"];
        if (val != null) obj.ArpDstHwMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["arp_arp_dst_hw_ax25"];
        if (val != null) obj.ArpDstHwAx25 = default(ByteString);
      }
      {
        var val = token["arp_arp_dst_atm_num_e164"];
        if (val != null) obj.ArpDstAtmNumE164 = val.Value<string>();
      }
      {
        var val = token["arp_arp_dst_atm_num_nsap"];
        if (val != null) obj.ArpDstAtmNumNsap = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_dst_atm_subaddr"];
        if (val != null) obj.ArpDstAtmSubaddr = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_dst_proto"];
        if (val != null) obj.ArpDstProto = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_dst_proto_ipv4"];
        if (val != null) obj.ArpDstProtoIpv4 = Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes());
      }
      {
        var val = token["arp_arp_dst_drarp_error_status"];
        if (val != null) obj.ArpDstDrarpErrorStatus = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_duplicate-address-frame"];
        if (val != null) obj.ArpDuplicateAddressFrame = default(Int64);
      }
      {
        var val = token["arp_arp_seconds-since-duplicate-address-frame"];
        if (val != null) obj.ArpSecondsSinceDuplicateAddressFrame = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["arp_arp_src_atm_data_country_code"];
        if (val != null) obj.ArpSrcAtmDataCountryCode = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["arp_arp_src_atm_data_country_code_group"];
        if (val != null) obj.ArpSrcAtmDataCountryCodeGroup = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["arp_arp_src_atm_high_order_dsp"];
        if (val != null) obj.ArpSrcAtmHighOrderDsp = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_atm_end_system_identifier"];
        if (val != null) obj.ArpSrcAtmEndSystemIdentifier = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_atm_selector"];
        if (val != null) obj.ArpSrcAtmSelector = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["arp_arp_src_atm_international_code_designator"];
        if (val != null) obj.ArpSrcAtmInternationalCodeDesignator = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["arp_arp_src_atm_international_code_designator_group"];
        if (val != null) obj.ArpSrcAtmInternationalCodeDesignatorGroup = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["arp_arp_src_atm_e_164_isdn"];
        if (val != null) obj.ArpSrcAtmE164Isdn = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_atm_e_164_isdn_group"];
        if (val != null) obj.ArpSrcAtmE164IsdnGroup = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_atm_rest_of_address"];
        if (val != null) obj.ArpSrcAtmRestOfAddress = StringToBytes(val.Value<string>());
      }
      {
        var val = token["arp_arp_src_atm_afi"];
        if (val != null) obj.ArpSrcAtmAfi = Convert.ToUInt32(val.Value<string>(), 16);
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

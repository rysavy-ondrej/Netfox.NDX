// This is file was generated by netdx on (2017-11-24 12:36:14 PM.
using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Ndx.Decoders.Core
{
  public sealed partial class Netbios
  {
    public static Netbios DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Netbios DecodeJson(JToken token)
    {
      var obj = new Netbios();
      {
        var val = token["netbios_netbios_command"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosCommand = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_hdr_len"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosHdrLen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_delimiter"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosDelimiter = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_xmit_corrl"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosXmitCorrl = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_resp_corrl"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosRespCorrl = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_call_name_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosCallNameType = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_nb_name_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosNbNameType = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_nb_name"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosNbName = propValue; }
      }
      {
        var val = token["netbios_netbios_version"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosVersion = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_netbios_no_receive_flags"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosNoReceiveFlags = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_no_receive_flags_netbios_no_receive_flags_send_no_ack"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosNoReceiveFlagsSendNoAck = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_netbios_largest_frame"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosLargestFrame = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_status_buffer_len"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosStatusBufferLen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_status"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosStatus = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_name_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosNameType = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_max_data_recv_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosMaxDataRecvSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_termination_indicator"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosTerminationIndicator = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_num_data_bytes_accepted"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosNumDataBytesAccepted = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_local_session"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosLocalSession = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_remote_session"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosRemoteSession = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_flags"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFlags = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_flags_netbios_flags_send_no_ack"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFlagsSendNoAck = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_flags_netbios_flags_ack"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFlagsAck = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_flags_netbios_flags_ack_with_data"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFlagsAckWithData = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_flags_netbios_flags_ack_expected"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFlagsAckExpected = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_flags_netbios_flags_recv_cont_req"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFlagsRecvContReq = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_netbios_data2"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosData2 = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_data2_netbios_data2_frame"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosData2Frame = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_data2_netbios_data2_user"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosData2User = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_data2_netbios_data2_status"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosData2Status = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_datagram_mac"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosDatagramMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(propValue.ToUpperInvariant().Replace(':','-')).GetAddressBytes()); }
      }
      {
        var val = token["netbios_netbios_datagram_bcast_mac"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosDatagramBcastMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(propValue.ToUpperInvariant().Replace(':','-')).GetAddressBytes()); }
      }
      {
        var val = token["netbios_netbios_resync_indicator"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosResyncIndicator = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_status_request"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosStatusRequest = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_local_session_no"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosLocalSessionNo = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_state_of_name"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosStateOfName = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["netbios_netbios_status_response"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosStatusResponse = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_fragment_netbios_fragment_overlap"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFragmentOverlap = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_fragment_overlap_netbios_fragment_overlap_conflict"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFragmentOverlapConflict = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_fragment_netbios_fragment_multipletails"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFragmentMultipletails = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_fragment_netbios_fragment_toolongfragment"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFragmentToolongfragment = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["netbios_fragment_netbios_fragment_error"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFragmentError = default(Int64); }
      }
      {
        var val = token["netbios_fragment_netbios_fragment_count"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFragmentCount = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["netbios_netbios_fragment"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFragment = default(Int64); }
      }
      {
        var val = token["netbios_netbios_fragments"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosFragments = default(Int32); }
      }
      {
        var val = token["netbios_netbios_reassembled_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.NetbiosReassembledLength = Convert.ToUInt32(propValue, 10); }
      }
      return obj;
    }
    public static Netbios DecodeJson(JsonTextReader reader)                        
    {                                                                                     
        if (reader.TokenType != JsonToken.StartObject) return null;                       
        var obj = new Netbios();                                                   
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
                    
    static void SetField(Netbios obj, string propName, string propValue)           
    {                                                                                     
      switch (propName)                                                                   
      {                                                                                   
      case "netbios_netbios_command": obj.NetbiosCommand = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_hdr_len": obj.NetbiosHdrLen = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_delimiter": obj.NetbiosDelimiter = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_xmit_corrl": obj.NetbiosXmitCorrl = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_resp_corrl": obj.NetbiosRespCorrl = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_call_name_type": obj.NetbiosCallNameType = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_nb_name_type": obj.NetbiosNbNameType = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_nb_name": obj.NetbiosNbName = propValue; break;
      case "netbios_netbios_version": obj.NetbiosVersion = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_netbios_no_receive_flags": obj.NetbiosNoReceiveFlags = Convert.ToUInt32(propValue, 16); break;
      case "netbios_no_receive_flags_netbios_no_receive_flags_send_no_ack": obj.NetbiosNoReceiveFlagsSendNoAck = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_netbios_largest_frame": obj.NetbiosLargestFrame = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_status_buffer_len": obj.NetbiosStatusBufferLen = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_status": obj.NetbiosStatus = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_name_type": obj.NetbiosNameType = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_max_data_recv_size": obj.NetbiosMaxDataRecvSize = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_termination_indicator": obj.NetbiosTerminationIndicator = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_num_data_bytes_accepted": obj.NetbiosNumDataBytesAccepted = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_local_session": obj.NetbiosLocalSession = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_remote_session": obj.NetbiosRemoteSession = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_flags": obj.NetbiosFlags = Convert.ToUInt32(propValue, 16); break;
      case "netbios_flags_netbios_flags_send_no_ack": obj.NetbiosFlagsSendNoAck = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_flags_netbios_flags_ack": obj.NetbiosFlagsAck = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_flags_netbios_flags_ack_with_data": obj.NetbiosFlagsAckWithData = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_flags_netbios_flags_ack_expected": obj.NetbiosFlagsAckExpected = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_flags_netbios_flags_recv_cont_req": obj.NetbiosFlagsRecvContReq = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_netbios_data2": obj.NetbiosData2 = Convert.ToUInt32(propValue, 16); break;
      case "netbios_data2_netbios_data2_frame": obj.NetbiosData2Frame = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_data2_netbios_data2_user": obj.NetbiosData2User = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_data2_netbios_data2_status": obj.NetbiosData2Status = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_datagram_mac": obj.NetbiosDatagramMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(propValue.ToUpperInvariant().Replace(':','-')).GetAddressBytes()); break;
      case "netbios_netbios_datagram_bcast_mac": obj.NetbiosDatagramBcastMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(propValue.ToUpperInvariant().Replace(':','-')).GetAddressBytes()); break;
      case "netbios_netbios_resync_indicator": obj.NetbiosResyncIndicator = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_status_request": obj.NetbiosStatusRequest = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_local_session_no": obj.NetbiosLocalSessionNo = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_state_of_name": obj.NetbiosStateOfName = Convert.ToUInt32(propValue, 16); break;
      case "netbios_netbios_status_response": obj.NetbiosStatusResponse = Convert.ToUInt32(propValue, 10); break;
      case "netbios_fragment_netbios_fragment_overlap": obj.NetbiosFragmentOverlap = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_fragment_overlap_netbios_fragment_overlap_conflict": obj.NetbiosFragmentOverlapConflict = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_fragment_netbios_fragment_multipletails": obj.NetbiosFragmentMultipletails = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_fragment_netbios_fragment_toolongfragment": obj.NetbiosFragmentToolongfragment = Convert.ToInt32(propValue, 10) != 0; break;
      case "netbios_fragment_netbios_fragment_error": obj.NetbiosFragmentError = default(Int64); break;
      case "netbios_fragment_netbios_fragment_count": obj.NetbiosFragmentCount = Convert.ToUInt32(propValue, 10); break;
      case "netbios_netbios_fragment": obj.NetbiosFragment = default(Int64); break;
      case "netbios_netbios_fragments": obj.NetbiosFragments = default(Int32); break;
      case "netbios_netbios_reassembled_length": obj.NetbiosReassembledLength = Convert.ToUInt32(propValue, 10); break;
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

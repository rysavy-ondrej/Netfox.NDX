using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
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
        if (val != null) obj.NetbiosCommand = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_hdr_len"];
        if (val != null) obj.NetbiosHdrLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_delimiter"];
        if (val != null) obj.NetbiosDelimiter = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_xmit_corrl"];
        if (val != null) obj.NetbiosXmitCorrl = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_resp_corrl"];
        if (val != null) obj.NetbiosRespCorrl = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_call_name_type"];
        if (val != null) obj.NetbiosCallNameType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_nb_name_type"];
        if (val != null) obj.NetbiosNbNameType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_nb_name"];
        if (val != null) obj.NetbiosNbName = val.Value<string>();
      }
      {
        var val = token["netbios_netbios_version"];
        if (val != null) obj.NetbiosVersion = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_netbios_no_receive_flags"];
        if (val != null) obj.NetbiosNoReceiveFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_no_receive_flags_netbios_no_receive_flags_send_no_ack"];
        if (val != null) obj.NetbiosNoReceiveFlagsSendNoAck = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_netbios_largest_frame"];
        if (val != null) obj.NetbiosLargestFrame = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_status_buffer_len"];
        if (val != null) obj.NetbiosStatusBufferLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_status"];
        if (val != null) obj.NetbiosStatus = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_name_type"];
        if (val != null) obj.NetbiosNameType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_max_data_recv_size"];
        if (val != null) obj.NetbiosMaxDataRecvSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_termination_indicator"];
        if (val != null) obj.NetbiosTerminationIndicator = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_num_data_bytes_accepted"];
        if (val != null) obj.NetbiosNumDataBytesAccepted = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_local_session"];
        if (val != null) obj.NetbiosLocalSession = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_remote_session"];
        if (val != null) obj.NetbiosRemoteSession = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_flags"];
        if (val != null) obj.NetbiosFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_flags_netbios_flags_send_no_ack"];
        if (val != null) obj.NetbiosFlagsSendNoAck = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_flags_netbios_flags_ack"];
        if (val != null) obj.NetbiosFlagsAck = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_flags_netbios_flags_ack_with_data"];
        if (val != null) obj.NetbiosFlagsAckWithData = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_flags_netbios_flags_ack_expected"];
        if (val != null) obj.NetbiosFlagsAckExpected = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_flags_netbios_flags_recv_cont_req"];
        if (val != null) obj.NetbiosFlagsRecvContReq = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_netbios_data2"];
        if (val != null) obj.NetbiosData2 = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_data2_netbios_data2_frame"];
        if (val != null) obj.NetbiosData2Frame = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_data2_netbios_data2_user"];
        if (val != null) obj.NetbiosData2User = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_data2_netbios_data2_status"];
        if (val != null) obj.NetbiosData2Status = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_datagram_mac"];
        if (val != null) obj.NetbiosDatagramMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["netbios_netbios_datagram_bcast_mac"];
        if (val != null) obj.NetbiosDatagramBcastMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["netbios_netbios_resync_indicator"];
        if (val != null) obj.NetbiosResyncIndicator = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_status_request"];
        if (val != null) obj.NetbiosStatusRequest = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_local_session_no"];
        if (val != null) obj.NetbiosLocalSessionNo = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_state_of_name"];
        if (val != null) obj.NetbiosStateOfName = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["netbios_netbios_status_response"];
        if (val != null) obj.NetbiosStatusResponse = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_fragment_netbios_fragment_overlap"];
        if (val != null) obj.NetbiosFragmentOverlap = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_fragment_overlap_netbios_fragment_overlap_conflict"];
        if (val != null) obj.NetbiosFragmentOverlapConflict = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_fragment_netbios_fragment_multipletails"];
        if (val != null) obj.NetbiosFragmentMultipletails = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_fragment_netbios_fragment_toolongfragment"];
        if (val != null) obj.NetbiosFragmentToolongfragment = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["netbios_fragment_netbios_fragment_error"];
        if (val != null) obj.NetbiosFragmentError = default(Int64);
      }
      {
        var val = token["netbios_fragment_netbios_fragment_count"];
        if (val != null) obj.NetbiosFragmentCount = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["netbios_netbios_fragment"];
        if (val != null) obj.NetbiosFragment = default(Int64);
      }
      {
        var val = token["netbios_netbios_fragments"];
        if (val != null) obj.NetbiosFragments = default(Int32);
      }
      {
        var val = token["netbios_netbios_reassembled_length"];
        if (val != null) obj.NetbiosReassembledLength = Convert.ToUInt32(val.Value<string>(), 10);
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

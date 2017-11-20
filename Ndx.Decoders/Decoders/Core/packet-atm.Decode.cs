using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Core
{
  public sealed partial class Atm
  {
    public static Atm DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Atm DecodeJson(JToken token)
    {
      var obj = new Atm();
      {
        var val = token["atm_atm_aal"];
        if (val != null) obj.AtmAal = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_GFC"];
        if (val != null) obj.AtmGFC = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_vpi"];
        if (val != null) obj.AtmVpi = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_vci"];
        if (val != null) obj.AtmVci = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_cid"];
        if (val != null) obj.AtmCid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_reserved"];
        if (val != null) obj.AtmReserved = StringToBytes(val.Value<string>());
      }
      {
        var val = token["atm_atm_le_client_client"];
        if (val != null) obj.AtmLeClientClient = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_lan_destination_tag"];
        if (val != null) obj.AtmLanDestinationTag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_lan_destination_mac"];
        if (val != null) obj.AtmLanDestinationMac = Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes());
      }
      {
        var val = token["atm_atm_le_control_tlv_type"];
        if (val != null) obj.AtmLeControlTlvType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_control_tlv_length"];
        if (val != null) obj.AtmLeControlTlvLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_lan_destination_route_desc"];
        if (val != null) obj.AtmLanDestinationRouteDesc = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_lan_destination_lan_id"];
        if (val != null) obj.AtmLanDestinationLanId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_lan_destination_bridge_num"];
        if (val != null) obj.AtmLanDestinationBridgeNum = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_source_atm"];
        if (val != null) obj.AtmSourceAtm = StringToBytes(val.Value<string>());
      }
      {
        var val = token["atm_atm_target_atm"];
        if (val != null) obj.AtmTargetAtm = StringToBytes(val.Value<string>());
      }
      {
        var val = token["atm_atm_le_configure_join_frame_lan_type"];
        if (val != null) obj.AtmLeConfigureJoinFrameLanType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_configure_join_frame_max_frame_size"];
        if (val != null) obj.AtmLeConfigureJoinFrameMaxFrameSize = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_configure_join_frame_num_tlvs"];
        if (val != null) obj.AtmLeConfigureJoinFrameNumTlvs = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_le_configure_join_frame_elan_name_size"];
        if (val != null) obj.AtmLeConfigureJoinFrameElanNameSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_le_registration_frame_num_tlvs"];
        if (val != null) obj.AtmLeRegistrationFrameNumTlvs = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_le_arp_frame_num_tlvs"];
        if (val != null) obj.AtmLeArpFrameNumTlvs = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_le_verify_frame_num_tlvs"];
        if (val != null) obj.AtmLeVerifyFrameNumTlvs = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_le_configure_join_frame_elan_name"];
        if (val != null) obj.AtmLeConfigureJoinFrameElanName = StringToBytes(val.Value<string>());
      }
      {
        var val = token["atm_atm_le_control_marker"];
        if (val != null) obj.AtmLeControlMarker = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_control_protocol"];
        if (val != null) obj.AtmLeControlProtocol = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_control_version"];
        if (val != null) obj.AtmLeControlVersion = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_control_opcode"];
        if (val != null) obj.AtmLeControlOpcode = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_control_status"];
        if (val != null) obj.AtmLeControlStatus = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_control_transaction_id"];
        if (val != null) obj.AtmLeControlTransactionId = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_control_requester_lecid"];
        if (val != null) obj.AtmLeControlRequesterLecid = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_le_control_flag"];
        if (val != null) obj.AtmLeControlFlag = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_le_control_flag_atm_le_control_flag_v2_capable"];
        if (val != null) obj.AtmLeControlFlagV2Capable = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["atm_le_control_flag_atm_le_control_flag_selective_multicast"];
        if (val != null) obj.AtmLeControlFlagSelectiveMulticast = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["atm_le_control_flag_atm_le_control_flag_v2_required"];
        if (val != null) obj.AtmLeControlFlagV2Required = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["atm_le_control_flag_atm_le_control_flag_flag_proxy"];
        if (val != null) obj.AtmLeControlFlagFlagProxy = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["atm_le_control_flag_atm_le_control_flag_exclude_explorer_frames"];
        if (val != null) obj.AtmLeControlFlagExcludeExplorerFrames = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["atm_le_control_flag_atm_le_control_flag_address"];
        if (val != null) obj.AtmLeControlFlagAddress = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["atm_le_control_flag_atm_le_control_flag_topology_change"];
        if (val != null) obj.AtmLeControlFlagTopologyChange = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["atm_atm_traffic_type"];
        if (val != null) obj.AtmTrafficType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_traffic_vcmx"];
        if (val != null) obj.AtmTrafficVcmx = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_traffic_lane"];
        if (val != null) obj.AtmTrafficLane = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_traffic_ipsilon"];
        if (val != null) obj.AtmTrafficIpsilon = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_cells"];
        if (val != null) obj.AtmCells = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_hf_atm_aal5t_uu"];
        if (val != null) obj.AtmHfAtmAal5TUu = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_hf_atm_aal5t_cpi"];
        if (val != null) obj.AtmHfAtmAal5TCpi = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_aal5t_len"];
        if (val != null) obj.AtmAal5TLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aal5t_crc"];
        if (val != null) obj.AtmAal5TCrc = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_payload_type"];
        if (val != null) obj.AtmPayloadType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_cell_loss_priority"];
        if (val != null) obj.AtmCellLossPriority = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["atm_atm_header_error_check"];
        if (val != null) obj.AtmHeaderErrorCheck = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_channel"];
        if (val != null) obj.AtmChannel = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aa1_csi"];
        if (val != null) obj.AtmAa1Csi = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aa1_seq_count"];
        if (val != null) obj.AtmAa1SeqCount = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aa1_crc"];
        if (val != null) obj.AtmAa1Crc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aa1_parity"];
        if (val != null) obj.AtmAa1Parity = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aa1_payload"];
        if (val != null) obj.AtmAa1Payload = StringToBytes(val.Value<string>());
      }
      {
        var val = token["atm_atm_aal3_4_seg_type"];
        if (val != null) obj.AtmAal34SegType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aal3_4_seq_num"];
        if (val != null) obj.AtmAal34SeqNum = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aal3_4_multiplex_id"];
        if (val != null) obj.AtmAal34MultiplexId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aal3_4_information"];
        if (val != null) obj.AtmAal34Information = StringToBytes(val.Value<string>());
      }
      {
        var val = token["atm_atm_aal3_4_length_indicator"];
        if (val != null) obj.AtmAal34LengthIndicator = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aal3_4_crc"];
        if (val != null) obj.AtmAal34Crc = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aal_oamcell_type"];
        if (val != null) obj.AtmAalOamcellType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_aal_oamcell_type_atm_aal_oamcell_type_fm"];
        if (val != null) obj.AtmAalOamcellTypeFm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_aal_oamcell_type_atm_aal_oamcell_type_pm"];
        if (val != null) obj.AtmAalOamcellTypePm = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_aal_oamcell_type_atm_aal_oamcell_type_ad"];
        if (val != null) obj.AtmAalOamcellTypeAd = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_aal_oamcell_type_atm_aal_oamcell_type_ft"];
        if (val != null) obj.AtmAalOamcellTypeFt = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["atm_atm_aal_oamcell_func_spec"];
        if (val != null) obj.AtmAalOamcellFuncSpec = StringToBytes(val.Value<string>());
      }
      {
        var val = token["atm_atm_aal_oamcell_crc"];
        if (val != null) obj.AtmAalOamcellCrc = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["atm_atm_padding"];
        if (val != null) obj.AtmPadding = StringToBytes(val.Value<string>());
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

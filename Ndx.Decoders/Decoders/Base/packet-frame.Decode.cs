using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Basic
{
  public sealed partial class Frame
  {
    public static Frame DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Frame DecodeJson(JToken token)
    {
      var obj = new Frame();
      {
        var val = token["frame_frame_time"];
        if (val != null) obj.FrameTime = default(Int64);
      }
      {
        var val = token["frame_frame_offset_shift"];
        if (val != null) obj.FrameOffsetShift = default(Int64);
      }
      {
        var val = token["frame_frame_time_epoch"];
        if (val != null) obj.FrameTimeEpoch = default(Int64);
      }
      {
        var val = token["frame_frame_time_delta"];
        if (val != null) obj.FrameTimeDelta = default(Int64);
      }
      {
        var val = token["frame_frame_time_delta_displayed"];
        if (val != null) obj.FrameTimeDeltaDisplayed = default(Int64);
      }
      {
        var val = token["frame_frame_time_relative"];
        if (val != null) obj.FrameTimeRelative = default(Int64);
      }
      {
        var val = token["frame_frame_ref_time"];
        if (val != null) obj.FrameRefTime = default(Int32);
      }
      {
        var val = token["frame_frame_number"];
        if (val != null) obj.FrameNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_len"];
        if (val != null) obj.FrameLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_cap_len"];
        if (val != null) obj.FrameCapLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_md5_hash"];
        if (val != null) obj.FrameMd5Hash = val.Value<string>();
      }
      {
        var val = token["frame_frame_p2p_dir"];
        if (val != null) obj.FrameP2PDir = Convert.ToInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_link_nr"];
        if (val != null) obj.FrameLinkNr = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_file_off"];
        if (val != null) obj.FrameFileOff = Convert.ToInt64(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_marked"];
        if (val != null) obj.FrameMarked = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_ignored"];
        if (val != null) obj.FrameIgnored = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_protocols"];
        if (val != null) obj.FrameProtocols = val.Value<string>();
      }
      {
        var val = token["frame_frame_coloring_rule_name"];
        if (val != null) obj.FrameColoringRuleName = val.Value<string>();
      }
      {
        var val = token["frame_frame_coloring_rule_string"];
        if (val != null) obj.FrameColoringRuleString = val.Value<string>();
      }
      {
        var val = token["frame_frame_interface_id"];
        if (val != null) obj.FrameInterfaceId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_interface_name"];
        if (val != null) obj.FrameInterfaceName = val.Value<string>();
      }
      {
        var val = token["frame_frame_interface_description"];
        if (val != null) obj.FrameInterfaceDescription = val.Value<string>();
      }
      {
        var val = token["frame_frame_packet_flags"];
        if (val != null) obj.FramePacketFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["frame_frame_packet_flags_direction"];
        if (val != null) obj.FramePacketFlagsDirection = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["frame_frame_packet_flags_reception_type"];
        if (val != null) obj.FramePacketFlagsReceptionType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_packet_flags_fcs_length"];
        if (val != null) obj.FramePacketFlagsFcsLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_packet_flags_reserved"];
        if (val != null) obj.FramePacketFlagsReserved = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["frame_frame_packet_flags_crc_error"];
        if (val != null) obj.FramePacketFlagsCrcError = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_packet_flags_packet_too_error"];
        if (val != null) obj.FramePacketFlagsPacketTooError = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_packet_flags_packet_too_short_error"];
        if (val != null) obj.FramePacketFlagsPacketTooShortError = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_packet_flags_wrong_inter_frame_gap_error"];
        if (val != null) obj.FramePacketFlagsWrongInterFrameGapError = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_packet_flags_unaligned_frame_error"];
        if (val != null) obj.FramePacketFlagsUnalignedFrameError = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_packet_flags_start_frame_delimiter_error"];
        if (val != null) obj.FramePacketFlagsStartFrameDelimiterError = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_packet_flags_preamble_error"];
        if (val != null) obj.FramePacketFlagsPreambleError = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_packet_flags_symbol_error"];
        if (val != null) obj.FramePacketFlagsSymbolError = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["frame_frame_comment"];
        if (val != null) obj.FrameComment = val.Value<string>();
      }
      {
        var val = token["frame_frame_encap_type"];
        if (val != null) obj.FrameEncapType = Convert.ToInt32(val.Value<string>(), 10);
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

using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Core
{
  public sealed partial class Ppp
  {
    public static Ppp DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Ppp DecodeJson(JToken token)
    {
      var obj = new Ppp();
      {
        var val = token["ppp_ppp_hdlc_fragment"];
        if (val != null) obj.PppHdlcFragment = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ppp_ppp_hdlc_data"];
        if (val != null) obj.PppHdlcData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ppp_ppp_direction"];
        if (val != null) obj.PppDirection = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ppp_ppp_address"];
        if (val != null) obj.PppAddress = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_ppp_control"];
        if (val != null) obj.PppControl = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_ppp_protocol"];
        if (val != null) obj.PppProtocol = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_ppp_code"];
        if (val != null) obj.PppCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ppp_ppp_identifier"];
        if (val != null) obj.PppIdentifier = default(UInt32);
      }
      {
        var val = token["ppp_ppp_length"];
        if (val != null) obj.PppLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ppp_ppp_magic_number"];
        if (val != null) obj.PppMagicNumber = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_ppp_oui"];
        if (val != null) obj.PppOui = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_ppp_kind"];
        if (val != null) obj.PppKind = default(UInt32);
      }
      {
        var val = token["ppp_ppp_data"];
        if (val != null) obj.PppData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["ppp_ppp_fcs_16"];
        if (val != null) obj.PppFcs16 = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_ppp_fcs_32"];
        if (val != null) obj.PppFcs32 = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_ppp_fcs_status"];
        if (val != null) obj.PppFcsStatus = default(UInt32);
      }
      {
        var val = token["pppmuxcp_flags_pppmuxcp_flags_pid"];
        if (val != null) obj.PppmuxcpFlagsPid = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["pppmuxcp_flags_pppmuxcp_flags_field_length"];
        if (val != null) obj.PppmuxcpFlagsFieldLength = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["ppp_pppmuxcp_opt_type"];
        if (val != null) obj.PppmuxcpOptType = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ppp_pppmuxcp_opt_length"];
        if (val != null) obj.PppmuxcpOptLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ppp_pppmuxcp_flags"];
        if (val != null) obj.PppmuxcpFlags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_pppmuxcp_sub_frame_length"];
        if (val != null) obj.PppmuxcpSubFrameLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["ppp_pppmuxcp_def_prot_id"];
        if (val != null) obj.PppmuxcpDefProtId = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["ppp_pppmux_protocol"];
        if (val != null) obj.PppmuxProtocol = Convert.ToUInt32(val.Value<string>(), 16);
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

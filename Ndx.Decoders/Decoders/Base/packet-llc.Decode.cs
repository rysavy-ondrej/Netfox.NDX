using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Basic
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
        if (val != null) obj.LlcDsap = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_dsap_llc_dsap_sap"];
        if (val != null) obj.LlcDsapSap = default(UInt32);
      }
      {
        var val = token["llc_dsap_llc_dsap_ig"];
        if (val != null) obj.LlcDsapIg = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["llc_llc_ssap"];
        if (val != null) obj.LlcSsap = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_ssap_llc_ssap_sap"];
        if (val != null) obj.LlcSsapSap = default(UInt32);
      }
      {
        var val = token["llc_ssap_llc_ssap_cr"];
        if (val != null) obj.LlcSsapCr = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["llc_llc_control"];
        if (val != null) obj.LlcControl = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_control_llc_control_n_r"];
        if (val != null) obj.LlcControlNR = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["llc_control_llc_control_n_s"];
        if (val != null) obj.LlcControlNS = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["llc_control_llc_control_p"];
        if (val != null) obj.LlcControlP = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["llc_control_llc_control_f"];
        if (val != null) obj.LlcControlF = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["llc_control_llc_control_s_ftype"];
        if (val != null) obj.LlcControlSFtype = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_control_llc_control_u_modifier_cmd"];
        if (val != null) obj.LlcControlUModifierCmd = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_control_llc_control_u_modifier_resp"];
        if (val != null) obj.LlcControlUModifierResp = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_control_llc_control_ftype"];
        if (val != null) obj.LlcControlFtype = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_llc_type"];
        if (val != null) obj.LlcType = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_llc_oui"];
        if (val != null) obj.LlcOui = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["llc_llc_pid"];
        if (val != null) obj.LlcPid = Convert.ToUInt32(val.Value<string>(), 16);
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

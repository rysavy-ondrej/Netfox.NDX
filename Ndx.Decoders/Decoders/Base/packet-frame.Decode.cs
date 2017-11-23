using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Base
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
        var val = token["frame_frame_encap_type"];
        if (val != null) obj.FrameEncapType = Convert.ToInt32(val.Value<string>(), 10);
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
        var val = token["frame_frame_protocols"];
        if (val != null) obj.FrameProtocols = val.Value<string>();
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

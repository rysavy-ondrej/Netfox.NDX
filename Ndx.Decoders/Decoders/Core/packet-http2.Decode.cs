using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
namespace Ndx.Decoders.Core
{
  public sealed partial class Http2
  {
    public static Http2 DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Http2 DecodeJson(JToken token)
    {
      var obj = new Http2();
      {
        var val = token["http2_http2_stream"];
        if (val != null) obj.Http2Stream = default(Int32);
      }
      {
        var val = token["http2_http2_length"];
        if (val != null) obj.Http2Length = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_type"];
        if (val != null) obj.Http2Type = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_r"];
        if (val != null) obj.Http2R = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_headers_http2_headers_weight"];
        if (val != null) obj.Http2HeadersWeight = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_headers_http2_headers_weight_real"];
        if (val != null) obj.Http2HeadersWeightReal = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_streamid"];
        if (val != null) obj.Http2Streamid = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_magic"];
        if (val != null) obj.Http2Magic = val.Value<string>();
      }
      {
        var val = token["http2_http2_unknown"];
        if (val != null) obj.Http2Unknown = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_flags"];
        if (val != null) obj.Http2Flags = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_flags_http2_flags_end_stream"];
        if (val != null) obj.Http2FlagsEndStream = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_flags_http2_flags_eh"];
        if (val != null) obj.Http2FlagsEh = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_flags_http2_flags_padded"];
        if (val != null) obj.Http2FlagsPadded = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_flags_http2_flags_priority"];
        if (val != null) obj.Http2FlagsPriority = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_http2_flags_ack_ping"];
        if (val != null) obj.Http2FlagsAckPing = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_flags_http2_flags_unused"];
        if (val != null) obj.Http2FlagsUnused = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_flags_http2_flags_unused_settings"];
        if (val != null) obj.Http2FlagsUnusedSettings = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_flags_http2_flags_unused_ping"];
        if (val != null) obj.Http2FlagsUnusedPing = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_flags_http2_flags_unused_continuation"];
        if (val != null) obj.Http2FlagsUnusedContinuation = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_flags_http2_flags_unused_push_promise"];
        if (val != null) obj.Http2FlagsUnusedPushPromise = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_flags_http2_flags_unused_data"];
        if (val != null) obj.Http2FlagsUnusedData = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_flags_http2_flags_unused_headers"];
        if (val != null) obj.Http2FlagsUnusedHeaders = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_http2_flags_ack_settings"];
        if (val != null) obj.Http2FlagsAckSettings = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_http2_padding"];
        if (val != null) obj.Http2Padding = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_http2_pad_length"];
        if (val != null) obj.Http2PadLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_exclusive"];
        if (val != null) obj.Http2Exclusive = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_http2_stream_dependency"];
        if (val != null) obj.Http2StreamDependency = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_data_data"];
        if (val != null) obj.Http2DataData = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_data_padding"];
        if (val != null) obj.Http2DataPadding = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_body_fragments"];
        if (val != null) obj.Http2BodyFragments = default(Int32);
      }
      {
        var val = token["http2_http2_body_fragment"];
        if (val != null) obj.Http2BodyFragment = default(Int64);
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_overlap"];
        if (val != null) obj.Http2BodyFragmentOverlap = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_body_fragment_overlap_http2_body_fragment_overlap_conflicts"];
        if (val != null) obj.Http2BodyFragmentOverlapConflicts = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_multiple_tails"];
        if (val != null) obj.Http2BodyFragmentMultipleTails = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_too_long_fragment"];
        if (val != null) obj.Http2BodyFragmentTooLongFragment = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_error"];
        if (val != null) obj.Http2BodyFragmentError = default(Int64);
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_count"];
        if (val != null) obj.Http2BodyFragmentCount = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_body_reassembled_in"];
        if (val != null) obj.Http2BodyReassembledIn = default(Int64);
      }
      {
        var val = token["http2_http2_body_reassembled_length"];
        if (val != null) obj.Http2BodyReassembledLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_headers"];
        if (val != null) obj.Http2Headers = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_headers_http2_headers_padding"];
        if (val != null) obj.Http2HeadersPadding = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_header"];
        if (val != null) obj.Http2Header = default(Int32);
      }
      {
        var val = token["http2_header_http2_header_length"];
        if (val != null) obj.Http2HeaderLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_header_http2_header_count"];
        if (val != null) obj.Http2HeaderCount = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_header_name_http2_header_name_length"];
        if (val != null) obj.Http2HeaderNameLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_header_http2_header_name"];
        if (val != null) obj.Http2HeaderName = val.Value<string>();
      }
      {
        var val = token["http2_header_value_http2_header_value_length"];
        if (val != null) obj.Http2HeaderValueLength = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_header_http2_header_value"];
        if (val != null) obj.Http2HeaderValue = val.Value<string>();
      }
      {
        var val = token["http2_header_http2_header_repr"];
        if (val != null) obj.Http2HeaderRepr = val.Value<string>();
      }
      {
        var val = token["http2_header_http2_header_index"];
        if (val != null) obj.Http2HeaderIndex = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_header_table_size_update"];
        if (val != null) obj.Http2HeaderTableSizeUpdate = default(Int32);
      }
      {
        var val = token["http2_header_table_size_update_http2_header_table_size_update_header_table_size"];
        if (val != null) obj.Http2HeaderTableSizeUpdateHeaderTableSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_rst_stream_error"];
        if (val != null) obj.Http2RstStreamError = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_settings"];
        if (val != null) obj.Http2Settings = default(Int32);
      }
      {
        var val = token["http2_settings_http2_settings_id"];
        if (val != null) obj.Http2SettingsId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_settings_http2_settings_header_table_size"];
        if (val != null) obj.Http2SettingsHeaderTableSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_settings_http2_settings_enable_push"];
        if (val != null) obj.Http2SettingsEnablePush = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_settings_http2_settings_max_concurrent_streams"];
        if (val != null) obj.Http2SettingsMaxConcurrentStreams = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_settings_http2_settings_initial_window_size"];
        if (val != null) obj.Http2SettingsInitialWindowSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_settings_http2_settings_max_frame_size"];
        if (val != null) obj.Http2SettingsMaxFrameSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_settings_http2_settings_max_header_list_size"];
        if (val != null) obj.Http2SettingsMaxHeaderListSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_settings_http2_settings_unknown"];
        if (val != null) obj.Http2SettingsUnknown = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_push_promise_r"];
        if (val != null) obj.Http2PushPromiseR = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_http2_push_promise_promised_stream_id"];
        if (val != null) obj.Http2PushPromisePromisedStreamId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_push_promise_header"];
        if (val != null) obj.Http2PushPromiseHeader = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_push_promise_padding"];
        if (val != null) obj.Http2PushPromisePadding = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_ping"];
        if (val != null) obj.Http2Ping = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_pong"];
        if (val != null) obj.Http2Pong = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_goway_r"];
        if (val != null) obj.Http2GowayR = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_http2_goaway_last_stream_id"];
        if (val != null) obj.Http2GoawayLastStreamId = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_goaway_error"];
        if (val != null) obj.Http2GoawayError = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_goaway_addata"];
        if (val != null) obj.Http2GoawayAddata = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_http2_window_update_r"];
        if (val != null) obj.Http2WindowUpdateR = Convert.ToUInt32(val.Value<string>(), 16);
      }
      {
        var val = token["http2_http2_window_update_window_size_increment"];
        if (val != null) obj.Http2WindowUpdateWindowSizeIncrement = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_continuation_header"];
        if (val != null) obj.Http2ContinuationHeader = val.Value<string>();
      }
      {
        var val = token["http2_http2_continuation_padding"];
        if (val != null) obj.Http2ContinuationPadding = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http2_altsvc_origin_http2_altsvc_origin_len"];
        if (val != null) obj.Http2AltsvcOriginLen = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http2_http2_altsvc_origin"];
        if (val != null) obj.Http2AltsvcOrigin = val.Value<string>();
      }
      {
        var val = token["http2_http2_altsvc_field_value"];
        if (val != null) obj.Http2AltsvcFieldValue = val.Value<string>();
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

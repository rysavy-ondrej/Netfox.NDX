// This is file was generated by netdx on (2017-11-24 12:35:00 PM.
using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Stream = default(Int32); }
      }
      {
        var val = token["http2_http2_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Length = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Type = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_r"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2R = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_headers_http2_headers_weight"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeadersWeight = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_headers_http2_headers_weight_real"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeadersWeightReal = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_streamid"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Streamid = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_magic"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Magic = propValue; }
      }
      {
        var val = token["http2_http2_unknown"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Unknown = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_flags"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Flags = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_flags_http2_flags_end_stream"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsEndStream = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_flags_http2_flags_eh"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsEh = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_flags_http2_flags_padded"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsPadded = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_flags_http2_flags_priority"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsPriority = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_http2_flags_ack_ping"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsAckPing = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_flags_http2_flags_unused"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsUnused = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_flags_http2_flags_unused_settings"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsUnusedSettings = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_flags_http2_flags_unused_ping"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsUnusedPing = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_flags_http2_flags_unused_continuation"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsUnusedContinuation = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_flags_http2_flags_unused_push_promise"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsUnusedPushPromise = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_flags_http2_flags_unused_data"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsUnusedData = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_flags_http2_flags_unused_headers"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsUnusedHeaders = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_http2_flags_ack_settings"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2FlagsAckSettings = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_http2_padding"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Padding = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_http2_pad_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2PadLength = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_exclusive"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Exclusive = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_http2_stream_dependency"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2StreamDependency = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_data_data"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2DataData = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_data_padding"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2DataPadding = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_body_fragments"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyFragments = default(Int32); }
      }
      {
        var val = token["http2_http2_body_fragment"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyFragment = default(Int64); }
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_overlap"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyFragmentOverlap = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_body_fragment_overlap_http2_body_fragment_overlap_conflicts"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyFragmentOverlapConflicts = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_multiple_tails"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyFragmentMultipleTails = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_too_long_fragment"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyFragmentTooLongFragment = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_error"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyFragmentError = default(Int64); }
      }
      {
        var val = token["http2_body_fragment_http2_body_fragment_count"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyFragmentCount = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_body_reassembled_in"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyReassembledIn = default(Int64); }
      }
      {
        var val = token["http2_http2_body_reassembled_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2BodyReassembledLength = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_headers"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Headers = StringToBytes(propValue); }
      }
      {
        var val = token["http2_headers_http2_headers_padding"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeadersPadding = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_header"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Header = default(Int32); }
      }
      {
        var val = token["http2_header_http2_header_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderLength = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_header_http2_header_count"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderCount = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_header_name_http2_header_name_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderNameLength = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_header_http2_header_name"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderName = propValue; }
      }
      {
        var val = token["http2_header_value_http2_header_value_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderValueLength = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_header_http2_header_value"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderValue = propValue; }
      }
      {
        var val = token["http2_header_http2_header_repr"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderRepr = propValue; }
      }
      {
        var val = token["http2_header_http2_header_index"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderIndex = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_header_table_size_update"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderTableSizeUpdate = default(Int32); }
      }
      {
        var val = token["http2_header_table_size_update_http2_header_table_size_update_header_table_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2HeaderTableSizeUpdateHeaderTableSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_rst_stream_error"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2RstStreamError = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_settings"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Settings = default(Int32); }
      }
      {
        var val = token["http2_settings_http2_settings_id"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2SettingsId = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_settings_http2_settings_header_table_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2SettingsHeaderTableSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_settings_http2_settings_enable_push"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2SettingsEnablePush = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_settings_http2_settings_max_concurrent_streams"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2SettingsMaxConcurrentStreams = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_settings_http2_settings_initial_window_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2SettingsInitialWindowSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_settings_http2_settings_max_frame_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2SettingsMaxFrameSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_settings_http2_settings_max_header_list_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2SettingsMaxHeaderListSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_settings_http2_settings_unknown"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2SettingsUnknown = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_push_promise_r"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2PushPromiseR = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_http2_push_promise_promised_stream_id"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2PushPromisePromisedStreamId = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_push_promise_header"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2PushPromiseHeader = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_push_promise_padding"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2PushPromisePadding = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_ping"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Ping = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_pong"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2Pong = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_goway_r"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2GowayR = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_http2_goaway_last_stream_id"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2GoawayLastStreamId = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_goaway_error"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2GoawayError = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_goaway_addata"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2GoawayAddata = StringToBytes(propValue); }
      }
      {
        var val = token["http2_http2_window_update_r"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2WindowUpdateR = Convert.ToUInt32(propValue, 16); }
      }
      {
        var val = token["http2_http2_window_update_window_size_increment"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2WindowUpdateWindowSizeIncrement = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_continuation_header"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2ContinuationHeader = propValue; }
      }
      {
        var val = token["http2_http2_continuation_padding"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2ContinuationPadding = StringToBytes(propValue); }
      }
      {
        var val = token["http2_altsvc_origin_http2_altsvc_origin_len"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2AltsvcOriginLen = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http2_http2_altsvc_origin"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2AltsvcOrigin = propValue; }
      }
      {
        var val = token["http2_http2_altsvc_field_value"];
        if (val != null) { var propValue = val.Value<string>(); obj.Http2AltsvcFieldValue = propValue; }
      }
      return obj;
    }
    public static Http2 DecodeJson(JsonTextReader reader)                        
    {                                                                                     
        if (reader.TokenType != JsonToken.StartObject) return null;                       
        var obj = new Http2();                                                   
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
                    
    static void SetField(Http2 obj, string propName, string propValue)           
    {                                                                                     
      switch (propName)                                                                   
      {                                                                                   
      case "http2_http2_stream": obj.Http2Stream = default(Int32); break;
      case "http2_http2_length": obj.Http2Length = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_type": obj.Http2Type = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_r": obj.Http2R = Convert.ToUInt32(propValue, 16); break;
      case "http2_headers_http2_headers_weight": obj.Http2HeadersWeight = Convert.ToUInt32(propValue, 10); break;
      case "http2_headers_http2_headers_weight_real": obj.Http2HeadersWeightReal = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_streamid": obj.Http2Streamid = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_magic": obj.Http2Magic = propValue; break;
      case "http2_http2_unknown": obj.Http2Unknown = StringToBytes(propValue); break;
      case "http2_http2_flags": obj.Http2Flags = Convert.ToUInt32(propValue, 16); break;
      case "http2_flags_http2_flags_end_stream": obj.Http2FlagsEndStream = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_flags_http2_flags_eh": obj.Http2FlagsEh = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_flags_http2_flags_padded": obj.Http2FlagsPadded = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_flags_http2_flags_priority": obj.Http2FlagsPriority = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_http2_flags_ack_ping": obj.Http2FlagsAckPing = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_flags_http2_flags_unused": obj.Http2FlagsUnused = Convert.ToUInt32(propValue, 16); break;
      case "http2_flags_http2_flags_unused_settings": obj.Http2FlagsUnusedSettings = Convert.ToUInt32(propValue, 16); break;
      case "http2_flags_http2_flags_unused_ping": obj.Http2FlagsUnusedPing = Convert.ToUInt32(propValue, 16); break;
      case "http2_flags_http2_flags_unused_continuation": obj.Http2FlagsUnusedContinuation = Convert.ToUInt32(propValue, 16); break;
      case "http2_flags_http2_flags_unused_push_promise": obj.Http2FlagsUnusedPushPromise = Convert.ToUInt32(propValue, 16); break;
      case "http2_flags_http2_flags_unused_data": obj.Http2FlagsUnusedData = Convert.ToUInt32(propValue, 16); break;
      case "http2_flags_http2_flags_unused_headers": obj.Http2FlagsUnusedHeaders = Convert.ToUInt32(propValue, 16); break;
      case "http2_http2_flags_ack_settings": obj.Http2FlagsAckSettings = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_http2_padding": obj.Http2Padding = Convert.ToUInt32(propValue, 16); break;
      case "http2_http2_pad_length": obj.Http2PadLength = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_exclusive": obj.Http2Exclusive = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_http2_stream_dependency": obj.Http2StreamDependency = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_data_data": obj.Http2DataData = StringToBytes(propValue); break;
      case "http2_http2_data_padding": obj.Http2DataPadding = StringToBytes(propValue); break;
      case "http2_http2_body_fragments": obj.Http2BodyFragments = default(Int32); break;
      case "http2_http2_body_fragment": obj.Http2BodyFragment = default(Int64); break;
      case "http2_body_fragment_http2_body_fragment_overlap": obj.Http2BodyFragmentOverlap = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_body_fragment_overlap_http2_body_fragment_overlap_conflicts": obj.Http2BodyFragmentOverlapConflicts = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_body_fragment_http2_body_fragment_multiple_tails": obj.Http2BodyFragmentMultipleTails = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_body_fragment_http2_body_fragment_too_long_fragment": obj.Http2BodyFragmentTooLongFragment = Convert.ToInt32(propValue, 10) != 0; break;
      case "http2_body_fragment_http2_body_fragment_error": obj.Http2BodyFragmentError = default(Int64); break;
      case "http2_body_fragment_http2_body_fragment_count": obj.Http2BodyFragmentCount = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_body_reassembled_in": obj.Http2BodyReassembledIn = default(Int64); break;
      case "http2_http2_body_reassembled_length": obj.Http2BodyReassembledLength = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_headers": obj.Http2Headers = StringToBytes(propValue); break;
      case "http2_headers_http2_headers_padding": obj.Http2HeadersPadding = StringToBytes(propValue); break;
      case "http2_http2_header": obj.Http2Header = default(Int32); break;
      case "http2_header_http2_header_length": obj.Http2HeaderLength = Convert.ToUInt32(propValue, 10); break;
      case "http2_header_http2_header_count": obj.Http2HeaderCount = Convert.ToUInt32(propValue, 10); break;
      case "http2_header_name_http2_header_name_length": obj.Http2HeaderNameLength = Convert.ToUInt32(propValue, 10); break;
      case "http2_header_http2_header_name": obj.Http2HeaderName = propValue; break;
      case "http2_header_value_http2_header_value_length": obj.Http2HeaderValueLength = Convert.ToUInt32(propValue, 10); break;
      case "http2_header_http2_header_value": obj.Http2HeaderValue = propValue; break;
      case "http2_header_http2_header_repr": obj.Http2HeaderRepr = propValue; break;
      case "http2_header_http2_header_index": obj.Http2HeaderIndex = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_header_table_size_update": obj.Http2HeaderTableSizeUpdate = default(Int32); break;
      case "http2_header_table_size_update_http2_header_table_size_update_header_table_size": obj.Http2HeaderTableSizeUpdateHeaderTableSize = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_rst_stream_error": obj.Http2RstStreamError = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_settings": obj.Http2Settings = default(Int32); break;
      case "http2_settings_http2_settings_id": obj.Http2SettingsId = Convert.ToUInt32(propValue, 10); break;
      case "http2_settings_http2_settings_header_table_size": obj.Http2SettingsHeaderTableSize = Convert.ToUInt32(propValue, 10); break;
      case "http2_settings_http2_settings_enable_push": obj.Http2SettingsEnablePush = Convert.ToUInt32(propValue, 10); break;
      case "http2_settings_http2_settings_max_concurrent_streams": obj.Http2SettingsMaxConcurrentStreams = Convert.ToUInt32(propValue, 10); break;
      case "http2_settings_http2_settings_initial_window_size": obj.Http2SettingsInitialWindowSize = Convert.ToUInt32(propValue, 10); break;
      case "http2_settings_http2_settings_max_frame_size": obj.Http2SettingsMaxFrameSize = Convert.ToUInt32(propValue, 10); break;
      case "http2_settings_http2_settings_max_header_list_size": obj.Http2SettingsMaxHeaderListSize = Convert.ToUInt32(propValue, 10); break;
      case "http2_settings_http2_settings_unknown": obj.Http2SettingsUnknown = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_push_promise_r": obj.Http2PushPromiseR = Convert.ToUInt32(propValue, 16); break;
      case "http2_http2_push_promise_promised_stream_id": obj.Http2PushPromisePromisedStreamId = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_push_promise_header": obj.Http2PushPromiseHeader = StringToBytes(propValue); break;
      case "http2_http2_push_promise_padding": obj.Http2PushPromisePadding = StringToBytes(propValue); break;
      case "http2_http2_ping": obj.Http2Ping = StringToBytes(propValue); break;
      case "http2_http2_pong": obj.Http2Pong = StringToBytes(propValue); break;
      case "http2_http2_goway_r": obj.Http2GowayR = Convert.ToUInt32(propValue, 16); break;
      case "http2_http2_goaway_last_stream_id": obj.Http2GoawayLastStreamId = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_goaway_error": obj.Http2GoawayError = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_goaway_addata": obj.Http2GoawayAddata = StringToBytes(propValue); break;
      case "http2_http2_window_update_r": obj.Http2WindowUpdateR = Convert.ToUInt32(propValue, 16); break;
      case "http2_http2_window_update_window_size_increment": obj.Http2WindowUpdateWindowSizeIncrement = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_continuation_header": obj.Http2ContinuationHeader = propValue; break;
      case "http2_http2_continuation_padding": obj.Http2ContinuationPadding = StringToBytes(propValue); break;
      case "http2_altsvc_origin_http2_altsvc_origin_len": obj.Http2AltsvcOriginLen = Convert.ToUInt32(propValue, 10); break;
      case "http2_http2_altsvc_origin": obj.Http2AltsvcOrigin = propValue; break;
      case "http2_http2_altsvc_field_value": obj.Http2AltsvcFieldValue = propValue; break;
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

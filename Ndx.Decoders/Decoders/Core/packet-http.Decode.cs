// This is file was generated by netdx on (2017-11-24 12:34:56 PM.
using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Ndx.Decoders.Core
{
  public sealed partial class Http
  {
    public static Http DecodeJson(string jsonLine)
    {
      var jsonObject = JToken.Parse(jsonLine);
      return DecodeJson(jsonObject);
    }
    public static Http DecodeJson(JToken token)
    {
      var obj = new Http();
      {
        var val = token["http_http_notification"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpNotification = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http_http_response"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpResponse = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http_http_request"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpRequest = Convert.ToInt32(propValue, 10) != 0; }
      }
      {
        var val = token["http_http_response_number"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpResponseNumber = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http_http_request_number"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpRequestNumber = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http_http_authbasic"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpAuthbasic = propValue; }
      }
      {
        var val = token["text_http_request_method"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpRequestMethod = propValue; }
      }
      {
        var val = token["text_http_request_version"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpRequestVersion = propValue; }
      }
      {
        var val = token["text_http_response_version"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpResponseVersion = propValue; }
      }
      {
        var val = token["http_http_request_full_uri"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpRequestFullUri = propValue; }
      }
      {
        var val = token["text_http_response_code"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpResponseCode = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["text_http_response_phrase"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpResponsePhrase = propValue; }
      }
      {
        var val = token["http_http_authorization"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpAuthorization = propValue; }
      }
      {
        var val = token["http_http_proxy_authenticate"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpProxyAuthenticate = propValue; }
      }
      {
        var val = token["http_http_proxy_authorization"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpProxyAuthorization = propValue; }
      }
      {
        var val = token["http_http_proxy_connect_host"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpProxyConnectHost = propValue; }
      }
      {
        var val = token["http_http_proxy_connect_port"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpProxyConnectPort = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http_http_www_authenticate"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpWwwAuthenticate = propValue; }
      }
      {
        var val = token["http_http_content_type"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpContentType = propValue; }
      }
      {
        var val = token["http_http_content_length_header"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpContentLengthHeader = propValue; }
      }
      {
        var val = token["http_http_content_length"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpContentLength = Convert.ToUInt64(propValue, 10); }
      }
      {
        var val = token["http_http_content_encoding"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpContentEncoding = propValue; }
      }
      {
        var val = token["http_http_transfer_encoding"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpTransferEncoding = propValue; }
      }
      {
        var val = token["http_http_upgrade"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpUpgrade = propValue; }
      }
      {
        var val = token["http_http_user_agent"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpUserAgent = propValue; }
      }
      {
        var val = token["http_http_host"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpHost = propValue; }
      }
      {
        var val = token["http_http_connection"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpConnection = propValue; }
      }
      {
        var val = token["http_http_cookie"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpCookie = propValue; }
      }
      {
        var val = token["http_http_cookie_pair"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpCookiePair = propValue; }
      }
      {
        var val = token["http_http_accept"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpAccept = propValue; }
      }
      {
        var val = token["http_http_referer"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpReferer = propValue; }
      }
      {
        var val = token["http_http_accept_language"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpAcceptLanguage = propValue; }
      }
      {
        var val = token["http_http_accept_encoding"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpAcceptEncoding = propValue; }
      }
      {
        var val = token["http_http_date"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpDate = propValue; }
      }
      {
        var val = token["http_http_cache_control"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpCacheControl = propValue; }
      }
      {
        var val = token["http_http_server"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpServer = propValue; }
      }
      {
        var val = token["http_http_location"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpLocation = propValue; }
      }
      {
        var val = token["http_http_set_cookie"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpSetCookie = propValue; }
      }
      {
        var val = token["http_http_last_modified"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpLastModified = propValue; }
      }
      {
        var val = token["http_http_x_forwarded_for"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpXForwardedFor = propValue; }
      }
      {
        var val = token["http_http_chunked_trailer_part"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpChunkedTrailerPart = propValue; }
      }
      {
        var val = token["http_http_chunk_boundary"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpChunkBoundary = StringToBytes(propValue); }
      }
      {
        var val = token["http_http_chunk_size"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpChunkSize = Convert.ToUInt32(propValue, 10); }
      }
      {
        var val = token["http_http_file_data"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpFileData = propValue; }
      }
      {
        var val = token["http_http_time"];
        if (val != null) { var propValue = val.Value<string>(); obj.HttpTime = Convert.ToSingle(propValue); }
      }
      return obj;
    }
    public static Http DecodeJson(JsonTextReader reader)                        
    {                                                                                     
        if (reader.TokenType != JsonToken.StartObject) return null;                       
        var obj = new Http();                                                   
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
                    
    static void SetField(Http obj, string propName, string propValue)           
    {                                                                                     
      switch (propName)                                                                   
      {                                                                                   
      case "http_http_notification": obj.HttpNotification = Convert.ToInt32(propValue, 10) != 0; break;
      case "http_http_response": obj.HttpResponse = Convert.ToInt32(propValue, 10) != 0; break;
      case "http_http_request": obj.HttpRequest = Convert.ToInt32(propValue, 10) != 0; break;
      case "http_http_response_number": obj.HttpResponseNumber = Convert.ToUInt32(propValue, 10); break;
      case "http_http_request_number": obj.HttpRequestNumber = Convert.ToUInt32(propValue, 10); break;
      case "http_http_authbasic": obj.HttpAuthbasic = propValue; break;
      case "text_http_request_method": obj.HttpRequestMethod = propValue; break;
      case "text_http_request_version": obj.HttpRequestVersion = propValue; break;
      case "text_http_response_version": obj.HttpResponseVersion = propValue; break;
      case "http_http_request_full_uri": obj.HttpRequestFullUri = propValue; break;
      case "text_http_response_code": obj.HttpResponseCode = Convert.ToUInt32(propValue, 10); break;
      case "text_http_response_phrase": obj.HttpResponsePhrase = propValue; break;
      case "http_http_authorization": obj.HttpAuthorization = propValue; break;
      case "http_http_proxy_authenticate": obj.HttpProxyAuthenticate = propValue; break;
      case "http_http_proxy_authorization": obj.HttpProxyAuthorization = propValue; break;
      case "http_http_proxy_connect_host": obj.HttpProxyConnectHost = propValue; break;
      case "http_http_proxy_connect_port": obj.HttpProxyConnectPort = Convert.ToUInt32(propValue, 10); break;
      case "http_http_www_authenticate": obj.HttpWwwAuthenticate = propValue; break;
      case "http_http_content_type": obj.HttpContentType = propValue; break;
      case "http_http_content_length_header": obj.HttpContentLengthHeader = propValue; break;
      case "http_http_content_length": obj.HttpContentLength = Convert.ToUInt64(propValue, 10); break;
      case "http_http_content_encoding": obj.HttpContentEncoding = propValue; break;
      case "http_http_transfer_encoding": obj.HttpTransferEncoding = propValue; break;
      case "http_http_upgrade": obj.HttpUpgrade = propValue; break;
      case "http_http_user_agent": obj.HttpUserAgent = propValue; break;
      case "http_http_host": obj.HttpHost = propValue; break;
      case "http_http_connection": obj.HttpConnection = propValue; break;
      case "http_http_cookie": obj.HttpCookie = propValue; break;
      case "http_http_cookie_pair": obj.HttpCookiePair = propValue; break;
      case "http_http_accept": obj.HttpAccept = propValue; break;
      case "http_http_referer": obj.HttpReferer = propValue; break;
      case "http_http_accept_language": obj.HttpAcceptLanguage = propValue; break;
      case "http_http_accept_encoding": obj.HttpAcceptEncoding = propValue; break;
      case "http_http_date": obj.HttpDate = propValue; break;
      case "http_http_cache_control": obj.HttpCacheControl = propValue; break;
      case "http_http_server": obj.HttpServer = propValue; break;
      case "http_http_location": obj.HttpLocation = propValue; break;
      case "http_http_set_cookie": obj.HttpSetCookie = propValue; break;
      case "http_http_last_modified": obj.HttpLastModified = propValue; break;
      case "http_http_x_forwarded_for": obj.HttpXForwardedFor = propValue; break;
      case "http_http_chunked_trailer_part": obj.HttpChunkedTrailerPart = propValue; break;
      case "http_http_chunk_boundary": obj.HttpChunkBoundary = StringToBytes(propValue); break;
      case "http_http_chunk_size": obj.HttpChunkSize = Convert.ToUInt32(propValue, 10); break;
      case "http_http_file_data": obj.HttpFileData = propValue; break;
      case "http_http_time": obj.HttpTime = Convert.ToSingle(propValue); break;
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

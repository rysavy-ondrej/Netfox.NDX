using Newtonsoft.Json.Linq;
using Google.Protobuf;
using System;
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
        if (val != null) obj.HttpNotification = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http_http_response"];
        if (val != null) obj.HttpResponse = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http_http_request"];
        if (val != null) obj.HttpRequest = Convert.ToInt32(val.Value<string>(), 10) != 0;
      }
      {
        var val = token["http_http_response_number"];
        if (val != null) obj.HttpResponseNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http_http_request_number"];
        if (val != null) obj.HttpRequestNumber = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http_http_authbasic"];
        if (val != null) obj.HttpAuthbasic = val.Value<string>();
      }
      {
        var val = token["text_http_request_method"];
        if (val != null) obj.HttpRequestMethod = val.Value<string>();
      }
      {
        var val = token["text_http_request_version"];
        if (val != null) obj.HttpRequestVersion = val.Value<string>();
      }
      {
        var val = token["text_http_response_version"];
        if (val != null) obj.HttpResponseVersion = val.Value<string>();
      }
      {
        var val = token["http_http_request_full_uri"];
        if (val != null) obj.HttpRequestFullUri = val.Value<string>();
      }
      {
        var val = token["text_http_response_code"];
        if (val != null) obj.HttpResponseCode = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["text_http_response_phrase"];
        if (val != null) obj.HttpResponsePhrase = val.Value<string>();
      }
      {
        var val = token["http_http_authorization"];
        if (val != null) obj.HttpAuthorization = val.Value<string>();
      }
      {
        var val = token["http_http_proxy_authenticate"];
        if (val != null) obj.HttpProxyAuthenticate = val.Value<string>();
      }
      {
        var val = token["http_http_proxy_authorization"];
        if (val != null) obj.HttpProxyAuthorization = val.Value<string>();
      }
      {
        var val = token["http_http_proxy_connect_host"];
        if (val != null) obj.HttpProxyConnectHost = val.Value<string>();
      }
      {
        var val = token["http_http_proxy_connect_port"];
        if (val != null) obj.HttpProxyConnectPort = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http_http_www_authenticate"];
        if (val != null) obj.HttpWwwAuthenticate = val.Value<string>();
      }
      {
        var val = token["http_http_content_type"];
        if (val != null) obj.HttpContentType = val.Value<string>();
      }
      {
        var val = token["http_http_content_length_header"];
        if (val != null) obj.HttpContentLengthHeader = val.Value<string>();
      }
      {
        var val = token["http_http_content_length"];
        if (val != null) obj.HttpContentLength = Convert.ToUInt64(val.Value<string>(), 10);
      }
      {
        var val = token["http_http_content_encoding"];
        if (val != null) obj.HttpContentEncoding = val.Value<string>();
      }
      {
        var val = token["http_http_transfer_encoding"];
        if (val != null) obj.HttpTransferEncoding = val.Value<string>();
      }
      {
        var val = token["http_http_upgrade"];
        if (val != null) obj.HttpUpgrade = val.Value<string>();
      }
      {
        var val = token["http_http_user_agent"];
        if (val != null) obj.HttpUserAgent = val.Value<string>();
      }
      {
        var val = token["http_http_host"];
        if (val != null) obj.HttpHost = val.Value<string>();
      }
      {
        var val = token["http_http_connection"];
        if (val != null) obj.HttpConnection = val.Value<string>();
      }
      {
        var val = token["http_http_cookie"];
        if (val != null) obj.HttpCookie = val.Value<string>();
      }
      {
        var val = token["http_http_cookie_pair"];
        if (val != null) obj.HttpCookiePair = val.Value<string>();
      }
      {
        var val = token["http_http_accept"];
        if (val != null) obj.HttpAccept = val.Value<string>();
      }
      {
        var val = token["http_http_referer"];
        if (val != null) obj.HttpReferer = val.Value<string>();
      }
      {
        var val = token["http_http_accept_language"];
        if (val != null) obj.HttpAcceptLanguage = val.Value<string>();
      }
      {
        var val = token["http_http_accept_encoding"];
        if (val != null) obj.HttpAcceptEncoding = val.Value<string>();
      }
      {
        var val = token["http_http_date"];
        if (val != null) obj.HttpDate = val.Value<string>();
      }
      {
        var val = token["http_http_cache_control"];
        if (val != null) obj.HttpCacheControl = val.Value<string>();
      }
      {
        var val = token["http_http_server"];
        if (val != null) obj.HttpServer = val.Value<string>();
      }
      {
        var val = token["http_http_location"];
        if (val != null) obj.HttpLocation = val.Value<string>();
      }
      {
        var val = token["http_http_set_cookie"];
        if (val != null) obj.HttpSetCookie = val.Value<string>();
      }
      {
        var val = token["http_http_last_modified"];
        if (val != null) obj.HttpLastModified = val.Value<string>();
      }
      {
        var val = token["http_http_x_forwarded_for"];
        if (val != null) obj.HttpXForwardedFor = val.Value<string>();
      }
      {
        var val = token["http_http_chunked_trailer_part"];
        if (val != null) obj.HttpChunkedTrailerPart = val.Value<string>();
      }
      {
        var val = token["http_http_chunk_boundary"];
        if (val != null) obj.HttpChunkBoundary = StringToBytes(val.Value<string>());
      }
      {
        var val = token["http_http_chunk_size"];
        if (val != null) obj.HttpChunkSize = Convert.ToUInt32(val.Value<string>(), 10);
      }
      {
        var val = token["http_http_file_data"];
        if (val != null) obj.HttpFileData = val.Value<string>();
      }
      {
        var val = token["http_http_time"];
        if (val != null) obj.HttpTime = Convert.ToSingle(val.Value<string>());
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

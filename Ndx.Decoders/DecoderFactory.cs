using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Ndx.Decoders
{
    /// <summary>
    /// This class helps to create protocols. It registers available decoders using reflection.
    /// </summary>
    public class DecoderFactory
    {
        private static Dictionary<string, Func<JToken, object>> m_tokenDecoders;
        private static Dictionary<string, Func<JsonTextReader, object>> m_readerDecoders;

        public Dictionary<string, Func<JToken, object>> TokenDecoders => m_tokenDecoders;
        public Dictionary<string, Func<JsonTextReader, object>> ReaderDecoders => m_readerDecoders;
        static void RegisterDecoders()
        {
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            m_tokenDecoders = new Dictionary<string, Func<JToken, object>>();
            m_readerDecoders = new Dictionary<string, Func<JsonTextReader, object>>();
            foreach (var typ in allTypes)
            {
                var method = typ.GetMethod("DecodeJson", new Type[] { typeof(JToken) });
                if (method == null) continue;
                var input = Expression.Parameter(typeof(JToken));
                var lambda = Expression.Lambda<Func<JToken, object>>(Expression.Call(method, input), input).Compile();
                m_tokenDecoders.Add(typ.Name.ToLowerInvariant(), lambda);
            }
            foreach (var typ in allTypes)
            {
                var method = typ.GetMethod("DecodeJson", new Type[] { typeof(JsonTextReader) });
                if (method == null) continue;
                var input = Expression.Parameter(typeof(JsonTextReader));
                var lambda = Expression.Lambda<Func<JsonTextReader, object>>(Expression.Call(method, input), input).Compile();
                m_readerDecoders.Add(typ.Name.ToLowerInvariant(), lambda);
            }
        }
        /// <summary>
        /// Creates a new <see cref="DecoderFactory"/> and performs decoder registration if necessary.
        /// </summary>
        public DecoderFactory()
        {
            if (m_readerDecoders == null) RegisterDecoders();
        }

        /// <summary>
        /// Decodes the required protocol from the input JSON.
        /// </summary>
        /// <param name="protocol">Protocol name.</param>
        /// <param name="token">Root token of the input JSON.</param>
        /// <returns>An object that can be type casted to the required protocol.
        /// If either <paramref name="protocol"/> or <paramref name="token"/> is null
        /// then the result is null.
        /// </returns>
        public object DecodeProtocol(String protocol, JToken token)
        {
            if (String.IsNullOrEmpty(protocol)) return null;
            if (token == null) return null;

            if (m_tokenDecoders.TryGetValue(protocol.ToLowerInvariant(), out var decoder))
            {
                return decoder(token);
            }
            return null;
        }

        /// <summary>
        /// Decodes the required protocol from the input JSON. The input should start with StartObject.
        /// </summary>
        /// <param name="protocolName">Protocol name.</param>
        /// <param name="token">Root token of the input JSON.</param>
        /// <returns>An object that can be type casted to the required protocol.
        /// If either <paramref name="protocolName"/> or <paramref name="token"/> is null
        /// then the result is null.
        /// </returns>
        public object DecodeProtocol(String protocolName, JsonTextReader reader, bool consumeObjectForUndefinedProtocol = true)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (protocolName == null) throw new ArgumentNullException(nameof(protocolName));

            if (m_readerDecoders.TryGetValue(protocolName.ToLowerInvariant(), out var decoder))
            {                                                           
                return decoder(reader);
            }
            else if (consumeObjectForUndefinedProtocol)
            {
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
                    reader.Read();
                }
                reader.Read();
            }
            return null;
        }

        /// <summary>
        /// Decodes the protocol of the class provided as the type parameter.
        /// </summary>
        /// <typeparam name="T">The class of representing the protocol.</typeparam>
        /// <param name="token">Root token of the input JSON.</param>
        /// <returns>An object representing the decoded protocol.</returns>
        public T DecodeProtocol<T>(JToken token)
        {
            return (T)DecodeProtocol(typeof(T).Name, token);
        }
        public T DecodeProtocol<T>(JsonTextReader reader)
        {
            return (T)DecodeProtocol(typeof(T).Name, reader);
        }
    }
}

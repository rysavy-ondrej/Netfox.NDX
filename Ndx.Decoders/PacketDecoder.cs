using Ndx.Captures;
using Ndx.Decoders.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace Ndx.Decoders
{
    /// <summary>
    /// The class implementes decoding JSON input to <see cref="Packet"/>.
    /// </summary>
    public class PacketDecoder
    {
        static Dictionary<Type, Action<Packet.Types.Protocol, object>> m_setters;

        /// <summary>
        /// Creates a new <see cref="PacketDecoder"/> object.
        /// </summary>
        public PacketDecoder()
        {
            if (m_setters == null) InitializeSetters();
        }

        bool ReadProperty(JsonTextReader reader, out string propName, out string propValue)
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                propName = reader.Value as String;
                propValue = reader.ReadAsString();
                reader.Read();
                return true;
            }
            propName = null;
            propValue = null;
            return true;
        }

        T ReadProtocol<T>(DecoderFactory factory, JsonTextReader reader)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                return factory.DecodeProtocol<T>(reader);    
            }
            else
            {
                return default(T);
            }
        }
        bool ConsumeStartObject(JsonTextReader reader)
        { 
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                return true;
            }
            return false;
        }
        bool ConsumeEndObject(JsonTextReader reader)
        {
            if (reader.TokenType == JsonToken.EndObject)
            {
                reader.Read();
                return true;
            }
            return false;
        }
        bool ConsumePropertyName(JsonTextReader reader, string name)
        {
            if (reader.TokenType == JsonToken.PropertyName && (name.Equals((string)reader.Value)))
            {
                reader.Read();
                return true;
            }
            return false;
        }
        string ConsumePropertyName(JsonTextReader reader)
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var propName = (string)reader.Value;
                reader.Read();
                return propName;
            }
            return null;
        }

        /// <summary>
        /// Decodes input JSON string  to <see cref="Packet"/> object.
        /// </summary>
        /// <param name="factory">The decoder factory object.</param>
        /// <param name="input">The JSON input string.</param>
        /// <returns><see cref="Packet"/> object for the given input JSON string.</returns>
        public Packet Decode(DecoderFactory factory, string input)
        {
            var packet = new Packet();
            using (var reader = new JsonTextReader(new StringReader(input)))
            {
                // None
                reader.Read();
                // {
                ConsumeStartObject(reader); 
                // "timestamp" : "1508164622563",
                ReadProperty(reader, out var tsName, out var tsValue);
                packet.TimeStamp = Convert.ToInt64(tsValue);
                // layers:
                ConsumePropertyName(reader, "layers");
                // {
                ConsumeStartObject(reader);
                // frame
                ConsumePropertyName(reader, "frame");
                // { ... } 
                var frame = Frame.DecodeJson(reader);
                packet.Protocols.Add(new Packet.Types.Protocol() { Frame = frame });

                while (reader.TokenType != JsonToken.EndObject)
                {
                    var protoName = ConsumePropertyName(reader);
                    var protoObj = factory.DecodeProtocol(protoName, reader);
                    if (protoObj != null) packet.Protocols.Add(CreateProtocol(protoObj));
                }
                ConsumeEndObject(reader);
                ConsumeEndObject(reader);
                System.Diagnostics.Debug.Assert(reader.TokenType == JsonToken.None, $"Unexpected TokenType {reader.TokenType}");
            }
            return packet;
        }

        /// <summary>
        /// Decodes the input JSON representation of a packet using the provided decoder factory.
        /// </summary>
        /// <param name="factory">The decoder factory.</param>
        /// <param name="input">JSON input.</param>
        /// <returns>New <see cref="Packet"/> instance for the provided input.</returns>
        public Packet Decode(DecoderFactory factory, JsonPacket input)
        {
            var packet = new Packet()
            {
                TimeStamp = input.Timestamp
            };

            foreach(var protocol in input.Protocols)
            {
                var protocolObject = factory.DecodeProtocol(protocol, input.GetProtocol(protocol));
                if (protocolObject == null) continue;

                var protocolWrapper = CreateProtocol(protocolObject);
                if (protocolWrapper == null) continue;
                
                packet.Protocols.Add(protocolWrapper);                
            }
            return packet;
        }

        /// <summary>
        /// Initializes Setter dictionary by using reflection on the current assembly.
        /// </summary>
        static void InitializeSetters()
        {
            m_setters = new Dictionary<Type, Action<Packet.Types.Protocol, object>>();
            var t = typeof(Packet.Types.Protocol);
            var setProperties = t.GetProperties();
            foreach(var setProp in setProperties)
            {
                if (setProp.SetMethod == null) continue;                
                var setterType = setProp.PropertyType;
                var arg1 = Expression.Parameter(typeof(Packet.Types.Protocol));
                var arg2 = Expression.Parameter(typeof(object));

                var lambda = Expression.Lambda<Action<Packet.Types.Protocol, object>>(
                        Expression.Call(arg1, setProp.SetMethod, Expression.TypeAs(arg2, setterType)), arg1, arg2                    
                    ).Compile();

                m_setters[setterType] = lambda;
            }
        }

        /// <summary>
        /// Creates a new protocol for the given input.
        /// </summary>
        /// <typeparam name="T">The protocol type.</typeparam>
        /// <param name="input">An input object.</param>
        /// <returns>Packet wrapped in <see cref="Packet.Types.Protocol"/> object that cen be used for discrimination.</returns>
        public Packet.Types.Protocol CreateProtocol<T>(T input) where T : class
        {
            if (input == null) return null;
            if (typeof(T) == typeof(object))
            {
                if (m_setters.TryGetValue(input.GetType(), out var setter))
                {
                    var result = new Packet.Types.Protocol();
                    setter(result, input);
                    return result;
                }
            }
            else
            {
                if (m_setters.TryGetValue(typeof(T), out var setter))
                {
                    var result = new Packet.Types.Protocol();
                    setter(result, input);
                    return result;
                }
            }
            return null;
        }
    }
}

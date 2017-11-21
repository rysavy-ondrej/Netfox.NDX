using Ndx.Captures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Decoders
{
    /// <summary>
    /// The class implementes decoding JSON input to <see cref="Packet"/>.
    /// </summary>
    public class PacketDecoder
    {
        static Dictionary<Type, Action<Packet.Types.Protocol, object>> m_setters;

        public PacketDecoder()
        {
            if (m_setters == null) InitializeSetters();
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

        public static void InitializeSetters()
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
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public Packet.Types.Protocol CreateProtocol<T>(T input)
        {            
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Decoders
{
    public partial class Packet
    {


        /// <summary>
        /// Gets the instance of protocol class specified by type <typeparamref name="T"/> or null.
        /// </summary>
        /// <typeparam name="T">The class of the protocol.</typeparam>
        /// <returns>object for protocol specified by type <typeparamref name="T"/> or null.</returns>
        public T Protocol<T>() where T: class
        {
            if (Enum.TryParse<Types.Protocol.ProtocolTypeOneofCase>(typeof(T).Name, out var pt))
            {
                var p = this.protocols_.FirstOrDefault(x => x.ProtocolTypeCase == pt);
                return p?.GetAs<T>();
            }
            else
            {
                return null;
            }
        }

        public bool HasProtocol(string protocol)
        {
            if (Enum.TryParse<Types.Protocol.ProtocolTypeOneofCase>(protocol, out var pt))
            {
                return HasProtocol(pt);
            }
            return false;
        }

        public bool HasProtocol(Types.Protocol.ProtocolTypeOneofCase protocol)
        {
            return this.protocols_.Any(x => x.ProtocolTypeCase == protocol);
        }

        public static partial class Types
        {
            public sealed partial class Protocol
            {
                /// <summary>
                /// Returns the value of Protocol object as type <typeparamref name="T"/>.
                /// </summary>
                /// <typeparam name="T">The class of the protocol.</typeparam>
                /// <returns>the value of Protocol object as type <typeparamref name="T"/> or null.</returns>
                internal T GetAs<T>() where T : class
                {
                    return this.protocolType_ as T;
                }
            }
        }
    }
}

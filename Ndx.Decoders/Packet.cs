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
            var p = this.protocols_.FirstOrDefault(x => typeof(T).Name.Equals(x.ProtocolTypeCase.ToString(), StringComparison.InvariantCultureIgnoreCase));
            return p?.GetAs<T>();
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

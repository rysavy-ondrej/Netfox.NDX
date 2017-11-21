using Ndx.Decoders.Basic;
using Ndx.Decoders.Core;
using Newtonsoft.Json.Linq;
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
    /// This class helps to create protocols. It registers available decoders using reflection.
    /// </summary>
    public class DecoderFactory
    {
        private static Dictionary<string, Func<JToken, object>> m_decoders;

        public Dictionary<string, Func<JToken, object>> Decoders => m_decoders;

        static void RegisterDecoders()
        {
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            m_decoders = new Dictionary<string, Func<JToken, object>>();
            foreach (var typ in allTypes)
            {
                var method = typ.GetMethod(nameof(Arp.DecodeJson), new Type[] { typeof(JToken) });
                if (method == null) continue;
                var input = Expression.Parameter(typeof(JToken));
                var lambda = Expression.Lambda<Func<JToken, object>>(Expression.Call(method, input), input).Compile();
                m_decoders.Add(typ.Name.ToLowerInvariant(), lambda);               
            }
        }
        /// <summary>
        /// Creates a new <see cref="DecoderFactory"/> and performs decoder registration if necessary.
        /// </summary>
        public DecoderFactory()
        {
            if (m_decoders == null) RegisterDecoders();
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

            if (m_decoders.TryGetValue(protocol.ToLowerInvariant(), out var decoder))
            {
                return decoder(token);
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
    }
}

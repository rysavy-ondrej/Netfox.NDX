using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;

namespace Ndx.Diagnostics
{

    public class Context 
    {
        private Rule m_rule;
        private IDictionary<string,PacketFields> m_objects;
        private IEnumerable<PacketFields> m_input;
        public Context(Rule rule, IEnumerable<PacketFields> input, IDictionary<string,PacketFields> objects)
        {
            m_rule = rule;
            m_input = input;
            m_objects = objects;
        }

        public PacketFields this[string name]
        {
            get
            {
                if (m_objects.TryGetValue(name, out PacketFields value))
                {
                    return value;
                }
                else
                {
                    return PacketFields.Empty;
                }
            }
        }

        public Rule Rule { get => m_rule;  }
        public IEnumerable<PacketFields> Input { get => m_input; set => m_input = value; }

        /// <summary>
        /// Represents left [from-to]~> right temporal relation.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool LeadsTo(TimeSpan from, TimeSpan to, PacketFields left, PacketFields right)
        {
            return
                !right.IsEmpty
                && left.DateTime + from <= right.DateTime
                && right.DateTime <= left.DateTime + to;
        }

        /// <summary>
        /// Represents left [from-to]~> !right temporal relation.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool LeadsToFalse(TimeSpan from, TimeSpan to, PacketFields left, PacketFields right)
        {
            return !left.IsEmpty && right.IsEmpty;
        }
    }

    /// <summary>
    /// This is the base class for all rules.
    /// </summary>
    public class Rule
    {
        public string Id { get; set;  }
        public string Description { get; set; }
        public IDictionary<string,Func<Context,IEnumerable<PacketFields>>> Events { get; }
        public IList<Func<Context, bool>> Assert { get; }
        public Rule()
        {
            Events = new Dictionary<string, Func<Context, IEnumerable<PacketFields>>>();
            Assert = new List<Func<Context, bool>>();
        }

        public IEnumerable<T> Evaluate<T>(IEnumerable<PacketFields> input, IDictionary<string, PacketFields> args, Func<Context, T> select)
        {
            var context = new Context(this, input, args);
            var evts = this.Events.SelectMany(p => p.Value(context), Tuple.Create).ToLookup(p => p.Item1.Key, p => p.Item2);
            var seq = LinqTemplate(evts);
            return seq.Where(evt =>
            {
                return Assert.All(f => f(new Context(this, null, evt)));
            }).Select(evt => select(new Context(this, null, evt)));
        }



        private static IEnumerable<Dictionary<string, PacketFields>> LinqTemplate(ILookup<string, PacketFields> events)
        {
            IEnumerable<Dictionary<string, PacketFields>> seed = new[] { new Dictionary<string, PacketFields> { { "__init", PacketFields.Empty } } };

            return events.Aggregate(seed: seed, func: (accumulator, valueTuple) =>
            {
                var eventName = valueTuple.Key;
                var eventEnumerable = valueTuple.DefaultIfEmpty(PacketFields.Empty)
                    .Select(packetFields => new KeyValuePair<string, PacketFields>(eventName, packetFields)).ToList();

                var result = accumulator.SelectMany(
                    (dictionary) => eventEnumerable,
                    (dictionary, valuePair) => new Dictionary<string, PacketFields>(dictionary) {{valuePair.Key, valuePair.Value}}).ToList();
                return result;
            });
         }
    }
}

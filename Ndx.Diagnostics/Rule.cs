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
            if (left.IsEmpty) return true;
            return
                !right.IsEmpty
                && left.DateTime + from <= right.DateTime
                && right.DateTime <= left.DateTime + to;
        }
        public bool NotLeadsTo(TimeSpan from, TimeSpan to, PacketFields left, PacketFields right)
        {
            if (left.IsEmpty) return true;
            if (right.IsEmpty) return true;
            return (left.DateTime + from <= right.DateTime
                   && right.DateTime <= left.DateTime + to) == false;
        }

    }

    /// <summary>
    /// This is class represents a diagnostic rule template.
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

        /// <summary>
        /// Evaluates the rule for the provided input and arguments.
        /// </summary>
        /// <typeparam name="T">The type of result.</typeparam>
        /// <param name="input">The input stream of events.</param>
        /// <param name="args">Arguments of the rule.</param>
        /// <param name="select">Selector function to produce result records.</param>
        /// <returns>Stream of resulting events.</returns>
        public IEnumerable<T> Evaluate<T>(IEnumerable<PacketFields> input, IDictionary<string, PacketFields> args, Func<Context, T> select)
        {
            var context = new Context(this, input, args);
            var evts = this.Events.SelectMany(p => p.Value(context), Tuple.Create).ToLookup(p => p.Item1.Key, p => p.Item2);
            var seq = LinqTemplate(evts);
            return seq.Where(evt => Assert.All(f => f(new Context(this, null, evt)))).                
                Select(evt => select(new Context(this, null, evt)));
        }

        /// <summary>
        /// Computes cross join for the provided events. 
        /// </summary>
        /// <param name="events">Lookup that represents all input events.</param>
        /// <returns>Rows of the cross join suitable for further processing using where and select operations.</returns>
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
                    (dictionary, valuePair) => new Dictionary<string, PacketFields>(dictionary) {{valuePair.Key, valuePair.Value}});
                return result;
            });
         }
    }
}

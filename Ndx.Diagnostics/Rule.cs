using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace Ndx.Diagnostics
{

    /// <summary>
    /// This is class represents a diagnostic rule template.
    /// </summary>
    public class Rule
    {
        private  AssertPredicateExpression[] m_asserts;
        private  Dictionary<string, DisplayFilterExpression> m_events;
        /// <summary>
        /// Gets or sets the rule identifier.
        /// </summary>
        public string Id { get; set;  }
        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the array representing named parameters of the rule.
        /// </summary>
        public string[] Params { get; private set; }

        /// <summary>
        /// Creates an empty rule. 
        /// </summary>
        public Rule()
        {
        }

        /// <summary>
        /// Evaluates the rule for the provided input and arguments.
        /// </summary>
        /// <typeparam name="T">The type of result.</typeparam>
        /// <param name="input">The input stream of events.</param>
        /// <param name="args">Arguments of the rule.</param>
        /// <param name="select">Selector function to produce result records.</param>
        /// <returns>Stream of resulting events.</returns>
        public IEnumerable<T> Evaluate<T>(IEnumerable<PacketFields> input, IDictionary<string, PacketFields> args, Func<PacketFields[],T> selector)
        {
            // convert argumenst to array:
            var argValues = this.Params.Select(x => args[x]).ToArray();
            var assertArguments = Params.Concat(m_events.Keys).ToArray();
            IEnumerable<PacketFields> GetEvent(DisplayFilterExpression eventFilter)
            {
                return input.Where(pf => eventFilter.FilterFunction.Invoke(pf) == true);
            }


            IEnumerable<PacketFields[]> CrossJoin1(IEnumerable<PacketFields> e1)
            {
                return e1.Select(_e1 => argValues.Concat(new PacketFields[] { _e1 }).ToArray());
            }

            IEnumerable<PacketFields[]> CrossJoin2(IEnumerable<PacketFields> e1, IEnumerable<PacketFields> e2)
            {
                var result = e1.SelectMany(_e1 => e2, (x1, x2) => argValues.Concat(new PacketFields[] { x1, x2 }).ToArray());
                return result;
            }
            IEnumerable<PacketFields[]> CrossJoin3(IEnumerable<PacketFields> e1, IEnumerable<PacketFields> e2, IEnumerable<PacketFields> e3)
            {
                var r2 = e2.SelectMany(_e2 => e3, (x2,x3) => (x2,x3));
                var r1 = e1.SelectMany(_e1 => r2, (x1,rx) => (x1,rx.x2,rx.x3)); 
                var result = r1.Select( (t) => argValues.Concat(new PacketFields[] { t.x1, t.x2, t.x3 }).ToArray());
                return result;
            }
            IEnumerable<PacketFields[]> CrossJoin4(IEnumerable<PacketFields> e1, IEnumerable<PacketFields> e2, IEnumerable<PacketFields> e3, IEnumerable<PacketFields> e4)
            {
                var r3 = e3.SelectMany(_e3 => e4, (x3, x4) => (x3, x4));
                var r2 = e2.SelectMany(_e2 => r3, (x2, rx) => (x2, rx.x3, rx.x4));
                var r1 = e1.SelectMany(_e1 => r2, (x1, rx) => (x1, rx.x2, rx.x3, rx.x4));
                var result = r1.Select((t) => argValues.Concat(new PacketFields[] { t.x1, t.x2, t.x3 }).ToArray());
                return result;
            }
            IEnumerable<PacketFields[]> CrossJoin(params IEnumerable<PacketFields>[] es)
            {
                switch(es.Length)
                {
                    case 1: return CrossJoin1(es[0]);
                    case 2: return CrossJoin2(es[0], es[1]);
                    case 3: return CrossJoin3(es[0], es[1], es[2]);
                    case 4: return CrossJoin4(es[0], es[1], es[2],es[3]);
                    default: return null;
                }
            }

            var events = m_events.Select(x=> GetEvent(x.Value).DefaultIfEmpty()).ToArray();
            var sequence = CrossJoin(events);
            return sequence.Where(evt => m_asserts.All(f => f.FlowFilter(evt) == true)).                
                Select(evt => selector(evt));
        }


        /// <summary>
        /// Loads the rule from YAML specification.
        /// </summary>
        /// <param name="yamlDocument"></param>
        /// <returns></returns>
        public static Rule Load(string yamlDocument)
        {
            var input = new StringReader(yamlDocument);
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var yamlRule   = (YamlMappingNode)mapping.Children[new YamlScalarNode("rule")];
            var yamlParams = (YamlSequenceNode)mapping.Children[new YamlScalarNode("params")];
            var yamlEvents = (YamlMappingNode)mapping.Children[new YamlScalarNode("events")];
            var yamlAssert = (YamlSequenceNode)mapping.Children[new YamlScalarNode("assert")];
            var yamlSelect = (YamlMappingNode)mapping.Children[new YamlScalarNode("select")];

            var rule = new Rule();
            rule.Id = ((YamlScalarNode)yamlRule.Children[new YamlScalarNode("id")])?.Value ?? String.Empty;
            rule.Description = ((YamlScalarNode)yamlRule.Children[new YamlScalarNode("description")])?.Value ?? String.Empty;
            rule.Params = yamlParams?.Select(x => ((YamlScalarNode)x).Value).ToArray() ?? new String[] { };
            var events = yamlEvents.Children.Keys.Select(k => ((YamlScalarNode)k).Value).ToArray();
            var assertArguments = rule.Params.Concat(events).ToArray();
            // assert:
            rule.m_asserts = yamlAssert.Select(x => ((YamlScalarNode)x).Value).Select(x => AssertPredicateExpression.Parse(x,assertArguments)).ToArray();
            rule.m_events = yamlEvents.Select(x => (name: ((YamlScalarNode)x.Key).Value, events: ((YamlScalarNode)x.Value).Value)).Select(x => (name: x.name, events: DisplayFilterExpression.Parse(x.events))).ToDictionary(x=> x.name, x => x.events );

            return rule;
        }
    }
}

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
        private List<AssertPredicateExpression> m_asserts;
        private Dictionary<string, DisplayFilterExpression> m_events;
        private string[] m_parameterNames;
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
        public string[] Parameters { get => m_parameterNames;  }
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
        /// <param name="ruleArguments">Arguments of the rule.</param>
        /// <param name="select">Selector function to produce result records.</param>
        /// <returns>Stream of resulting events.</returns>
        public IEnumerable<T> Evaluate<T>(IEnumerable<DecodedFrame> input, IDictionary<string, DecodedFrame> ruleArguments, Func<DecodedFrame[],T> selector)
        {
            // convert argumenst to array:
            var ruleArgumentValues = this.Parameters.Select(x => ruleArguments[x]).ToArray();
            var assertArgumentNames = Parameters.Concat(m_events.Keys).ToArray();

            IEnumerable<DecodedFrame> GetEvent(DisplayFilterExpression eventFilter)
            {
                return input.Where(pf => eventFilter.FilterFunction(pf)?.ToBoolean() ?? false).ToList();
            }

            DecodedFrame[] GetArray(params DecodedFrame[] inputFields)
            {
                var argumentValuesArrayLength = ruleArgumentValues.Length;
                var outArray = new DecodedFrame[argumentValuesArrayLength + inputFields.Length];
                for(int i = 0; i < ruleArgumentValues.Length; i++)
                {
                    outArray[i] = ruleArgumentValues[i];
                }
                for (int j = 0; j < inputFields.Length; j++)
                {
                    outArray[argumentValuesArrayLength + j] = inputFields[j];
                }
                return outArray;
            }

            IEnumerable<DecodedFrame[]> CrossJoin1(IEnumerable<DecodedFrame> e1)
            {
                return e1.Select(_e1 => GetArray(_e1));
            }

            IEnumerable<DecodedFrame[]> CrossJoin2(IEnumerable<DecodedFrame> e1, IEnumerable<DecodedFrame> e2)
            {
                var result = e1.SelectMany(_e1 => e2, (x1, x2) => GetArray(x1,x2));
                return result;
            }
            IEnumerable<DecodedFrame[]> CrossJoin3(IEnumerable<DecodedFrame> e1, IEnumerable<DecodedFrame> e2, IEnumerable<DecodedFrame> e3)
            {
                var r2 = e2.SelectMany(_e2 => e3, (x2,x3) => (x2,x3));
                var r1 = e1.SelectMany(_e1 => r2, (x1,rx) => (x1,rx.x2,rx.x3)); 
                var result = r1.Select( (t) => GetArray(t.x1, t.x2, t.x3));
                return result;
            }
            IEnumerable<DecodedFrame[]> CrossJoin4(IEnumerable<DecodedFrame> e1, IEnumerable<DecodedFrame> e2, IEnumerable<DecodedFrame> e3, IEnumerable<DecodedFrame> e4)
            {
                var r3 = e3.SelectMany(_e3 => e4, (x3, x4) => (x3, x4));
                var r2 = e2.SelectMany(_e2 => r3, (x2, rx) => (x2, rx.x3, rx.x4));
                var r1 = e1.SelectMany(_e1 => r2, (x1, rx) => (x1, rx.x2, rx.x3, rx.x4));
                var result = r1.Select((t) => GetArray(t.x1, t.x2, t.x3, t.x4));
                return result;
            }
            IEnumerable<DecodedFrame[]> CrossJoin(params IEnumerable<DecodedFrame>[] es)
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

            var events = m_events.Select(x => GetEvent(x.Value).DefaultIfEmpty()).ToArray();

            var sequence = CrossJoin(events);

            return sequence.Where(evt => m_asserts.All(f => f.FlowFilter(evt)?.ToBoolean() ?? false)).Select(evt => selector(evt));
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
            var yamlResult = (YamlScalarNode)yamlRule.Children[new YamlScalarNode("result")];

            var eventNames = yamlEvents.Children.Keys.Select(k => ((YamlScalarNode)k).Value).ToArray();
            var paramNames = yamlParams?.Select(x => ((YamlScalarNode)x).Value).ToArray() ?? new String[] { };
            var assertArgumentNames = paramNames.Concat(eventNames).ToArray();
            var rule = new Rule
            {
                Id = ((YamlScalarNode)yamlRule.Children[new YamlScalarNode("id")])?.Value ?? String.Empty,
                Description = ((YamlScalarNode)yamlRule.Children[new YamlScalarNode("description")])?.Value ?? String.Empty,
                m_parameterNames = paramNames,
                m_events = yamlEvents.Select(x => (name: ((YamlScalarNode)x.Key).Value, events: ((YamlScalarNode)x.Value).Value)).Select(x => (name: x.name, events: DisplayFilterExpression.Parse(x.events))).ToDictionary(x => x.name, x => x.events),
                m_asserts = yamlAssert.Select(x => ((YamlScalarNode)x).Value).Select(x => AssertPredicateExpression.Parse(x, assertArgumentNames)).ToList()
            };
            return rule;
        }
    }
}

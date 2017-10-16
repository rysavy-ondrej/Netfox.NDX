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
        /// <summary>
        /// Gets or sets the rule identifier.
        /// </summary>
        public string Id { get; set;  }
        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets the EVENT selector functions.
        /// </summary>
        public IDictionary<string,Func<Context,IEnumerable<PacketFields>>> Events { get; }
        /// <summary>
        /// Gets the collection of ASSERT expressions.
        /// </summary>
        public IList<Func<Context, bool>> Assert { get; }
        /// <summary>
        /// Gets the array representing named parameters of the rule.
        /// </summary>
        public string[] Params { get; private set; }

        /// <summary>
        /// Creates an empty rule. 
        /// </summary>
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

            var events = yamlEvents.Children.Keys.Select(k => ((YamlScalarNode)k).Value);
            // assert:
            var asserts = yamlAssert.Select(x => ((YamlScalarNode)x).Value).Select(x => AssertPredicateExpression.Parse(x, events.ToArray())).ToArray();
            

            return rule;
        }
    }
}

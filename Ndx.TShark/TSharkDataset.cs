using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Ndx.Utils;
using Newtonsoft.Json.Linq;

namespace Ndx.TShark
{

    public class FieldMap
    {

        public FieldMap()
        {
            m_fieldMap = new Dictionary<string, NVariant>();
        }

        public FieldMap(Dictionary<string, JValue> dict)
        {
            m_fieldMap = dict.ToDictionary(x => x.Key, x => NVariant.Make(x.Value));
        }

        private Dictionary<string, NVariant> m_fieldMap;

        // Define the indexer to allow client code to use [] notation.
        public NVariant this[string name]
        {
            get
            {
                if (m_fieldMap.TryGetValue(name, out NVariant value))
                {
                    return value;
                }
                else
                {
                    return NVariant.Empty;
                }
            }
        }
    }
    /// <summary>
    /// This class provides functions for processing TShark output in the form of line separated JSON.
    /// </summary>
    public class TSharkDataset
    {
        public static FieldMap ParseFields(string line)
        {
            JValue GetPropertyValue(JToken token)
            {
                if (token.Type == JTokenType.Boolean
                   || token.Type == JTokenType.Bytes
                   || token.Type == JTokenType.Date
                   || token.Type == JTokenType.Float
                   || token.Type == JTokenType.Guid
                   || token.Type == JTokenType.Integer
                   || token.Type == JTokenType.String
                   || token.Type == JTokenType.TimeSpan
                   || token.Type == JTokenType.Uri
                   )
                {
                    return token.Value<JValue>();
                }
                else
                    return new JValue(token.ToString());
            }
            var json = JToken.Parse(line);

            var dict = json["layers"].Children<JProperty>().SelectMany(x => x.Value.ToArray()).ToDictionary(y => ((JProperty)y).Name, y => GetPropertyValue(((JProperty)y).Value));
            dict.Add("timestamp", json["timestamp"].Value<JValue>());
            return new FieldMap(dict);
        }

        public static IEnumerable<FieldMap> LoadEventsFromFile(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            return lines.Where(x => x.StartsWith(@"{""timestamp""")).Select(ParseFields);
        }
    }
}

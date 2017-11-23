using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Decoders
{
    /// <summary>
    /// Implements custom deserializer that allows duplicate property names in the same object.
    /// </summary>
    /// <see cref="https://stackoverflow.com/questions/20714160/how-to-deserialize-json-with-duplicate-property-names-in-the-same-object"/>
    class CustomJsonDeserializer
    {
        public static JToken DeserializeAndCombineDuplicates(JsonTextReader reader)
        {
            if (reader.TokenType == JsonToken.None)
            {
                reader.Read();
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                JObject obj = new JObject();
                while (reader.TokenType != JsonToken.EndObject)
                {
                    string propName = (string)reader.Value;
                    reader.Read();
                    JToken newValue = DeserializeAndCombineDuplicates(reader);

                    JToken existingValue = obj[propName];
                    if (existingValue == null)
                    {
                        obj.Add(new JProperty(propName, newValue));
                    }
                    else if (existingValue.Type == JTokenType.Array)
                    {
                        CombineWithArray((JArray)existingValue, newValue);
                    }
                    else // Convert existing non-array property value to an array
                    {
                        JProperty prop = (JProperty)existingValue.Parent;
                        JArray array = new JArray();
                        prop.Value = array;
                        array.Add(existingValue);
                        CombineWithArray(array, newValue);
                    }

                    reader.Read();
                }
                return obj;
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();
                JArray array = new JArray();
                while (reader.TokenType != JsonToken.EndArray)
                {
                    array.Add(DeserializeAndCombineDuplicates(reader));
                    reader.Read();
                }
                return array;
            }

            return new JValue(reader.Value);
        }

        public static JToken Parse(string jsonLine)
        {
            using (var reader = new JsonTextReader(new StringReader(jsonLine)))
            {
                return DeserializeAndCombineDuplicates(reader);    
            }
        }

        static void CombineWithArray(JArray array, JToken value)
        {
            if (value.Type == JTokenType.Array)
            {
                foreach (JToken child in value.Children())
                    array.Add(child);
            }
            else
            {
                array.Add(value);
            }
        }
    }
}

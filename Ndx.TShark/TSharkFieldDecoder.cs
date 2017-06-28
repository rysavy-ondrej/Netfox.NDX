﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Newtonsoft.Json.Linq;

namespace Ndx.TShark
{
    /// <summary>
    /// This class represents TSHARK decoder that accepts a collection of fields to extract.
    /// </summary>
    public class TSharkFieldDecoder : TSharkProcess
    {
        private List<string> m_fields;
        /// <summary>
        /// Gets a collection of fields that is to be exported by TSHARK.
        /// </summary>
        public IList<string> Fields { get => m_fields; }


        public TSharkFieldDecoder() : base()
        {
            m_fields = new List<string>();
        }

        public TSharkFieldDecoder(string pipeName) : base(pipeName)
        {
            m_fields = new List<string>();
        }

        public TSharkFieldDecoder(IEnumerable<string> fields) : base()
        {
            m_fields = new List<string>(fields);
        }

        public TSharkFieldDecoder(string pipeName, IEnumerable<string> fields) : base(pipeName)
        {
            m_fields = new List<string>(fields);
        }

        protected override string GetOutputFilter()
        {
            var fields = String.Join(" ", m_fields.Select(x => $"-e {x}"));
            var exportedFields = $"-e { Field.Frame.Number} -e { Field.Frame.Protocols} { fields}";
            return exportedFields;
        }

        /// <summary>
        /// Gets the <see cref="PacketFields"/> object for the result line generated by the TSHARK process.
        /// </summary>
        /// <param name="line">Result line generated by the associated TSHARK process.</param>
        /// <returns><see cref="PacketFields"/> object for the result line generated by the TSHARK process.</returns>
        protected override PacketFields GetResult(string line)
        {
            var jsonObject = JToken.Parse(line);
            var fields = jsonObject["layers"].ToDictionary(y => ((JProperty)y).Name, y => ((JProperty)y).Value);
            var result = new PacketFields()
            {
                Timestamp = (long)jsonObject["timestamp"]
            };
            foreach (var field in fields)
            {
                switch (field.Key)
                {
                    case "frame_number":
                        result.FrameNumber = (int)field.Value.First;
                        break;
                    case "frame_protocols":
                        result.FrameProtocols = (string)field.Value.First;
                        break;
                    default:
                        result.Fields.Add(field.Key, (string)field.Value.First);
                        break;
                }
            }
            return result;
        }
    }
}

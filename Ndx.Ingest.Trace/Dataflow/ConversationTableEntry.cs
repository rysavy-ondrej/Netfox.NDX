using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Ingest.Trace
{
    public class ConversationTableEntry
    {
        int m_id;
        private FlowKey m_originatorKey;
        private FlowKey m_responderKey;

        public ConversationTableEntry(int id, FlowKey originatorKey, FlowKey responderKey)
        {
            m_id = id;
            m_originatorKey = originatorKey;
            m_responderKey = responderKey;
        }

        public int Id { get => m_id; set => m_id = value; }
        public FlowKey OriginatorKey { get => m_originatorKey; set => m_originatorKey = value; }
        public FlowKey ResponderKey { get => m_responderKey; set => m_responderKey = value; }


        public static IBinaryConverter<ConversationTableEntry> Converter = new BinaryConverter();

        class BinaryConverter : IBinaryConverter<ConversationTableEntry>
        {
            public ConversationTableEntry ReadObject(BinaryReader reader)
            {
                var id = reader.ReadInt32();
                var originatorKey = FlowKey.Converter.ReadObject(reader);
                var responderKey = FlowKey.Converter.ReadObject(reader);
                return new ConversationTableEntry(id, originatorKey, responderKey);
            }

            public void WriteObject(BinaryWriter writer, ConversationTableEntry value)
            {
                writer.Write(value.m_id);
                FlowKey.Converter.WriteObject(writer, value.m_originatorKey);
                FlowKey.Converter.WriteObject(writer, value.m_responderKey);
            }
        }
    }
}

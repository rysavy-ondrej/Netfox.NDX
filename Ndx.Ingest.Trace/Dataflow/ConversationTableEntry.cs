using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Metacap
{
    public class ConversationTableEntry
    {
        internal static readonly int __size = sizeof(int)+sizeof(int)+sizeof(int)+_FlowKey.__size+_FlowKey.__size;

        int m_id;
        private int m_originatorFlowId;
        private int m_responderFlowId;
        private FlowKey m_originatorKey;
        private FlowKey m_responderKey;

        public ConversationTableEntry(int id, int originatorId, int responderId, FlowKey originatorKey, FlowKey responderKey)
        {
            m_id = id;
            m_originatorFlowId = originatorId;
            m_responderFlowId = responderId;
            m_originatorKey = originatorKey;
            m_responderKey = responderKey;
        }

        public ConversationTableEntry(byte[] buffer)
        {
            m_id = BitConverter.ToInt32(buffer, 0);
            m_originatorFlowId = BitConverter.ToInt32(buffer, sizeof(int));
            m_responderFlowId = BitConverter.ToInt32(buffer, sizeof(int) + sizeof(int));
            m_originatorKey = new FlowKey(buffer, sizeof(int) + sizeof(int)+sizeof(int));
            m_responderKey = new FlowKey(buffer, sizeof(int) + sizeof(int) + sizeof(int)+_FlowKey.__size);
        }

        public int Id { get => m_id; set => m_id = value; }
        public FlowKey OriginatorKey { get => m_originatorKey; set => m_originatorKey = value; }
        public FlowKey ResponderKey { get => m_responderKey; set => m_responderKey = value; }
        public int OriginatorFlowId { get => m_originatorFlowId; set => m_originatorFlowId = value; }
        public int ResponderFlowId { get => m_responderFlowId; set => m_responderFlowId = value; }

        public static IBinaryConverter<ConversationTableEntry> Converter = new BinaryConverter();

        class BinaryConverter : IBinaryConverter<ConversationTableEntry>
        {
            public ConversationTableEntry ReadObject(BinaryReader reader)
            {
                var id = reader.ReadInt32();
                var originatorId = reader.ReadInt32();
                var responderId = reader.ReadInt32();

                var originatorKey = FlowKey.Converter.ReadObject(reader);
                var responderKey = FlowKey.Converter.ReadObject(reader);
                return new ConversationTableEntry(id, originatorId, responderId, originatorKey, responderKey);
            }

            public void WriteObject(BinaryWriter writer, ConversationTableEntry value)
            {
                writer.Write(value.m_id);
                writer.Write(value.m_originatorFlowId);
                writer.Write(value.m_responderFlowId);
                FlowKey.Converter.WriteObject(writer, value.m_originatorKey);
                FlowKey.Converter.WriteObject(writer, value.m_responderKey);
            }
        }
    }
}

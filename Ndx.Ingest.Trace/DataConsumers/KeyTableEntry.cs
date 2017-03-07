using System;
using System.Collections.Generic;
using System.IO;

namespace Ndx.Ingest.Trace
{
        public class KeyTableEntry
        {
            private FlowKey m_key;
            private IndexRecord m_indexRecord;

            

            public KeyTableEntry(FlowKey key, IndexRecord value)
            {
                this.m_key = key;
                this.m_indexRecord = value;
            }


        public FlowKey Key => m_key;
        public IndexRecord IndexRecord => m_indexRecord;

        class BinaryConverter : IBinaryConverter<KeyTableEntry>
        {
            public bool CanRead => true;

            public bool CanWrite => true;

            public KeyTableEntry ReadObject(BinaryReader reader)
            {
                var key = FlowKey.Converter.ReadObject(reader);
                if (key == null) return null;
                var value = IndexRecord.Converter.ReadObject(reader);
                if (value == null) return null;
                return new KeyTableEntry(key, value);
            }

            public void WriteObject(BinaryWriter writer, KeyTableEntry entry)
            {
                FlowKey.Converter.WriteObject(writer, entry.m_key);
                IndexRecord.Converter.WriteObject(writer, entry.m_indexRecord);
            }
        }

            public static IBinaryConverter<KeyTableEntry> Converter = new BinaryConverter();

        }

    public unsafe class IndexRecord
    {
        public int FlowRecordOffset;
        public List<int> PacketBlockList;
        public IndexRecord()
        {
            PacketBlockList = new List<int>();
        }
        public byte[] GetBytes()
        {
            var buffer = new byte[sizeof(int) * (PacketBlockList.Count + 1)];
            fixed (byte* ptr = buffer)
            {
                int* intPtr = (int*)ptr;
                intPtr[0] = FlowRecordOffset;
                for (int i = 0; i < PacketBlockList.Count; i++)
                {
                    intPtr[i + 1] = PacketBlockList[i];
                }
            }
            return buffer;
        }
        public static IndexRecord FromBytes(byte[] bytes)
        {
            var obj = new IndexRecord();
            fixed (byte* ptr = bytes)
            {
                int* intPtr = (int*)ptr;
                obj.FlowRecordOffset = intPtr[0];
                for (int i = 1; i < bytes.Length / sizeof(int); i++)
                {
                    obj.PacketBlockList.Add(intPtr[i]);
                }
            }
            return obj;
        }

        class BinaryConverter : IBinaryConverter<IndexRecord>
        {
            public bool CanRead => true;

            public bool CanWrite => true;

            public IndexRecord ReadObject(BinaryReader reader)
            {
                var len = reader.ReadInt32();
                var bytes = reader.ReadBytes(len);
                if (bytes.Length < len)
                    return null;
                else
                    return IndexRecord.FromBytes(bytes);
            }

            public void WriteObject(BinaryWriter writer, IndexRecord value)
            {
                // size of struct
                var bytes = value.GetBytes();
                writer.Write(bytes.Length);
                writer.Write(bytes);
            }
        }


        public static IBinaryConverter<IndexRecord> Converter = new BinaryConverter();
    }
}

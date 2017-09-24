using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Ndx.Utils
{
    public static class ZipStorage
    {
        /// <summary>
        /// Reads dictionary from the ZipArchive Entry.
        /// </summary>
        /// <typeparam name="ObjectType"></typeparam>
        /// <typeparam name="KeyType"></typeparam>
        /// <typeparam name="ValueType"></typeparam>
        /// <param name="archive"></param>
        /// <param name="path"></param>
        /// <param name="table"></param>
        /// <param name="parser"></param>
        /// <param name="getKey"></param>
        /// <param name="getValue"></param>
        public static void MergeFrom<ObjectType, KeyType, ValueType>(this ZipArchive archive, string path, IDictionary<KeyType, ValueType> table, MessageParser<ObjectType> parser, Func<ObjectType, KeyType> getKey, Func<ObjectType, ValueType> getValue)
            where ObjectType : Google.Protobuf.IMessage<ObjectType>
        {
            var tableEntry = archive.GetEntry(path);
            using (var tableStream = tableEntry.Open())
            {
                while (true)
                {
                    var item = parser.ParseDelimitedFrom(tableStream);
                    if (item != null)
                    {
                        table[getKey(item)] = getValue(item);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public static void WriteTo<KeyType, ValueType, ObjectType>(this ZipArchive archive, string path, IDictionary<KeyType, ValueType> table, Func<KeyValuePair<KeyType, ValueType>, ObjectType> getObject)
            where ObjectType : Google.Protobuf.IMessage<ObjectType>
        {
            var tableEntry = archive.CreateEntry(path, CompressionLevel.Fastest);
            using (var tableStream = tableEntry.Open())
            {
                foreach (var item in table)
                {
                    getObject(item).WriteDelimitedTo(tableStream);
                }
            }
        }
    }
}

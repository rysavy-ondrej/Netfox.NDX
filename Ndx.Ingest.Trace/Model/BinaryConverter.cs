using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Ingest.Trace
{
    public interface IBinaryConverter<T>
    {
        bool CanRead { get; }
        bool CanWrite { get; }
        /// <summary>
        /// Reads object using the provided reader. 
        /// </summary>
        /// <param name="reader"><see cref="BinaryReader"/> used to read object.</param>
        /// <returns>An instance of <see cref="T"/> or <c><![CDATA[default{T}]]>]]></c> when end-of-file was reached.</returns>
        T ReadObject(BinaryReader reader);
        void WriteObject(BinaryWriter writer, T value);
    }
}

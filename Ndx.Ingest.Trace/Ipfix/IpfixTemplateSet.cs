using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;
using PacketDotNet.Utils;

namespace Ndx.Ipfix
{
    /// <summary>
    /// Template set is a collection of template records.  
    /// </summary>
    public class IpfixTemplateSet : IpfixSet
    {
        /// <summary>
        /// Identification of the set. This should be 2 for template set.
        /// </summary>
        ushort m_setId;

        /// <summary>
        /// The length of the set in octets.
        /// </summary>
        ushort m_length;

        /// <summary>
        /// Collection of Template records.
        /// </summary>
        IpfixTemplateRecord[] m_records;

        /// <summary>
        /// Provides access to field specifiers of the current template record.
        /// </summary>
        /// <param name="index">Index.</param>
        public IpfixTemplateRecord this[int index]
        {
            get
            {
                if (index > m_records.Length) throw new IndexOutOfRangeException();
                return m_records[index];
            }
        }
    }

    /// <summary>
    /// Ipfix field specifier class. It may contain also information elements as specified by 
    /// http://www.iana.org/assignments/ipfix/.
    /// </summary>
    public class IpfixFieldSpecifier
    {
        ushort m_fieldId;
        ushort m_fieldLength;

        /// <summary>
        /// The field identifier. It corresponds to information elemtn id as defined by IANA for IPFIX.
        /// </summary>
        public ushort Id { get => m_fieldId; set => m_fieldId = value; }

        /// <summary>
        /// Gets or sets the length of the field.
        /// </summary>
        public ushort Length { get => m_fieldLength; set => m_fieldLength = value; }
    }
    /// <summary>
    /// IPFIX Template Record  
    /// </summary>
    public class IpfixTemplateRecord
    {
        /// <summary>
        /// Each Template Record is given a unique Template ID in the range 256 to 65535.
        /// </summary>
        protected uint m_templateId;
        /// <summary>
        ///  Number of fields in this Template Record.
        /// </summary>
        protected uint m_fieldCount;
        /// <summary>
        /// An array of field specifiers. Each field specifier defines 
        /// field id and field length.
        /// </summary>
        protected IpfixFieldSpecifier[] m_fields;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ndx.Ingest.Trace.IpfixTemplateRecord"/> class.
        /// This also initializes a field array.
        /// </summary>
        public IpfixTemplateRecord(uint templateId, uint fieldCount)
        {
            m_templateId = templateId;
            m_fieldCount = fieldCount;
            m_fields = new IpfixFieldSpecifier[fieldCount];

            for (int i = 0; i < m_fields.Length; i++)
            {
                m_fields[i] = new IpfixFieldSpecifier();
            }
        }

        public IpfixTemplateRecord(uint templateId, IpfixFieldSpecifier[] fields)
        {
            m_templateId = templateId;
            m_fieldCount = (uint)fields.Length;
            m_fields = new IpfixFieldSpecifier[m_fieldCount];

            for (int i = 0; i < m_fields.Length; i++)
            {
                m_fields[i] = fields[i];
            }
        }

        /// <summary>
        /// Provides access to field specifiers of the current template record.
        /// </summary>
        /// <param name="index">Index.</param>
        public IpfixFieldSpecifier this[int index]
        {
            get
            {
                if (index > m_fields.Length) throw new IndexOutOfRangeException();
                return m_fields[index];
            }
        }
        /// <summary>
        /// The identifier of the template.
        /// </summary>
        /// <value>The identifier.</value>
        public uint Id { get => m_templateId; }

        /// <summary>
        /// The number of fields defined by this template.
        /// </summary>
        public int Count => m_fields.Length;

        /// <summary>
        /// Compiles the current template into an object that 
        /// provides a fast access to individual fields of data records.
        /// </summary>
        /// <returns>The compiled template record providing fast access to data fields.</returns>
        public CompiledTemplate Compile()
        {
            return new CompiledTemplate(this);
        }


        /// <summary>
        /// Loads the template from JSON.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public IpfixTemplateRecord Load(Stream stream)
        {
            using (var reader = new StreamReader(stream)) 
            using (var jreader = new JsonTextReader(reader))
            {
                var fieldsList = new List<IpfixFieldSpecifier>();
                var jsonObject = Newtonsoft.Json.Linq.JObject.Load(jreader);
                var properties = jsonObject.Properties().ToDictionary(x => x.Name);
                var templateId = (uint)properties["templateId"].Value;
                var fields = properties["fields"].Value;
                foreach(var field in fields)
                {
                    var fieldSpec = new IpfixFieldSpecifier();
                    var fieldIdToken = field.SelectToken("id");
                    if (fieldIdToken.Type == Newtonsoft.Json.Linq.JTokenType.Integer)
                    {                                                 
                        fieldSpec.Id = (ushort)fieldIdToken;
                    }
                    else
                    {
                        fieldSpec.Id = IpfixInfoElements.GetFieldId((string)fieldIdToken);
                    }
                    var fieldLenToken = field.SelectToken("len");
                    if(fieldLenToken.Type == Newtonsoft.Json.Linq.JTokenType.Integer)
                    {
                        fieldSpec.Length = (ushort)fieldLenToken;
                    }
                    else
                    {
                        fieldSpec.Length = IpfixInfoElements.GetFieldLength((string)fieldLenToken);
                    }
                    fieldsList.Add(fieldSpec);
                }
                return new IpfixTemplateRecord(templateId, fieldsList.ToArray());
            }
        }

        static public IpfixTemplateRecord Parse(string content)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            using (var stream = new MemoryStream(byteArray))
            {
                return Load(stream);
            }
        }


        public static IpfixTemplateRecord ConversationIpv4
        {
            get
            {
                using(var stream = new MemoryStream(Ndx.Ingest.Trace.Resource.ConversationIpv4))
                {
                    return Load(stream);
                }
            }
        }

        public static IpfixTemplateRecord FlowIpv4
        {
            get
            {
                using (var stream = new MemoryStream(Ndx.Ingest.Trace.Resource.FlowIpv4))
                {
                    return Load(stream);
                }
            }
        }
        public static IpfixTemplateRecord ConversationIpv6
        {
            get
            {
                using (var stream = new MemoryStream(Ndx.Ingest.Trace.Resource.ConversationIpv6))
                {
                    return Load(stream);
                }
            }
        }
        public static IpfixTemplateRecord FlowIpv6
        {
            get
            {
                using (var stream = new MemoryStream(Ndx.Ingest.Trace.Resource.FlowIpv6))
                {
                    return Load(stream);
                }
            }
        }
        public static IpfixTemplateRecord PacketMeta
        {
            get
            {
                using (var stream = new MemoryStream(Ndx.Ingest.Trace.Resource.PacketMeta))
                {
                    return Load(stream);
                }
            }
        }

        /// <summary>
        /// Compiled template provides a fast access to data fields. Use <seealso cref="CompiledTemplate.GetFieldBytes"/>
        /// to read access to data fields.
        /// </summary>
        public class CompiledTemplate
        {
            /// <summary>
            /// Gets the field bytes for the specified field in the data record. The method expect that only bytes of the record are provided. 
            /// </summary>
            /// <returns>The field bytes.</returns>
            /// <param name="bytes">Bytes that represent the content of data record.</param>
            /// <param name="fieldId">Field identifier.</param>
            public ByteArraySegment GetBytes(ByteArraySegment bytes, ushort fieldId)
            {
                return m_accessMethod(bytes, fieldId);
            }

            public UInt16 GetUInt16(ByteArraySegment bytes, ushort fieldId)
            {
                var arr = GetBytes(bytes, fieldId);
                return MiscUtil.Conversion.EndianBitConverter.Big.ToUInt16(arr.Bytes, arr.Offset);
            }

            public UInt32 GetUInt32(ByteArraySegment bytes, ushort fieldId)
            {
                var arr = GetBytes(bytes, fieldId);
                return MiscUtil.Conversion.EndianBitConverter.Big.ToUInt32(arr.Bytes, arr.Offset);
            }

            public UInt64 GetUInt64(ByteArraySegment bytes, ushort fieldId)
            {
                var arr = GetBytes(bytes, fieldId);
                return MiscUtil.Conversion.EndianBitConverter.Big.ToUInt64(arr.Bytes, arr.Offset);
            }

            public Int16 GetInt16(ByteArraySegment bytes, ushort fieldId)
            {
                var arr = GetBytes(bytes, fieldId);
                return MiscUtil.Conversion.EndianBitConverter.Big.ToInt16(arr.Bytes, arr.Offset);
            }

            public Int32 GetInt32(ByteArraySegment bytes, ushort fieldId)
            {
                var arr = GetBytes(bytes, fieldId);
                return MiscUtil.Conversion.EndianBitConverter.Big.ToInt32(arr.Bytes, arr.Offset);
            }

            public Int64 GetInt64(ByteArraySegment bytes, ushort fieldId)
            {
                var arr = GetBytes(bytes, fieldId);
                return MiscUtil.Conversion.EndianBitConverter.Big.ToInt64(arr.Bytes, arr.Offset);
            }

            /// <summary>
            /// The total length of data record.
            /// </summary>
            int m_recordLength;
            /// <summary>
            /// The access method 
            /// </summary>
            AccessMethod m_accessMethod;

            public int RecordLength { get => m_recordLength; }

            public CompiledTemplate(IpfixTemplateRecord template)
            {
                m_recordLength = template.m_fields.Sum(x => x.Length);
                CreateAccessDelegate(template);
            }


            /// <summary>
            /// Creates an empty array that can accomodate the new record for this template.
            /// </summary>
            /// <returns></returns>
            public byte[] NewRecord()
            {
                return new byte[m_recordLength];
            }

            public static ByteArraySegment GetByteRange(ByteArraySegment bas, int offset, int length)
            {
                return new ByteArraySegment(bas.Bytes, bas.Offset + offset, length);
            }

            delegate ByteArraySegment AccessMethod(ByteArraySegment bas, ushort fieldId);
            /// <summary>
            /// Creates a function that implements switch for fast access to field.
            /// </summary>
            /// <param name="template">The template record for the generated accessor.</param>
            private void CreateAccessDelegate(IpfixTemplateRecord template)
            {
                try
                {
                    var fieldIdParameter = Expression.Parameter(typeof(ushort), "fieldId");
                    var byteArrayParameter = Expression.Parameter(typeof(ByteArraySegment), "bytes");
                    var resultByteArray = Expression.Parameter(typeof(ByteArraySegment), "result");

                    //LabelTarget label = Expression.Label(typeof(int));

                    var caseList = new List<SwitchCase>();
                    var fieldOffset = 0;
                    for (var i = 0; i < template.m_fields.Length; i++)
                    {
                        var fieldId = template.m_fields[i].Id;
                        var fieldLen = (int)template.m_fields[i].Length;
                        var caseBodyExpr = Expression.Call(instance: null,
                                                           method: typeof(CompiledTemplate).GetMethod("GetByteRange"),
                                                           arg0: byteArrayParameter,
                                                           arg1: Expression.Constant(fieldOffset, typeof(int)),
                                                           arg2: Expression.Constant(fieldLen, typeof(int)));


                        var caseExpr = Expression.SwitchCase(body: Expression.Assign(resultByteArray, caseBodyExpr),
                                                             testValues: new[] { Expression.Constant(fieldId, typeof(ushort)) });


                        caseList.Add(caseExpr);
                        fieldOffset += fieldLen;
                    }

                    var switchExpr = Expression.Switch(type: typeof(void),
                                                       switchValue: fieldIdParameter,
                                                       defaultBody: Expression.Assign(resultByteArray, Expression.Constant(null, typeof(ByteArraySegment))),
                                                       comparison: null,
                                                       cases: caseList.ToArray());

                    LabelTarget returnTarget = Expression.Label(typeof(ByteArraySegment));
                    var block = Expression.Block(
                                        new[] { resultByteArray },
                                        Expression.Assign(resultByteArray, Expression.Constant(null, typeof(ByteArraySegment))),
                                        switchExpr,
                                        //Expression.Return(returnTarget, resultByteArray),
                                        Expression.Label(returnTarget, resultByteArray));

                    var lambdaFun = Expression.Lambda<AccessMethod>(body: block,
                                  tailCall: false,
                                  parameters: new[] { byteArrayParameter, fieldIdParameter });

                    m_accessMethod = lambdaFun.Compile();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}

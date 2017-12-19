// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using System;
using System.Collections.Generic;
using Kaitai;

namespace Ndx.Decoders.Binary
{
    public partial class Snmp : KaitaiStruct
    {
        public static Snmp FromFile(string fileName)
        {
            return new Snmp(new KaitaiStream(fileName));
        }

        public enum TypeTag
        {
            EndOfContent = 0,
            Boolean = 1,
            Integer = 2,
            BitString = 3,
            OctetString = 4,
            NullValue = 5,
            ObjectId = 6,
            ObjectDescriptor = 7,
            External = 8,
            Real = 9,
            Enumerated = 10,
            EmbeddedPdv = 11,
            Utf8string = 12,
            RelativeOid = 13,
            Sequence10 = 16,
            PrintableString = 19,
            Ia5string = 22,
            Sequence30 = 48,
            Set = 49,
            SnmpPduGet = 160,
            SnmpPduGetnext = 161,
            SnmpPduResponse = 162,
            SnmpPduSet = 163,
            SnmpPduTrapv1 = 164,
            SnmpPduTrapv2 = 167,
        }

        public enum SnmpPduType
        {
            SnmpPduGet = 0,
            SnmpPduGetnext = 1,
            SnmpPduResponse = 2,
            SnmpPduSet = 3,
            SnmpPduTrapv1 = 4,
            SnmpPduTrapv2 = 7,
        }

        public enum SnmpErrorStatus
        {
            NoError = 0,
            TooBig = 1,
            NoSuchName = 2,
            BadValue = 3,
            ReadOnly = 4,
            GenErr = 5,
        }

        public Snmp(KaitaiStream io, KaitaiStruct parent = null, Snmp root = null) : base(io)
        {
            m_parent = parent;
            m_root = root ?? this;
            _parse();
        }

        private void _parse()
        {
            _hdr = new Asn1Hdr(m_io, this, m_root);
            _version = new Asn1Obj(m_io, this, m_root);
            _community = new Asn1Obj(m_io, this, m_root);
            _pduType = new Asn1Hdr(m_io, this, m_root);
            switch (PduType.Tag) {
            case TypeTag.SnmpPduTrapv2: {
                _data = new Trap2(m_io, this, m_root);
                break;
            }
            case TypeTag.SnmpPduSet: {
                _data = new SetRequest(m_io, this, m_root);
                break;
            }
            case TypeTag.SnmpPduGet: {
                _data = new GetRequest(m_io, this, m_root);
                break;
            }
            case TypeTag.SnmpPduResponse: {
                _data = new Response(m_io, this, m_root);
                break;
            }
            case TypeTag.SnmpPduGetnext: {
                _data = new GetNextRequest(m_io, this, m_root);
                break;
            }
            case TypeTag.SnmpPduTrapv1: {
                _data = new Trap1(m_io, this, m_root);
                break;
            }
            }
        }
        public partial class ErrorStatus : KaitaiStruct
        {
            public static ErrorStatus FromFile(string fileName)
            {
                return new ErrorStatus(new KaitaiStream(fileName));
            }

            public ErrorStatus(KaitaiStream io, Pdu parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                f_code = false;
                _hdr = new Asn1Hdr(m_io, this, m_root);
                __raw_val = m_io.ReadBytes(Hdr.Len.Result);
                var io___raw_val = new KaitaiStream(__raw_val);
                _val = new BodyInteger(io___raw_val, this, m_root);
            }
            private bool f_code;
            private SnmpErrorStatus _code;
            public SnmpErrorStatus Code
            {
                get
                {
                    if (f_code)
                        return _code;
                    _code = (SnmpErrorStatus) (((Snmp.SnmpErrorStatus) Val.Value));
                    f_code = true;
                    return _code;
                }
            }
            private Asn1Hdr _hdr;
            private BodyInteger _val;
            private Snmp m_root;
            private Snmp.Pdu m_parent;
            private byte[] __raw_val;
            public Asn1Hdr Hdr { get { return _hdr; } }
            public BodyInteger Val { get { return _val; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp.Pdu M_Parent { get { return m_parent; } }
            public byte[] M_RawVal { get { return __raw_val; } }
        }
        public partial class VariableBindings : KaitaiStruct
        {
            public static VariableBindings FromFile(string fileName)
            {
                return new VariableBindings(new KaitaiStream(fileName));
            }

            public VariableBindings(KaitaiStream io, Pdu parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _seqTypeTag = m_io.EnsureFixedContents(new byte[] { 48 });
                _len = new LenEncoded(m_io, this, m_root);
                _entries = new List<VariableBinding>();
                while (!m_io.IsEof) {
                    _entries.Add(new VariableBinding(m_io, this, m_root));
                }
            }
            private byte[] _seqTypeTag;
            private LenEncoded _len;
            private List<VariableBinding> _entries;
            private Snmp m_root;
            private Snmp.Pdu m_parent;
            public byte[] SeqTypeTag { get { return _seqTypeTag; } }
            public LenEncoded Len { get { return _len; } }
            public List<VariableBinding> Entries { get { return _entries; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp.Pdu M_Parent { get { return m_parent; } }
        }
        public partial class BodySequence : KaitaiStruct
        {
            public static BodySequence FromFile(string fileName)
            {
                return new BodySequence(new KaitaiStream(fileName));
            }

            public BodySequence(KaitaiStream io, Asn1Obj parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _entries = new List<Asn1Obj>();
                while (!m_io.IsEof) {
                    _entries.Add(new Asn1Obj(m_io, this, m_root));
                }
            }
            private List<Asn1Obj> _entries;
            private Snmp m_root;
            private Snmp.Asn1Obj m_parent;
            public List<Asn1Obj> Entries { get { return _entries; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp.Asn1Obj M_Parent { get { return m_parent; } }
        }
        public partial class Trap1 : KaitaiStruct
        {
            public static Trap1 FromFile(string fileName)
            {
                return new Trap1(new KaitaiStream(fileName));
            }

            public Trap1(KaitaiStream io, Snmp parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _items = new List<Asn1Obj>();
                while (!m_io.IsEof) {
                    _items.Add(new Asn1Obj(m_io, this, m_root));
                }
            }
            private List<Asn1Obj> _items;
            private Snmp m_root;
            private Snmp m_parent;
            public List<Asn1Obj> Items { get { return _items; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp M_Parent { get { return m_parent; } }
        }
        public partial class BodyUtf8string : KaitaiStruct
        {
            public static BodyUtf8string FromFile(string fileName)
            {
                return new BodyUtf8string(new KaitaiStream(fileName));
            }

            public BodyUtf8string(KaitaiStream io, Asn1Obj parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _value = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesFull());
            }
            private string _value;
            private Snmp m_root;
            private Snmp.Asn1Obj m_parent;
            public string Value { get { return _value; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp.Asn1Obj M_Parent { get { return m_parent; } }
        }
        public partial class GetRequest : KaitaiStruct
        {
            public static GetRequest FromFile(string fileName)
            {
                return new GetRequest(new KaitaiStream(fileName));
            }

            public GetRequest(KaitaiStream io, Snmp parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _pdu = new Pdu(m_io, this, m_root);
            }
            private Pdu _pdu;
            private Snmp m_root;
            private Snmp m_parent;
            public Pdu Pdu { get { return _pdu; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp M_Parent { get { return m_parent; } }
        }
        public partial class Trap2 : KaitaiStruct
        {
            public static Trap2 FromFile(string fileName)
            {
                return new Trap2(new KaitaiStream(fileName));
            }

            public Trap2(KaitaiStream io, Snmp parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _requestId = new Pdu(m_io, this, m_root);
            }
            private Pdu _requestId;
            private Snmp m_root;
            private Snmp m_parent;
            public Pdu RequestId { get { return _requestId; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp M_Parent { get { return m_parent; } }
        }
        public partial class Asn1Hdr : KaitaiStruct
        {
            public static Asn1Hdr FromFile(string fileName)
            {
                return new Asn1Hdr(new KaitaiStream(fileName));
            }

            public Asn1Hdr(KaitaiStream io, KaitaiStruct parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _tag = ((Snmp.TypeTag) m_io.ReadU1());
                _len = new LenEncoded(m_io, this, m_root);
            }
            private TypeTag _tag;
            private LenEncoded _len;
            private Snmp m_root;
            private KaitaiStruct m_parent;
            public TypeTag Tag { get { return _tag; } }
            public LenEncoded Len { get { return _len; } }
            public Snmp M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class VariableBinding : KaitaiStruct
        {
            public static VariableBinding FromFile(string fileName)
            {
                return new VariableBinding(new KaitaiStream(fileName));
            }

            public VariableBinding(KaitaiStream io, VariableBindings parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _seqTypeTag = m_io.EnsureFixedContents(new byte[] { 48 });
                _len = new LenEncoded(m_io, this, m_root);
                _name = new Asn1Obj(m_io, this, m_root);
                _value = new Asn1Obj(m_io, this, m_root);
            }
            private byte[] _seqTypeTag;
            private LenEncoded _len;
            private Asn1Obj _name;
            private Asn1Obj _value;
            private Snmp m_root;
            private Snmp.VariableBindings m_parent;
            public byte[] SeqTypeTag { get { return _seqTypeTag; } }
            public LenEncoded Len { get { return _len; } }
            public Asn1Obj Name { get { return _name; } }
            public Asn1Obj Value { get { return _value; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp.VariableBindings M_Parent { get { return m_parent; } }
        }
        public partial class BodyInteger : KaitaiStruct
        {
            public static BodyInteger FromFile(string fileName)
            {
                return new BodyInteger(new KaitaiStream(fileName));
            }

            public BodyInteger(KaitaiStream io, KaitaiStruct parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                f_value = false;
                _bytes = new List<byte>();
                while (!m_io.IsEof) {
                    _bytes.Add(m_io.ReadU1());
                }
            }
            private bool f_value;
            private int _value;

            /// <summary>
            /// Resulting value as normal integer
            /// </summary>
            public int Value
            {
                get
                {
                    if (f_value)
                        return _value;
                    _value = (int) ((((((((Bytes[(Bytes.Count - 1)] + ((Bytes.Count - 1) >= 2 ? (Bytes[((Bytes.Count - 1) - 1)] << 8) : 0)) + ((Bytes.Count - 1) >= 3 ? (Bytes[((Bytes.Count - 1) - 2)] << 16) : 0)) + ((Bytes.Count - 1) >= 4 ? (Bytes[((Bytes.Count - 1) - 3)] << 24) : 0)) + ((Bytes.Count - 1) >= 5 ? (Bytes[((Bytes.Count - 1) - 4)] << 32) : 0)) + ((Bytes.Count - 1) >= 6 ? (Bytes[((Bytes.Count - 1) - 5)] << 40) : 0)) + ((Bytes.Count - 1) >= 7 ? (Bytes[((Bytes.Count - 1) - 6)] << 48) : 0)) + ((Bytes.Count - 1) >= 8 ? (Bytes[((Bytes.Count - 1) - 7)] << 56) : 0)));
                    f_value = true;
                    return _value;
                }
            }
            private List<byte> _bytes;
            private Snmp m_root;
            private KaitaiStruct m_parent;
            public List<byte> Bytes { get { return _bytes; } }
            public Snmp M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Response : KaitaiStruct
        {
            public static Response FromFile(string fileName)
            {
                return new Response(new KaitaiStream(fileName));
            }

            public Response(KaitaiStream io, Snmp parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _pdu = new Pdu(m_io, this, m_root);
            }
            private Pdu _pdu;
            private Snmp m_root;
            private Snmp m_parent;
            public Pdu Pdu { get { return _pdu; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp M_Parent { get { return m_parent; } }
        }
        public partial class Pdu : KaitaiStruct
        {
            public static Pdu FromFile(string fileName)
            {
                return new Pdu(new KaitaiStream(fileName));
            }

            public Pdu(KaitaiStream io, KaitaiStruct parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _requestId = new Asn1Obj(m_io, this, m_root);
                _errorStatus = new ErrorStatus(m_io, this, m_root);
                _errorIndex = new Asn1Obj(m_io, this, m_root);
                _variableBindings = new VariableBindings(m_io, this, m_root);
            }
            private Asn1Obj _requestId;
            private ErrorStatus _errorStatus;
            private Asn1Obj _errorIndex;
            private VariableBindings _variableBindings;
            private Snmp m_root;
            private KaitaiStruct m_parent;
            public Asn1Obj RequestId { get { return _requestId; } }
            public ErrorStatus ErrorStatus { get { return _errorStatus; } }
            public Asn1Obj ErrorIndex { get { return _errorIndex; } }
            public VariableBindings VariableBindings { get { return _variableBindings; } }
            public Snmp M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class GetNextRequest : KaitaiStruct
        {
            public static GetNextRequest FromFile(string fileName)
            {
                return new GetNextRequest(new KaitaiStream(fileName));
            }

            public GetNextRequest(KaitaiStream io, Snmp parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _pdu = new Pdu(m_io, this, m_root);
            }
            private Pdu _pdu;
            private Snmp m_root;
            private Snmp m_parent;
            public Pdu Pdu { get { return _pdu; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp M_Parent { get { return m_parent; } }
        }
        public partial class SetRequest : KaitaiStruct
        {
            public static SetRequest FromFile(string fileName)
            {
                return new SetRequest(new KaitaiStream(fileName));
            }

            public SetRequest(KaitaiStream io, Snmp parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _pdu = new Pdu(m_io, this, m_root);
            }
            private Pdu _pdu;
            private Snmp m_root;
            private Snmp m_parent;
            public Pdu Pdu { get { return _pdu; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp M_Parent { get { return m_parent; } }
        }
        public partial class LenEncoded : KaitaiStruct
        {
            public static LenEncoded FromFile(string fileName)
            {
                return new LenEncoded(new KaitaiStream(fileName));
            }

            public LenEncoded(KaitaiStream io, KaitaiStruct parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                f_result = false;
                _b1 = m_io.ReadU1();
                if (B1 == 130) {
                    _int2 = m_io.ReadU2be();
                }
            }
            private bool f_result;
            private ushort _result;
            public ushort Result
            {
                get
                {
                    if (f_result)
                        return _result;
                    _result = (ushort) (((B1 & 128) == 0 ? B1 : Int2));
                    f_result = true;
                    return _result;
                }
            }
            private byte _b1;
            private ushort _int2;
            private Snmp m_root;
            private KaitaiStruct m_parent;
            public byte B1 { get { return _b1; } }
            public ushort Int2 { get { return _int2; } }
            public Snmp M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Asn1Obj : KaitaiStruct
        {
            public static Asn1Obj FromFile(string fileName)
            {
                return new Asn1Obj(new KaitaiStream(fileName));
            }

            public Asn1Obj(KaitaiStream io, KaitaiStruct parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _hdr = new Asn1Hdr(m_io, this, m_root);
                switch (Hdr.Tag) {
                case Snmp.TypeTag.Sequence30: {
                    __raw_body = m_io.ReadBytes(Hdr.Len.Result);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new BodySequence(io___raw_body, this, m_root);
                    break;
                }
                case Snmp.TypeTag.Sequence10: {
                    __raw_body = m_io.ReadBytes(Hdr.Len.Result);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new BodySequence(io___raw_body, this, m_root);
                    break;
                }
                case Snmp.TypeTag.Utf8string: {
                    __raw_body = m_io.ReadBytes(Hdr.Len.Result);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new BodyUtf8string(io___raw_body, this, m_root);
                    break;
                }
                case Snmp.TypeTag.PrintableString: {
                    __raw_body = m_io.ReadBytes(Hdr.Len.Result);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new BodyPrintableString(io___raw_body, this, m_root);
                    break;
                }
                case Snmp.TypeTag.Integer: {
                    __raw_body = m_io.ReadBytes(Hdr.Len.Result);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new BodyInteger(io___raw_body, this, m_root);
                    break;
                }
                case Snmp.TypeTag.Set: {
                    __raw_body = m_io.ReadBytes(Hdr.Len.Result);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new BodySequence(io___raw_body, this, m_root);
                    break;
                }
                case Snmp.TypeTag.OctetString: {
                    __raw_body = m_io.ReadBytes(Hdr.Len.Result);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new BodyPrintableString(io___raw_body, this, m_root);
                    break;
                }
                default: {
                    _body = m_io.ReadBytes(Hdr.Len.Result);
                    break;
                }
                }
            }
            private Asn1Hdr _hdr;
            private object _body;
            private Snmp m_root;
            private KaitaiStruct m_parent;
            private byte[] __raw_body;
            public Asn1Hdr Hdr { get { return _hdr; } }
            public object Body { get { return _body; } }
            public Snmp M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
            public byte[] M_RawBody { get { return __raw_body; } }
        }
        public partial class BodyPrintableString : KaitaiStruct
        {
            public static BodyPrintableString FromFile(string fileName)
            {
                return new BodyPrintableString(new KaitaiStream(fileName));
            }

            public BodyPrintableString(KaitaiStream io, Asn1Obj parent = null, Snmp root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _value = System.Text.Encoding.GetEncoding("ASCII").GetString(m_io.ReadBytesFull());
            }
            private string _value;
            private Snmp m_root;
            private Snmp.Asn1Obj m_parent;
            public string Value { get { return _value; } }
            public Snmp M_Root { get { return m_root; } }
            public Snmp.Asn1Obj M_Parent { get { return m_parent; } }
        }
        private Asn1Hdr _hdr;
        private Asn1Obj _version;
        private Asn1Obj _community;
        private Asn1Hdr _pduType;
        private KaitaiStruct _data;
        private Snmp m_root;
        private KaitaiStruct m_parent;
        public Asn1Hdr Hdr { get { return _hdr; } }
        public Asn1Obj Version { get { return _version; } }
        public Asn1Obj Community { get { return _community; } }
        public Asn1Hdr PduType { get { return _pduType; } }
        public KaitaiStruct Data { get { return _data; } }
        public Snmp M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}

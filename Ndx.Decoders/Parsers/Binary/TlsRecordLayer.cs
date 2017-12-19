// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using System;
using System.Collections.Generic;
using Kaitai;

namespace Ndx.Decoders.Binary
{
    public partial class TlsRecordLayer : KaitaiStruct
    {
        public static TlsRecordLayer FromFile(string fileName)
        {
            return new TlsRecordLayer(new KaitaiStream(fileName));
        }

        public enum TlsContentType
        {
            ChangeCipherSpec = 20,
            Alert = 21,
            Handshake = 22,
            ApplicationData = 23,
        }

        public enum TlsHandshakeType
        {
            HelloRequest = 0,
            ClientHello = 1,
            ServerHello = 2,
            NewSessionTicket = 4,
            Certificate = 11,
            ServerKeyExchange = 12,
            CertificateRequest = 13,
            ServerHelloDone = 14,
            CertificateVerify = 15,
            ClientKeyExchange = 16,
            Finished = 20,
        }

        public TlsRecordLayer(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
        {
            m_parent = parent;
            m_root = root ?? this;
            _parse();
        }

        private void _parse()
        {
            _contentType = ((TlsContentType) m_io.ReadU1());
            _version = new TlsVersion(m_io, this, m_root);
            _length = m_io.ReadU2be();
            switch (ContentType) {
            case TlsContentType.Handshake: {
                __raw_body = m_io.ReadBytes(Length);
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new TlsHandshake(io___raw_body, this, m_root);
                break;
            }
            case TlsContentType.ApplicationData: {
                __raw_body = m_io.ReadBytes(Length);
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new TlsApplicationData(io___raw_body, this, m_root);
                break;
            }
            case TlsContentType.ChangeCipherSpec: {
                __raw_body = m_io.ReadBytes(Length);
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new TlsChangeCipherSpec(io___raw_body, this, m_root);
                break;
            }
            case TlsContentType.Alert: {
                __raw_body = m_io.ReadBytes(Length);
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new TlsEncryptedMessage(io___raw_body, this, m_root);
                break;
            }
            default: {
                __raw_body = m_io.ReadBytes(Length);
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new TlsEncryptedMessage(io___raw_body, this, m_root);
                break;
            }
            }
        }
        public partial class ServerName : KaitaiStruct
        {
            public static ServerName FromFile(string fileName)
            {
                return new ServerName(new KaitaiStream(fileName));
            }

            public ServerName(KaitaiStream io, Sni parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _nameType = m_io.ReadU1();
                _length = m_io.ReadU2be();
                _hostName = m_io.ReadBytes(Length);
            }
            private byte _nameType;
            private ushort _length;
            private byte[] _hostName;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.Sni m_parent;
            public byte NameType { get { return _nameType; } }
            public ushort Length { get { return _length; } }
            public byte[] HostName { get { return _hostName; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.Sni M_Parent { get { return m_parent; } }
        }
        public partial class Random : KaitaiStruct
        {
            public static Random FromFile(string fileName)
            {
                return new Random(new KaitaiStream(fileName));
            }

            public Random(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _gmtUnixTime = m_io.ReadU4be();
                _randomBytes = m_io.ReadBytes(28);
            }
            private uint _gmtUnixTime;
            private byte[] _randomBytes;
            private TlsRecordLayer m_root;
            private KaitaiStruct m_parent;
            public uint GmtUnixTime { get { return _gmtUnixTime; } }
            public byte[] RandomBytes { get { return _randomBytes; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class TlsCertificateRequest : KaitaiStruct
        {
            public static TlsCertificateRequest FromFile(string fileName)
            {
                return new TlsCertificateRequest(new KaitaiStream(fileName));
            }

            public TlsCertificateRequest(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _empty = m_io.ReadBytes(0);
            }
            private byte[] _empty;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public byte[] Empty { get { return _empty; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class TlsCertificate : KaitaiStruct
        {
            public static TlsCertificate FromFile(string fileName)
            {
                return new TlsCertificate(new KaitaiStream(fileName));
            }

            public TlsCertificate(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _certLength = new TlsLength(m_io, this, m_root);
                __raw_certificates = new List<byte[]>();
                _certificates = new List<Certificate>();
                while (!m_io.IsEof) {
                    __raw_certificates.Add(m_io.ReadBytes(CertLength.Value));
                    var io___raw_certificates = new KaitaiStream(__raw_certificates[__raw_certificates.Count - 1]);
                    _certificates.Add(new Certificate(io___raw_certificates, this, m_root));
                }
            }
            private TlsLength _certLength;
            private List<Certificate> _certificates;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            private List<byte[]> __raw_certificates;
            public TlsLength CertLength { get { return _certLength; } }
            public List<Certificate> Certificates { get { return _certificates; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
            public List<byte[]> M_RawCertificates { get { return __raw_certificates; } }
        }
        public partial class Certificate : KaitaiStruct
        {
            public static Certificate FromFile(string fileName)
            {
                return new Certificate(new KaitaiStream(fileName));
            }

            public Certificate(KaitaiStream io, TlsCertificate parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _certLength = new TlsLength(m_io, this, m_root);
                _body = m_io.ReadBytes(CertLength.Value);
            }
            private TlsLength _certLength;
            private byte[] _body;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsCertificate m_parent;
            public TlsLength CertLength { get { return _certLength; } }
            public byte[] Body { get { return _body; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsCertificate M_Parent { get { return m_parent; } }
        }
        public partial class SessionId : KaitaiStruct
        {
            public static SessionId FromFile(string fileName)
            {
                return new SessionId(new KaitaiStream(fileName));
            }

            public SessionId(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _len = m_io.ReadU1();
                _sid = m_io.ReadBytes(Len);
            }
            private byte _len;
            private byte[] _sid;
            private TlsRecordLayer m_root;
            private KaitaiStruct m_parent;
            public byte Len { get { return _len; } }
            public byte[] Sid { get { return _sid; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Sni : KaitaiStruct
        {
            public static Sni FromFile(string fileName)
            {
                return new Sni(new KaitaiStream(fileName));
            }

            public Sni(KaitaiStream io, Extension parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _listLength = m_io.ReadU2be();
                _serverNames = new List<ServerName>();
                while (!m_io.IsEof) {
                    _serverNames.Add(new ServerName(m_io, this, m_root));
                }
            }
            private ushort _listLength;
            private List<ServerName> _serverNames;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.Extension m_parent;
            public ushort ListLength { get { return _listLength; } }
            public List<ServerName> ServerNames { get { return _serverNames; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.Extension M_Parent { get { return m_parent; } }
        }
        public partial class TlsServerHello : KaitaiStruct
        {
            public static TlsServerHello FromFile(string fileName)
            {
                return new TlsServerHello(new KaitaiStream(fileName));
            }

            public TlsServerHello(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _version = new TlsVersion(m_io, this, m_root);
                _random = new Random(m_io, this, m_root);
                _sessionId = new SessionId(m_io, this, m_root);
                _cipherSuites = new CipherSuites(m_io, this, m_root);
                _compressionMethods = new CompressionMethods(m_io, this, m_root);
                if (M_Io.IsEof == false) {
                    _extensions = new Extensions(m_io, this, m_root);
                }
            }
            private TlsVersion _version;
            private Random _random;
            private SessionId _sessionId;
            private CipherSuites _cipherSuites;
            private CompressionMethods _compressionMethods;
            private Extensions _extensions;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public TlsVersion Version { get { return _version; } }
            public Random Random { get { return _random; } }
            public SessionId SessionId { get { return _sessionId; } }
            public CipherSuites CipherSuites { get { return _cipherSuites; } }
            public CompressionMethods CompressionMethods { get { return _compressionMethods; } }
            public Extensions Extensions { get { return _extensions; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class CipherSuites : KaitaiStruct
        {
            public static CipherSuites FromFile(string fileName)
            {
                return new CipherSuites(new KaitaiStream(fileName));
            }

            public CipherSuites(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _len = m_io.ReadU2be();
                _cipherSuiteList = new List<ushort>((int) ((Len / 2)));
                for (var i = 0; i < (Len / 2); i++) {
                    _cipherSuiteList.Add(m_io.ReadU2be());
                }
            }
            private ushort _len;
            private List<ushort> _cipherSuiteList;
            private TlsRecordLayer m_root;
            private KaitaiStruct m_parent;
            public ushort Len { get { return _len; } }
            public List<ushort> CipherSuiteList { get { return _cipherSuiteList; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class TlsClientKeyExchange : KaitaiStruct
        {
            public static TlsClientKeyExchange FromFile(string fileName)
            {
                return new TlsClientKeyExchange(new KaitaiStream(fileName));
            }

            public TlsClientKeyExchange(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _tlsPremasterSecret = new TlsPreMasterSecret(m_io, this, m_root);
            }
            private TlsPreMasterSecret _tlsPremasterSecret;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public TlsPreMasterSecret TlsPremasterSecret { get { return _tlsPremasterSecret; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class TlsChangeCipherSpec : KaitaiStruct
        {
            public static TlsChangeCipherSpec FromFile(string fileName)
            {
                return new TlsChangeCipherSpec(new KaitaiStream(fileName));
            }

            public TlsChangeCipherSpec(KaitaiStream io, TlsRecordLayer parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _changeMessage = m_io.ReadBytesFull();
            }
            private byte[] _changeMessage;
            private TlsRecordLayer m_root;
            private TlsRecordLayer m_parent;
            public byte[] ChangeMessage { get { return _changeMessage; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer M_Parent { get { return m_parent; } }
        }
        public partial class CompressionMethods : KaitaiStruct
        {
            public static CompressionMethods FromFile(string fileName)
            {
                return new CompressionMethods(new KaitaiStream(fileName));
            }

            public CompressionMethods(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _len = m_io.ReadU1();
                _bytes = m_io.ReadBytes(Len);
            }
            private byte _len;
            private byte[] _bytes;
            private TlsRecordLayer m_root;
            private KaitaiStruct m_parent;
            public byte Len { get { return _len; } }
            public byte[] Bytes { get { return _bytes; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class TlsCertificateVerify : KaitaiStruct
        {
            public static TlsCertificateVerify FromFile(string fileName)
            {
                return new TlsCertificateVerify(new KaitaiStream(fileName));
            }

            public TlsCertificateVerify(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _empty = m_io.ReadBytes(0);
            }
            private byte[] _empty;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public byte[] Empty { get { return _empty; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class Alpn : KaitaiStruct
        {
            public static Alpn FromFile(string fileName)
            {
                return new Alpn(new KaitaiStream(fileName));
            }

            public Alpn(KaitaiStream io, Extension parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _extLen = m_io.ReadU2be();
                _alpnProtocols = new List<Protocol>();
                while (!m_io.IsEof) {
                    _alpnProtocols.Add(new Protocol(m_io, this, m_root));
                }
            }
            private ushort _extLen;
            private List<Protocol> _alpnProtocols;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.Extension m_parent;
            public ushort ExtLen { get { return _extLen; } }
            public List<Protocol> AlpnProtocols { get { return _alpnProtocols; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.Extension M_Parent { get { return m_parent; } }
        }
        public partial class Extensions : KaitaiStruct
        {
            public static Extensions FromFile(string fileName)
            {
                return new Extensions(new KaitaiStream(fileName));
            }

            public Extensions(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _len = m_io.ReadU2be();
                _extensionList = new List<Extension>();
                while (!m_io.IsEof) {
                    _extensionList.Add(new Extension(m_io, this, m_root));
                }
            }
            private ushort _len;
            private List<Extension> _extensionList;
            private TlsRecordLayer m_root;
            private KaitaiStruct m_parent;
            public ushort Len { get { return _len; } }
            public List<Extension> ExtensionList { get { return _extensionList; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class TlsPreMasterSecret : KaitaiStruct
        {
            public static TlsPreMasterSecret FromFile(string fileName)
            {
                return new TlsPreMasterSecret(new KaitaiStream(fileName));
            }

            public TlsPreMasterSecret(KaitaiStream io, TlsClientKeyExchange parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _secretLength = m_io.ReadU2be();
                _secret = m_io.ReadBytes(SecretLength);
            }
            private ushort _secretLength;
            private byte[] _secret;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsClientKeyExchange m_parent;
            public ushort SecretLength { get { return _secretLength; } }
            public byte[] Secret { get { return _secret; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsClientKeyExchange M_Parent { get { return m_parent; } }
        }
        public partial class TlsServerKeyExchange : KaitaiStruct
        {
            public static TlsServerKeyExchange FromFile(string fileName)
            {
                return new TlsServerKeyExchange(new KaitaiStream(fileName));
            }

            public TlsServerKeyExchange(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _empty = m_io.ReadBytes(0);
            }
            private byte[] _empty;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public byte[] Empty { get { return _empty; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class TlsApplicationData : KaitaiStruct
        {
            public static TlsApplicationData FromFile(string fileName)
            {
                return new TlsApplicationData(new KaitaiStream(fileName));
            }

            public TlsApplicationData(KaitaiStream io, TlsRecordLayer parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _body = m_io.ReadBytesFull();
            }
            private byte[] _body;
            private TlsRecordLayer m_root;
            private TlsRecordLayer m_parent;
            public byte[] Body { get { return _body; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer M_Parent { get { return m_parent; } }
        }
        public partial class TlsClientHello : KaitaiStruct
        {
            public static TlsClientHello FromFile(string fileName)
            {
                return new TlsClientHello(new KaitaiStream(fileName));
            }

            public TlsClientHello(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _version = new TlsVersion(m_io, this, m_root);
                _random = new Random(m_io, this, m_root);
                _sessionId = new SessionId(m_io, this, m_root);
                _cipherSuites = new CipherSuites(m_io, this, m_root);
                _compressionMethods = new CompressionMethods(m_io, this, m_root);
                if (M_Io.IsEof == false) {
                    _extensions = new Extensions(m_io, this, m_root);
                }
            }
            private TlsVersion _version;
            private Random _random;
            private SessionId _sessionId;
            private CipherSuites _cipherSuites;
            private CompressionMethods _compressionMethods;
            private Extensions _extensions;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public TlsVersion Version { get { return _version; } }
            public Random Random { get { return _random; } }
            public SessionId SessionId { get { return _sessionId; } }
            public CipherSuites CipherSuites { get { return _cipherSuites; } }
            public CompressionMethods CompressionMethods { get { return _compressionMethods; } }
            public Extensions Extensions { get { return _extensions; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class TlsServerHelloDone : KaitaiStruct
        {
            public static TlsServerHelloDone FromFile(string fileName)
            {
                return new TlsServerHelloDone(new KaitaiStream(fileName));
            }

            public TlsServerHelloDone(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _empty = m_io.ReadBytes(0);
            }
            private byte[] _empty;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public byte[] Empty { get { return _empty; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class TlsEncryptedMessage : KaitaiStruct
        {
            public static TlsEncryptedMessage FromFile(string fileName)
            {
                return new TlsEncryptedMessage(new KaitaiStream(fileName));
            }

            public TlsEncryptedMessage(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _encryptedMessage = m_io.ReadBytesFull();
            }
            private byte[] _encryptedMessage;
            private TlsRecordLayer m_root;
            private KaitaiStruct m_parent;
            public byte[] EncryptedMessage { get { return _encryptedMessage; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class TlsEmpty : KaitaiStruct
        {
            public static TlsEmpty FromFile(string fileName)
            {
                return new TlsEmpty(new KaitaiStream(fileName));
            }

            public TlsEmpty(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _empty = m_io.ReadBytes(0);
            }
            private byte[] _empty;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public byte[] Empty { get { return _empty; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class TlsHandshake : KaitaiStruct
        {
            public static TlsHandshake FromFile(string fileName)
            {
                return new TlsHandshake(new KaitaiStream(fileName));
            }

            public TlsHandshake(KaitaiStream io, TlsRecordLayer parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _handshakeType = ((TlsRecordLayer.TlsHandshakeType) m_io.ReadU1());
                _bodyLength = new TlsLength(m_io, this, m_root);
                switch (HandshakeType) {
                case TlsRecordLayer.TlsHandshakeType.HelloRequest: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsEmpty(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.Certificate: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsCertificate(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.CertificateVerify: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsCertificateVerify(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.ServerKeyExchange: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsServerKeyExchange(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.ClientHello: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsClientHello(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.Finished: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsFinished(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.ClientKeyExchange: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsClientKeyExchange(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.ServerHello: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsServerHello(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.CertificateRequest: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsCertificateRequest(io___raw_body, this, m_root);
                    break;
                }
                case TlsRecordLayer.TlsHandshakeType.ServerHelloDone: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsServerHelloDone(io___raw_body, this, m_root);
                    break;
                }
                default: {
                    __raw_body = m_io.ReadBytes((M_Parent.Length - 4));
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new TlsEncryptedMessage(io___raw_body, this, m_root);
                    break;
                }
                }
            }
            private TlsHandshakeType _handshakeType;
            private TlsLength _bodyLength;
            private KaitaiStruct _body;
            private TlsRecordLayer m_root;
            private TlsRecordLayer m_parent;
            private byte[] __raw_body;
            public TlsHandshakeType HandshakeType { get { return _handshakeType; } }
            public TlsLength BodyLength { get { return _bodyLength; } }
            public KaitaiStruct Body { get { return _body; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer M_Parent { get { return m_parent; } }
            public byte[] M_RawBody { get { return __raw_body; } }
        }
        public partial class Protocol : KaitaiStruct
        {
            public static Protocol FromFile(string fileName)
            {
                return new Protocol(new KaitaiStream(fileName));
            }

            public Protocol(KaitaiStream io, Alpn parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _strlen = m_io.ReadU1();
                _name = m_io.ReadBytes(Strlen);
            }
            private byte _strlen;
            private byte[] _name;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.Alpn m_parent;
            public byte Strlen { get { return _strlen; } }
            public byte[] Name { get { return _name; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.Alpn M_Parent { get { return m_parent; } }
        }
        public partial class TlsLength : KaitaiStruct
        {
            public static TlsLength FromFile(string fileName)
            {
                return new TlsLength(new KaitaiStream(fileName));
            }

            public TlsLength(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                f_value = false;
                _hlen = m_io.ReadU1();
                _llen = m_io.ReadU2be();
            }
            private bool f_value;
            private int _value;
            public int Value
            {
                get
                {
                    if (f_value)
                        return _value;
                    _value = (int) (((Llen + Hlen) << 16));
                    f_value = true;
                    return _value;
                }
            }
            private byte _hlen;
            private ushort _llen;
            private TlsRecordLayer m_root;
            private KaitaiStruct m_parent;
            public byte Hlen { get { return _hlen; } }
            public ushort Llen { get { return _llen; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class TlsVersion : KaitaiStruct
        {
            public static TlsVersion FromFile(string fileName)
            {
                return new TlsVersion(new KaitaiStream(fileName));
            }

            public TlsVersion(KaitaiStream io, KaitaiStruct parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _major = m_io.ReadU1();
                _minor = m_io.ReadU1();
            }
            private byte _major;
            private byte _minor;
            private TlsRecordLayer m_root;
            private KaitaiStruct m_parent;
            public byte Major { get { return _major; } }
            public byte Minor { get { return _minor; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class TlsFinished : KaitaiStruct
        {
            public static TlsFinished FromFile(string fileName)
            {
                return new TlsFinished(new KaitaiStream(fileName));
            }

            public TlsFinished(KaitaiStream io, TlsHandshake parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _verifyData = m_io.ReadBytesFull();
            }
            private byte[] _verifyData;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.TlsHandshake m_parent;
            public byte[] VerifyData { get { return _verifyData; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.TlsHandshake M_Parent { get { return m_parent; } }
        }
        public partial class Extension : KaitaiStruct
        {
            public static Extension FromFile(string fileName)
            {
                return new Extension(new KaitaiStream(fileName));
            }

            public Extension(KaitaiStream io, Extensions parent = null, TlsRecordLayer root = null) : base(io)
            {
                m_parent = parent;
                m_root = root;
                _parse();
            }

            private void _parse()
            {
                _type = m_io.ReadU2be();
                _len = m_io.ReadU2be();
                switch (Type) {
                case 0: {
                    __raw_body = m_io.ReadBytes(Len);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new Sni(io___raw_body, this, m_root);
                    break;
                }
                case 16: {
                    __raw_body = m_io.ReadBytes(Len);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new Alpn(io___raw_body, this, m_root);
                    break;
                }
                default: {
                    _body = m_io.ReadBytes(Len);
                    break;
                }
                }
            }
            private ushort _type;
            private ushort _len;
            private object _body;
            private TlsRecordLayer m_root;
            private TlsRecordLayer.Extensions m_parent;
            private byte[] __raw_body;
            public ushort Type { get { return _type; } }
            public ushort Len { get { return _len; } }
            public object Body { get { return _body; } }
            public TlsRecordLayer M_Root { get { return m_root; } }
            public TlsRecordLayer.Extensions M_Parent { get { return m_parent; } }
            public byte[] M_RawBody { get { return __raw_body; } }
        }
        private TlsContentType _contentType;
        private TlsVersion _version;
        private ushort _length;
        private KaitaiStruct _body;
        private TlsRecordLayer m_root;
        private KaitaiStruct m_parent;
        private byte[] __raw_body;
        public TlsContentType ContentType { get { return _contentType; } }
        public TlsVersion Version { get { return _version; } }
        public ushort Length { get { return _length; } }
        public KaitaiStruct Body { get { return _body; } }
        public TlsRecordLayer M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
        public byte[] M_RawBody { get { return __raw_body; } }
    }
}

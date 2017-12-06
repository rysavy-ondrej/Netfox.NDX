    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Newtonsoft.Json.Linq;
using Ndx.Decoders;

namespace Ndx.TShark
{
    public class TSharkPacketDecoderProcess : TSharkProcess<Packet>
    {

        DecoderFactory m_factory;
        PacketDecoder m_decoder;
        public TSharkPacketDecoderProcess(string pipename, DecoderFactory factory, PacketDecoder decoder) : base(pipename)
        {
            m_factory = factory;
            m_decoder = decoder;
        }
        protected override string GetOutputFilter()
        {
            return "";
        }

        protected override Packet GetResult(string line)
        {
            var packet = m_decoder.Decode(m_factory, line);
            return packet;
        }
    }
}

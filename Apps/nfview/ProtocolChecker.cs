using Apache.Ignite.Core;
using Apache.Ignite.Core.Compute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Decoders;
using System.Diagnostics;
using Ndx.Decoders.Core;
using Ndx.Diagnostics;
using System.Security;
using Apache.Ignite.Core.Events;
using Apache.Ignite.Core.Binary;
using Google.Protobuf;
using Ndx.Decoders.Base;
using Apache.Ignite.Linq;
using Apache.Ignite.Core.Cache;

namespace nfview
{

    class PacketBinarizable : IBinarizable
    {
        private Packet m_packet;


        public Packet Packet { get => m_packet; set => m_packet = value; }

        public DateTimeOffset TimeStamp => DateTimeOffset.FromUnixTimeMilliseconds(m_packet?.TimeStamp ?? 0);

        public long FrameNumber => m_packet?.Protocol<Frame>()?.FrameNumber ?? 0;

        public PacketBinarizable() { }

        public PacketBinarizable(Packet packet)
        {
            this.m_packet = packet;
        }
      
        public void ReadBinary(IBinaryReader reader)
        {
            var buffer = reader.ReadByteArray("Packet");
            m_packet = Packet.Parser.ParseFrom(buffer);
        }

        public void WriteBinary(IBinaryWriter writer)
        {
            writer.WriteByteArray("Packet", m_packet.ToByteArray());                       
        }

        public override string ToString()
        {
            return m_packet.ToString();
        }
    }

    class ProtocolChecker
    {
        static void Main(string[] args)
        {
            var inputfile = args.Length == 1 ? args[0] : @"C:\Users\rysavy\Documents\Network Monitor 3\Captures\1.dcap";
            var checker = new ProtocolChecker();
            checker.LoadTrace(inputfile);
            checker.Timeline();
            checker.ArpCheck();
            checker.DnsCheck();

            checker.Dump(checker.TimelineData);
        }



        private void Dump<T>(ICache<long, T> cache)
        {
            Console.WriteLine($"{cache.Name} :");
            foreach(var c in cache)
            {
                Console.WriteLine($"  {c.Key} : {c.Value}");
            }
        }

        IIgnite m_ignite;
        public ProtocolChecker()
        {
            m_ignite = Ignition.Start();
        }

        public void LoadTrace(string path)
        {
            var packets = m_ignite.GetOrCreateCache<long, PacketBinarizable>("packets");

            using (var ldr = m_ignite.GetDataStreamer<long, PacketBinarizable>("packets"))
            {
                foreach (var p in PacketReader.ReadAllPackets(path))
                {
                    ldr.AddData(p.Protocol<Frame>().FrameNumber, new PacketBinarizable(p));
                }
            }
        }

        private void Timeline()
        {
            var packets = m_ignite.GetCache<long, PacketBinarizable>("packets");
            var timeline = from p in packets
                           group p by p.Value.Packet.TimeStamp into g
                           select new { time = g.Key, packets = (long)g.Count(), bytes = g.Sum(x => x.Value.Packet.Protocol<Frame>().FrameLen) };

            TimelineData = m_ignite.GetOrCreateCache<long, Tuple<long, long>>("TIMELINE");
            using (var ldr = m_ignite.GetDataStreamer<long, Tuple<long, long>>("TIMELINE"))
            {
                foreach (var p in timeline)
                {
                    ldr.AddData(p.time, Tuple.Create(p.packets, p.bytes));
                }
            }
        }

        ICache<long, Tuple<long, long>> TimelineData;
        ICache<long, Tuple<PacketBinarizable>> ArpRequestNoReply;
        ICache<long, Tuple<PacketBinarizable, PacketBinarizable>> ArpRequestReplyOk;


        public void ArpCheck()
        {
            var packets = m_ignite.GetCache<long, PacketBinarizable>("packets");

            var arpPackets = packets.Where(p => p.Value.Packet.HasProtocol(Packet.Types.Protocol.ProtocolTypeOneofCase.Arp));

            var arpPairs =
                from e1 in arpPackets.Where(e => e.Value.Packet.Protocol<Arp>().ArpOpcode == (uint)ArpOpcode.ARP_REQUEST)
                join e2 in arpPackets.Where(e => e.Value.Packet.Protocol<Arp>().ArpOpcode == (uint)ArpOpcode.ARP_REPLY)
                on e1.Value.Packet.Protocol<Arp>().ArpSrcProtoIpv4Address equals e2.Value.Packet.Protocol<Arp>().ArpDstProtoIpv4Address
                into replies
                let window = new TimeWindow(e1.Value.Packet.TimeStamp, e1.Value.Packet.TimeStamp + 1000)
                from arpReply in replies.DefaultIfEmpty().Where(x => x == null || window.In(x.Value.Packet.TimeStamp))
                select new { name = "ARP_PAIRS", request = e1, reply = arpReply };

            ArpRequestReplyOk = m_ignite.GetOrCreateCache<long, Tuple<PacketBinarizable, PacketBinarizable>>("ARP_REQUEST_REPLY_OK");
            using (var ldr = m_ignite.GetDataStreamer<long, Tuple<PacketBinarizable,PacketBinarizable> > ("ARP_REQUEST_REPLY_OK"))
            {
                foreach (var p in arpPairs.Where(x => x.reply != null))
                {
                    ldr.AddData(p.request.Key, Tuple.Create(p.request.Value, p.reply.Value));
                }
            }
            ArpRequestNoReply = m_ignite.GetOrCreateCache<long, Tuple<PacketBinarizable>>("ARP_REQUEST_NO_REPLY");
            using (var ldr = m_ignite.GetDataStreamer<long, Tuple<PacketBinarizable>>("ARP_REQUEST_NO_REPLY"))
            {
                foreach (var p in arpPairs.Where(x => x.reply == null))
                {
                    ldr.AddData(p.request.Key, Tuple.Create(p.request.Value));

                }
            }
        }

        Apache.Ignite.Core.Cache.ICache<long, Tuple<PacketBinarizable>> DnsQueryNoResponse;
        Apache.Ignite.Core.Cache.ICache<long, Tuple<PacketBinarizable, PacketBinarizable>> DnsQueryResponseOk;
        Apache.Ignite.Core.Cache.ICache<long, Tuple<PacketBinarizable, PacketBinarizable>> DnsQueryResponseError;

        public void DnsCheck()
        {
            var packets = m_ignite.GetCache<long, PacketBinarizable>("packets");

            var dnsPackets = packets.Where(p => p.Value.Packet.HasProtocol(Packet.Types.Protocol.ProtocolTypeOneofCase.Dns));
            var dnsPairs = from e1 in dnsPackets.Where(x => !x.Value.Packet.Protocol<Dns>().DnsFlags.DnsFlagsResponse)
                           join e2 in dnsPackets.Where(x => x.Value.Packet.Protocol<Dns>().DnsFlags.DnsFlagsResponse) on e1.Value.Packet.Protocol<Dns>().DnsId equals e2.Value.Packet.Protocol<Dns>().DnsId into responses
                           from resp in responses.DefaultIfEmpty()
                           select new { name = "DNS_PAIRS", query = e1, response = resp };

            DnsQueryNoResponse = m_ignite.GetOrCreateCache<long, Tuple<PacketBinarizable>>("DNS_QUERY_NO_RESPONSE");
            using (var ldr = m_ignite.GetDataStreamer<long, Tuple<PacketBinarizable>>("DNS_QUERY_NO_RESPONSE"))
            {
                foreach (var p in dnsPairs.Where(x => x.response == null))
                {
                    ldr.AddData(p.query.Key, Tuple.Create(p.query.Value));
                }
            }

            DnsQueryResponseError = m_ignite.GetOrCreateCache<long, Tuple<PacketBinarizable, PacketBinarizable>>("DNS_QUERY_RESPONSE_ERROR");
            using (var ldr = m_ignite.GetDataStreamer<long, Tuple<PacketBinarizable, PacketBinarizable>>("DNS_QUERY_RESPONSE_ERROR"))
            {
                foreach (var p in dnsPairs.Where(x => x.response != null).Where(p => p.response.Value.Packet.Protocol<Dns>().DnsFlags.DnsFlagsRcode != (uint)Dns.ReturnCode.Success))
                {
                    ldr.AddData(p.query.Key, Tuple.Create(p.query.Value, p.response.Value));
                }
            }

            DnsQueryResponseOk = m_ignite.GetOrCreateCache<long, Tuple<PacketBinarizable, PacketBinarizable>>("DNS_QUERY_RESPONSE_OK");
            using (var ldr = m_ignite.GetDataStreamer<long, Tuple<PacketBinarizable, PacketBinarizable>>("DNS_QUERY_RESPONSE_OK"))
            {
                foreach (var p in dnsPairs.Where(x => x.response != null).Where(p => p.response.Value.Packet.Protocol<Dns>().DnsFlags.DnsFlagsRcode == (uint)Dns.ReturnCode.Success))
                {
                    ldr.AddData(p.query.Key, Tuple.Create(p.query.Value, p.response.Value));
                }
            }
        }
        
    }
}

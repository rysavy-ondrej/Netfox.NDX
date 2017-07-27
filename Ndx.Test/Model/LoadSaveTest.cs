using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ndx.Model;
using System.Net;
using System.IO;
using Google.Protobuf;

namespace Ndx.Test
{
    [TestFixture]
    class LoadSaveTest
    {
        [Test]
        public void Conversation_StoreLoad()
        {
            var c1 = new Conversation()
            {
                ConversationId = 124,
                ParentId = 0,
                ConversationKey = new FlowKey()
                {
                    IpProtocol = IpProtocolType.Tcp,
                    SourceIpAddress = IPAddress.Parse("192.168.186.11"),
                    SourcePort = 13235,
                    DestinationIpAddress = IPAddress.Parse("54.241.152.100"),
                    DestinationPort = 80
                }
            };

            var stream = new MemoryStream();
            c1.WriteTo(stream);
            stream.Position = 0;
            var c2 = Conversation.Parser.ParseFrom(stream);
        }
    }
}

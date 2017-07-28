using System;
using System.Linq;
using Ndx.Captures;
using Ndx.Decoders;
using Ndx.TShark;
using NUnit.Framework;
namespace Ndx.Test
{
    [TestFixture]
    public class DecoderTests
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        [Test]
        public void SshDecoderTest_FieldDecoder()
        {
            var tsharkProcess = new TSharkFieldDecoderProcess(SSH.Fields);            
            var frames = PcapReader.ReadFile(@"C:\Temp\NAS-SSH-154.0.166.83.cap");
            var packets = frames.Take(1000).Decode(tsharkProcess);
            foreach(var packet in packets)
            {
                Console.WriteLine(packet);
            }
        }
        [Test]
        public void SshDecoderTest_ProtocolDecoder()
        {
            var tsharkProcess = new TSharkProtocolDecoderProcess(new string[] { "ssh", "tcp" });
            var frames = PcapReader.ReadFile(@"C:\Temp\NAS-SSH-154.0.166.83.cap");
            var packets = frames.Take(1000).Decode(tsharkProcess).Where(x=>x.FrameProtocols.Contains("ssh"));
            foreach (var packet in packets)
            {
                Console.WriteLine(packet);
            }
        }
    }
}

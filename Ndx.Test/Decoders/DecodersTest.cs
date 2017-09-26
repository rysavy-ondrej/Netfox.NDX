using System;
using System.Linq;
using System.Reactive.Linq;
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
            var frames = PcapFile.ReadFile(@"C:\Temp\NAS-SSH-154.0.166.83.cap");
            var packets = frames.Take(1000).Decode(tsharkProcess);

            packets.ForEach(packet => Console.WriteLine(packet));
        }
        [Test]
        public void SshDecoderTest_ProtocolDecoder()
        {
            var tsharkProcess = new TSharkProtocolDecoderProcess(new string[] { "ssh", "tcp" });
            var frames = PcapFile.ReadFile(@"C:\Temp\NAS-SSH-154.0.166.83.cap");
            var packets = frames.Take(1000).Decode(tsharkProcess).Where(x=>x.FrameProtocols.Contains("ssh"));
            packets.ForEach(packet => Console.WriteLine(packet));
        }
    }
}

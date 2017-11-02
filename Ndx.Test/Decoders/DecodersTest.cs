using System;
using System.IO;
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
        string source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\ssh.cap");
        [Test]
        public void SshDecoderTest_FieldDecoder()
        {
            var tsharkProcess = new TSharkFieldDecoderProcess(); // SSH.Fields);            
            var frames = PcapFile.ReadFile(source);
            var packets = frames.Decode(tsharkProcess).Where(x=>x.FrameProtocols.Contains("ssh"));

            Console.WriteLine("SSH Packets:");
            packets.ForEach(packet => Console.WriteLine(packet));
        }
        [Test]
        public void SshDecoderTest_ProtocolDecoder()
        {
            var tsharkProcess = new TSharkProtocolDecoderProcess(new string[] { "ssh", "tcp" });
            var frames = PcapFile.ReadFile(source);
            var packets = frames.Decode(tsharkProcess).Where(x=>x.FrameProtocols.Contains("ssh"));

            Console.WriteLine("SSH Packets:");
            packets.ForEach(packet => Console.WriteLine(packet));
        }
    }
}

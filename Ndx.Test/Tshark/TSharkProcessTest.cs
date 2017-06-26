using Ndx.Metacap;
using NUnit.Framework;
using PacketDotNet;
using System.Net;
using System.Net.Sockets;
using Ndx.Model;
using System.Net.NetworkInformation;
using Ndx.TShark;
using System.IO;
using System;
using System.Threading.Tasks;

namespace Ndx.Test
{

    [TestFixture]
    public class TSharkProcessTest
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string m_inputPcap = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap");
        [Test]
        public void ProcessCaptureByTShark()
        {
            // Use random pipe name to avoid clashes with other processes
            var rand = new Random();
            var id = rand.Next(UInt16.MaxValue);
            var wsender = new WiresharkSender($"tshark{id}", DataLinkType.Ethernet);
            var tshark = new TSharkProcess()
            {
                PipeName = $"tshark{id}"
            };
            tshark.Fields.Add("http.request.method");
            tshark.Fields.Add("http.request.uri");
            tshark.Fields.Add("http.request.version");
            tshark.Fields.Add("http.host");
            tshark.Fields.Add("http.user_agent");
            tshark.Fields.Add("http.accept");
            tshark.Fields.Add("http.accept_language");
            tshark.Fields.Add("http.accept_encoding");
            tshark.Fields.Add("http.connection");
            tshark.Fields.Add("http.referer");
            tshark.Fields.Add("http.request.full_uri");
            tshark.Start();

            // wait till the sender is connected to tshark
            // otherwise we lost data...
            Task.WaitAll(wsender.Connected);

            var frameCount = 0;
            var frames = Ndx.Captures.PcapReader.ReadFile(m_inputPcap);
            foreach (var frame in frames)
            {
                var res = wsender.Send(frame);
                frameCount++;
            }
            // close sender first
            wsender.Close();
            // then close, the control is returned when tshark finishes
            tshark.Close();

            var outputCount = 0;
            // results can be read when tshark finishes:s
            foreach (var r in tshark.Result)
            {
                Console.WriteLine($"{r.FrameNumber} {r.FrameProtocols}");
                outputCount++;
            }
            Assert.AreEqual(frameCount, outputCount);
        }
        
    }
}

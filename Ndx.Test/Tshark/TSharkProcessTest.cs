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
            var rand = new Random();
            var id = rand.Next(UInt16.MaxValue);
            var ws = new WiresharkSender($"tshark{id}", DataLinkType.Ethernet);
            var frames = Ndx.Captures.PcapReader.ReadFile(m_inputPcap);
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

            foreach(var frame in frames)
            {
                ws.Send(frame);
            }
            ws.Close();
            tshark.Close();
           
        }
        
    }
}

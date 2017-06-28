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
using System.Net.Mime;
using System.Linq;
using Google.Protobuf;

namespace Ndx.Test
{

    [TestFixture]
    public class TSharkProcessTest
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        [Test]
        public void ProcessHttpCaptureByTShark()
        {
            ProcessCapture(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap"));
        }
        [Test]
        public void ProcessHttpGzipCaptureByTShark()
        {
            ProcessCapture(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http_gzip.cap"));
        }

        [Test]
        public void ProcessHttpWithJpegsCaptureByTShark()
        {
            ProcessCapture(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http_with_jpegs.cap"));
        }
        [Test]
        public void ProcessHttpChunkedGzipCaptureByTShark()
        {
            ProcessCapture(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http_chunked_gzip.cap"));
        }



        void ProcessCapture(string path)
        {
            // Use random pipe name to avoid clashes with other processes
            var rand = new Random();
            var pipename = $"ndx.tshark_{rand.Next(UInt16.MaxValue)}";
            var wsender = new WiresharkSender(pipename, DataLinkType.Ethernet);
            var tshark = new TSharkProcess()
            {
                PipeName = pipename
            };

            var outdir = Path.ChangeExtension(path, "out");
            var outTxtFilename = Path.ChangeExtension(path, "txt");
            var outPbfFilename = Path.ChangeExtension(path, "pbf");
            if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);
            var outputfile = File.CreateText(Path.Combine(outTxtFilename));
            var outputPbfStream = new CodedOutputStream(File.Create(outPbfFilename));
            var outputCount = 0;
            void Tshark_PacketDecoded(object sender, PacketFields e)
            {
                outputfile.WriteLine($"{e.FrameNumber} [{e.Timestamp}] {e.FrameProtocols}:");
                foreach (var f in e.Fields)
                {
                        outputfile.WriteLine($"  {f.Key}={f.Value}");
                }

                e.WriteTo(outputPbfStream);
                outputCount++;
                outputfile.WriteLine();
            }
            
            tshark.PacketDecoded += Tshark_PacketDecoded;
            // fields for http request
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
            tshark.Fields.Add("http.request_number");
            // fields for http response
            tshark.Fields.Add("http.response.code");
            tshark.Fields.Add("http.response.code.desc");
            tshark.Fields.Add("http.content_type");
            tshark.Fields.Add("http.content_encoding");
            tshark.Fields.Add("http.server");
            tshark.Fields.Add("http.content_length");
            tshark.Fields.Add("http.date");
            //tshark.Fields.Add("http.file_data");
            tshark.Fields.Add("http.response_number");
            tshark.ExportedObjectsPath = outdir;
            tshark.ExportObjects = true;
            tshark.Start();

            // wait till the sender is connected to tshark
            // otherwise we lost data...
            Task.WaitAll(wsender.Connected);

            var frameCount = 0;
            var frames = Ndx.Captures.PcapReader.ReadFile(path);
            foreach (var frame in frames)
            {
                File.WriteAllBytes(Path.Combine(outdir, $"{frameCount}.packet"),frame.Bytes);
                var res = wsender.Send(frame);
                frameCount++;
            }
            // close sender first
            wsender.Close();
            // then close, the control is returned when tshark finishes
            tshark.Close();
            outputfile.Flush();
            outputPbfStream.Flush();
            Assert.AreEqual(frameCount, outputCount);
        }
    }
}

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
using System.Threading.Tasks.Dataflow;

namespace Ndx.Test
{
    [TestFixture]
    public class TSharkProcessTest
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        [Test]
        public void ProcessHttpCaptureByTShark()
        {
            //ProcessCapture(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap"));
            ProcessCaptureByBlock(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap"));
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
            // This causes the problem with dumpcap!
            ProcessCapture(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http_chunked_gzip.cap"));
        }


        void ProcessCaptureByBlock(string path)
        {
            var fields = new[] {
                "http.request.method",
                "http.request.uri",
                "http.request.version",
                "http.host",
                "http.user_agent",
                "http.accept",
                "http.accept_language",
                "http.accept_encoding",
                "http.connection",
                "http.referer",
                "http.request.full_uri",
                "http.request_number",
                // fields for http response
                "http.response.code",
                "http.response.code.desc",
                "http.content_type",
                "http.content_encoding",
                "http.server",
                "http.content_length",
                "http.date",
                "http.response_number"
            };

            var tsharkBlock = new TSharkBlock(fields);

            var outObjsFolder = Path.ChangeExtension(path, "1.out");
            var outTxtFilename = Path.ChangeExtension(path, "1.txt");
            var outPbfFilename = Path.ChangeExtension(path, "1.pbf");
            var outputTxtfile = File.CreateText(Path.Combine(outTxtFilename));
            var outputPbfStream = new CodedOutputStream(File.Create(outPbfFilename));
            var outputCount = 0;

            var consumer = new ActionBlock<PacketFields>(
                async e =>
                {
                    await outputTxtfile.WriteLineAsync($"{e.FrameNumber} [{e.Timestamp}] {e.FrameProtocols}:");
                    foreach (var f in e.Fields)
                    {
                        await outputTxtfile.WriteLineAsync($"  {f.Key}={f.Value}");
                    }
                    await outputTxtfile.WriteLineAsync();
                    await outputTxtfile.FlushAsync();
                    e.WriteTo(outputPbfStream);
                    outputPbfStream.Flush();
                    outputCount++;
                });
            tsharkBlock.LinkTo(consumer, new DataflowLinkOptions() { PropagateCompletion = true });

            var frames = Captures.PcapReader.ReadFile(path);
            tsharkBlock.Consume(frames);
            consumer.Completion.Wait();
            outputTxtfile.Close();
            outputPbfStream.Dispose();
        }

        void ProcessCapture(string path)
        {
            var rand = new Random();
            var pipename = $"ndx.tshark_{rand.Next(UInt16.MaxValue)}";

            var outObjsFolder = Path.ChangeExtension(path, "2.out");
            var outTxtFilename = Path.ChangeExtension(path, "2.txt");
            var outPbfFilename = Path.ChangeExtension(path, "2.pbf");
            if (!Directory.Exists(outObjsFolder)) Directory.CreateDirectory(outObjsFolder);
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
                outputfile.WriteLine();
                e.WriteTo(outputPbfStream);
                outputCount++;
            }

                var wsender = new WiresharkSender(pipename, DataLinkType.Ethernet);
                var tshark = new TSharkProcess(pipename);            
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
            tshark.Fields.Add("http.response_number");
            tshark.ExportedObjectsPath = outObjsFolder;
            tshark.ExportObjects = true;

            tshark.Start();

            // wait till the sender is connected to tshark
            // otherwise we lost data...
            Task.WaitAll(wsender.Connected);
            // check if tshark is still running

            if (!tshark.IsRunning)
            {
                Assert.Fail("TSHARK is not running!");
            }

            var frameCount = 0;
            var frames = Captures.PcapReader.ReadFile(path);
            foreach (var frame in frames)
            {
                File.WriteAllBytes(Path.Combine(outObjsFolder, $"{frameCount}.packet"),frame.Bytes);
                var res = wsender.Send(frame);
                frameCount++;
            }
            // close sender first
            wsender.Close();
            // then close, the control is returned when tshark finishes
            tshark.Close();
            outputfile.Flush();
            outputPbfStream.Flush();
            outputfile.Close();
            Assert.AreEqual(frameCount, outputCount);
        }
    }
}

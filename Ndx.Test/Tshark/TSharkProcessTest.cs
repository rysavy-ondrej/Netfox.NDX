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
            DecodeFields(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap"));
            DecodeProtocol(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap"));
        }
        [Test]
        public void ProcessHttpGzipCaptureByTShark()
        {
            DecodeFields(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http_gzip.cap"));
        }

        [Test]
        public void ProcessHttpWithJpegsCaptureByTShark()
        {
            DecodeFields(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http_with_jpegs.cap"));
        }
        [Test]
        public void ProcessHttpChunkedGzipCaptureByTShark()
        {
            // This causes the problem with dumpcap!
            DecodeFields(Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http_chunked_gzip.cap"));
        }


        async Task DecodeProtocol(string path)
        {
            var process = new TSharkProtocolDecoderProcess(new[] { "dns", "http" });
            await DecodeCaptureAsync(path, process, ".proto");
        }

        async Task DecodeFields(string path)
        {
            var fields = new[] {
                "http.request.method", "http.request.uri", "http.request.version", "http.host",
                "http.user_agent", "http.accept", "http.accept_language", "http.accept_encoding",
                "http.connection", "http.referer", "http.request.full_uri", "http.request_number",
                // fields for http response
                "http.response.code", "http.response.code.desc",  "http.content_type",
                "http.content_encoding", "http.server",  "http.content_length",
                "http.date", "http.response_number",
                "dns.a", "dns.cname", "dns.id", "dns.ns",
            };
            var process = new TSharkFieldDecoderProcess(fields);
            await DecodeCaptureAsync(path, process, ".fields");
        }

        async Task DecodeCaptureAsync(string path, TSharkProcess<DecodedFrame> tsharkProcess, string prefix)
        {
            var tsharkBlock = new TSharkBlock<DecodedFrame>(tsharkProcess);

            var outObjsFolder = Path.ChangeExtension(path, $"{prefix}.out");
            var outTxtFilename = Path.ChangeExtension(path, $"{prefix}.txt");
            var outPbfFilename = Path.ChangeExtension(path, $"{prefix}.pbf");
            var outputTxtfile = File.CreateText(Path.Combine(outTxtFilename));
            var outputPbfStream = new CodedOutputStream(File.Create(outPbfFilename));
            var outputCount = 0;

            var consumer = new ActionBlock<DecodedFrame>(
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

            var frames = Captures.PcapFile.ReadFile(path);
            await tsharkBlock.ConsumeAsync(frames);
            consumer.Completion.Wait();
            outputTxtfile.Close();
            outputPbfStream.Dispose();
        }
    }
}

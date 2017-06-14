using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Ingest;
using NUnit.Framework;

namespace Ndx.Test
{
    /// <summary>
    /// This class test the whole Ingest Pcap processing pipeline.
    /// </summary>
    [TestFixture]
    public class LoadFromTest
    {
        TestContext testContext = TestContext.CurrentContext;
        [Test]
        public void LoadTestFile()
        {
            //var input = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\http.cap");
            var input = @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\2adc3aaa83b46ef8d86457e0209e0aa9.cap";
            var sink = new ActionBlock<Model.Conversation>(c => Console.Write(c));
            var frameSink = new ActionBlock<Model.MetaFrame>(x => Console.Write("."));
            var ct = new ConversationTracker();
            var cf = new CaptureReader(1024, 512, new System.Threading.CancellationToken());
            cf.RawFrameSource.LinkTo(ct.FrameAnalyzer, new DataflowLinkOptions() { PropagateCompletion = true });
            ct.ConversationBuffer.LinkTo(sink, new DataflowLinkOptions() { PropagateCompletion = true });
            ct.MetaframeBuffer.LinkTo(frameSink, new DataflowLinkOptions() { PropagateCompletion = true });

            cf.ReadFrom(new FileInfo(input));
            Task.WaitAll(ct.Completion);
        }
    }
}

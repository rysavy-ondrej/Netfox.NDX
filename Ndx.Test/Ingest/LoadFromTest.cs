using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Captures;
using Ndx.Ingest;
using Ndx.Model;
using NUnit.Framework;

namespace Ndx.Test
{
    /// <summary>
    /// This class test the whole Ingest Pcap processing pipeline.
    /// Pipelines usually consists of:
    /// source ---> filter --->  ... ---> filter ---> sink
    /// </summary>
    [TestFixture]
    public class LoadFromTest
    {
        TestContext m_testContext = TestContext.CurrentContext;
        [Test]
        public async Task LoadTestFile()
        {
            var conversations = new HashSet<int>();
            var frameCount = 0;

            var source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap");
            var tracker = new ConversationTracker();
            var filter = new TransformBlock<Frame, Frame>(x=> { var c = tracker.ProcessFrame(x); x.ConversationId = c.ConversationId; return x; });
            var sink = new ActionBlock<Frame>(x => { frameCount++; conversations.Add(x.ConversationId); });
            filter.LinkTo(sink, new DataflowLinkOptions() { PropagateCompletion = true });

            await PcapReader.ReadFile(source).ForEachAsync(async f => await filter.SendAsync(f));
            filter.Complete();
            await sink.Completion;
            Assert.AreEqual(3, conversations.Count);
            Assert.AreEqual(43, frameCount);
        }
    }
}

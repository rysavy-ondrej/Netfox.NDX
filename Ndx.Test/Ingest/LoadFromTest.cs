using System;
using System.Collections.Generic;
using System.IO;
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
    /// </summary>
    [TestFixture]
    public class LoadFromTest
    {
        TestContext m_testContext = TestContext.CurrentContext;
        [Test]
        public void LoadTestFile()
        {
            var conversations = new HashSet<Conversation>(new Conversation.ReferenceComparer());
            var frameCount = 0;
            var input = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap");
            var sink = new ActionBlock<KeyValuePair<Conversation, MetaFrame>>(x => { frameCount++; conversations.Add(x.Key); });
            var ct = new ConversationTracker();
            ct.Output.LinkTo(sink, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach(var f in PcapReader.ReadFile(input))
            {
                ct.Input.Post(f);
            }
            ct.Input.Complete();
            Task.WaitAll(sink.Completion);
            Assert.AreEqual(3, conversations.Count);
            Assert.AreEqual(43, frameCount);
        }
    }
}

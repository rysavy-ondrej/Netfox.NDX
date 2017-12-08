using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Ndx.Captures;
using Ndx.Ipflow;
using Ndx.Model;
using NUnit.Framework;
using System.Reactive.Threading.Tasks;

namespace Ndx.Test
{
    /// <summary>
    /// This test uses NFX Object Pile to store conversations and frame metadata.
    /// https://github.com/aumcode/nfx/tree/master/Source/NFX/ApplicationModel/Pile
    /// </summary>
    [TestFixture]
    public class ConversationsTests
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string captureFile = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap");

        [Test]
        public async Task ConversationsTests_TrackConversations()
        {
            var frameAnalyzer = new FrameFlowHelper();
            var frames = PcapFile.ReadFile(captureFile).AsObservable();
            var tracker = new ConversationTracker<Frame>(frameAnalyzer);

            var framesCountTask = tracker.Sum(x => 1).SingleAsync().ToTask();
            var conversationCountTask = tracker.ClosedConversations.Sum(x => 1).SingleAsync().ToTask();
            using (frames.Subscribe(tracker))
            {
                Task.WaitAll(framesCountTask, conversationCountTask);
            }
            Assert.AreEqual(3, conversationCountTask.Result);
            Assert.AreEqual(43, framesCountTask.Result);
            // wait for completion            
            Console.WriteLine($"Done, conversations = {conversationCountTask.Result}, frames = {framesCountTask.Result}");
        }
    }
}

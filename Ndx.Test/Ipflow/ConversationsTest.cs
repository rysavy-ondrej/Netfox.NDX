using Ndx.Captures;
using Ndx.Ipflow;
using Ndx.Model;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Ndx.Test
{
    [TestFixture]
    public class ConversationsTests
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string captureFile = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap");

        [Test]
        public async Task ConversationsTests_TrackConversations()
        {
            
            var frames = PcapFile.ReadFile(captureFile);
            var tracker = new ConversationTracker<Frame>(new FrameFlowHelper());

            var framesCountTask = tracker.Count().ToTask();
            var conversationCountTask = tracker.ClosedConversations.Count().ToTask();

            using (frames.Subscribe(tracker))
            {
                var framesCount = await framesCountTask;
                var conversationCount = await conversationCountTask;
                
                Assert.AreEqual(3, conversationCount);
                Assert.AreEqual(43, framesCount);
                Console.WriteLine($"Done, conversations = {conversationCount}, frames = {framesCount}");
            }
        }

        [Test]
        public async Task ConversationsTests_TrackConversations_Alternative()
        {

            var frames = PcapFile.ReadFile(captureFile);
            var tracker = new ConversationTracker<Frame>(new FrameFlowHelper());

            var frameTask = tracker.Count().ForEachAsync(x => { Assert.AreEqual(43, x); Console.WriteLine($" f={x}"); });
            var convTask = tracker.ClosedConversations.Count().ForEachAsync(x => { Assert.AreEqual(3, x); Console.WriteLine($" c={x}"); });

            using (frames.Subscribe(tracker))
            {   // at this moment both task should be completed, because work is scheduled on the!
                Console.WriteLine("!");
                await frameTask;
                await convTask;
                Console.WriteLine("@");
            }
            // Output will be:
            // f=43
            // c=3
            // !
            // @
            
        }

        [Test]
        public async Task ConversationsTests_TrackConversations_Scheduler()
        {

            var frames = PcapFile.ReadFile(captureFile);
            var tracker = new ConversationTracker<Frame>(new FrameFlowHelper());

            var frameTask = tracker.Count().ForEachAsync(x => { Assert.AreEqual(43, x); Console.WriteLine($" f={x}"); });
            var convTask = tracker.ClosedConversations.Count().ForEachAsync(x => { Assert.AreEqual(3, x); Console.WriteLine($" c={x}"); });

            using (frames.SubscribeOn(Scheduler.Default).Subscribe(tracker))
            {
                // current thread continues 
                Console.WriteLine("!");
                // so it is necessary to wait for completion:
                await frameTask;
                await convTask;
                Console.WriteLine("@");
                // Output will be:
                // !
                // f=43
                // c=3
                // @
            }
        }

    }
}

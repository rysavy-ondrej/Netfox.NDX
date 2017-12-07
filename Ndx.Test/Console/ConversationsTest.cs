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

            var conversationCount = 0;
            var framesCount = 0;

            var observer1 = new Ndx.Utils.Observer<Conversation>((_) => conversationCount++);
            var observer2 = new Ndx.Utils.Observer<(int,Frame)>((_) => framesCount++);
            using (tracker.Conversations.Subscribe(observer1))
            using (tracker.Packets.Subscribe(observer2))
            using (frames.Subscribe(tracker))
            {           
            }

            Assert.AreEqual(conversationCount, 3);
            Assert.AreEqual(framesCount, 43);
            // wait for completion            
            Console.WriteLine($"Done, conversations = {conversationCount}, frames = {framesCount}");
        }       
    }
}

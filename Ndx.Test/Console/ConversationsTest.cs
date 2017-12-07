using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Ndx.Captures;
using Ndx.Ingest;
using Ndx.Model;
using NUnit.Framework;
using Ndx.Ingest.Tracker;

namespace Ndx.Test
{
    /// <summary>
    /// This test uses NFX Object Pile to store conversations and frame metadata.
    /// https://github.com/aumcode/nfx/tree/master/Source/NFX/ApplicationModel/Pile
    /// </summary>
    [TestFixture]
    public class ConversationsTest
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string captureFile = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap");
        string converFile = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.conv");
       
        [Test]
        public async Task Conversations_TrackConversations_Linq()
        {
            var frameAnalyzer = new FrameFlowHelper();
            var frames = PcapFile.ReadFile(captureFile);
            var tracker = new ConversationTracker<Frame>(frameAnalyzer);
            var observer = new Ndx.Utils.Observer<Conversation>(Console.WriteLine);
            using (tracker.Conversations.Subscribe(observer))
            {
                await frames.Select(x => { var c = tracker.ProcessRecord(x); x.ConversationId = c.ConversationId; return x; }).Where(x => x != null).ForEachAsync(Console.WriteLine);
                tracker.Complete();
            }
            
        }

        [Test]
        public async Task Conversations_TrackConversations_WriteTo()
        {
            var conversationsFilename = Path.ChangeExtension(captureFile, "conversations");
            var framesFilename = Path.ChangeExtension(captureFile, "frames");

            using (var conversationStream = File.Create(conversationsFilename))
            using (var frameStream = File.Create(framesFilename))
            {
                var frameAnalyzer = new FrameFlowHelper();
                var tracker = new ConversationTracker<Frame>(frameAnalyzer);
                tracker.Conversations.Subscribe(conversation => conversation.WriteDelimitedTo(conversationStream));

                var frames = PcapFile.ReadFile(captureFile);
                Frame ProcessFrame(Frame f)
                {
                    var c = tracker.ProcessRecord(f);
                    f.ConversationId = c.ConversationId;
                    return f;
                }
                await frames.Select(ProcessFrame).Where(x => x != null).ForEachAsync(frame => frame.WriteDelimitedTo(frameStream));
                tracker.Complete();
            }
        }
    }
}

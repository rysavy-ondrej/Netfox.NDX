using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Model;

namespace Ndx.TShark
{
    public class TSharkBlock : IPropagatorBlock<Frame, DecodedFrame>
    {
        private ActionBlock<Frame> m_inputBlock;
        private BufferBlock<DecodedFrame> m_outputBlock;
        private TSharkSender m_wsender;
        private TSharkProcess m_tshark;

        public TSharkBlock(TSharkProcess tsharkProcess, DataLinkType datalinkType = DataLinkType.Ethernet)
        {
            var m_pipename = $"ndx.tshark_{new Random().Next(Int32.MaxValue)}";

            m_wsender = new TSharkSender(m_pipename, datalinkType);

            m_inputBlock = new ActionBlock<Frame>(SendFrame);
            m_inputBlock.Completion.ContinueWith((t) => m_wsender.Close());

            m_outputBlock = new BufferBlock<DecodedFrame>();

            // create and initialize TSHARK:
            m_tshark = tsharkProcess;
            m_tshark.PipeName = m_pipename;
            m_tshark.PacketDecoded += PacketDecoded;
            m_tshark.Start();
            m_tshark.Completion.ContinueWith((t) => m_outputBlock.Complete());

            m_wsender.Connected.Wait();
        }

        private async Task SendFrame(Frame rawFrame)
        {
            await m_wsender.SendAsync(rawFrame);
        }

        private void PacketDecoded(object sender, DecodedFrame e)
        {
            m_outputBlock.Post(e);
        }

        public Task Completion => m_outputBlock.Completion;

        public void Complete()
        {
            m_inputBlock.Complete();
        }

        public DecodedFrame ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<DecodedFrame> target, out bool messageConsumed)
        {
            return ((ISourceBlock<DecodedFrame>)m_outputBlock).ConsumeMessage(messageHeader, target, out messageConsumed);
        }

        public void Fault(Exception exception)
        {
            m_wsender.Close();
            m_tshark.Kill();
            m_outputBlock.Complete();
        }

        public IDisposable LinkTo(ITargetBlock<DecodedFrame> target, DataflowLinkOptions linkOptions)
        {
            return ((ISourceBlock<DecodedFrame>)m_outputBlock).LinkTo(target, linkOptions);
        }

        public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, Frame messageValue, ISourceBlock<Frame> source, bool consumeToAccept)
        {
            return ((ITargetBlock<Frame>)m_inputBlock).OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        public async Task ConsumeAsync(IObservable<Frame> frames)
        {
            await frames.ForEachAsync(async frame =>
            {
                await m_inputBlock.SendAsync(frame);
            });
            m_inputBlock.Complete();
        }

        /// <summary>
        /// Consumes all frames from the provided collection and completes.
        /// </summary>
        /// <param name="frames">A collection of frames to be processed.</param>
        public void Consume(IEnumerable<Frame> frames)
        {
            foreach (var frame in frames)
            {
                m_inputBlock.Post(frame);
            }
            m_inputBlock.Complete();
        }
        /// <summary>
        /// Consumes all frames from the provided collection and completes.
        /// </summary>
        /// <param name="frames">A collection of frames to be processed.</param>
        /// <returns><see cref="Task"/> object that completes when this operation is done.</returns>
        public async Task ConsumeAsync(IEnumerable<Frame> frames)
        {
            foreach (var frame in frames)
            {
                await m_inputBlock.SendAsync(frame);
            }
            m_inputBlock.Complete();
        }

        public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<DecodedFrame> target)
        {
            ((ISourceBlock<DecodedFrame>)m_outputBlock).ReleaseReservation(messageHeader, target);
        }

        public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<DecodedFrame> target)
        {
            return ((ISourceBlock<DecodedFrame>)m_outputBlock).ReserveMessage(messageHeader, target);
        }
    }
}
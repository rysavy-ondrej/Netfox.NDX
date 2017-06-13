using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Captures;
using Ndx.Model;

namespace Ndx.Metacap
{
    /// <summary>
    /// This is a provider of raw frames to be pumped to dataflow pipeline.
    /// </summary>
    public sealed class CaptureReader
    {
        int m_framesCount;

        int m_bufferSize;

        IPropagatorBlock<FileInfo, RawFrame> m_dataflowblock;

        CancellationToken m_cancellationToken;

        public int FramesCount => m_framesCount;


        /// <summary>
        /// The target of the <see cref="FileInfo"/> objects.
        /// </summary>
        public ITargetBlock<FileInfo> FileInfoTarget => m_dataflowblock;

        /// <summary>
        /// The source of <see cref="RawFrame"/> data.
        /// </summary>
        public ISourceBlock<RawFrame> RawFrameSource => m_dataflowblock;

        public CaptureReader(int inputBufferSize, int bufferCapacity, CancellationToken ct)
        {
            m_bufferSize = inputBufferSize;
            m_dataflowblock = GetBlock(bufferCapacity);
            m_cancellationToken = ct;
        }

        IPropagatorBlock<FileInfo, RawFrame> GetBlock(int capacity)
        {
            var opt = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = capacity
            };

            var source = new BufferBlock<RawFrame>(opt);

            async Task ReadFramesAsync(FileInfo fileInfo)
            {
                if (fileInfo.Exists)
                {
                    foreach (var frame in PcapReader.ReadFile(fileInfo.FullName, m_bufferSize))
                    {
                        if (frame != null)
                        {
                            m_framesCount++;
                            await source.SendAsync(frame, m_cancellationToken);
                        }
                        if (m_cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
            }

            var target = new ActionBlock<FileInfo>(ReadFramesAsync);

            target.Completion.ContinueWith(completion =>
            {
                if (completion.IsFaulted)
                {
                    ((IDataflowBlock)source).Fault(completion.Exception);
                }
                else
                {
                    source.Complete();
                }
            });

            return DataflowBlock.Encapsulate(target, source);
        }

        public void Complete()
        {
            m_dataflowblock.Complete();
        }

        /// <summary>
        /// Adding a new source of the frames.
        /// </summary>
        /// <param name="fileInfo"></param>
        public void ReadFrom(FileInfo fileInfo)
        {
            m_dataflowblock.SendAsync(fileInfo);
        }
    }
}

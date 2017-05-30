using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
namespace Ndx.Metacap
{
    public sealed class PcapReaderProvider
    {
        int m_framesCount;

        int m_bufferSize;

        IPropagatorBlock<FileInfo, RawFrame> m_dataflowblock;

        CancellationToken m_cancellationToken;

        public int FramesCount => m_framesCount;

        public ITargetBlock<FileInfo> FileInfoTarget => m_dataflowblock;

        public ISourceBlock<RawFrame> RawFrameSource => m_dataflowblock;

        public PcapReaderProvider(int inputBufferSize, int bufferCapacity, CancellationToken ct)
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
                            break;
                    }
                }
            }

            var target = new ActionBlock<FileInfo>(ReadFramesAsync);

            target.Completion.ContinueWith(completion =>
            {
                if (completion.IsFaulted) ((IDataflowBlock)source).Fault(completion.Exception);
                else source.Complete();
            });

            return DataflowBlock.Encapsulate(target, source);
        }

        public void Complete()
        {
            m_dataflowblock.Complete();
        }

        public void ReadFrom(FileInfo fileInfo)
        {
            m_dataflowblock.SendAsync(fileInfo);
        }

    }
}

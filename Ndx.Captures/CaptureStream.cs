using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Captures
{

    /// <summary>
    /// This interface must be implemented by all stream providers for <see cref="CaptureStream"/>.
    /// It enables to efficiently manage underlying streams, e.g., use caching to limit 
    /// a number of open streams.
    /// </summary>
    public interface IStreamProvider : IDisposable
    {
        Stream EnterStream(int streamid);
        void LeaveStream(Stream stream);
    }
    /// <summary>
    /// This class provides access to possible multiple underlying captures in a uniform way of Stream class.
    /// The stream is readonly.
    /// </summary>
    public class CaptureStream : Stream
    {
        long m_position;
        Stream m_activeStream;
        IStreamProvider m_streamProvider;
        public CaptureStream(IStreamProvider streamProvider)
        {
            m_position = 0;
            m_streamProvider = streamProvider;
            Seek(m_position, SeekOrigin.Begin);            
        }


        class SingleStreamProvider : IStreamProvider
        {
            Stream m_stream;
            internal SingleStreamProvider(Stream stream)
            {
                m_stream = stream;
            }
            public Stream EnterStream(int streamid)
            {
                return m_stream;
            }

            public void Dispose()
            {
                m_stream.Dispose();
            }

            public void LeaveStream(Stream stream)
            {
                
            }
        }

        public static CaptureStream CreateSingleCapture(Stream stream)
        {
            return new CaptureStream(new SingleStreamProvider(stream));
        }

        public static long GetPointer(int index, long offset) => ((long)index << 40) + (long)offset;

        public static int GetStreamIndex(long pointer) => (int)((long)pointer >> 40);

        public static long GetInStreamOffset(long pointer) => (long)(pointer & 0xffffffffff);        

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => m_position; set => Seek(value, SeekOrigin.Begin); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read bytes form the stream. If reads all bytes in the current underlying stream than further reads will read 0 bytes till Seek is used to set the new position.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns>Number of bytes read or 0 if underlying stream reached its end.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_activeStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin != SeekOrigin.Begin)
            {
                throw new ArgumentException("Only SeekOrigin.Begin is supported by CaptureStream.");
            }

            var requestedOffset = GetInStreamOffset(offset);
            var requestedStream = GetStreamIndex(offset);
            var currentFile = GetStreamIndex(m_position);

            if (m_activeStream == null || requestedStream != currentFile)
            {
                try
                {
                    if (m_activeStream != null)
                    {
                        m_streamProvider.LeaveStream(m_activeStream);
                    }
                    m_activeStream = m_streamProvider.EnterStream(requestedStream);
                }
                catch (Exception e)
                {
                    throw new ObjectDisposedException($"Underlying stream {requestedStream} is not available", e);
                }
            }
            var newOffset = m_activeStream.Seek(requestedOffset, SeekOrigin.Begin);
            m_position = GetPointer(requestedStream, newOffset);
            return m_position;
        }


        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (m_activeStream != null)
            {
                m_streamProvider.LeaveStream(m_activeStream);
            }
            base.Dispose(disposing);
            m_streamProvider.Dispose();
        }
    }
}

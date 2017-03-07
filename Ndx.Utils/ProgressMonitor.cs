//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Threading.Tasks;
namespace Ndx.Utils
{
    using System.Diagnostics;
    using System.Threading;
    public sealed class ProgressMonitor : IDisposable
    {
        long m_totalBytes;
        long m_processedBytes;
        long m_totalItems;
        int m_processedItems;
        int m_refreshInterval = 1000;
        CancellationTokenSource m_cancellationTokenSource;
        Task m_monitorTask;
        public ProgressMonitor(int totalItems, long totalSize, bool start=true)
        {
            m_totalItems = totalItems;
            m_totalBytes = totalSize;
            m_cancellationTokenSource = new CancellationTokenSource();
            if (start) Start();
        }

        public void Start()
        {
            if (m_monitorTask == null)
            {
                m_monitorTask = ProgressInfoTaskAsync(m_cancellationTokenSource.Token);
            }
        }

        public void Stop()
        {
            if (m_monitorTask != null && m_monitorTask.Status == TaskStatus.Running)
            {
                this.m_cancellationTokenSource.Cancel();
            }
        }

        public void Increase(int items, long bytes)
        {
            Interlocked.Add(ref m_processedItems, items);
            Interlocked.Add(ref m_processedBytes, bytes);
        }

        public long ProcessedBytes { get { return m_processedBytes; } }

        public long TotalBytes { get { return m_totalBytes; } }

        public long ProcessedItems { get { return m_processedItems; } }

        public long TotalItems { get { return m_totalItems; } }

        /// <summary>
        /// Gets or sets the refresh interval. It must be between 100ms and 1 minute.
        /// </summary>
        public int RefreshInterval
        {
            get { return m_refreshInterval; }
            set
            {
                if(value <= 60*1000 && value >= 100 )
                {
                    m_refreshInterval = value;
                }
            }
        }

        public long AvgByteSpeed { get { return m_stopwatch != null && m_stopwatch.ElapsedMilliseconds > 0 ? (1000 * m_processedBytes) / m_stopwatch.ElapsedMilliseconds : 0; } }
        public object AvgItemSpeed { get { return m_stopwatch != null && m_stopwatch.ElapsedMilliseconds > 0 ? (1000 * m_processedItems) / m_stopwatch.ElapsedMilliseconds : 0; } }

        Stopwatch m_stopwatch;
        private async Task ProgressInfoTaskAsync(CancellationToken cancellationToken)
        {
            m_stopwatch = new Stopwatch();
            m_stopwatch.Start();
            var lastItems = 0;
            var lastBytes = 0L;
            while (!cancellationToken.IsCancellationRequested)
            {
                var processedBytes = Utils.Format.ByteSize(m_processedBytes);
                var totalBytes = m_totalBytes > 0 ? Utils.Format.ByteSize(m_totalBytes) : "?";
                var byteSpeed = (long)(m_processedBytes - lastBytes) / ((long)RefreshInterval / 1000);

                var processedItems = m_processedItems.ToString();
                var totalItems = m_totalItems > 0 ? m_totalItems.ToString() : "?";
                var itemSpeed = (long)(m_processedItems - lastItems) / ((long)RefreshInterval / 1000);


                Console.Write($"\rTime elapsed: {m_stopwatch.ElapsedMilliseconds} ms, {processedItems} of {totalItems}, {itemSpeed} records/s, {processedBytes} of {totalBytes}, {Utils.Format.ByteSize(byteSpeed)}/s.                    ");
                lastBytes = m_processedBytes;
                lastItems = m_processedItems;
                await Task.Delay(RefreshInterval, cancellationToken);
            }
            m_stopwatch.Stop();
        }

        public void Close()
        {
            if (m_monitorTask!=null && m_monitorTask.Status == TaskStatus.Running)
            {
                this.m_cancellationTokenSource.Cancel();
            }

        }

        public void Dispose()
        {
            Close();
            m_cancellationTokenSource.Dispose();
        }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ndx.Ingest.Trace;
using Ndx.Shell.Commands;

namespace Ndx.Tools.Metacap
{

    class CreateIndexCommand : Command
    {
        private string m_inputPath;
        private string m_outputPath;
        private string m_filter;

        /// <summary>
        /// Gets or sets the path to the input PCAP file.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string InputPath { get => m_inputPath; set => m_inputPath = value; }

        /// <summary>
        /// Gets or sets the path to the input PCAP file.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string OutputPath { get => m_outputPath; set => m_outputPath = value; }

        [Parameter(Mandatory = false)]
        public string Filter { get => m_filter; set => m_filter = value; }

        protected override void BeginProcessing()
        {
        }



        protected override void EndProcessing()
        {
        }


        protected override void ProcessRecord()
        {
            var filterFun = FilterHelper.GetFilterFunction(m_filter);
            using (var consumer = new McapFileConsumer(m_outputPath))
            {
                var cts = new CancellationTokenSource();
                var reader = new PcapReaderProvider(32768, 1000, cts.Token);

                var ingestOptions = new IngestOptions() { FlowFilter = filterFun };
                var ingest = new PcapFileIngestor(reader.RawFrameSource, consumer.RawFrameTarget, consumer.PacketBlockTarget, consumer.FlowRecordTarget, ingestOptions);

                var fileInfo = new FileInfo(m_inputPath);
                reader.ReadFrom(fileInfo);
                reader.Complete();

                Task.WaitAll(ingest.Completion, consumer.Completion);
            }
        }
    }
}

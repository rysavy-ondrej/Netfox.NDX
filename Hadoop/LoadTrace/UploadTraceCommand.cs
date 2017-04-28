using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using Ndx.Ingest.Trace;
using Ndx.Shell.Commands;
using Microsoft.Hadoop;
namespace Ndx.Hadoop.LoadTrace
{
    /// <summary>
    /// This class implements a command that exports metacap data to the Cassandra DB.
    /// </summary>
    [Command("Upload", "Trace")]
    class UploadTraceCommand : Command
    {
        private string m_capfile;
        private string m_targetPath;
        bool m_readyToRun = false;
        private string m_cluster;

        [Parameter(Mandatory = true)]
        public string HadoopCluster { get => m_cluster; set => m_cluster = value; }

        [Parameter(Mandatory = true)]
        public string Capfile { get => m_capfile; set => m_capfile = value; }

        [Parameter(Mandatory = true)]
        public string TargetPath { get => m_targetPath; internal set => m_targetPath = value; }


        protected override void BeginProcessing()
        {
            //m_hadoop = Microsoft.Hadoop.MapReduce.Hadoop.Connect(m_cluster,)
        }

        protected override void EndProcessing()
        {
        }

     
        protected override void ProcessRecord()
        {
            if (!m_readyToRun) return;
        }
    }
}

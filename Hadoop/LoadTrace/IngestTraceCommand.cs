using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using Ndx.Ingest.Trace;
using Ndx.Shell.Commands;

namespace Ndx.Hadoop.LoadTrace
{
    [Command("Ingest", "Trace")]
    class IngestTraceCommand : Command 
    {
        private string m_capfile;
        private McapFile m_mcap;
        private string m_cassandraHost;
        private int m_cassandraPort;
        private Cluster m_cluster;
        private ISession m_session;
        private DbAccess m_access;
        private string m_keyspace;
        private bool m_readyToRun;

        protected override void BeginProcessing()
        {
            try
            {
                var mcapfile = Path.ChangeExtension(m_capfile, "mcap");
                if (m_capfile == null)
                {
                    throw new FileNotFoundException($"File '{mcapfile}' cannot be found.");
                }

                m_mcap = McapFile.Open(mcapfile, m_capfile);
                m_cluster = Cluster.Builder().AddContactPoint(m_cassandraHost).WithPort(m_cassandraPort).Build();
                m_session = m_cluster.Connect();
                m_access = new DbAccess(m_session);

                var createScript = Resources.DatabaseCreate.Replace("${KEYSPACE}", m_keyspace);
                m_session.Execute(createScript);

                m_readyToRun = true;
            }
            catch (Exception e)
            {
                WriteError(e, "Cannot process inout file.");
            }
        }
    }
}

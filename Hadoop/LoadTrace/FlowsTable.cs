using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using Ndx.Metacap;

namespace Ndx.Hadoop.LoadTrace
{
    public class DbAccess
    {
        PreparedStatement m_insertFlow;
        public DbAccess(ISession session)
        {
            m_insertFlow = session.Prepare(
                "insert into flows(flowId, pcapUri, protocol, sourceAddress, destinationAddress, sourcePort, destinationPort, service)" +
                "values (:flowId, :flowPcap, :protocol, :sourceAddress, :destinationAddress, :sourcePort, :destinationPort, :service)"
                );
        }

        public IStatement InsertFlow(int id, string pcapUri, FlowRecord flow)
        {
            var st = m_insertFlow.Bind(id, pcapUri, flow.Key.Protocol, flow.Key.SourceAddress, flow.Key.DestinationAddress, flow.Key.SourcePort, flow.Key.DestinationPort, flow.RecognizedProtocol);
            return st;                
        }


    }
}

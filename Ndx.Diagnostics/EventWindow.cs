using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Diagnostics
{

    /// <summary>
    /// 
    /// </summary>
    public class TimeWindow
    {
        long m_from;
        long m_to;
        public TimeWindow(long from, long to)
        {
            m_from = from;
            m_to = to;
        }

        public bool In(long timestamp)
        {
            return timestamp >= m_from && timestamp <= m_to;
        }

        public (T, bool) In<T>(T value, long timestamp)
        {
            if (timestamp >= m_from && timestamp <= m_to)
            {
                return (value, true);
            }
            else
            {
                return (value, false);
            }
        }        
    }

}

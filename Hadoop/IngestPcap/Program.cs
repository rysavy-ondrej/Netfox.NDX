using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestPcap
{
    /// <summary>
    /// This program represents a Spark job that ingests Pcap files dropped in the specified input folder into 
    /// the Hadoop and creates an index in CassandraDB.
    /// Usage: IngestPcap INPUT-FOLDER
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: IngestPcap INPUT-FOLDER");
                return;
            }


        }
    }
}

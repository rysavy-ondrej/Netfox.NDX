using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Spark.CSharp.Core;
using Microsoft.Spark.CSharp.Streaming;

namespace Ndx.Hadoop
{
    /// <summary>
    /// This program represents a Spark job that ingests network trace files from the specified folder to 
    /// the Hadoop and creates an index in CassandraDB.
    /// Usage: IngestPcap INPUT-FOLDER
    /// </summary>
    class IngestNetworkTrace
    {
        public string[] Input { get; private set; }

        static void Main(string[] args)
        {

            var execName = System.AppDomain.CurrentDomain.FriendlyName;

            if (args.Length < 2)
            {
                Console.WriteLine($"Usage: {execName} <checkpointDirectory> <inputDirectory>");
                return;
            }

            string checkpointPath = args[0];
            string inputDir = args[1];

                      
            var worker = new IngestNetworkTrace();
            worker.Input = worker.GetSourceFiles(inputDir);
            var ssc = StreamingContext.GetOrCreate(checkpointPath, worker.CreateContext);
//            ssc.Checkpoint(checkpointPath);
            ssc.Start();
            ssc.AwaitTermination();
            ssc.Stop();
        }

        private StreamingContext CreateContext()
        {
            var sparkConf = new SparkConf();
            sparkConf.SetAppName("IngestNetworkTrace");
            var sc = new SparkContext(sparkConf);
            var context = new StreamingContext(sc, 30000);

            return context;
        }


        /// <summary>
        /// Gets the list of files that resides in the specified directory.
        /// </summary>
        /// <param name="path">Path that can be a local path or URI.</param>
        /// <returns></returns>
        string []GetSourceFiles(string path)
        {
            var uri = new Uri(path);
            if (uri.IsFile)
            {
                var searchPath = uri.LocalPath;
                var files = Directory.GetFiles(searchPath);
                return files;
            }
            if (uri.IsUnc)
            {

            }
            return new string[] { };
        }

    }
}

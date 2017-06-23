using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.TShark
{
    class TSharkProcess
    {


        public TSharkProcess()
        {
            m_fields = new List<string>();
        }

        string m_pipeName = "tshark";

        List<string> m_fields;
        private Process m_tsharkProcess;

        public IList<string> Fields { get => m_fields; }
        public string PipeName { get => m_pipeName; set => m_pipeName = value; }

        Process Start()
        {
            try
            {
                m_tsharkProcess = new Process();
                m_tsharkProcess.StartInfo.FileName = @"C:\Program Files\Wireshark\tshark.exe";
                var pipeName = $@"\\.\pipe\{m_pipeName}";
                m_tsharkProcess.StartInfo.Arguments = $"-i {pipeName} -T json {m_fields.Select(x=>$"-e {x}")}";
                m_tsharkProcess.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
                m_tsharkProcess.StartInfo.RedirectStandardOutput = true;
                m_tsharkProcess.StartInfo.UseShellExecute = false;
                m_tsharkProcess.Start();
                var myStreamReader = m_tsharkProcess.StandardOutput;
                string myString = myStreamReader.ReadLine(); //read the standard output of the spawned process. 
                Console.WriteLine(myString);
                return m_tsharkProcess;
            }
            catch (Exception)
            {
                return null;
            }
        }

        void Stop()
        {
            m_tsharkProcess?.StandardInput.Close();
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}

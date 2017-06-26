using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;

namespace Ndx.TShark
{
    public class TSharkProcess
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

        public bool Start()
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = @"C:\Program Files\Wireshark\tshark.exe";
                var pipeName = $@"\\.\pipe\{m_pipeName}";
                var fields = String.Join(" ",m_fields.Select(x => $"-e {x}"));
                process.StartInfo.Arguments = $"-i {pipeName} -T json -e frame.number -e frame.protocols {fields}";
                process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
                process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardInput = true;
                //process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.BeginOutputReadLine();
                m_tsharkProcess = process;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsRunning => !(m_tsharkProcess?.HasExited ?? true);

        public Task<int> GetCompletionTask(int samplingMs=1000) => new Task<int>(() =>
                                                     {
                                                         while (!m_tsharkProcess.WaitForExit(samplingMs)) ;
                                                         return m_tsharkProcess.ExitCode;
                                                     });

        public void Close()
        {
            if (m_tsharkProcess != null)
            {
                m_tsharkProcess.WaitForExit();
                m_tsharkProcess.Close();
            }
        }

        public void Kill()
        {
            m_tsharkProcess.Kill();
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var packetFields = new PacketFields();
            // Data are provided line by line, thus we need to initialize JSON parser first, than convert JSON 
            // result to packetFields collection.
           e.Data
        }

        MemoryStream m_jsonStream = new MemoryStream();

        /// <summary>
        /// This list contains the result of TShark processing.
        /// </summary>
        List<PacketFields> m_packetFieldsCollection;
    }
}

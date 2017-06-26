using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ndx.TShark
{
    public class TSharkProcess
    {
        MemoryStream m_jsonStream;
        TextWriter m_writer;
        public TSharkProcess()
        {
            m_fields = new List<string>();
            m_jsonStream = new MemoryStream();
            m_writer = new StreamWriter(m_jsonStream, Encoding.UTF8, 1024, true);
        }

        string m_pipeName = "tshark";

        List<string> m_fields;
        private Process m_tsharkProcess;
        private bool m_finished;

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
                         
        public void Close()
        {
            if (m_tsharkProcess != null)
            {
                m_tsharkProcess.WaitForExit();
                m_tsharkProcess.Close();
            }
            if (m_writer != null)
            {
                m_writer.Close();
            }
            m_finished = true;
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
            m_writer.WriteLine(e.Data);
            // http://www.newtonsoft.com/json/help/html/LINQtoJSON.htm
        }


        public IEnumerable<PacketFields> Result
        {
            get
            {
                if (!m_finished) throw new InvalidOperationException("TShark process not finished, cannot get results."); 
                m_jsonStream.Position = 0;
                var testReader = new StreamReader(m_jsonStream);
                var jsonReader = new JsonTextReader(testReader);
                var jsonObject = JObject.ReadFrom(jsonReader);
                var col = jsonObject.Select(x => x["_source"]["layers"].ToDictionary(y => ((JProperty)y).Name, y => ((JProperty)y).Value));
                return col.Select(entries =>
                {
                    var result = new PacketFields() {
                        FrameNumber = (int)entries["frame.number"].First,
                        FrameProtocols = (string)entries["frame.protocols"].First };
                    foreach (var entry in entries)
                    {
                        result.Fields.Add(entry.Key, (string)entry.Value.First);
                    }
                    return result;
                }); 
            }   
        }
    }
}

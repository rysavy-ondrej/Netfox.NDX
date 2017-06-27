using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ndx.TShark
{
    public static class Field
    {
        public static class Frame
        {
            public const string Number = "frame.number";
            public const string Protocols = "frame.protocols";
        }
    }

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

        public IList<string> Fields { get => m_fields; }
        public string PipeName { get => m_pipeName; set => m_pipeName = value; }

        public bool Start()
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = @"C:\Program Files\Wireshark\tshark.exe";
                var pipeName = $@"\\.\pipe\{m_pipeName}";
                var fields = String.Join(" ", m_fields.Select(x => $"-e {x}"));
                process.StartInfo.Arguments = $"-i {pipeName} -T json -e {Field.Frame.Number} -e {Field.Frame.Protocols} {fields}";
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
        }

        public void Kill()
        {
            m_tsharkProcess.Kill();
        }


        /// <summary>
        /// This event is called when the packet was decoded.
        /// </summary>
        public event EventHandler<PacketFields> PacketDecoded;

        private void OnPacketDecoded(PacketFields packetFields)
        {
            PacketDecoded?.Invoke(this, packetFields);
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // [
            //   {
            //      "_index": "packets-2017-06-23",
            //      "_type": "pcap_file",
            //      "_score": null,
            //      "_source": {
            //          "layers": {
            //
            //          }
            //      }
            //   }
            //
            //  ,
            //   {
            //     LAST OBJECTS  
            //   }
            //
            //  ]
            if (e.Data != null)
            {
                var data = e.Data.Trim();
                if (data.Equals(",") || data.Equals("]"))
                {
                    m_writer.Flush();
                    var result = GetResult();
                    OnPacketDecoded(result);
                    m_jsonStream.Position = 0;
                }
                else if (!(data.Equals("[") || String.IsNullOrEmpty(data)))
                {
                    m_writer.WriteLine(data);
                }
            }
        }

        PacketFields GetResult()
        {
            m_jsonStream.Position = 0;
            var testReader = new StreamReader(m_jsonStream);
            var jsonReader = new JsonTextReader(testReader);
            var jsonObject = JToken.ReadFrom(jsonReader);
            var fields = jsonObject["_source"]["layers"].ToDictionary(y => ((JProperty)y).Name, y => ((JProperty)y).Value);
            var result = new PacketFields();
            foreach (var field in fields)
            {
                switch(field.Key)
                {
                    case Field.Frame.Number:
                        result.FrameNumber = (int)field.Value.First;
                        break;
                    case Field.Frame.Protocols:
                        result.FrameProtocols = (string)field.Value.First;
                        break;
                    default:
                        result.Fields.Add(field.Key, (string)field.Value.First);
                        break;
                }
            }
            return result;
        }
    }
}
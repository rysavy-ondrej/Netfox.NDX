using Google.Protobuf;
using Microsoft.Extensions.CommandLineUtils;
using Ndx.Captures;
using Ndx.Decoders;
using Ndx.Decoders.Base;
using Ndx.TShark;
using Ndx.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Netdx
{
    internal class ExportTrace
    {
        public async Task ExecuteAsync(FileInfo inputFile, Stream outstream)
        {
            if (!inputFile.Exists)
            {
                throw new FileNotFoundException("Input file does not exist.", inputFile.FullName);
            }

            var sumPacketSize = 0;
            var fileSize = inputFile.Length;
            Task WritePacket(Packet packet)
            {                
                packet.WriteDelimitedTo(outstream);
                sumPacketSize += (int)(packet.Protocol<Frame>()?.FrameLen ?? 0);
                m_progressBar.Report((double)sumPacketSize / (double)fileSize);
                return Task.CompletedTask;
            }

            var random = new Random(Process.GetCurrentProcess().Id);      
            // initialize decoders
            var factory = new DecoderFactory();
            var decoder = new PacketDecoder();
            var randomPipeName = $"netdx-export-trace-{random.Next()}";
            
            // set up processing pipeline
            var tsharkProcess = new TSharkPacketDecoderProcess(randomPipeName, factory, decoder);            
            var tsharkBlock = new TSharkBlock<Packet>(tsharkProcess);
            var consumer = new ActionBlock<Packet>(WritePacket);
            tsharkBlock.LinkTo(consumer, new DataflowLinkOptions() { PropagateCompletion = true });

            // read packets 
            var frames = PcapFile.ReadFile(inputFile.FullName);
            await tsharkBlock.ConsumeAsync(frames);

            await consumer.Completion;
            m_progressBar.Report(1);
            outstream.Flush();
        }

        internal static string Name = "Export-Trace";
        private ProgressBar m_progressBar;

        public ExportTrace(ProgressBar pb)
        {
            this.m_progressBar = pb;
        }

        internal static Action<CommandLineApplication> Register(ProgressBar pb = null)
        {
            return 
            (CommandLineApplication target) =>
            {
                var infiles = target.Argument("input", "Input trace in PCAP format. Use STDIN for reading from stdin.", true);
                var outdir = target.Option("-o|--outdir", "The output directory were to put files with decoded packets.", CommandOptionType.SingleValue);
                var outfile = target.Option("-" +"w|--writeTo", "The output filename were to put decoded packets. If multiple files are used, the output is concatenated in this single file.", CommandOptionType.SingleValue);
                target.Description = "Prepares data for further processing by NDX tools.";
                target.HelpOption("-?|-h|--help");
                

                target.OnExecute(() =>
                {
                    var debug = true;


                    if (infiles.Values.Count == 0)
                    {
                        target.Error.WriteLine("No input specified!");
                        target.ShowHelp(Name);
                        return 0;
                    }
                    var cmd = new ExportTrace(pb);
                    
                    
                    
                    Stream GetOutstream(string infile, out string outpath)
                    {
                        var pathPrefix = outdir.HasValue() ? outdir.Value() : String.Empty;
                        if (outfile.HasValue())
                        {                            
                            outpath = Path.Combine(pathPrefix, outfile.Value());
                            return File.Open(outpath, FileMode.Append, FileAccess.Write);
                        }
                        else
                        {
                            outpath = Path.ChangeExtension(Path.Combine(pathPrefix, infile), "dcap");
                            return File.Open(outpath, FileMode.OpenOrCreate, FileAccess.Write);
                        }
                    }

                    foreach (var infile in infiles.Values)
                    {                        
                        using (var outstream = GetOutstream(infile, out var filename))
                        {
                           

                            try
                            {
                                var inputFile = new FileInfo(infile);
                                Console.Write($"[{Format.ByteSize(inputFile.Length)}] {inputFile.FullName} -> {filename}: ");

                                pb.Start();
                                var task = cmd.ExecuteAsync(inputFile, outstream);
                                // Do we have something useful to do here?
                                task.Wait();
                                pb.Stop();
                                Console.WriteLine();
                            }
                            catch (Exception e)
                            {
                                target.Error.WriteLine();
                                if (!debug)
                                {
                                    target.Error.WriteLine($"ERROR: {e.Message}");
                                    target.Error.WriteLine("Use switch -d to see details about this error.");
                                }
                                else
                                {
                                    
                                    while(e.InnerException!=null)
                                    {
                                        target.Error.WriteLine($"ERROR: {e.Message}");
                                        target.Error.WriteLine($"SOURC: {e.Source}");
                                        target.Error.WriteLine($"STACK: {e.StackTrace}");
                                        target.Error.WriteLine("-------");
                                        e = e.InnerException;
                                    }                                    
                                }
                            }
                        }
                    }
                    return 0;
                });
            };
        }
    }
}

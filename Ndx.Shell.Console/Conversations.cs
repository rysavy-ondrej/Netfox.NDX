using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Ndx.Model;

namespace Ndx.Shell.Console
{
    public static class Conversations
    {
        /// <summary>
        /// Saves the conversation dictionary to the specified file. 
        /// </summary>
        /// <param name="dictionary">Conversation dictionary.</param>
        /// <param name="path">Path to the output file.</param>
        public static void WriteTo(IDictionary<Conversation, IList<MetaFrame>> dictionary, string path)
        {
            using (var output = File.Open(path, FileMode.Create, FileAccess.Write))
            {
                var conv = dictionary.First();
                {
                    conv.Key.WriteTo(output);
                    var buf = BitConverter.GetBytes(conv.Value.Count);
                    output.Write(buf, 0, buf.Length);
                    foreach (var item in conv.Value)
                    {
                        item.WriteTo(output);
                    }
                    output.Flush();
                }
            }
        }
        
        public static void ReadFrom(string path, IDictionary<Conversation, IList<MetaFrame>> dictionary)
        {
            using (var input = File.OpenRead(path))
            {
                var buf = BitConverter.GetBytes(0);
                while (input.Position < input.Length)
                {
                    System.Console.Write($"{input.Position},");
                    var conv = Conversation.Parser.ParseFrom(input);
                    System.Console.Write($"{input.Position},");
                    var list = new List<MetaFrame>();                    
                    input.Read(buf, 0, buf.Length);
                    var count = BitConverter.ToInt32(buf,0);
                    System.Console.Write($"{input.Position},");
                    for (int i = 0; i < count; i++)
                    {
                        System.Console.Write($"{input.Position},");
                        list.Add(MetaFrame.Parser.ParseFrom(input));    
                    }
                    System.Console.WriteLine($"{input.Position}.");
                    dictionary[conv] = list;
                }
            }
        }
    }
}

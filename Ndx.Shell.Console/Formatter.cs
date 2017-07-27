using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Shell.Console
{
    public static class Formatter
    {


        /// <summary>
        /// Gets the string that represents MD Table for the specified enumerable. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MdTable<T>(IEnumerable<T> data)
        {
            var sb = new StringBuilder();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            sb.AppendLine("| "+ String.Join(" | ", properties.Select(p => p.Name)) + " |");
            sb.AppendLine("| " + String.Join(" | ", properties.Select(p=>"-----")) + " |");
            foreach (var x in data)
            {
                sb.AppendLine("| " + String.Join(" | ", properties.Select(p => p.GetValue(x))) + " |");
            }
            return sb.ToString();
        }
            
    }
}

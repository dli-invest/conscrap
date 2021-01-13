using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ConScrap
{
    // add approach to parse file
    // https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualbasic.fileio.textfieldparser?redirectedfrom=MSDN&view=net-5.0
    // use similar approach to golang to check if entry is in csv
    public class Csv
    {
        /// <summary>
        ///  Generates csv file from a list of items from generic list
        /// </summary>
        public static string GenerateReport<T>(List<T> items) where T : class
        {
            var output = "";
            var delimiter = ",";
            var properties = typeof(T).GetProperties()
                .Where(n =>
                    n.PropertyType == typeof(string)
                    || n.PropertyType == typeof(bool)
                    || n.PropertyType == typeof(char)
                    || n.PropertyType == typeof(byte)
                    || n.PropertyType == typeof(decimal)
                    || n.PropertyType == typeof(int)
                );
            using (var sw = new StringWriter())
            {
                var header = properties
                    .Select(n => n.Name)
                    .Aggregate((a, b) => a + delimiter + b);
                sw.WriteLine(header);
                foreach (var item in items)
                {
                    var row = properties
                        .Select(n => n.GetValue(item, null))
                        .Select(n => n == null ? null : n.ToString())
                        .Aggregate((a, b) => a + delimiter + b);
                    sw.WriteLine(row);
                }
                output = sw.ToString();
            }
            return output;
        }
    }
}
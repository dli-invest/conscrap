using System;
using ConScrap;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
// simple add to create csvs for scanning
namespace ConScrap.Init
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string dataPath ="data"; // Your code goes here
            await ConScrap.FetchStocks(false, dataPath);
        }
        private static void Dump(object o)
        {
            string json = JsonSerializer.Serialize(o);
            Console.WriteLine(json);
        }
    }
}

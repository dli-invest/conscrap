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
            // assume Args[0] is single stonk to parse
            string[] stonks =  {
                ""
            };
            if (args.Length == 0)
            {
               stonks = Constants.stocks;
            }  else {
               stonks[0] = args[0];
               dataPath = "data/" + args[0];
            }
            // debugger to see if stonks are being passed
            await ConScrap.FetchStocks(stonks, true, dataPath);
        }
        private static void Dump(object o)
        {
            string json = JsonSerializer.Serialize(o);
            Console.WriteLine(json);
        }
    }
}

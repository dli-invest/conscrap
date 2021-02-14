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
            // dont send data to discord
            await ConScrap.FetchStocks(false, dataPath);

            // // \todo make sure csvs can be copied over correctly
            // // algorithm to add new entries, load existing entries from csv
            // // https://stackoverflow.com/questions/26201952/selecting-distinct-elements-from-two-lists-using-linq
            // List<Types.YahooComment> yComments = File.ReadAllLines(@"WriteText.csv")
            //                     .Skip(1)
            //                     .Select(v => Types.YahooComment.FromCsv(v))
            //                     .ToList();
        }
    }
}

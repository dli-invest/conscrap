using System;
using ConScrap;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
/// \todo add logic to save results to folder
/// \todo CI/CD job
/// \todo figure out emoji support
/// \todo publish package or exe to github
/// \todo logging to log file
namespace ConScrap.TwitCmd
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string dataPath ="data/twits"; // Your code goes here
            await ConScrap.twitFetchStocks(true, dataPath);
        }
    }
}

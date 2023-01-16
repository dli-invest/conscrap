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
namespace ConScrap.Cmd
{
    class Program
    {
        static Task Main(string[] args)
        {
            // var client = new StockTwitsClient();
            // var bserData = await client.GetData("DMYI");
            var yahooRpt = ConScrap.MkTexRpt();
            return Task.CompletedTask;
        }
    }
}

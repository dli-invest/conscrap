using System;
using ConScrap;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
// simple add to create csvs for scanning
namespace ConScrap.Init
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string dataPath ="data"; // Your code goes here
            bool exists = System.IO.Directory.Exists(dataPath);

            if(!exists)
                System.IO.Directory.CreateDirectory(dataPath);

            var stocks = new List<String> {
                    "PKK.CN"
            };
            // change logic after tested
            string webhook = Environment.GetEnvironmentVariable("DISCORD_WEBHOOK");
            foreach (string stock in stocks)
            {
                string stockFile = String.Format(@"{0}/{1}.csv", dataPath, stock);
                List<Types.YahooComment> oldComments = new List<Types.YahooComment> {};
                if(File.Exists(stockFile))
                {
                   // load stock file into variable
                   oldComments = File.ReadAllLines(stockFile)
                                // skip header
                                .Skip(1)
                                .Select(v => Types.YahooComment.FromCsv(v))
                                .ToList();
                }
                // list data from csv if it exists
                List<Types.YahooComment> comments = ConScrap.GetYahooComments(stock);
                // distinct elements
                List<Types.YahooComment> newComments = comments.Except(oldComments).ToList();
                foreach (Types.YahooComment comment in newComments)
                {
                    var embed = comment.mapCommentForDiscord(stockFile);
                    var discordData = new Types.DiscordData {
                        embeds = new List<Types.DiscordEmbed> {
                            embed
                        }
                    };
                    await Discord.SendDiscord(webhook, discordData);
                }
                // send to discord
                string csvEntries = Csv.GenerateReport(comments);
                // diff list if exists
                System.IO.File.WriteAllText(stockFile, csvEntries);
            }

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

using System;
using ConScrap;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Threading.Tasks;
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

                var countDiff = comments.Count - oldComments.Count;
                // Console.WriteLine(newComments)
                var differences = comments.Take(countDiff);
                foreach (Types.YahooComment comment in differences)
                {
                    Console.WriteLine(comment.PostDate);
                    Console.WriteLine(comment.Content);
                    Types.DiscordEmbed embed = comment.mapCommentForDiscord(stockFile);
                    List<Types.DiscordEmbed> embeds = new List<Types.DiscordEmbed> {};
                    embeds.Add(embed);
                    Types.DiscordData discordData = new Types.DiscordData {
                        content = "Test Doc",
                        // embeds = embeds
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

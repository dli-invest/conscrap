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
            bool exists = System.IO.Directory.Exists(dataPath);

            if(!exists)
                System.IO.Directory.CreateDirectory(dataPath);

            var stocks = new List<String> {
                    "PKK.CN",
                    "IDK.CN",
                    "ART.V",
                    "BEE.CN",
                    "N.V"
            };
            // change logic after tested
            string webhook = Environment.GetEnvironmentVariable("DISCORD_WEBHOOK");
            /// \todo rate limit selenium instances to 10
            /// see bser logic
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
                string msgUrls = String.Format("https://finance.yahoo.com/quote/{0}/community?p={0}", stock);
                /// \todo batch comments in 10 to send off
                foreach (Types.YahooComment comment in differences)
                {
                    Types.DiscordEmbed embed = comment.mapCommentForDiscord(msgUrls);
                    List<Types.DiscordEmbed> embeds = new List<Types.DiscordEmbed> {};
                    embeds.Add(embed);
                    Types.DiscordData discordData = new Types.DiscordData {
                        embeds = embeds
                    };
                    Dump(discordData);
                    // await Discord.SendDiscord(webhook, discordData);
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
        private static void Dump(object o)
        {
            string json = JsonSerializer.Serialize(o);
            Console.WriteLine(json);
        }
    }
}

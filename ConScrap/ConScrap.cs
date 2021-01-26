using System.Collections.Generic;
using System;
using System.Text.Json;
using System.IO;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
/// \todo save report to file
/// \todo save to csv or faunadb
/// \todo improved cli interface
/// \todo exe with working env vars
/// \todo scrap for only new reports and/or html format
namespace ConScrap
{
    /// \todo get list of stocks
    public class ConScrap
    {
        public static List<Types.YahooComment> GetYahooComments(string ticker = "PKK.CN")
        {
            string readText = Browser.GetAllEntries(ticker);
            var htmlDoc = Parse.MkHtmlDoc(readText);
            var yahooData = Parse.ExtractYahooConversationsHtml(readText);
            // htmlDoc = Parse.MkHtmlDoc(yahooData.ToString());
            var yahooComments = Parse.ExtractComments(yahooData);
            return yahooComments;
        }

        public static string MkTexRpt()
        {
            var yahooComments = GetYahooComments();
            var dateString = System.DateTime.Now.ToString("yyyy-MM-dd");
            Scriban.Template tmpl = Render.ParseTemplate(Constants.ReportTemplate);
            var rpt = tmpl.Render(new { comments = yahooComments, date = dateString });
            return rpt;
        }

        public static string MkTexRpt(List<Types.YahooComment> yahooComments)
        {
            var dateString = System.DateTime.Now.ToString("yyyy-MM-dd");
            Scriban.Template tmpl = Render.ParseTemplate(Constants.ReportTemplate);
            var rpt = tmpl.Render(new { comments = yahooComments, date = dateString });
            return rpt;
        }

        public async static Task ProcessStock(string stock, Types.FetchConfig fetchConfig)
        {
            var dataPath = fetchConfig.dataPath;
            var discordThrottler = fetchConfig.discordThrottler;
            var seleniumThroller = fetchConfig.seleniumThroller;
            var sendDiscord = fetchConfig.sendDiscord;
            // change logic after tested
            string webhook = Environment.GetEnvironmentVariable("DISCORD_WEBHOOK");
            string stockFile = String.Format(@"{0}/{1}.csv", dataPath, stock);
            List<Types.YahooComment> oldComments = new List<Types.YahooComment> { };
            if (File.Exists(stockFile))
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
                try
                {
                    await discordThrottler.WaitAsync();
                    Types.DiscordEmbed embed = comment.mapCommentForDiscord(msgUrls, stock);
                    List<Types.DiscordEmbed> embeds = new List<Types.DiscordEmbed> { };
                    embeds.Add(embed);
                    Types.DiscordData discordData = new Types.DiscordData
                    {
                        embeds = embeds
                    };
                    if (sendDiscord)
                    {
                        Dump(discordData);
                        await Task.Delay(2000);
                        await Discord.SendDiscord(webhook, discordData);
                    }
                }
                finally
                {
                    try
                    {
                        await discordThrottler.WaitAsync();
                        Types.DiscordEmbed embed = comment.mapCommentForDiscord(msgUrls, stock);
                        List<Types.DiscordEmbed> embeds = new List<Types.DiscordEmbed> { };
                        embeds.Add(embed);
                        Types.DiscordData discordData = new Types.DiscordData
                        {
                            embeds = embeds
                        };
                        Dump(discordData);
                        if (sendDiscord)
                        {
                            await Task.Delay(2000);
                            await Discord.SendDiscord(webhook, discordData);
                        }
                    }
                    finally
                    {
                        // here we release the throttler immediately
                        discordThrottler.Release();
                    }
                }
                // send to discord
                string csvEntries = Csv.GenerateReport(comments);
                // diff list if exists
                System.IO.File.WriteAllText(stockFile, csvEntries);
            }
            return;
        }
        public async static Task FetchStocks(bool sendDiscord = true, string dataPath = "data")
        {
            bool exists = System.IO.Directory.Exists(dataPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(dataPath);

            var stocks = Constants.stocks;
            /// \todo rate limit selenium instances to 10
            /// see bser logic
            /// \todo skip failure tickers
            SemaphoreSlim discordThrottler = new SemaphoreSlim(30, 30);
            SemaphoreSlim seleniumThroller = new SemaphoreSlim(5);
            // Console.WriteLine(fetchConfig);
            Types.FetchConfig fetchConfig = new Types.FetchConfig
            {
                sendDiscord = sendDiscord,
                dataPath = dataPath,
                discordThrottler = new SemaphoreSlim(30),
                seleniumThroller = new SemaphoreSlim(10)
            };
            foreach (string stock in stocks)
            {
                await ProcessStock(stock, fetchConfig);
            }
        }

        private static void Dump(object o)
        {
            string json = JsonSerializer.Serialize(o);
            Console.WriteLine(json);
        }
    }
}
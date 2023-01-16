using System.Collections.Generic;
using System;
using System.Text.Json;
using System.IO;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
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
        public static List<Types.YahooComment> GetYahooComments(string ticker = "ACT.CN")
        {
            string readText = Browser.GetAllEntries(ticker);
            // print read text
            var htmlDoc = Parse.MkHtmlDoc(readText);
            // save htmlDoc as file
            var yahooData = Parse.ExtractYahooConversationsHtml(readText);
            // htmlDoc = Parse.MkHtmlDoc(yahooData.ToString());
            // print yahooData
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

        // add timeout per stock
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
            List<Types.YahooComment> comments = new List<Types.YahooComment>{};
            try {
                comments = ConScrap.GetYahooComments(stock);
            } catch(Exception ex) {
                Console.WriteLine(ex);
                return;
            }

            // print all comments using loop
            // Console.WriteLine("All comments");
            // var countDiff = comments.Count - oldComments.Count;
            // find new comments in comments by looking at Author And Content field in oldComments
            var newComments = comments.Where(x => !oldComments.Any(y => y.Author == x.Author && y.Content == x.Content)).ToList();
            string msgUrls = String.Format("https://finance.yahoo.com/quote/{0}/community?p={0}", stock);
            /// \todo batch comments in 10 to send off
            foreach (Types.YahooComment comment in newComments)
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
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }
                finally
                {
                    discordThrottler.Release();
                }
            }
            // get unique comments newComments and oldComments
            var uniqueComments = comments.Union(oldComments).ToList();
            // send to discord
            string csvEntries = Csv.GenerateReport(uniqueComments);
            // diff list if exists
            System.IO.File.WriteAllText(stockFile, csvEntries);
            return;
        }

        // TODO add env var to fetchStocks to get a list of stocks, this function should just parse
        // added in stocks, rather than grab it from a constant
        public async static Task FetchStocks(string[] stocks, bool sendDiscord = true, string dataPath = "data")
        {
            bool exists = System.IO.Directory.Exists(dataPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(dataPath);
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
                discordThrottler = discordThrottler,
                seleniumThroller = seleniumThroller
            };
            foreach (string stock in stocks)
            {
                try {
                    await ProcessStock(stock, fetchConfig);
                } catch(Exception ex) {
                    // 
                    Console.WriteLine(ex);
                }
            }
        }

        // stock twits functions
        public async static Task twitFetchStocks(bool sendDiscord = true, string dataFolder = "data/twits") 
        {
            bool exists = System.IO.Directory.Exists(dataFolder);

            if (!exists)
                System.IO.Directory.CreateDirectory(dataFolder);

            var stocks = Constants.tstocks;
            /// \todo rate limit selenium instances to 10
            /// see bser logic
            /// \todo skip failure tickers
            SemaphoreSlim discordThrottler = new SemaphoreSlim(30, 30);
            SemaphoreSlim seleniumThroller = new SemaphoreSlim(5);
            // Console.WriteLine(fetchConfig);
            Types.FetchConfig fetchConfig = new Types.FetchConfig
            {
                sendDiscord = sendDiscord,
                dataPath = dataFolder,
                discordThrottler = discordThrottler,
                seleniumThroller=seleniumThroller
            };
            foreach (string stock in stocks)
            {
                try {
                    await twitProcessStock(stock, fetchConfig);
                } // catch IndexOutOfRangeException error
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    if (ex is IndexOutOfRangeException)
                    {
                        Console.WriteLine("IndexOutOfRangeException");
                        return;
                    } 
                    Console.WriteLine("Error: " + ex.Message);
                } 
            }
        }

        public async static Task twitProcessStock(string stock, Types.FetchConfig fetchConfig)
        {
            var dataPath = fetchConfig.dataPath;
            var discordThrottler = fetchConfig.discordThrottler;
            var sendDiscord = fetchConfig.sendDiscord;
            string webhook = Environment.GetEnvironmentVariable("DISCORD_WEBHOOK");
            string stockFile = String.Format(@"{0}/{1}.csv", dataPath, stock);
            StockTwitsClient TwitsClient = new StockTwitsClient();
            Types.stSymbolResp twitsResp = await TwitsClient.GetData(stock);
            List<Types.stMessage> oldComments = new List<Types.stMessage> { };
            if (File.Exists(stockFile))
            {
                // load stock file into variable
                oldComments = File.ReadAllLines(stockFile)
                             // skip header
                             .Skip(1)
                             .Select(v => Types.stMessage.FromCsv(v))
                             .ToList();
            }
            List<Types.stMessage> newComments = twitsResp.messages;
            if(newComments == null || newComments.Count == 0) {
                Console.WriteLine("ERROR");
                return;
            }
            List<Types.stMessage> finalComments = oldComments.Concat(newComments)
                .GroupBy(x=>x.id)
                .Where(x=>x.Count() == 1)
                .Select(x=>x.FirstOrDefault())
                .ToList(); 
            DateTime today = DateTime.Today;
            foreach (Types.stMessage comment in newComments)
            {
                DateTime created_at = DateTime.Parse(comment.created_at);
                // if post + 24 hours (post happened today)
                // send to discord
                if (created_at.AddHours(24) > today) {
                    try
                    {
                        //
                        if (comment.body == null) {
                            continue;
                        }
                        await discordThrottler.WaitAsync();
                        Types.DiscordEmbed embed = comment.mapCommentForDiscord(stock);
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
                        discordThrottler.Release();
                    }
                }
            }
            string csvEntries = Csv.GenerateReport(finalComments);
            // diff list if exists
            System.IO.File.WriteAllText(stockFile, csvEntries);
            // diff list with storage
        }

        private static void Dump(object o)
        {
            string json = JsonSerializer.Serialize(o);
            Console.WriteLine(json);
        }
    }
}

using System;
using System.Collections.Generic;
using ConScrap;
using System.Threading.Tasks;
namespace ConScrap.Cron
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string webhook = Environment.GetEnvironmentVariable("DISCORD_WEBHOOK");
            Console.WriteLine("Hello World!");
            var data = new Types.DiscordData {
                content="sample response",
                embeds = new List<Types.DiscordEmbed> {
                    new Types.DiscordEmbed { 
                        description = "test description",
                        title = "sample title",
                        url = "https://stock-market.com"
                    }
                }
            };
            await Discord.SendDiscord(webhook, data);
            
        }
    }
}

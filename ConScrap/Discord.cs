using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/// \todo implement discord module
/// simple functions that map YahooComments to Data points
namespace ConScrap
{
    public class Discord 
    {
        
        public async static Task SendDiscord(string webhook, Types.DiscordData data)
        {
            var client = new HttpClient();
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var response = await client.PostAsJsonAsync(webhook, data, options);
            Console.WriteLine(response);
        }
    }
}

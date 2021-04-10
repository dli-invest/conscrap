using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
namespace ConScrap
{
    /// <summary>
    /// Stock Twits Http Client
    /// Implements rate limiting and the v1 endpoints.
    /// \todo label this package as v1 (intend to support v2 as a hobby)
    /// </summary>
    /// <remark>
    /// Consider making validators in a different class
    /// </remark>
    public class StockTwitsClient
    {
        public HttpClient Client { get; } = new HttpClient();
        private SemaphoreSlim Throttler { get; } = null;
        private int RateLimit = 200;
        /// <param name="rateLimit">Rate limit for requests (should be 1 for personal apikey).</param>
        public StockTwitsClient(int rateLimit = 200)
        {
            /// \todo TODO figure out how to handle urls when v2 comes out
            Client.BaseAddress = new Uri("https://api.stocktwits.com");
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            RateLimit = rateLimit;
            // rateLimit is 200 requests per hour
            Throttler = new SemaphoreSlim(rateLimit);
        }
        /// <summary>
        /// Overloaded function that can get values for given metadata type
        /// Fetch game data by metadata - calls /v1/data/{metaType}
        /// </summary>
        /// <remark>
        /// Since arbitary json data is returned from the api, data is a List of dictionary of System.Text.Json.JsonElement
        // I believe for most purposes using `itemData.data[0]["code"].ToString()` should be adequate.
        /// </remark>
        public async Task<Types.stSymbolResp> GetData(string ticker)
        {
            await Throttler.WaitAsync();
            string endpoint = String.Format("/api/2/streams/symbol/{0}.json", ticker);
            Types.stSymbolResp symbolResp;
            try
            {
                var response = await Client.GetAsync(endpoint);

                // let's wait here for 1 second to honor the API's rate limit   
                // rate limit is 200 per 60 minutes                      
                await Task.Delay(1);
                // add error handling
                // response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                symbolResp = JsonSerializer.Deserialize<Types.stSymbolResp>(responseBody);
                if (!response.IsSuccessStatusCode)
                {
                    // PrintRespErrors(bserData);
                    Console.WriteLine("FAILED MESSAGE");
                }
            }
            finally
            {
                // here we release the throttler immediately
                Throttler.Release();
            }
            return symbolResp;
        }
    }
}

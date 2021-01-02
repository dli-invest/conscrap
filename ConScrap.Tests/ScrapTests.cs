using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using ConScrap;
using System.Collections.Generic;
using Scriban;
using System.IO;
using System.Text;

namespace ConScrap.Tests
{
    // [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TestScrap : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestScrap(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        // no functionality right now
        public void Dispose()
        {
            // Tear down (called once per test)
        }

        [Fact]
        public void TestScrapComments()
        {
            string path = @"SampleData/yahoopkk.html";
            // Open the file to read from.
            string readText = File.ReadAllText(path);
            HtmlAgilityPack.HtmlNode yahooHtml = Parse.ExtractYahooConversationsHtml(readText);
            object parsedConversations = Parse.ExtractComments(yahooHtml);
            // /div/div[1]/span/span
            Console.WriteLine(yahooHtml.GetType().ToString());
        }

    }
}

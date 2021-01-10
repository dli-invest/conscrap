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
            var parsedConversations = Parse.ExtractComments(yahooHtml);
            // /div/div[1]/span/span
            // Console.WriteLine(parsedConversations.GetType().ToString());
            foreach (var node in parsedConversations)
            {
                // Console.WriteLine(node.PostDate);
                // Console.WriteLine(node.Content);
            }
        }

        [Fact]
        public void TestExtractDate()
        {
            string path = @"SampleData/yahoopkk_comment.html";
            // Open the file to read from.
            string readText = File.ReadAllText(path);
            HtmlAgilityPack.HtmlDocument htmlDoc = Parse.MkHtmlDoc(readText);
            var yahooComment = Parse.GetYahooComment(htmlDoc.DocumentNode);
            Assert.Equal("3 days ago", yahooComment.PostDate);
            Assert.Equal("\$2.50 today. NASDAQ here Peak comes!\$4 by end of January", yahooComment.Content);
            Assert.Equal("Derek", yahooComment.Author);
        }

        [Fact]
        public void TestScrapShowButton()
        {
            string path = @"SampleData/yahoopkk_button.html";
            // Open the file to read from.
            string readText = File.ReadAllText(path);
            HtmlAgilityPack.HtmlDocument htmlDoc = Parse.MkHtmlDoc(readText);
            var showButtonNode = Parse.GetShowButton(htmlDoc.DocumentNode);
            Assert.Equal("Show more", showButtonNode.InnerText);
            // Assert.Equal("<span data-reactid=\"677\">Show more</span>", showButtonNode.InnerHtml);
        }
    }
}

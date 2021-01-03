using System;
using Xunit;
using Xunit.Abstractions;

namespace ConScrap.Tests
{
    // browser stack can have at most 5 parallel tests
    // limit to 5 tests
    // [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [Collection("Sequential")]
    public class BrowserTest : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BrowserTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        // no functionality right now
        public void Dispose()
        {
            // Tear down (called once per test)
        }

        [Fact]
        public void TestExtractSelenium()
        {
            Browser.TestBrowser();
            // Console.WriteLine(yahooComment.CommentDate);
            // object parsedConversations = Parse.ExtractComments(yahooHtml);
        }

        [Fact]
        public void TestExtractPkkComments()
        {
            var yahooRpt = ConScrap.MkTexRpt();
            // Console.WriteLine(yahooComment.CommentDate);
            // object parsedConversations = Parse.ExtractComments(yahooHtml);
        }
    }
}

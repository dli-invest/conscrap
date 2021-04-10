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
    public class TestTwits : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestTwits(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        // no functionality right now
        public void Dispose()
        {
            // Tear down (called once per test)
        }

        [Fact]
        public void TestTwitComments()
        {
            var client = new StockTwitsClient();
            var bserData = client.GetData("DMYI");
        }
    }
}

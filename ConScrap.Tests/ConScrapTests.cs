using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using ConScrap;
using System.Collections.Generic;
using Scriban;

namespace ConScrap.Tests
{
    // [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConScrap : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ConScrap(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        // no functionality right now
        public void Dispose()
        {
            // Tear down (called once per test)
        }

        [Fact]
        public void TestGetData()
        {
            string readText = Constants.SampleTemplateLatex;
            var template = Render.ParseTemplate(readText);
            var ProductList = new List<string> { "test1", "test2", "test3" };
            var result = template.Render(new { Products = ProductList });
            _testOutputHelper.WriteLine(result);
            _testOutputHelper.WriteLine(result.GetType().ToString());
            Console.WriteLine(result);
        }
    }
}
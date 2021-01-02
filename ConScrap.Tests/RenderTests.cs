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
    public class TestRender : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestRender(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        // no functionality right now
        public void Dispose()
        {
            // Tear down (called once per test)
        }

        [Fact]
        public void TestSampleTemplate()
        {
            string readText = Constants.SampleTemplateLatex;
            var template = Render.ParseTemplate(readText);
            var ProductList = new List<string> { "test1", "test2", "test3" };
            var result = template.Render(new { Products = ProductList });
            // making sure I can render templates
            _testOutputHelper.WriteLine(result);
            _testOutputHelper.WriteLine(result.GetType().ToString());
        }

        [Fact]
        public void TestPlainBox()
        {
            string readText = Constants.PlainBox;
            var template = Render.ParseTemplate(readText);
            var result = template.Render(new { options = "colback=red!5!white,colframe=red!35!black", text="stock market millionaire" });
            // close enough string equivalents
            _testOutputHelper.WriteLine(result);
            _testOutputHelper.WriteLine(result.GetType().ToString());
        }
    }
}

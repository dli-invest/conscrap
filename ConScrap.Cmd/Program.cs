using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scriban;
using ConScrap.Render;
namespace ConScrap.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string readText = Constants.SampleTemplateLatex
            var ProductList = new List<string> { "test1", "test2", "test3" };
            var template = Template.Parse(readText);
            var result = template.Render(new { Products = ProductList });
            Console.WriteLine(result);
        }
    }
}

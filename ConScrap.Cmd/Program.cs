using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scriban;
using ConScrap;
namespace ConScrap.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"../ConScrap.Tests/SampleData/yahoopkk.html";
            // Open the file to read from.
            string readText = File.ReadAllText(path);
            var htmlDoc = Parse.MkHtmlDoc(readText);
            var yahooCommentNodes = Parse.ExtractComments(yahooHtml);
            // Console.WriteLine(yahooCommentNodes.GetType().ToString());
            // var comment = Parse.GetYahooComment(yahooCommentNodes.childNodes[0]);
            // Console.WriteLine(comment);
        }
    }
}

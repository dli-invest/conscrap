using System;
using System.Collections.Generic;
using Scriban;
using ConScrap;
/// \todo add logic to save results to folder
/// \todo CI/CD job
/// \todo figure out emoji support
/// \todo publish package or exe to github
/// \todo logging to log file
namespace ConScrap.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var yahooRpt = ConScrap.MkTexRpt();
            // save report to tex file
            Console.WriteLine(yahooRpt);
            // foreach (var comment in yahooComments)
            // {
            //     Console.WriteLine(comment.PostDate);
            //     Console.WriteLine(comment.Content);
            //     Console.WriteLine(comment.Author);
            // }
            // Console.WriteLine(yahooCommentNodes.GetType().ToString());
            // var comment = Parse.GetYahooComment(yahooCommentNodes.childNodes[0]);
            // Console.WriteLine(comment);
        }
    }
}

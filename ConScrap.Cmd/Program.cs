using System;
using ConScrap;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
            var yahooComments = ConScrap.GetYahooComments();
            var yahooRpt = ConScrap.MkTexRpt(yahooComments);
            // save report to tex file
            // Console.WriteLine(yahooRpt);

            // Get values from yahoo comments
            var values = Csv.GenerateReport<Types.YahooComment>(yahooComments);
            System.IO.File.WriteAllText(@"WriteText.csv", values);

            // \todo make sure csvs can be copied over correctly
            // algorithm to add new entries, load existing entries from csv
            // get new entries from list Union looks good, each stock has its own list
            // overwrite file with new entries
            // https://stackoverflow.com/questions/26201952/selecting-distinct-elements-from-two-lists-using-linq
            List<Types.YahooComment> yComments = File.ReadAllLines(@"WriteText.csv")
                                .Skip(1)
                                .Select(v => Types.YahooComment.FromCsv(v))
                                .ToList();
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

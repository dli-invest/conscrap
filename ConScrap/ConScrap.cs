using System.Collections.Generic;

/// \todo save report to file
/// \todo save to csv or faunadb
/// \todo improved cli interface
/// \todo exe with working env vars
/// \todo scrap for only new reports and/or html format
namespace ConScrap
{
    public class ConScrap 
    {
        public static List<Types.YahooComment> GetYahooComments()
        {
            string readText = Browser.GetAllEntries();
            var htmlDoc = Parse.MkHtmlDoc(readText);
            var yahooData = Parse.ExtractYahooConversationsHtml(readText);
            // htmlDoc = Parse.MkHtmlDoc(yahooData.ToString());
            var yahooComments = Parse.ExtractComments(yahooData);
            return yahooComments;
        }

        public static string MkTexRpt()
        {
            var yahooComments = GetYahooComments();
            var dateString = System.DateTime.Now.ToString("yyyy-MM-dd");
            Scriban.Template tmpl = Render.ParseTemplate(Constants.ReportTemplate);
            var rpt = tmpl.Render(new { comments = yahooComments, date=dateString});
            return rpt;
        }

        public static string MkTexRpt(List<Types.YahooComment> yahooComments)
        {
            var dateString = System.DateTime.Now.ToString("yyyy-MM-dd");
            Scriban.Template tmpl = Render.ParseTemplate(Constants.ReportTemplate);
            var rpt = tmpl.Render(new { comments = yahooComments, date=dateString});
            return rpt;
        }
    }
} 
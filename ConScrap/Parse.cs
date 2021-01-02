using System;
using HtmlAgilityPack;
namespace ConScrap
{
    /// <summary>
    /// Parsing Html and extracting articles of interest
    /// </summary>
    // //*[@id="canvass-0-CanvassApplet"]/div/ul
    //
    public class Parse
    {
        /// <summary>
        ///     Extracts the html node containing all the comments
        /// </summary>
        /// <param name="yahooHtml"> Yahoo html in str format </param>
        public static HtmlAgilityPack.HtmlNodeCollection ExtractYahooConversationsHtml(string yahooHtml)
        {
            // Console.WriteLine(yahooHtml);
            var commentsBodyClass = "comments-body";
            var commentListClass = "comments-list";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(yahooHtml);
            // var commentsSelector = "div[@class='" + commentsBodyClass + "']";
            // var commentListSelector = "div[@class='" + commentListClass + "']";
            var commentListSelector = "ul[contains(@class, '" + commentListClass + "')]";
            string xpath = String.Format("//{0}", commentListSelector);
            var htmlComments = htmlDoc.
                DocumentNode.
                SelectSingleNode(xpath);
            // Console.WriteLine(htmlComments);
            // Console.WriteLine(htmlComments.ToString());
            // Console.WriteLine(htmlComments.InnerHtml);
            foreach (var node in htmlComments.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    Console.WriteLine(node.InnerText);
                }
            }
            return htmlComments.ChildNodes;
        } 
        public static object ExtractComments(HtmlAgilityPack.HtmlNodeCollection yahooObject)
        {
            return new {};
        }
    }
}
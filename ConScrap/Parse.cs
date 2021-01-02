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
        public static HtmlAgilityPack.HtmlNode ExtractYahooConversationsHtml(string yahooHtml)
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
            // foreach (var node in htmlComments.ChildNodes)
            // {
            //     if (node.NodeType == HtmlNodeType.Element)
            //     {
            //         Console.WriteLine(node.InnerText);
            //     }
            // }
            return htmlComments;
        } 

        public static object GetYahooComment(HtmlAgilityPack.HtmlNode commentNode) 
        {
            // make new html soup for comment
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(commentNode.InnerHtml);
            var commentTag = htmlDoc.
                DocumentNode.
                SelectSingleNode(postDate);
            // get object data
            var yahooComment = new YahooComment(CommentDate=commentTag.InnerText);
            Console.WriteLine(commentTag.InnerHtml);
            return new {};
        }
        public static object ExtractComments(HtmlAgilityPack.HtmlNode yahooCommentNodes)
        {
            var postDate = "//div/div[1]/span/span";
            foreach (var node in yahooCommentNodes.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    
                    Console.WriteLine("------------");
                    Console.WriteLine(node.InnerHtml);
                    Console.WriteLine("------------");
                }
            }
            return new {};
        }
    }
}
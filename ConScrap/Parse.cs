using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using ConScrap.Types;
namespace ConScrap
{
    /// <summary>
    /// Parsing Html and extracting articles of interest
    /// </summary>

    public class Parse
    {
        /// <summary>
        ///     Extracts the html node containing all the comments
        /// </summary>
        /// <param name="yahooHtml"> Yahoo html in str format </param>
        public static HtmlAgilityPack.HtmlNode ExtractYahooConversationsHtml(string yahooHtml)
        {
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

        public static YahooComment GetYahooComment(HtmlAgilityPack.HtmlNode commentNode) 
        {
            var postDateXPath = Constants.YahooXPaths.postDateXPath;
            var contentXPath = Constants.YahooXPaths.contentXPath;
            var authorXPath = Constants.YahooXPaths.authorXPath;
            // make new html soup for comment
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(commentNode.InnerHtml);
            var postdateNode = htmlDoc.
                DocumentNode.
                SelectSingleNode(postDateXPath);
            
            var contentNode = htmlDoc.
                DocumentNode.
                SelectSingleNode(contentXPath);

            var authorNode = htmlDoc.
                DocumentNode.
                SelectSingleNode(authorXPath);

            // get object data
            var yahooComment = new YahooComment{
                PostDate=postdateNode.InnerText,
                Content=contentNode.InnerText,
                Author=authorNode.InnerText
            };
            return yahooComment;
        }

        /// <summary>
        ///     Make html document from htmlStr
        /// </summary>
        public static HtmlDocument MkHtmlDoc(string htmlStr)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlStr);
            return htmlDoc;
        }

        public static HtmlAgilityPack.HtmlNode GetShowButton(HtmlAgilityPack.HtmlNode yahooCommentNodes)
        {
            var showMoreXPath = Constants.YahooXPaths.showMoreXPath;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(yahooCommentNodes.InnerHtml);
            var postdateNode = htmlDoc.
                DocumentNode.
                SelectSingleNode(showMoreXPath);

            // Console.WriteLine(postdateNode.OuterHtml);
            // Console.WriteLine(postdateNode.InnerHtml);
            // Console.WriteLine(postdateNode.InnerText);
            return postdateNode;
        }

        public static List<YahooComment> ExtractComments(HtmlAgilityPack.HtmlNode yahooCommentNodes)
        {
            List<YahooComment> yahooComments= new List<YahooComment>();
            foreach (var node in yahooCommentNodes.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    var comment = GetYahooComment(node);
                    yahooComments.Add(comment);
                    // Console.WriteLine("------------");
                    // Console.WriteLine(node.InnerHtml);
                    // Console.WriteLine("------------");
                }
            }
            return yahooComments;
        }
    }
}
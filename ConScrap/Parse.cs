using System;
using System.Text;
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
            return htmlComments;
        } 

        ///<summary> Replace tex sensitive characters</summary>
        /// \todo Mark urls in tex using \url{} after removing characters
        public static string AdjustStrForTex(string s)
        {
            return s
                .Replace("#$%$", @"shit")
                .Replace("$", @"\$")
                .Replace("%", @"\%")
                .Replace("#", @"\#")
                .Replace("_", @"\_")
                .Replace("amp;", @"")
                .Replace("&", @"\&");
        }
        /// \todo ignore yahoo comment replies for now
        public static YahooComment GetYahooComment(HtmlAgilityPack.HtmlNode commentNode, bool parseForTex = false) 
        {
            var postDateXPath = Constants.YahooXPaths.postDateXPath;
            var contentXPath = Constants.YahooXPaths.contentXPath;
            var authorXPath = Constants.YahooXPaths.authorXPath;
            var likesXPath = Constants.YahooXPaths.likesXPath;
            var dislikeXPath = Constants.YahooXPaths.dislikesXPath;
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

            var likesNode = htmlDoc.
                DocumentNode.
                SelectSingleNode(likesXPath);

            var dislikesNode = htmlDoc.
                DocumentNode.
                SelectSingleNode(dislikeXPath);

            string author;
            string content;
            if (parseForTex) {
                // \todo add flag to parse for tex vs not, default mode is tex
                // perform character adjustment, make it option
                author = AdjustStrForTex(authorNode.InnerText);

                string contentCleaned = Encoding.ASCII.GetString(
                    Encoding.Convert(
                        Encoding.UTF8,
                        Encoding.GetEncoding(
                            Encoding.ASCII.EncodingName,
                            new EncoderReplacementFallback(string.Empty),
                            new DecoderExceptionFallback()
                            ),
                        Encoding.UTF8.GetBytes(contentNode.InnerText)
                    )
                );
                content = AdjustStrForTex(contentCleaned);
            } else {
                author = authorNode.InnerText;
                content = contentNode.InnerText;
            }

            var yahooComment = new YahooComment{
                PostDate=postdateNode.InnerText,
                Content=content,
                Author=author
            };

            // add conditions
            int number;
            if (likesNode != null) {
                // Console.WriteLine("Likes" + likesNode.InnerText);
                Int32.TryParse(likesNode.InnerText, out number);
                yahooComment.Likes = number;
                number = 0;
                // Console.WriteLine(likesNode.InnerHtml);
            }
            if (dislikesNode != null) {
                Int32.TryParse(dislikesNode.InnerText, out number);
                yahooComment.Dislikes = number;
            }
            // get likes, maybe nested comments as well
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
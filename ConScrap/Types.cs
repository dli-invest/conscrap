using System;
using System.Collections.Generic;
using System.Threading;
namespace ConScrap.Types
{
    public class YahooComment
    {
        public string PostDate { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }

        public string RelativeDate {get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public static YahooComment FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            YahooComment yComment = new YahooComment();
            yComment.PostDate = Convert.ToString(values[0]);
            yComment.Content = Convert.ToString(values[1]);
            yComment.Author = Convert.ToString(values[2]);

            int likes;
            int dislikes;
            Int32.TryParse(values[3], out likes);
            yComment.Likes = likes;
            Int32.TryParse(values[4], out dislikes);
            yComment.Dislikes = dislikes;
            return yComment;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                YahooComment yc = (YahooComment)obj;
                return (Author == yc.Author) && (Content == yc.Content);
            }
        }

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashContent = Content == null ? 0 : Content.GetHashCode();

            //Get hash code for the Code field.
            int hashAuthor = Author.GetHashCode();

            //Calculate the hash code for the product.
            return hashContent ^ hashAuthor;
        }

        // mapping data from yahoo finance to discord

        public DiscordEmbed mapCommentForDiscord(string yahooUrl = "https://finance.yahoo.com", string stock = "N/A")
        {
            var title = String.Format(@"{0} - {1} - {2}", stock, Author, PostDate);
            return new DiscordEmbed { 
                description = Content,
                title = title,
                url = yahooUrl,
                fields = new List<DiscordField> {
                    new DiscordField {
                        name = "likes",
                        value = Likes.ToString(),
                        inline = true
                    },
                    new DiscordField {
                        name = "Dislikes",
                        value = Dislikes.ToString(),
                        inline = true
                    }
                }
            };
        }
    }
    

    // implement discord data
    public class DiscordData
    {
        public string content { get; set; }
        public List<DiscordEmbed> embeds { get; set; }
    }

    public class DiscordEmbed
    {
        public string description { get; set; }
        public string url { get; set; }
        public string title { get; set; }

        public List<DiscordField> fields { get; set; }

        public string timestamp {get; set;}
        // public string timestamp { get; set; }
    }

    public class DiscordField
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool inline { get; set; }
    }

    public class FetchConfig
    {
        public string stock {get; set;}
        public bool sendDiscord {get; set;}
        public string dataPath {get; set;}

        public SemaphoreSlim discordThrottler {get; set;}

        public SemaphoreSlim seleniumThroller {get; set;}
    }

    public class stSymbolResp
    {
        public object response { get; set; }
        public List<stMessage> messages { get; set; }
    }

    public class stMessage
    {
        public Int64 id {get; set;}
        public string body {get; set;}
        public string created_at {get; set;}

        public static stMessage FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            try {
                 stMessage twitComment = new stMessage();
                twitComment.id = Convert.ToInt64(values[0]);
                twitComment.body = Convert.ToString(values[1]);
                if(values.Length > 2) {
                    twitComment.created_at = Convert.ToString(values[2]);
                }
                return twitComment;
            } catch (System.FormatException e) {
                return new stMessage();
            }
           
        }

        public DiscordEmbed mapCommentForDiscord(string stock = "N/A")
        {
            string twitUrl = "https://stocktwits.com/symbol";
            var title = String.Format(@"StockTwits - {0}", stock);
            var symbolUrl = String.Format(@"{0}/{1}", twitUrl, stock);
            var embed = new DiscordEmbed { 
                description = body,
                title = title,
                url = symbolUrl
            };

            if (created_at != null) {
                embed.timestamp = created_at;
            }
            return embed;
        }
    }
}

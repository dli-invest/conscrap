using System;

namespace ConScrap.Types
{
     public class YahooComment
     {
        public string PostDate { get; set; }
        public string Content {get; set; }
        public string Author {get; set;}

        public int Likes {get; set;}
        public int Dislikes {get; set;}
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
     }
}
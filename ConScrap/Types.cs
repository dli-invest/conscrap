
namespace ConScrap.Types
{
     public class YahooComment
     {
        public string PostDate { get; set; }
        public string Content {get; set; }
        public string Author {get; set;}

        public int Likes {get; set;}
        public int Dislikes {get; set;}
     }
}
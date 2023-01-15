
using System.Collections.Generic;

namespace ConScrap
{
    /// <summary>
    ///     Set of Constants including sample latex documents.
    ///     The latex documents should include the report date, format and
    ///     statistics.
    /// </summary>
    public class Constants
    {
        public const string SampleTemplateLatex = 
@"
\documentclass{article}
\usepackage[utf8]{inputenc}
\usepackage{amssymb}
\begin{document}
    \begin{itemize}
    {{ for product in products }}
        \item {{product}}
    {{ end }}
    \end{itemize}
\end{document}";

        public const string PlainBox =
@"
\begin{tcolorbox}[{{ if options }} {{options}} {{ else }} colback=red!5!white,colframe=red!75!black{{ end }}]
  {{text}}
\end{tcolorbox}
";

        public const string ReportTemplate = 
@"
\documentclass{scrreprt}
\usepackage[utf8]{inputenc}
\usepackage{amssymb}
\usepackage{tikz,lipsum,lmodern}
\usepackage[most]{tcolorbox}
\begin{document}
    \section{Yahoo Comments {{date}}}
    {{ for comment in comments }}
        \begin{tcolorbox}[colback=blue!5!white,colframe=blue!75!black,title={{comment.author}} - {{comment.post_date}}]
            {{comment.content}}
        \end{tcolorbox}
    {{ end }}
\end{document}";

        // copied from SampleData/yahoopkk_comment.html

        public const string yahooBasePath = "https://finance.yahoo.com";
        public class YahooXPaths
        {

            // message-timestamp
            public const string postDateXPath = "//*[@data-spot-im-class='message-timestamp']";
            public const string postDateXPathLegacy = "//div/div[1]/span/span";
            
            public const string contentXPath = "//*[@data-spot-im-class='message-text']/text()";
            // public const string contentXPathLegacy = "//div/div[2]/div";

            public const string authorXPathLegacy = "//div/div[1]/button";

            public const string authorXPath = "//span[@data-spot-im-class='message-username']";

            // components-MessageActions-components-VoteButtons-index__votesCounter
            public const string likesXPath = "//span[contains(@class, 'components-MessageActions-components-VoteButtons-index__votesCounter')]/text()";

            public const string likesXPathLegacy = "//div/div[4]/div[2]/button[1]//text()";
            public const string dislikesXPath = "//div/div[4]/div[2]/button[2]//text()";

            public const string showMoreXPath = "//button[contains(., 'Show More Comments')]";
            // button that contains reply text
            public const string repliesXPath = "//button[contains(., 'Replies')]";

            public const string sortButtonXPath = "//button[contains(@class, 'sort-filter-button')]";

            public const string sortByCreatedAtXPath = "//ul[contains(@class, 'sorting-tabs')]/li[2]/button[1]";

            public const string sortByCreatedAtTextXPath = "//div[contain(., 'Newest Reactions')]";
        }

        public static string[] stocks  = {
                // "PKK.CN",
                // "IDK.CN",
                // "ART.V",
                "PYR.TO",
                "ZIM",
                "DCM.TO",
                "VPH.CN",
                "DM.V",
                "ACT.CN",
                "POND.V",
                "FGI",
                "ERTH.CN",
                "RET.V",
                "VEON",
                // "PAI.V",
                "DVN.CN",
                "NM.CN"
        };
        // stock twits stocks
        public static string[] tstocks = {
            "ZIM",
            "UAN",
            "FGI",
            "VEON"
            // "PQEFF"
        };
    }
}

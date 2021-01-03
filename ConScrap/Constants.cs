using System;

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

        // copied from SampleData/yahoopkk_comment.html
        public class YahooXPaths
        {
            public const string postDateXPath = "//div/div[1]/span/span";
            
            public const string contentXPath = "//div/div[2]/div";

            public const string authorXPath = "//div/div[1]/button";

            public const string showMoreXPath = "//button[contains(., 'Show more')]";
        }
    }
}

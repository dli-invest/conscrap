using System;

namespace ConScrap
{
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
    }
}

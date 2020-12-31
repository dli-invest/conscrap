using System;

namespace ConScrap
{
    public class Constants
    {
        public const string SampleTemplateLatex = 
@"
\begin{document}
    <ul id='products'>
    {{ for product in products }}
        <li>
        <h2>{{ product }}</h2>
            Price: {{ product}}
        </li>
    {{ end }}
    </ul>
\end{document}";
    }
}

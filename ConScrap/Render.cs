using System;
using Scriban;
namespace ConScrap
{
    public static class Render
    {
        public static Scriban.Template ParseTemplate(string templateStr) 
        {
            if (templateStr is null)
            {
                throw new ArgumentNullException(nameof(templateStr));
            }

            var template = Template.Parse(templateStr);
            return template;
        }
    }
}

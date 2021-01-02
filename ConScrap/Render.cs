using System;
using Scriban;
namespace ConScrap
{
    /// <summary>
    /// Simple Utility functions to Render Scriban Templates and load Template Files
    /// </summary>
    public static class Render
    {
        /// <summary>
        /// Parse Template
        /// </summary>
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

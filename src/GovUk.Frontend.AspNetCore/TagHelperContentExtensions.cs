using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore
{
    /// <summary>
    /// Utility extensions for <see cref="TagHelperContent"/>.
    /// </summary>
    public static class TagHelperContentExtensions
    {
        /// <summary>
        /// Creates a snapshot of the content in a specified <see cref="TagHelperContent"/>.
        /// </summary>
        public static IHtmlContent Snapshot(this TagHelperContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return new HtmlString(content.GetContent());
        }
    }
}

using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore
{
    internal static class TagHelperContentExtensions
    {
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

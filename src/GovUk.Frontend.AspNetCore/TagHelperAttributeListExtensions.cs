using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore
{
    internal static class TagHelperAttributeListExtensions
    {
        public static IDictionary<string, string> ToAttributesDictionary(this TagHelperAttributeList list) =>
            list.ToDictionary(
                a => a.Name,
                a => a.ValueStyle == HtmlAttributeValueStyle.Minimized ? string.Empty : a.Value.ToString());
    }
}

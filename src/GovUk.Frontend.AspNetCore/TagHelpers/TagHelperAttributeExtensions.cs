using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal static class TagHelperAttributeExtensions
{
    public static string? GetEncodedAttributeValue(this TagHelperAttribute attribute) =>
        attribute.ValueStyle switch
        {
            HtmlAttributeValueStyle.Minimized => null,
            _ => attribute.Value is IHtmlContent htmlContent ? htmlContent.ToHtmlString() : HtmlEncoder.Default.Encode(attribute.Value.ToString()!)
        };
}

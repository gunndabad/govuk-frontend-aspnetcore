using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal static class TagHelperOutputExtensions
{
    public static TemplateString? GetUrlAttribute(this TagHelperOutput output, string attributeName)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(attributeName);

        return output.Attributes.TryGetAttribute(attributeName, out var attr) && attr.Value is not null
            ? attr.Value is IHtmlContent hrefHtmlContent ? new TemplateString(hrefHtmlContent) : new TemplateString(attr.Value.ToString())
            : null;
    }
}

using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal static class TagHelperExtensions
{
    public static IHtmlContent? ToHtmlContent(this string? encodedValue) =>
        encodedValue is not null ? new HtmlString(encodedValue) : null;
}

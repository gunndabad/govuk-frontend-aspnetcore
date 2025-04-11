using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal static class Extensions
{
    [return: NotNullIfNotNull(nameof(value))]
    public static IHtmlContent? EncodeHtml(this string? value) =>
        value is not null ? new HtmlString(HtmlEncoder.Default.Encode(value)) : null;
}

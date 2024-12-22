using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal static class StringExtensions
{
    public static string? NormalizeEmptyString(this string? value) => string.IsNullOrEmpty(value) ? null : value;

    public static IHtmlContent? NormalizeEmptyString(this IHtmlContent? htmlString) =>
        string.IsNullOrEmpty(htmlString?.ToHtmlString()) ? null : htmlString;
}

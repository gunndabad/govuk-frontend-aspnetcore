using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal partial class DefaultComponentGenerator : ILegacyComponentGenerator
{
    internal const string FormGroupElement = "div";

    private static void AppendToDescribedBy(ref IHtmlContent describedBy, IHtmlContent value)
    {
        ArgumentNullException.ThrowIfNull(describedBy);

        var str = describedBy.ToHtmlString();
        str = (str + (" " + value)).Trim();
        describedBy = new HtmlString(str);
    }

    private static string[] ExplodeClasses(string? classes) =>
        classes is null ? [] : classes.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    [return: NotNullIfNotNull("text")]
    private static string? HtmlEncode(string? text) =>
        text is not null ? System.Text.Encodings.Web.HtmlEncoder.Default.Encode(text) : null;

    private static IHtmlContent? GetEncodedTextOrHtml(string? text, IHtmlContent? html) =>
        html.NormalizeEmptyString() ?? (text is not null ? new HtmlString(HtmlEncode(text)) : null);
}

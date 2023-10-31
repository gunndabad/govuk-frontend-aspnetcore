using System;
using System.Diagnostics.CodeAnalysis;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Default implementation of <see cref="IComponentGenerator"/>.
/// </summary>
public partial class DefaultComponentGenerator : IComponentGenerator
{
    private static string[] ExplodeClasses(string? classes) =>
        classes is null ? Array.Empty<string>() : classes.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    [return: NotNullIfNotNull("text")]
    private static string? HtmlEncode(string? text) =>
        text is not null ? System.Text.Encodings.Web.HtmlEncoder.Default.Encode(text) : null;

    private static string? GetEncodedTextOrHtml(string? text, string? html) =>
        html.NormalizeEmptyString() ?? HtmlEncode(text);
}

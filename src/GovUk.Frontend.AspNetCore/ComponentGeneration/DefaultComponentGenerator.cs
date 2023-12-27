using System;
using System.Diagnostics.CodeAnalysis;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Default implementation of <see cref="IComponentGenerator"/>.
/// </summary>
public partial class DefaultComponentGenerator : IComponentGenerator
{
    internal const string FormGroupElement = "div";

    private protected static void AppendToDescribedBy(ref string describedBy, string value)
    {
        describedBy += " " + value;
        describedBy = describedBy.Trim();
    }

    private protected virtual HtmlTag GenerateFormGroup(
        FormGroupOptions? options,
        bool haveError)
    {
        return new HtmlTag(FormGroupElement)
            .AddClass("govuk-form-group")
            .AddClassIf(haveError, "govuk-form-group--error")
            .AddClasses(ExplodeClasses(options?.Classes))
            .MergeEncodedAttributes(options?.Attributes);
    }

    private static string[] ExplodeClasses(string? classes) =>
        classes is null ? Array.Empty<string>() : classes.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    [return: NotNullIfNotNull("text")]
    private static string? HtmlEncode(string? text) =>
        text is not null ? System.Text.Encodings.Web.HtmlEncoder.Default.Encode(text) : null;

    private static string? GetEncodedTextOrHtml(string? text, string? html) =>
        html.NormalizeEmptyString() ?? HtmlEncode(text);
}

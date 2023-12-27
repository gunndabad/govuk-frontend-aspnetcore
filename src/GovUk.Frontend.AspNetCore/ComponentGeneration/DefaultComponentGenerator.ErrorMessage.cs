using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string ErrorMessageElement = "p";
    internal const string ErrorMessageDefaultVisuallyHiddenText = "Error";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateErrorMessage(ErrorMessageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(ErrorMessageElement)
            .AddClass("govuk-error-message")
            .AddClasses(ExplodeClasses(options.Classes))
            .AddEncodedAttributeIfNotNull("id", options.Id)
            .MergeEncodedAttributes(options.Attributes)
            .AppendIf(
                options.VisuallyHiddenText != string.Empty,
                () => new HtmlTag("span")
                    .AddClass("govuk-visually-hidden")
                    .AppendText(options.VisuallyHiddenText ?? ErrorMessageDefaultVisuallyHiddenText)
                    .AppendText(": "))
            .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));
    }
}

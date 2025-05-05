using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

partial class LegacyComponentGenerator
{
    internal const string ErrorMessageElement = "p";
    internal const string ErrorMessageDefaultVisuallyHiddenText = "Error";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateErrorMessage(ErrorMessageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(ErrorMessageElement)
            .WithCssClass("govuk-error-message")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributeWhenNotNull(options.Id, "id")
            .WithAttributes(options.Attributes)
            .When(
                options.VisuallyHiddenText?.ToHtmlString() != "",
                b => b.WithAppendedHtml(new HtmlTagBuilder("span")
                    .WithCssClass("govuk-visually-hidden")
                    .WithAppendedHtml(options.VisuallyHiddenText ?? new HtmlString(ErrorMessageDefaultVisuallyHiddenText))
                    .WithAppendedText(": ")))
            .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!);
    }
}

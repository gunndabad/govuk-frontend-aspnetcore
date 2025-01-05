using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string BackLinkElement = "a";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateBackLink(BackLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(BackLinkElement)
            .WithAttribute("href", options.Href.NormalizeEmptyString() ?? new HtmlString("#"))
            .WithCssClass("govuk-back-link")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes ?? [])
            .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html) ?? new HtmlString("Back"));
    }
}

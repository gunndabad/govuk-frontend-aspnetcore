using System;

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
            .AddAttribute("href", options.Href.NormalizeEmptyString() ?? "#", encodeValue: false)
            .AddCssClass("govuk-back-link")
            .AddCssClasses(ExplodeClasses(options.Classes))
            .AddAttributes(options.Attributes ?? [])
            .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html) ?? HtmlEncode("Back"));
    }
}

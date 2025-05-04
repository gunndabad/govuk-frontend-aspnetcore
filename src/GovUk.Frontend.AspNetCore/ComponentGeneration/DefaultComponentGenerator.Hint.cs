using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

partial class DefaultComponentGenerator
{
    internal const string HintElement = "div";

    /// <inheritdoc/>
    protected virtual HtmlTagBuilder GenerateHint(HintOptions options) =>
        GenerateHint(options, allowMissingContent: false);

    private HtmlTagBuilder GenerateHint(HintOptions options, bool allowMissingContent = false)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate(allowMissingContent);

        return new HtmlTagBuilder(HintElement)
            .WithCssClass("govuk-hint")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributeWhenNotNull(options.Id, "id")
            .WithAttributes(options.Attributes)
            .WhenNotNull(GetEncodedTextOrHtml(options.Text, options.Html), (content, b) => b.WithAppendedHtml(content));
    }
}

using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string HintElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateHint(HintOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(HintElement)
            .WithCssClass("govuk-hint")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributeWhenNotNull(options.Id, "id")
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!);
    }
}

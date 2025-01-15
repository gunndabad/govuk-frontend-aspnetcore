using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string TagElement = "strong";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateTag(TagOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(TagElement)
            .WithCssClass("govuk-tag")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!);
    }
}

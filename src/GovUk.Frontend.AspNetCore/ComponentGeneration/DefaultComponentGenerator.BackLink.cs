using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string BackLinkElement = "a";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateBackLink(BackLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(BackLinkElement)
            .UnencodedAttr("href", options.Href.NormalizeEmptyString() ?? "#")
            .AddClass("govuk-back-link")
            .AddClasses(ExplodeClasses(options.Classes))
            .MergeEncodedAttributes(options.Attributes)
            .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html) ?? HtmlEncode("Back"));
    }
}

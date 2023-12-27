using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string HintElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateHint(HintOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(HintElement)
            .AddClass("govuk-hint")
            .AddClasses(ExplodeClasses(options.Classes))
            .AddEncodedAttributeIfNotNull("id", options.Id)
            .MergeEncodedAttributes(options.Attributes)
            .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));
    }
}

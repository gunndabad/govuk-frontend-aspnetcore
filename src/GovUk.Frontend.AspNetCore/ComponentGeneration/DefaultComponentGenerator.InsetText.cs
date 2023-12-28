using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string InsetTextElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateInsetText(InsetTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(InsetTextElement)
            .AddClass("govuk-inset-text")
            .AddClasses(ExplodeClasses(options.Classes))
            .AddEncodedAttributeIfNotNull("id", options.Id)
            .MergeEncodedAttributes(options.Attributes)
            .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));
    }
}

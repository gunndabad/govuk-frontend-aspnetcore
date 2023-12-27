using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string TagElement = "strong";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateTag(TagOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(TagElement)
            .AddClass("govuk-tag")
            .AddClasses(ExplodeClasses(options.Classes))
            .MergeEncodedAttributes(options.Attributes)
            .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));
    }
}

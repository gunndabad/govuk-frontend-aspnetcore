using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string LabelElement = "label";
    internal const bool LabelDefaultIsPageHeading = false;

    /// <inheritdoc/>
    public virtual HtmlTag GenerateLabel(LabelOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var label = new HtmlTag(LabelElement)
            .AddEncodedAttributeIfNotNull("for", options.For)
            .AddClass("govuk-label")
            .AddClasses(ExplodeClasses(options.Classes))
            .MergeEncodedAttributes(options.Attributes)
            .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));

        if (options.IsPageHeading ?? LabelDefaultIsPageHeading)
        {
            return new HtmlTag("h1").AddClass("govuk-label-wrapper").Append(label);
        }
        else
        {
            return label;
        }
    }
}

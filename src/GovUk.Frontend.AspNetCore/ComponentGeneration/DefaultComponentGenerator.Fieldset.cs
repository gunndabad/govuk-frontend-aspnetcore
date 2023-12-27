using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string FieldsetElement = "fieldset";
    internal const bool FieldsetLegendDefaultIsPageHeading = false;
    internal const string FieldsetLegendElement = "legend";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateFieldset(FieldsetOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(FieldsetElement)
            .AddClass("govuk-fieldset")
            .AddClasses(ExplodeClasses(options.Classes))
            .AddEncodedAttributeIfNotNull("role", options.Role.NormalizeEmptyString())
            .AddEncodedAttributeIfNotNull("aria-describedby", options.DescribedBy.NormalizeEmptyString())
            .MergeEncodedAttributes(options.Attributes)
            .AppendIf(
                options.Legend is not null,
                () => new HtmlTag(FieldsetLegendElement)
                    .AddClass("govuk-fieldset__legend")
                    .AddClasses(ExplodeClasses(options.Legend?.Classes))
                    .MergeEncodedAttributes(options.Legend?.Attributes)
                    .Append((options.Legend!.IsPageHeading ?? FieldsetLegendDefaultIsPageHeading ? new HtmlTag("h1") : new HtmlTag("").NoTag())
                        .AddClass("govuk-fieldset__heading")
                        .AppendHtml(GetEncodedTextOrHtml(options.Legend.Text, options.Legend.Html))))
            .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html));
    }
}

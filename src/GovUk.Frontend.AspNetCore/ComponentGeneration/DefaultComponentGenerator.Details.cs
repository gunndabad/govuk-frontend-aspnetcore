using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;
public partial class DefaultComponentGenerator
{
    internal const string DetailsElement = "details";
    internal const string DetailsSummaryElement = "summary";
    internal const string DetailsTextElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateDetails(DetailsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(DetailsElement)
            .AddEncodedAttributeIfNotNull("id", options.Id)
            .AddClass("govuk-details")
            .AddClasses(ExplodeClasses(options.Classes))
            .AddEncodedAttributeIf(options.Open == true, "open", null)
            .MergeEncodedAttributes(options.Attributes)
            .Append(new HtmlTag(DetailsSummaryElement)
                .AddClass("govuk-details__summary")
                .MergeEncodedAttributes(options.SummaryAttributes)
                .Append(new HtmlTag("span")
                    .AddClass("govuk-details__summary-text")
                    .AppendHtml(GetEncodedTextOrHtml(options.SummaryText, options.SummaryHtml))))
            .Append(new HtmlTag(DetailsTextElement)
                .AddClass("govuk-details__text")
                .MergeEncodedAttributes(options.TextAttributes)
                .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html)));
    }
}

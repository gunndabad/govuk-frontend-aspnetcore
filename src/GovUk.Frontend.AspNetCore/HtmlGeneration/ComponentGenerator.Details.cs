using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string DetailsElement = "details";
    internal const bool DetailsDefaultOpen = false;
    internal const string DetailsSummaryElement = "summary";
    internal const string DetailsTextElement = "div";

    public TagBuilder GenerateDetails(
        bool open,
        IHtmlContent summaryContent,
        AttributeDictionary? summaryAttributes,
        IHtmlContent textContent,
        AttributeDictionary? textAttributes,
        AttributeDictionary? attributes)
    {
        Guard.ArgumentNotNull(nameof(summaryContent), summaryContent);
        Guard.ArgumentNotNull(nameof(textContent), textContent);

        var tagBuilder = new TagBuilder(DetailsElement);
        tagBuilder.MergeOptionalAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-details");
        tagBuilder.Attributes.Add("data-module", "govuk-details");

        if (open)
        {
            tagBuilder.Attributes.Add("open", string.Empty);
        }

        var summaryTagBuilder = new TagBuilder(DetailsSummaryElement);
        summaryTagBuilder.MergeOptionalAttributes(summaryAttributes);
        summaryTagBuilder.MergeCssClass("govuk-details__summary");

        var summaryTextTagBuilder = new TagBuilder("span");
        summaryTextTagBuilder.MergeCssClass("govuk-details__summary-text");
        summaryTextTagBuilder.InnerHtml.AppendHtml(summaryContent);
        summaryTagBuilder.InnerHtml.AppendHtml(summaryTextTagBuilder);

        tagBuilder.InnerHtml.AppendHtml(summaryTagBuilder);

        var textTagBuilder = new TagBuilder(DetailsTextElement);
        textTagBuilder.MergeOptionalAttributes(textAttributes);
        textTagBuilder.MergeCssClass("govuk-details__text");
        textTagBuilder.InnerHtml.AppendHtml(textContent);
        tagBuilder.InnerHtml.AppendHtml(textTagBuilder);

        return tagBuilder;
    }
}

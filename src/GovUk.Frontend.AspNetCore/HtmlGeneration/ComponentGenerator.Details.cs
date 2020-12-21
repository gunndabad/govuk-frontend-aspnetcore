#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string DetailsElement = "details";
        internal const bool DetailsDefaultOpen = false;
        internal const string DetailsSummaryElement = "summary";
        internal const string DetailsTextElement = "div";

        public TagBuilder GenerateDetails(
            bool open,
            IHtmlContent summaryContent,
            IDictionary<string, string> summaryAttributes,
            IHtmlContent textContent,
            IDictionary<string, string> textAttributes,
            IDictionary<string, string> attributes)
        {
            Guard.ArgumentNotNull(nameof(summaryContent), summaryContent);
            Guard.ArgumentNotNull(nameof(textContent), textContent);

            var tagBuilder = new TagBuilder(DetailsElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-details");
            tagBuilder.Attributes.Add("data-module", "govuk-details");

            if (open)
            {
                tagBuilder.Attributes.Add("open", string.Empty);
            }

            var summaryTagBuilder = new TagBuilder(DetailsSummaryElement);
            summaryTagBuilder.MergeAttributes(summaryAttributes);
            summaryTagBuilder.AddCssClass("govuk-details__summary");

            var summaryTextTagBuilder = new TagBuilder("span");
            summaryTextTagBuilder.AddCssClass("govuk-details__summary-text");
            summaryTextTagBuilder.InnerHtml.AppendHtml(summaryContent);
            summaryTagBuilder.InnerHtml.AppendHtml(summaryTextTagBuilder);

            tagBuilder.InnerHtml.AppendHtml(summaryTagBuilder);

            var textTagBuilder = new TagBuilder(DetailsTextElement);
            textTagBuilder.MergeAttributes(textAttributes);
            textTagBuilder.AddCssClass("govuk-details__text");
            textTagBuilder.InnerHtml.AppendHtml(textContent);
            tagBuilder.InnerHtml.AppendHtml(textTagBuilder);

            return tagBuilder;
        }
    }
}

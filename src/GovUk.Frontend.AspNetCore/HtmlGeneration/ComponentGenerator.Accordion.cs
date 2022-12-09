using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const int AccordionDefaultHeadingLevel = 2;
        internal const bool AccordionItemDefaultExpanded = false;
        internal const string AccordionElement = "div";
        internal const string AccordionItemElement = "div";
        internal const string AccordionItemSummaryElement = "div";
        internal const int AccordionMinHeadingLevel = 1;
        internal const int AccordionMaxHeadingLevel = 6;

        public TagBuilder GenerateAccordion(
            string id,
            int headingLevel,
            AttributeDictionary? attributes,
            IEnumerable<AccordionItem> items)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(id), id);

            if (headingLevel < AccordionMinHeadingLevel || headingLevel > AccordionMaxHeadingLevel)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(headingLevel)} must be between {AccordionMinHeadingLevel} and {AccordionMaxHeadingLevel}.",
                    nameof(headingLevel));
            }

            Guard.ArgumentNotNullOrEmpty(nameof(items), items);

            var tagBuilder = new TagBuilder(AccordionElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-accordion");
            tagBuilder.Attributes.Add("data-module", "govuk-accordion");
            tagBuilder.Attributes.Add("id", id);

            var index = 0;
            foreach (var item in items)
            {
                Guard.ArgumentValidNotNull(
                    nameof(items),
                    $"Item {index} is not valid; {nameof(AccordionItem.Content)} cannot be null.",
                    item.Content,
                    item.Content != null);

                Guard.ArgumentValidNotNull(
                    nameof(items),
                    $"Item {index} is not valid; {nameof(AccordionItem.HeadingContent)} cannot be null.",
                    item.HeadingContent,
                    item.HeadingContent != null);

                var idSuffix = index + 1;

                var section = new TagBuilder(AccordionItemElement);
                section.MergeOptionalAttributes(item.Attributes);
                section.MergeCssClass("govuk-accordion__section");

                if (item.Expanded)
                {
                    section.MergeCssClass("govuk-accordion__section--expanded");
                }

                var header = new TagBuilder("div");
                header.MergeCssClass("govuk-accordion__section-header");

                var headingId = $"{id}-heading-{idSuffix}";
                var heading = new TagBuilder($"h{headingLevel}");
                heading.MergeOptionalAttributes(item.HeadingAttributes);
                heading.MergeCssClass("govuk-accordion__section-heading");
                var headingContent = new TagBuilder("span");
                headingContent.MergeCssClass("govuk-accordion__section-button");
                headingContent.Attributes.Add("id", headingId);
                headingContent.InnerHtml.AppendHtml(item.HeadingContent);
                heading.InnerHtml.AppendHtml(headingContent);
                header.InnerHtml.AppendHtml(heading);

                if (item.SummaryContent != null)
                {
                    var summaryId = $"{id}-summary-{idSuffix}";
                    var summary = new TagBuilder(AccordionItemSummaryElement);
                    summary.MergeOptionalAttributes(item.SummaryAttributes);
                    summary.MergeCssClass("govuk-accordion__section-summary");
                    summary.MergeCssClass("govuk-body");
                    summary.Attributes.Add("id", summaryId);
                    summary.InnerHtml.AppendHtml(item.SummaryContent);
                    header.InnerHtml.AppendHtml(summary);
                }

                section.InnerHtml.AppendHtml(header);

                var contentId = $"{id}-content-{idSuffix}";
                var contentDiv = new TagBuilder("div");
                contentDiv.MergeCssClass("govuk-accordion__section-content");
                contentDiv.Attributes.Add("id", contentId);
                contentDiv.Attributes.Add("aria-labelledby", headingId);
                contentDiv.InnerHtml.AppendHtml(item.Content);
                section.InnerHtml.AppendHtml(contentDiv);

                tagBuilder.InnerHtml.AppendHtml(section);

                index++;
            }

            return tagBuilder;
        }
    }
}

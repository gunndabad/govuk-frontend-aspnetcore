#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            IDictionary<string, string> attributes,
            IEnumerable<AccordionItem> items)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (headingLevel < AccordionMinHeadingLevel || headingLevel > AccordionMaxHeadingLevel)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(headingLevel)} must be between {AccordionMinHeadingLevel} and {AccordionMaxHeadingLevel}.",
                    nameof(headingLevel));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder(AccordionElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-accordion");
            tagBuilder.Attributes.Add("data-module", "govuk-accordion");
            tagBuilder.Attributes.Add("id", id);

            var index = 0;
            foreach (var item in items)
            {
                if (item.Content == null)
                {
                    throw new ArgumentException(
                        $"Item {index} is not valid; {nameof(AccordionItem.Content)} cannot be null.",
                        nameof(items));
                }

                if (item.HeadingContent == null)
                {
                    throw new ArgumentException(
                        $"Item {index} is not valid; {nameof(AccordionItem.HeadingContent)} cannot be null.",
                        nameof(items));
                }

                var idSuffix = index + 1;

                var section = new TagBuilder(AccordionItemElement);
                section.MergeAttributes(item.Attributes);
                section.AddCssClass("govuk-accordion__section");

                if (item.Expanded)
                {
                    section.AddCssClass("govuk-accordion__section--expanded");
                }

                var header = new TagBuilder("div");
                header.AddCssClass("govuk-accordion__section-header");

                var headingId = $"{id}-heading-{idSuffix}";
                var heading = new TagBuilder($"h{headingLevel}");
                heading.MergeAttributes(item.HeadingAttributes);
                heading.AddCssClass("govuk-accordion__section-heading");
                var headingContent = new TagBuilder("span");
                headingContent.AddCssClass("govuk-accordion__section-button");
                headingContent.Attributes.Add("id", headingId);
                headingContent.InnerHtml.AppendHtml(item.HeadingContent);
                heading.InnerHtml.AppendHtml(headingContent);
                header.InnerHtml.AppendHtml(heading);

                if (item.SummaryContent != null)
                {
                    var summaryId = $"{id}-summary-{idSuffix}";
                    var summary = new TagBuilder(AccordionItemSummaryElement);
                    summary.MergeAttributes(item.SummaryAttributes);
                    summary.AddCssClass("govuk-accordion__section-summary");
                    summary.AddCssClass("govuk-body");
                    summary.Attributes.Add("id", summaryId);
                    summary.InnerHtml.AppendHtml(item.SummaryContent);
                    header.InnerHtml.AppendHtml(summary);
                }

                section.InnerHtml.AppendHtml(header);

                var contentId = $"{id}-content-{idSuffix}";
                var contentDiv = new TagBuilder("div");
                contentDiv.AddCssClass("govuk-accordion__section-content");
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

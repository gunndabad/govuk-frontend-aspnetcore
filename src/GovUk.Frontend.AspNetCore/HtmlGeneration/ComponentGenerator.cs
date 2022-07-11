#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator : IGovUkHtmlGenerator
    {
        internal const string FormGroupElement = "div";

        public TagBuilder GenerateAnchor(string href)
        {
            if (href == null)
            {
                throw new ArgumentNullException(nameof(href));
            }

            var tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", href);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateErrorSummary(
            bool disableAutofocus,
            IHtmlContent titleContent,
            AttributeDictionary titleAttributes,
            IHtmlContent descriptionContent,
            AttributeDictionary descriptionAttributes,
            AttributeDictionary attributes,
            IEnumerable<ErrorSummaryItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-error-summary");
            tagBuilder.Attributes.Add("aria-labelledby", "error-summary-title");
            tagBuilder.Attributes.Add("role", "alert");
            tagBuilder.Attributes.Add("tabindex", "-1");
            tagBuilder.Attributes.Add("data-module", "govuk-error-summary");

            if (disableAutofocus)
            {
                tagBuilder.Attributes.Add("data-disable-auto-focus", "true");
            }

            var heading = new TagBuilder("h2");
            heading.MergeAttributes(titleAttributes);
            heading.MergeCssClass("govuk-error-summary__title");
            heading.Attributes.Add("id", "error-summary-title");
            heading.InnerHtml.AppendHtml(titleContent);
            tagBuilder.InnerHtml.AppendHtml(heading);

            var body = new TagBuilder("div");
            body.MergeCssClass("govuk-error-summary__body");

            if (descriptionContent != null)
            {
                var p = new TagBuilder("p");
                p.MergeAttributes(descriptionAttributes);
                p.InnerHtml.AppendHtml(descriptionContent);
                body.InnerHtml.AppendHtml(p);
            }

            var ul = new TagBuilder("ul");
            ul.MergeCssClass("govuk-list");
            ul.MergeCssClass("govuk-error-summary__list");

            foreach (var item in items)
            {
                var li = new TagBuilder("li");
                li.MergeAttributes(item.Attributes);
                li.InnerHtml.AppendHtml(item.Content);
                ul.InnerHtml.AppendHtml(li);
            }

            body.InnerHtml.AppendHtml(ul);

            tagBuilder.InnerHtml.AppendHtml(body);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateFormGroup(
            bool haveError,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder(FormGroupElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-form-group");

            if (haveError)
            {
                tagBuilder.MergeCssClass("govuk-form-group--error");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateTabs(
            string id,
            string title,
            AttributeDictionary attributes,
            IEnumerable<TabsItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-tabs");
            tagBuilder.Attributes.Add("data-module", "govuk-tabs");

            if (id != null)
            {
                tagBuilder.Attributes.Add("id", id);
            }

            var h2 = new TagBuilder("h2");
            h2.MergeCssClass("govuk-tabs__title");
            h2.InnerHtml.Append(title);
            tagBuilder.InnerHtml.AppendHtml(h2);

            var ul = new TagBuilder("ul");
            ul.MergeCssClass("govuk-tabs__list");

            foreach (var item in items)
            {
                var li = new TagBuilder("li");
                li.MergeCssClass("govuk-tabs__list-item");

                if (item == items.First())
                {
                    li.MergeCssClass("govuk-tabs__list-item--selected");
                }

                var a = new TagBuilder("a");
                a.MergeCssClass("govuk-tabs__tab");
                a.Attributes.Add("href", $"#{item.Id}");
                a.InnerHtml.Append(item.Label);
                li.InnerHtml.AppendHtml(a);

                ul.InnerHtml.AppendHtml(li);
            }

            tagBuilder.InnerHtml.AppendHtml(ul);

            foreach (var item in items)
            {
                var section = new TagBuilder("section");
                section.MergeAttributes(item.PanelAttributes);
                section.MergeCssClass("govuk-tabs__panel");
                section.Attributes.Add("id", item.Id);

                if (item != items.First())
                {
                    section.MergeCssClass("govuk-tabs__panel--hidden");
                }

                section.InnerHtml.AppendHtml(item.PanelContent);

                tagBuilder.InnerHtml.AppendHtml(section);
            }

            return tagBuilder;
        }

        private static void AppendToDescribedBy(ref string? describedBy, string value)
        {
            if (value == null)
            {
                return;
            }

            if (describedBy == null)
            {
                describedBy = value;
            }
            else
            {
                describedBy += $" {value}";
            }
        }
    }
}

#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator : IGovUkHtmlGenerator
    {
        internal const string FormGroupElement = "div";

        public virtual TagBuilder GenerateFormGroup(
            bool haveError,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(FormGroupElement);
            tagBuilder.MergeOptionalAttributes(attributes);
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
            Guard.ArgumentNotNull(nameof(items), items);
            Guard.ArgumentNotNull(nameof(title), title);

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

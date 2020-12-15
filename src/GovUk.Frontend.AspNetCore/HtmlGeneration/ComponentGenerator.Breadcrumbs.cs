#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string BreadcrumbsElement = "div";
        internal const bool BreadcrumbsDefaultCollapseOnMobile = false;
        internal const string BreadcrumbsItemElement = "li";

        public TagBuilder GenerateBreadcrumbs(
            bool collapseOnMobile,
            IDictionary<string, string> attributes,
            IEnumerable<BreadcrumbsItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder(BreadcrumbsElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-breadcrumbs");

            if (collapseOnMobile)
            {
                tagBuilder.AddCssClass("govuk-breadcrumbs--collapse-on-mobile");
            }

            var ol = new TagBuilder("ol");
            ol.AddCssClass("govuk-breadcrumbs__list");

            var index = 0;
            foreach (var item in items)
            {
                if (item.Content == null)
                {
                    throw new ArgumentException(
                        $"Item {index} is not valid; {nameof(BreadcrumbsItem.Content)} cannot be null.",
                        nameof(items));
                }

                var li = new TagBuilder(BreadcrumbsItemElement);
                li.MergeAttributes(item.Attributes);
                li.AddCssClass("govuk-breadcrumbs__list-item");

                IHtmlContent itemContent;

                if (item.Href != null)
                {
                    var itemLink = new TagBuilder("a");
                    itemLink.MergeAttributes(item.LinkAttributes);
                    itemLink.AddCssClass("govuk-breadcrumbs__link");
                    itemLink.Attributes.Add("href", item.Href);
                    itemLink.InnerHtml.AppendHtml(item.Content);
                    itemContent = itemLink;
                }
                else
                {
                    li.Attributes.Add("aria-current", "page");
                    itemContent = item.Content;
                }

                li.InnerHtml.AppendHtml(itemContent);

                ol.InnerHtml.AppendHtml(li);

                index++;
            }

            tagBuilder.InnerHtml.AppendHtml(ol);

            return tagBuilder;
        }
    }
}

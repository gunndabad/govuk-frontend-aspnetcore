using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal partial class ComponentGenerator
    {
        internal const string PaginationDefaultLandmarkLabel = "results";
        internal const string PaginationDefaultPreviousText = "Previous";
        internal const string PaginationDefaultNextText = "Next";
        internal const string PaginationElement = "nav";
        internal const string PaginationEllipsisElement = "li";
        internal const string PaginationItemElement = "li";
        internal const string PaginationNextElement = "div";
        internal const string PaginationPreviousElement = "div";

        public TagBuilder GeneratePagination(
            IEnumerable<PaginationItemBase> items,
            PaginationPrevious? previous,
            PaginationNext? next,
            string? landmarkLabel,
            AttributeDictionary attributes)
        {
            Guard.ArgumentNotNull(nameof(items), items);

            var tagBuilder = new TagBuilder(PaginationElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-pagination");
            tagBuilder.Attributes.Add("role", "navigation");
            tagBuilder.Attributes.Add("aria-label", landmarkLabel ?? PaginationDefaultLandmarkLabel);

            var blockLevel = !items.Any() && (next is not null || previous is not null);

            if (blockLevel)
            {
                tagBuilder.MergeCssClass("govuk-pagination--block");
            }

            if (previous is not null)
            {
                var previousTagBuilder = new TagBuilder(PaginationPreviousElement);
                previousTagBuilder.MergeOptionalAttributes(previous.Attributes);
                previousTagBuilder.MergeCssClass("govuk-pagination__prev");

                var link = new TagBuilder("a");
                link.MergeOptionalAttributes(previous.LinkAttributes);
                link.MergeCssClass("govuk-link govuk-pagination__link");
                link.Attributes.Add("rel", "prev");

                if (previous.Href is not null)
                {
                    link.Attributes.Add("href", previous.Href);
                }

                var previousArrow = GeneratePreviousArrow();
                link.InnerHtml.AppendHtml(previousArrow);

                var title = new TagBuilder("span");
                title.AddCssClass("govuk-pagination__link-title");

                if (blockLevel && previous.LabelText is null)
                {
                    title.AddCssClass("govuk-pagination__link-title--decorated");
                }

                title.InnerHtml.AppendHtml(previous.Text ?? new HtmlString(PaginationDefaultPreviousText));

                link.InnerHtml.AppendHtml(title);

                if (previous.LabelText is not null && blockLevel)
                {
                    var vht = new TagBuilder("span");
                    vht.AddCssClass("govuk-visually-hidden");
                    vht.InnerHtml.Append(":");
                    link.InnerHtml.AppendHtml(vht);

                    var label = new TagBuilder("span");
                    label.AddCssClass("govuk-pagination__link-label");
                    label.InnerHtml.Append(previous.LabelText);
                    link.InnerHtml.AppendHtml(label);
                }

                previousTagBuilder.InnerHtml.AppendHtml(link);

                tagBuilder.InnerHtml.AppendHtml(previousTagBuilder);
            }

            if (items.Any())
            {
                var ul = new TagBuilder("ul");
                ul.AddCssClass("govuk-pagination__list");

                var itemIndex = 0;

                foreach (var item in items)
                {
                    if (item is PaginationItem paginationItem)
                    {
                        Guard.ArgumentValidNotNull(
                            nameof(items),
                            $"Item {itemIndex} is not valid; {nameof(PaginationItem.Number)} cannot be null.",
                            paginationItem.Number,
                            paginationItem.Number != null);

                        var li = new TagBuilder(PaginationItemElement);
                        li.MergeOptionalAttributes(paginationItem.Attributes);
                        li.AddCssClass("govuk-pagination__item");

                        if (paginationItem.IsCurrent)
                        {
                            li.AddCssClass("govuk-pagination__item--current");
                        }

                        var itemLink = new TagBuilder("a");
                        li.MergeOptionalAttributes(paginationItem.Attributes);
                        itemLink.AddCssClass("govuk-link");
                        itemLink.AddCssClass("govuk-pagination__link");
                        itemLink.Attributes.Add("aria-label", paginationItem.VisuallyHiddenText ?? $"Page {paginationItem.Number}");

                        if (paginationItem.Href is not null)
                        {
                            itemLink.Attributes.Add("href", paginationItem.Href);
                        }

                        if (paginationItem.IsCurrent)
                        {
                            itemLink.Attributes.Add("aria-current", "page");
                        }

                        itemLink.InnerHtml.AppendHtml(paginationItem.Number);

                        li.InnerHtml.AppendHtml(itemLink);

                        ul.InnerHtml.AppendHtml(li);
                    }
                    else if (item is PaginationItemEllipsis ellipsisItem)
                    {
                        var li = new TagBuilder(PaginationEllipsisElement);
                        li.MergeOptionalAttributes(ellipsisItem.Attributes);
                        li.AddCssClass("govuk-pagination__item govuk-pagination__item--ellipses");
                        li.InnerHtml.AppendHtml(new HtmlString("&ctdot;"));
                        ul.InnerHtml.AppendHtml(li);
                    }
                    else
                    {
                        throw new NotSupportedException($"Unknown item type: '{item.GetType().FullName}'.");
                    }

                    itemIndex++;
                }

                tagBuilder.InnerHtml.AppendHtml(ul);
            }

            if (next is not null)
            {
                var nextArrow = GenerateNextArrow();

                var nextTagBuilder = new TagBuilder(PaginationNextElement);
                nextTagBuilder.MergeOptionalAttributes(next.Attributes);
                nextTagBuilder.MergeCssClass("govuk-pagination__next");

                var link = new TagBuilder("a");
                link.MergeOptionalAttributes(next.LinkAttributes);
                link.MergeCssClass("govuk-link govuk-pagination__link");
                link.Attributes.Add("rel", "next");

                if (next.Href is not null)
                {
                    link.Attributes.Add("href", next.Href);
                }

                if (blockLevel)
                {
                    link.InnerHtml.AppendHtml(nextArrow);
                }

                var title = new TagBuilder("span");
                title.AddCssClass("govuk-pagination__link-title");

                if (blockLevel && next.LabelText is null)
                {
                    title.AddCssClass("govuk-pagination__link-title--decorated");
                }

                title.InnerHtml.AppendHtml(next.Text ?? new HtmlString(PaginationDefaultNextText));

                link.InnerHtml.AppendHtml(title);

                if (next.LabelText is not null && blockLevel)
                {
                    var vht = new TagBuilder("span");
                    vht.AddCssClass("govuk-visually-hidden");
                    vht.InnerHtml.Append(":");
                    link.InnerHtml.AppendHtml(vht);

                    var label = new TagBuilder("span");
                    label.AddCssClass("govuk-pagination__link-label");
                    label.InnerHtml.Append(next.LabelText);
                    link.InnerHtml.AppendHtml(label);
                }

                if (!blockLevel)
                {
                    link.InnerHtml.AppendHtml(nextArrow);
                }

                nextTagBuilder.InnerHtml.AppendHtml(link);

                tagBuilder.InnerHtml.AppendHtml(nextTagBuilder);
            }

            return tagBuilder;

            TagBuilder GeneratePreviousArrow()
            {
                var svg = new TagBuilder("svg");
                svg.AddCssClass("govuk-pagination__icon");
                svg.AddCssClass("govuk-pagination__icon--prev");
                svg.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
                svg.Attributes.Add("height", "13");
                svg.Attributes.Add("width", "15");
                svg.Attributes.Add("aria-hidden", "true");
                svg.Attributes.Add("focusable", "false");
                svg.Attributes.Add("viewBox", "0 0 15 13");

                var path = new TagBuilder("path");
                path.Attributes.Add("d", "m6.5938-0.0078125-6.7266 6.7266 6.7441 6.4062 1.377-1.449-4.1856-3.9768h12.896v-2h-12.984l4.2931-4.293-1.414-1.414z");

                svg.InnerHtml.AppendHtml(path);

                return svg;
            }

            TagBuilder GenerateNextArrow()
            {
                var svg = new TagBuilder("svg");
                svg.AddCssClass("govuk-pagination__icon");
                svg.AddCssClass("govuk-pagination__icon--next");
                svg.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
                svg.Attributes.Add("height", "13");
                svg.Attributes.Add("width", "15");
                svg.Attributes.Add("aria-hidden", "true");
                svg.Attributes.Add("focusable", "false");
                svg.Attributes.Add("viewBox", "0 0 15 13");

                var path = new TagBuilder("path");
                path.Attributes.Add("d", "m8.107-0.0078125-1.4136 1.414 4.2926 4.293h-12.986v2h12.896l-4.1855 3.9766 1.377 1.4492 6.7441-6.4062-6.7246-6.7266z");

                svg.InnerHtml.AppendHtml(path);

                return svg;
            }
        }
    }
}

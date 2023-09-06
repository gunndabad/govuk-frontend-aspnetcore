using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string TabsDefaultTitle = "Contents";
    internal const string TabsElement = "div";
    internal const string TabsItemPanelElement = "div";

    public virtual TagBuilder GenerateTabs(
        string id,
        string idPrefix,
        string title,
        AttributeDictionary? attributes,
        IEnumerable<TabsItem> items)
    {
        Guard.ArgumentNotNull(nameof(title), title);
        Guard.ArgumentNotNull(nameof(items), items);

        var itemIds = ResolveItemIds();

        var tagBuilder = new TagBuilder(TabsElement);
        tagBuilder.MergeOptionalAttributes(attributes);
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

        var index = 0;
        foreach (var item in items)
        {
            Guard.ArgumentValidNotNull(
                nameof(items),
                $"Item {index} is not valid; {nameof(TabsItem.Label)} cannot be null.",
                item.Label,
                item.Label != null);

            var itemId = itemIds[index];

            var li = new TagBuilder("li");
            li.MergeCssClass("govuk-tabs__list-item");

            if (index == 0)
            {
                li.MergeCssClass("govuk-tabs__list-item--selected");
            }

            var a = new TagBuilder("a");
            a.MergeOptionalAttributes(item.LinkAttributes);
            a.MergeCssClass("govuk-tabs__tab");
            a.Attributes.Add("href", $"#{itemId}");
            a.InnerHtml.Append(item.Label);
            li.InnerHtml.AppendHtml(a);

            ul.InnerHtml.AppendHtml(li);

            index++;
        }

        tagBuilder.InnerHtml.AppendHtml(ul);

        index = 0;
        foreach (var item in items)
        {
            Guard.ArgumentValidNotNull(
                nameof(items),
                $"Item {index} is not valid; {nameof(TabsItem.PanelContent)} cannot be null.",
                item.PanelContent,
                item.PanelContent != null);

            var itemId = itemIds[index];

            var panel = new TagBuilder(TabsItemPanelElement);
            panel.MergeOptionalAttributes(item.PanelAttributes);
            panel.MergeCssClass("govuk-tabs__panel");
            panel.Attributes.Add("id", itemId);

            if (index != 0)
            {
                panel.MergeCssClass("govuk-tabs__panel--hidden");
            }

            panel.InnerHtml.AppendHtml(item.PanelContent);

            tagBuilder.InnerHtml.AppendHtml(panel);

            index++;
        }

        return tagBuilder;

        string[] ResolveItemIds() => items
            .Select((item, index) =>
            {
                if (item.Id != null)
                {
                    return item.Id;
                }

                if (idPrefix == null)
                {
                    throw new ArgumentException(
                        $"Item {index} is not valid; {nameof(TabsItem.Id)} must be specified when {nameof(idPrefix)} is null.",
                        nameof(items));
                }

                return $"{idPrefix}-{index + 1}";
            })
            .ToArray();
    }
}

using System;
using System.Linq;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string TabsDefaultTitle = "Contents";
    internal const string TabsElement = "div";
    internal const string TabsItemPanelElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateTabs(TabsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        HtmlTagBuilder TabListItem(TabsOptionsItem item, int index)
        {
            var tabPanelId = item.Id?.NormalizeEmptyString() is IHtmlContent id ? id : new HtmlString($"{options.IdPrefix}-{index}");

            return new HtmlTagBuilder("li")
                .WithCssClass("govuk-tabs__list-item")
                .When(index == 1, b => b.WithCssClass("govuk-tabs__list-item--selected"))
                .WithAppendedHtml(new HtmlTagBuilder("a")
                    .WithCssClass("govuk-tabs__tab")
                    .WithAttribute("href", $"#{tabPanelId.ToHtmlString()}", encodeValue: false)
                    .WithAttributes(item.Attributes)
                    .WithAppendedHtml(item.Label!));
        }

        HtmlTagBuilder TabPanel(TabsOptionsItem item, int index)
        {
            var tabPanelId = item.Id?.NormalizeEmptyString() is IHtmlContent id ? id : new HtmlString($"{options.IdPrefix}-{index}");

            return new HtmlTagBuilder(TabsItemPanelElement)
                .WithCssClass("govuk-tabs__panel")
                .When(index > 1, b => b.WithCssClass("govuk-tabs__panel--hidden"))
                .WithAttribute("id", tabPanelId)
                .WithAttributes(item.Panel?.Attributes)
                .When(
                    GetEncodedTextOrHtml(item.Panel?.Text, item.Panel?.Html) is not null,
                    b => b.WithAppendedHtml(
                        item.Panel?.Html.NormalizeEmptyString() is IHtmlContent html
                            ? html
                            : new HtmlTagBuilder("p").WithCssClass("govuk-body").WithAppendedText(item.Panel!.Text!)));
        }

        return new HtmlTagBuilder(TabsElement)
            .WhenNotNull(options.Id, (id, b) => b.WithAttribute("id", id))
            .WithCssClass("govuk-tabs")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes)
            .WithAttribute("data-module", "govuk-tabs", encodeValue: false)
            .WithAppendedHtml(new HtmlTagBuilder("h2")
                .WithCssClass("govuk-tabs__title")
                .WithAppendedHtml(options.Title.NormalizeEmptyString() ?? new HtmlString(TabsDefaultTitle)))
            .When(
                options.Items?.Count > 0,
                b => b
                    .WithAppendedHtml(new HtmlTagBuilder("ul")
                        .WithCssClass("govuk-tabs__list")
                        .WithAppendedHtml(options.Items!.Select((item, index) => TabListItem(item, index + 1))))
                    .WithAppendedHtml(options.Items!.Select((item, index) => TabPanel(item, index + 1))));
    }
}

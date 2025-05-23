using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string SummaryCardElement = "div";
    internal const int SummaryCardDefaultHeadingLevel = 2;
    internal const int SummaryCardMinHeadingLevel = 1;
    internal const int SummaryCardMaxHeadingLevel = 6;

    public TagBuilder GenerateSummaryCard(
        SummaryCardTitle? title,
        SummaryListActions? actions,
        IHtmlContent summaryList,
        AttributeDictionary? attributes)
    {
        Guard.ArgumentValidNotNull(
            nameof(summaryList),
            $"Summary card is not valid; {nameof(summaryList)} cannot be null.",
            summaryList,
            summaryList is not null);

        var tagBuilder = new TagBuilder(SummaryCardElement);
        tagBuilder.MergeOptionalAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-summary-card");

        var headerTagBuilder = new TagBuilder("div");
        headerTagBuilder.MergeCssClass("govuk-summary-card__title-wrapper");
        tagBuilder.InnerHtml.AppendHtml(headerTagBuilder);

        if (title is not null)
        {
            var titleTagBuilder = new TagBuilder($"h{title.HeadingLevel ?? SummaryCardDefaultHeadingLevel}");
            titleTagBuilder.MergeOptionalAttributes(title.Attributes);
            titleTagBuilder.MergeCssClass("govuk-summary-card__title");
            titleTagBuilder.InnerHtml.AppendHtml(title.Content ?? new HtmlString(""));

            headerTagBuilder.InnerHtml.AppendHtml(titleTagBuilder);
        }

        if (actions?.Items?.Count > 0)
        {
            var actionsWrapper = new TagBuilder(actions.Items.Count == 1 ? "div" : "ul");
            actionsWrapper.MergeOptionalAttributes(actions.Attributes);
            actionsWrapper.MergeCssClass("govuk-summary-card__actions");

            if (actions.Items!.Count == 1)
            {
                actionsWrapper.InnerHtml.AppendHtml(GenerateLink(actions.Items![0], actionIndex: 0, title));
            }
            else
            {
                var actionIndex = 0;
                foreach (var action in actions.Items)
                {
                    var li = new TagBuilder("li");
                    li.MergeCssClass("govuk-summary-card__action");
                    li.InnerHtml.AppendHtml(GenerateLink(action, actionIndex++, title));

                    actionsWrapper.InnerHtml.AppendHtml(li);
                }
            }

            headerTagBuilder.InnerHtml.AppendHtml(actionsWrapper);
        }

        var contentTagBuilder = new TagBuilder("div");
        contentTagBuilder.MergeCssClass("govuk-summary-card__content");
        tagBuilder.InnerHtml.AppendHtml(contentTagBuilder);
        contentTagBuilder.InnerHtml.AppendHtml(summaryList);

        return tagBuilder;
    }

    static TagBuilder GenerateLink(SummaryListAction action, int actionIndex, SummaryCardTitle? title)
    {
        Guard.ArgumentValidNotNull(
            nameof(SummaryCard.Actions),
            $"Action {actionIndex} is not valid; {nameof(SummaryListAction.Content)} cannot be null.",
            action.Content,
            action.Content is not null);

        var anchor = new TagBuilder(SummaryListRowActionElement);
        anchor.MergeOptionalAttributes(action.Attributes);
        anchor.MergeCssClass("govuk-link");
        anchor.InnerHtml.AppendHtml(action.Content);

        if (action.VisuallyHiddenText is not null || title?.Content is not null)
        {
            var vht = new TagBuilder("span");
            vht.MergeCssClass("govuk-visually-hidden");

            if (action.VisuallyHiddenText is not null)
            {
                vht.InnerHtml.Append(action.VisuallyHiddenText);
            }

            if (title?.Content is not null)
            {
                vht.InnerHtml.Append(" (");
                vht.InnerHtml.AppendHtml(title.Content);
                vht.InnerHtml.Append(")");
            }

            anchor.InnerHtml.AppendHtml(vht);
        }

        return anchor;
    }
}

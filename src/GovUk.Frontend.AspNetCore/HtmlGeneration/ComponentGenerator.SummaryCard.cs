#nullable enable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string SummaryCardElement = "div";
        internal const int SummaryCardDefaultHeadingLevel = 2;
        internal const int SummaryCardMinHeadingLevel = 1;
        internal const int SummaryCardMaxHeadingLevel = 6;
        internal const string SummaryCardActionsElement = "ul";
        internal const string SummaryCardActionElement = "li";

        public TagBuilder GenerateSummaryCard(SummaryCard summaryCard)
        {
            Guard.ArgumentValidNotNull(
                nameof(summaryCard.TitleContent),
                $"Summary card is not valid; {nameof(summaryCard.TitleContent)} cannot be null.",
                summaryCard.TitleContent,
                summaryCard.TitleContent != null);

            Guard.ArgumentValidNotNull(
               nameof(summaryCard.SummaryList),
               $"Summary card is not valid; {nameof(summaryCard.SummaryList)} cannot be null.",
               summaryCard.SummaryList,
               summaryCard.SummaryList != null && !string.IsNullOrWhiteSpace(summaryCard.SummaryList.ToString()));

            var tagBuilder = new TagBuilder(SummaryCardElement);
            if (summaryCard.CardAttributes != null) { tagBuilder.MergeAttributes(summaryCard.CardAttributes); }
            tagBuilder.MergeCssClass("govuk-summary-card");

            var titleTagBuilder = new TagBuilder($"h{summaryCard.HeadingLevel}");
            if (summaryCard.TitleAttributes != null) { titleTagBuilder.MergeAttributes(summaryCard.TitleAttributes); }
            titleTagBuilder.MergeCssClass("govuk-summary-card__title");
            titleTagBuilder.InnerHtml.AppendHtml(summaryCard.TitleContent);

            var headerTagBuilder = new TagBuilder("div");
            headerTagBuilder.MergeCssClass("govuk-summary-card__title-wrapper");
            headerTagBuilder.InnerHtml.AppendHtml(titleTagBuilder);
            tagBuilder.InnerHtml.AppendHtml(headerTagBuilder);

            if (summaryCard.Actions?.Items != null && summaryCard.Actions.Items.Count > 0)
            {
                var actionsWrapper = new TagBuilder(summaryCard.Actions.Items.Count == 1 ? "div" : SummaryCardActionsElement);
                if (summaryCard.Actions.Attributes != null) { actionsWrapper.MergeAttributes(summaryCard.Actions.Attributes); }
                actionsWrapper.MergeCssClass("govuk-summary-card__actions");

                var actionIndex = 1;
                foreach (var action in summaryCard.Actions.Items)
                {
                    var actionElement = new TagBuilder(summaryCard.Actions.Items.Count == 1 ? "div" : SummaryCardActionElement);
                    if (action.Attributes != null) { actionElement.MergeAttributes(action.Attributes); }
                    actionElement.MergeCssClass("govuk-summary-card__action");
                    actionElement.InnerHtml.AppendHtml(GenerateLink(action, actionIndex));

                    actionsWrapper.InnerHtml.AppendHtml(actionElement);
                    actionIndex++;
                }

                headerTagBuilder.InnerHtml.AppendHtml(actionsWrapper);
            }

            if (summaryCard.SummaryList != null)
            {
                var contentTagBuilder = new TagBuilder("div");
                contentTagBuilder.MergeCssClass("govuk-summary-card__content");
                tagBuilder.InnerHtml.AppendHtml(contentTagBuilder);
                contentTagBuilder.InnerHtml.AppendHtml(summaryCard.SummaryList);
            }

            return tagBuilder;
        }

        static TagBuilder GenerateLink(SummaryCardAction action, int actionIndex)
        {
            Guard.ArgumentValidNotNull(
                nameof(SummaryCard.Actions),
                $"Action {actionIndex} is not valid; {nameof(SummaryCardAction.Content)} cannot be null.",
                action.Content,
                action.Content != null);

            Guard.ArgumentNotNullOrEmpty(nameof(action.Href), action.Href);

            var anchor = new TagBuilder("a");
            anchor.MergeAttribute("href", action.Href);
            anchor.MergeCssClass("govuk-link");
            anchor.InnerHtml.AppendHtml(action.Content);

            if (action.VisuallyHiddenText != null)
            {
                var vht = new TagBuilder("span");
                vht.MergeCssClass("govuk-visually-hidden");
                vht.InnerHtml.Append(action.VisuallyHiddenText);
                anchor.InnerHtml.AppendHtml(vht);
            }

            return anchor;
        }
    }
}

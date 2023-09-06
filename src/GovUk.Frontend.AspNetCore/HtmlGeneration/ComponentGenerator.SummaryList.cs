using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string SummaryListElement = "dl";
    internal const string SummaryListRowActionElement = "a";
    internal const string SummaryListRowActionsElement = "dd";
    internal const string SummaryListRowElement = "div";
    internal const string SummaryListRowKeyElement = "dt";
    internal const string SummaryListRowValueElement = "dd";

    public TagBuilder GenerateSummaryList(
        IEnumerable<SummaryListRow> rows,
        AttributeDictionary? attributes)
    {
        Guard.ArgumentNotNull(nameof(rows), rows);

        var anyRowHasActions = rows.Any(r => r.Actions?.Items?.Any() == true);

        var tagBuilder = new TagBuilder(SummaryListElement);
        tagBuilder.MergeOptionalAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-summary-list");

        var rowIndex = 0;
        foreach (var row in rows)
        {
            Guard.ArgumentValidNotNull(
                nameof(rows),
                $"Row {rowIndex} is not valid; {nameof(SummaryListRow.Key)} cannot be null.",
                row.Key,
                row.Key != null);

            Guard.ArgumentValidNotNull(
                nameof(rows),
                $"Row {rowIndex} is not valid; {nameof(SummaryListRow.Key)}.{nameof(SummaryListRow.Key.Content)} cannot be null.",
                row.Key.Content,
                row.Key.Content != null);

            Guard.ArgumentValidNotNull(
                nameof(rows),
                $"Row {rowIndex} is not valid; {nameof(SummaryListRow.Value)} cannot be null.",
                row.Value,
                row.Value != null);

            Guard.ArgumentValidNotNull(
                nameof(rows),
                $"Row {rowIndex} is not valid; {nameof(SummaryListRow.Value)}.{nameof(SummaryListRow.Value.Content)} cannot be null.",
                row.Value.Content,
                row.Value.Content != null);

            var thisRowHasActions = row.Actions?.Items?.Any() == true;

            var rowTagBuilder = new TagBuilder(SummaryListRowElement);
            rowTagBuilder.MergeOptionalAttributes(row.Attributes);
            rowTagBuilder.MergeCssClass("govuk-summary-list__row");

            if (anyRowHasActions && !thisRowHasActions)
            {
                rowTagBuilder.MergeCssClass("govuk-summary-list__row--no-actions");
            }

            var dt = new TagBuilder(SummaryListRowKeyElement);
            dt.MergeOptionalAttributes(row.Key.Attributes);
            dt.MergeCssClass("govuk-summary-list__key");
            dt.InnerHtml.AppendHtml(row.Key.Content);
            rowTagBuilder.InnerHtml.AppendHtml(dt);

            if (row.Value is not null)
            {
                var dd = new TagBuilder(SummaryListRowValueElement);
                dd.MergeOptionalAttributes(row.Value.Attributes);
                dd.MergeCssClass("govuk-summary-list__value");
                dd.InnerHtml.AppendHtml(row.Value.Content);
                rowTagBuilder.InnerHtml.AppendHtml(dd);
            }

            if (thisRowHasActions)
            {
                var actionsDd = new TagBuilder(SummaryListRowActionsElement);
                actionsDd.MergeOptionalAttributes(row.Actions!.Attributes);
                actionsDd.MergeCssClass("govuk-summary-list__actions");

                if (row.Actions.Items!.Count == 1)
                {
                    actionsDd.InnerHtml.AppendHtml(GenerateLink(row.Actions.Items![0], rowIndex));
                }
                else
                {
                    var ul = new TagBuilder("ul");
                    ul.MergeCssClass("govuk-summary-list__actions-list");

                    foreach (var action in row.Actions.Items!)
                    {
                        var li = new TagBuilder("li");
                        li.MergeCssClass("govuk-summary-list__actions-list-item");
                        li.InnerHtml.AppendHtml(GenerateLink(action, rowIndex));

                        ul.InnerHtml.AppendHtml(li);
                    }

                    actionsDd.InnerHtml.AppendHtml(ul);
                }

                rowTagBuilder.InnerHtml.AppendHtml(actionsDd);
            }

            tagBuilder.InnerHtml.AppendHtml(rowTagBuilder);

            rowIndex++;
        }

        return tagBuilder;

        static TagBuilder GenerateLink(SummaryListAction action, int rowIndex)
        {
            Guard.ArgumentValidNotNull(
                nameof(rows),
                $"Row {rowIndex} is not valid; {nameof(SummaryListAction.Content)} cannot be null.",
                action.Content,
                action.Content != null);

            var anchor = new TagBuilder(SummaryListRowActionElement);
            anchor.MergeOptionalAttributes(action.Attributes);
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

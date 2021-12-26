#nullable enable
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string SummaryListElement = "dl";
        internal const string SummaryListRowActionElement = "a";
        internal const string SummaryListRowActionsElement = "dd";
        internal const string SummaryListRowElement = "div";
        internal const string SummaryListRowKeyElement = "dt";
        internal const string SummaryListRowValueElement = "dd";

        public TagBuilder GenerateSummaryList(
            AttributeDictionary? attributes,
            IEnumerable<SummaryListRow> rows)
        {
            Guard.ArgumentNotNull(nameof(rows), rows);

            var anyRowHasActions = rows.Any(r => r.Actions?.Items.Any() == true);

            var tagBuilder = new TagBuilder(SummaryListElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-summary-list");

            var index = 0;
            foreach (var row in rows)
            {
                Guard.ArgumentValidNotNull(
                    nameof(rows),
                    $"Row {index} is not valid; {nameof(SummaryListRow.Key)} cannot be null.",
                    row.Key,
                    row.Key != null);

                Guard.ArgumentValidNotNull(
                    nameof(rows),
                    $"Row {index} is not valid; {nameof(SummaryListRow.Key)}.{nameof(SummaryListRow.Key.Content)} cannot be null.",
                    row.Key.Content,
                    row.Key.Content != null);

                Guard.ArgumentValidNotNull(
                    nameof(rows),
                    $"Row {index} is not valid; {nameof(SummaryListRow.Value)} cannot be null.",
                    row.Value,
                    row.Value != null);

                Guard.ArgumentValidNotNull(
                    nameof(rows),
                    $"Row {index} is not valid; {nameof(SummaryListRow.Value)}.{nameof(SummaryListRow.Value.Content)} cannot be null.",
                    row.Value.Content,
                    row.Value.Content != null);

                var rowTagBuilder = new TagBuilder(SummaryListRowElement);
                rowTagBuilder.MergeAttributes(row.Attributes);
                rowTagBuilder.MergeCssClass("govuk-summary-list__row");

                var dt = new TagBuilder(SummaryListRowKeyElement);
                dt.MergeAttributes(row.Key.Attributes);
                dt.MergeCssClass("govuk-summary-list__key");
                dt.InnerHtml.AppendHtml(row.Key.Content);
                rowTagBuilder.InnerHtml.AppendHtml(dt);

                var dd = new TagBuilder(SummaryListRowValueElement);
                dd.MergeAttributes(row.Value.Attributes);
                dd.MergeCssClass("govuk-summary-list__value");
                dd.InnerHtml.AppendHtml(row.Value.Content);
                rowTagBuilder.InnerHtml.AppendHtml(dd);

                if (anyRowHasActions)
                {
                    if (row.Actions?.Items.Any() == true)
                    {
                        var actionsDd = new TagBuilder(SummaryListRowActionsElement);
                        actionsDd.MergeAttributes(row.Actions.Attributes);
                        actionsDd.MergeCssClass("govuk-summary-list__actions");

                        if (row.Actions.Items.Count() == 1)
                        {
                            actionsDd.InnerHtml.AppendHtml(GenerateLink(row.Actions.Items.Single()));
                        }
                        else
                        {
                            var ul = new TagBuilder("ul");
                            ul.MergeCssClass("govuk-summary-list__actions-list");

                            foreach (var action in row.Actions.Items!)
                            {
                                var li = new TagBuilder("li");
                                li.MergeCssClass("govuk-summary-list__actions-list-item");
                                li.InnerHtml.AppendHtml(GenerateLink(action));

                                ul.InnerHtml.AppendHtml(li);
                            }

                            actionsDd.InnerHtml.AppendHtml(ul);
                        }

                        rowTagBuilder.InnerHtml.AppendHtml(actionsDd);
                    }
                    else
                    {
                        var span = new TagBuilder("span");
                        span.MergeCssClass("govuk-summary-list__actions");
                        rowTagBuilder.InnerHtml.AppendHtml(span);
                    }
                }

                tagBuilder.InnerHtml.AppendHtml(rowTagBuilder);

                index++;
            }

            return tagBuilder;

            static TagBuilder GenerateLink(SummaryListRowAction action)
            {
                var anchor = new TagBuilder(SummaryListRowActionElement);
                anchor.MergeAttributes(action.Attributes);
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
}

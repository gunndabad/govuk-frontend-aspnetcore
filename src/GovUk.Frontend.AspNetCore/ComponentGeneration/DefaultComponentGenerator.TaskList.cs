using System;
using System.Linq;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string TaskListElement = "ul";

    /// <inheritdoc/>
    public HtmlTag GenerateTaskList(TaskListOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var idPrefix = options.IdPrefix ?? "task-list";

        return new HtmlTag(TaskListElement)
            .AddClass("govuk-task-list")
            .AddClasses(ExplodeClasses(options.Classes))
            .MergeEncodedAttributes(options.Attributes)
            .Append(
                options.Items!.Select(
                    (item, index) =>
                    {
                        var hintId = $"{idPrefix}-{index + 1}-hint";
                        var statusId = $"{idPrefix}-{index + 1}-status";

                        var gotLink = item.Href.NormalizeEmptyString() is not null;
                        var gotHint = item.Hint is not null;

                        return new HtmlTag("li")
                            .AddClass("govuk-task-list__item")
                            .AddClassIf(gotLink, "govuk-task-list__item--with-link")
                            .AddClasses(ExplodeClasses(item.Classes))
                            .Append(
                                new HtmlTag("div")
                                    .AddClass("govuk-task-list__name-and-hint")
                                    .Append(
                                        gotLink
                                            ? new HtmlTag("a")
                                                .AddClasses("govuk-link", "govuk-task-list__link")
                                                .AddClasses(ExplodeClasses(item.Title!.Classes))
                                                .UnencodedAttr("href", item.Href)
                                                .Attr("aria-describedby", (gotHint ? $"{hintId} " : "") + statusId)
                                                .AppendHtml(GetEncodedTextOrHtml(item.Title.Text, item.Title.Html))
                                            : new HtmlTag("div")
                                                .AddClasses(ExplodeClasses(item.Title!.Classes))
                                                .AppendHtml(GetEncodedTextOrHtml(item.Title!.Text, item.Title.Html))
                                    )
                                    .AppendIf(
                                        gotHint,
                                        () =>
                                            new HtmlTag("div")
                                                .Id(hintId)
                                                .AddClass("govuk-task-list__hint")
                                                .AppendHtml(GetEncodedTextOrHtml(item.Hint!.Text, item.Hint.Html))
                                    )
                            )
                            .Append(
                                new HtmlTag("div")
                                    .AddClass("govuk-task-list__status")
                                    .AddClasses(ExplodeClasses(item.Status!.Classes))
                                    .Id(statusId)
                                    .Append(
                                        item.Status.Tag is not null
                                            ? GenerateTag(item.Status.Tag)
                                            : HtmlTag
                                                .Placeholder()
                                                .AppendHtml(GetEncodedTextOrHtml(item.Status.Text, item.Status.Html))
                                    )
                            );
                    }
                )
            );
    }
}

using System;
using System.Linq;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

partial class LegacyComponentGenerator
{
    internal const string TaskListElement = "ul";

    /// <inheritdoc/>
    public HtmlTagBuilder GenerateTaskList(TaskListOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var idPrefix = options.IdPrefix ?? new HtmlString("task-list");

        return new HtmlTagBuilder(TaskListElement)
            .WithCssClass("govuk-task-list")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(options.Items!.Select((item, index) =>
            {
                var hintId = $"{idPrefix}-{index + 1}-hint";
                var statusId = $"{idPrefix}-{index + 1}-status";

                var gotLink = item.Href.NormalizeEmptyString() is not null;
                var gotHint = item.Hint is not null;

                return new HtmlTagBuilder("li")
                    .WithCssClass("govuk-task-list__item")
                    .When(gotLink, b => b.WithCssClass("govuk-task-list__item--with-link"))
                    .WithCssClasses(ExplodeClasses(item.Classes?.ToHtmlString()))
                    .WithAppendedHtml(new HtmlTagBuilder("div")
                        .WithCssClass("govuk-task-list__name-and-hint")
                        .WithAppendedHtml(gotLink ?
                            new HtmlTagBuilder("a")
                                .WithCssClasses("govuk-link", "govuk-task-list__link")
                                .WithCssClasses(ExplodeClasses(item.Title!.Classes?.ToHtmlString()))
                                .WithAttribute("href", item.Href!)
                                .WithAttribute("aria-describedby", (gotHint ? $"{hintId} " : "") + statusId, encodeValue: false)
                                .WithAppendedHtml(GetEncodedTextOrHtml(item.Title.Text, item.Title.Html)!) :
                            new HtmlTagBuilder("div")
                                .WithCssClasses(ExplodeClasses(item.Title!.Classes?.ToHtmlString()))
                                .WithAppendedHtml(GetEncodedTextOrHtml(item.Title!.Text, item.Title.Html)!))
                        .When(
                            gotHint,
                            b => b.WithAppendedHtml(new HtmlTagBuilder("div")
                                .WithAttribute("id", hintId, encodeValue: false)
                                .WithCssClass("govuk-task-list__hint")
                                .WithAppendedHtml(GetEncodedTextOrHtml(item.Hint!.Text, item.Hint.Html)!))))
                    .WithAppendedHtml(new HtmlTagBuilder("div")
                        .WithCssClass("govuk-task-list__status")
                        .WithCssClasses(ExplodeClasses(item.Status!.Classes?.ToHtmlString()))
                        .WithAttribute("id", statusId, encodeValue: false)
                        .WithAppendedHtml(item.Status.Tag is not null ?
                            GenerateTag(item.Status.Tag) :
                            GetEncodedTextOrHtml(item.Status.Text, item.Status.Html)!));
            }));
    }
}

using System;
using System.Collections.Immutable;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string ExitThisPageElement = "div";

    /// <inheritdoc/>
    public HtmlTag GenerateExitThisPage(ExitThisPageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(ExitThisPageElement)
            .AddEncodedAttributeIfNotNull("id", options.Id)
            .AddClass("govuk-exit-this-page")
            .AddClasses(ExplodeClasses(options.Classes))
            .UnencodedAttr("data-module", "govuk-exit-this-page")
            .MergeEncodedAttributes(options.Attributes)
            .AddEncodedAttributeIf(
                options.ActivatedText.NormalizeEmptyString() is not null,
                "data-i18n.activated",
                HtmlEncode(options.ActivatedText)
            )
            .AddEncodedAttributeIf(
                options.TimedOutText.NormalizeEmptyString() is not null,
                "data-i18n.timed-out",
                HtmlEncode(options.TimedOutText)
            )
            .AddEncodedAttributeIf(
                options.PressTwoMoreTimesText.NormalizeEmptyString() is not null,
                "data-i18n.press-two-more-times",
                HtmlEncode(options.PressTwoMoreTimesText)
            )
            .AddEncodedAttributeIf(
                options.PressOneMoreTimeText.NormalizeEmptyString() is not null,
                "data-i18n.press-one-more-time",
                HtmlEncode(options.PressOneMoreTimeText)
            )
            .Append(
                GenerateButton(
                    new ButtonOptions()
                    {
                        Html = (options.Html.NormalizeEmptyString() ?? options.Text.NormalizeEmptyString()) is not null
                            ? options.Html
                            : new HtmlTag("span")
                                .AddClass("govuk-visually-hidden")
                                .Text("Emergency")
                                .After(new HtmlTag(null).Text("Exit this page"))
                                .ToHtmlString(),
                        Text = options.Text.NormalizeEmptyString(),
                        Classes = "govuk-button--warning govuk-exit-this-page__button govuk-js-exit-this-page-button",
                        Href = options.RedirectUrl.NormalizeEmptyString() ?? "https://www.bbc.co.uk/weather",
                        Attributes = ImmutableDictionary<string, string?>.Empty.Add("rel", "nofollow noreferrer"),
                    }
                )
            );
    }
}

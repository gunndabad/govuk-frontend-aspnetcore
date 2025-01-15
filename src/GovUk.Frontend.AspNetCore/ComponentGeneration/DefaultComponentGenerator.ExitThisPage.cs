using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string ExitThisPageElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateExitThisPage(ExitThisPageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(ExitThisPageElement)
            .WithAttributeWhenNotNull(options.Id, "id")
            .WithCssClass("govuk-exit-this-page")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttribute("data-module", "govuk-exit-this-page", encodeValue: false)
            .WithAttributes(options.Attributes)
            .WithAttributeWhenNotNull(options.ActivatedText.NormalizeEmptyString(), "data-i18n.activated")
            .WithAttributeWhenNotNull(options.TimedOutText.NormalizeEmptyString(), "data-i18n.timed-out")
            .WithAttributeWhenNotNull(options.PressTwoMoreTimesText.NormalizeEmptyString(), "data-i18n.press-two-more-times")
            .WithAttributeWhenNotNull(options.PressOneMoreTimeText.NormalizeEmptyString(), "data-i18n.press-one-more-time")
            .WithAppendedHtml(() =>
            {
                IHtmlContent buttonHtml;

                if (options.Html.NormalizeEmptyString() is not null ||
                    options.Text.NormalizeEmptyString() is not null)
                {
                    buttonHtml = options.Html!;
                }
                else
                {
                    var buttonHtmlContentBuilder = new HtmlContentBuilder();
                    buttonHtmlContentBuilder.AppendHtml(
                        new HtmlTagBuilder("span")
                            .WithCssClass("govuk-visually-hidden")
                            .WithAppendedText("Emergency"));
                    buttonHtmlContentBuilder.Append(" Exit this page");

                    buttonHtml = buttonHtmlContentBuilder;
                }

                return GenerateButton(new ButtonOptions()
                {
                    Html = buttonHtml,
                    Text = options.Text.NormalizeEmptyString(),
                    Classes = new HtmlString(
                        "govuk-button--warning govuk-exit-this-page__button govuk-js-exit-this-page-button"),
                    Href = options.RedirectUrl.NormalizeEmptyString() ??
                           new HtmlString("https://www.bbc.co.uk/weather"),
                    Attributes = new EncodedAttributesDictionaryBuilder()
                        .With("rel", "nofollow noreferrer", encodeValue: false)
                });
            });
    }
}

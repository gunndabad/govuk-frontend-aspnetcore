using System;
using System.Linq;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string CookieBannerElement = "div";

    /// <inheritdoc/>
    public HtmlTag GenerateCookieBanner(CookieBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(CookieBannerElement)
            .AddClass("govuk-cookie-banner")
            .AddClasses(ExplodeClasses(options.Classes))
            .BooleanAttr("data-nosnippet")
            .UnencodedAttr("role", "region")
            .UnencodedAttr("aria-label", options.AriaLabel ?? "Cookie banner")
            .AddBooleanAttributeIf(options.Hidden == true, "hidden")
            .MergeEncodedAttributes(options.Attributes)
            .Append(
                (options.Messages ?? Array.Empty<CookieBannerOptionsMessage>()).Select(message =>
                    new HtmlTag("div")
                        .AddClasses("govuk-cookie-banner__message", "govuk-width-container")
                        .AddClasses(ExplodeClasses(message.Classes))
                        .AddEncodedAttributeIfNotNull("role", message.Role)
                        .MergeEncodedAttributes(message.Attributes)
                        .AddBooleanAttributeIf(message.Hidden == true, "hidden")
                        .Append(
                            new HtmlTag("div")
                                .AddClass("govuk-grid-row")
                                .Append(
                                    new HtmlTag("div")
                                        .AddClass("govuk-grid-column-two-thirds")
                                        .AppendIf(
                                            (
                                                message.HeadingText.NormalizeEmptyString()
                                                ?? message.HeadingHtml.NormalizeEmptyString()
                                            )
                                                is not null,
                                            () =>
                                                new HtmlTag("h2")
                                                    .AddClasses("govuk-cookie-banner__heading", "govuk-heading-m")
                                                    .AppendHtml(
                                                        GetEncodedTextOrHtml(message.HeadingText, message.HeadingHtml)
                                                    )
                                        )
                                        .Append(
                                            new HtmlTag("div")
                                                .AddClass("govuk-cookie-banner__content")
                                                .AppendHtmlIf(
                                                    (
                                                        message.Html.NormalizeEmptyString()
                                                        ?? message.Text.NormalizeEmptyString()
                                                    )
                                                        is not null,
                                                    message.Html.NormalizeEmptyString()
                                                        ?? new HtmlTag("p")
                                                            .AddClass("govuk-body")
                                                            .AppendText(message.Text)
                                                            .ToHtmlString()
                                                )
                                        )
                                )
                        )
                        .AppendIf(
                            message.Actions is not null,
                            () =>
                                new HtmlTag("div")
                                    .AddClass("govuk-button-group")
                                    .Append(
                                        message.Actions!.Select(action =>
                                            string.IsNullOrEmpty(action.Href) || action.Type == "button"
                                                ? GenerateButton(
                                                    new ButtonOptions()
                                                    {
                                                        Text = action.Text,
                                                        Type = action.Type ?? "button",
                                                        Name = action.Name,
                                                        Value = action.Value,
                                                        Classes = action.Classes,
                                                        Href = action.Href,
                                                        Attributes = action.Attributes,
                                                    }
                                                )
                                                : new HtmlTag("a")
                                                    .AddClass("govuk-link")
                                                    .AddClasses(ExplodeClasses(action.Classes))
                                                    .UnencodedAttr("href", action.Href)
                                                    .MergeEncodedAttributes(action.Attributes)
                                                    .Text(action.Text)
                                        )
                                    )
                        )
                )
            );
    }
}

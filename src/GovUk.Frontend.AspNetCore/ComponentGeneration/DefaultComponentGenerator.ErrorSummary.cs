using System;
using System.Linq;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string ErrorSummaryDefaultTitle = "There is a problem";
    internal const string ErrorSummaryDescriptionElement = "p";
    internal const string ErrorSummaryElement = "div";
    internal const string ErrorSummaryItemElement = "li";
    internal const string ErrorSummaryTitleElement = "h2";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateErrorSummary(ErrorSummaryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(ErrorSummaryElement)
            .AddClass("govuk-error-summary")
            .AddClasses(ExplodeClasses(options.Classes))
            .AddEncodedAttributeIf(
                options.DisableAutoFocus.HasValue,
                "data-disable-auto-focus",
                options.DisableAutoFocus == true ? "true" : "false"
            )
            .MergeEncodedAttributes(options.Attributes)
            .UnencodedAttr("data-module", "govuk-error-summary")
            .Append(
                new HtmlTag("div")
                    .UnencodedAttr("role", "alert")
                    .Append(
                        new HtmlTag(ErrorSummaryTitleElement)
                            .AddClass("govuk-error-summary__title")
                            .MergeEncodedAttributes(options.TitleAttributes)
                            .AppendHtml(
                                GetEncodedTextOrHtml(options.TitleText, options.TitleHtml) ?? ErrorSummaryDefaultTitle
                            )
                    )
                    .Append(
                        new HtmlTag("div")
                            .AddClass("govuk-error-summary__body")
                            .AppendIf(
                                options.DescriptionHtml.NormalizeEmptyString() is not null
                                    || options.DescriptionText.NormalizeEmptyString() is not null,
                                () =>
                                    new HtmlTag(ErrorSummaryDescriptionElement)
                                        .MergeEncodedAttributes(options.DescriptionAttributes)
                                        .AppendHtml(
                                            GetEncodedTextOrHtml(options.DescriptionText, options.DescriptionHtml)
                                        )
                            )
                            .Append(
                                new HtmlTag("ul")
                                    .AddClasses("govuk-list", "govuk-error-summary__list")
                                    .Append(
                                        (options.ErrorList ?? Enumerable.Empty<ErrorSummaryOptionsErrorItem>()).Select(
                                            item =>
                                                new HtmlTag(ErrorSummaryItemElement)
                                                    .MergeEncodedAttributes(item.ItemAttributes)
                                                    .Append(
                                                        (
                                                            item.Href.NormalizeEmptyString() is not null
                                                                ? new HtmlTag("a").MergeEncodedAttributes(
                                                                    item.Attributes
                                                                )
                                                                : new HtmlTag("").NoTag()
                                                        )
                                                            .AddEncodedAttributeIfNotNull(
                                                                "href",
                                                                item.Href.NormalizeEmptyString()
                                                            )
                                                            .AppendHtml(GetEncodedTextOrHtml(item.Text, item.Html))
                                                    )
                                        )
                                    )
                            )
                    )
            );
    }
}

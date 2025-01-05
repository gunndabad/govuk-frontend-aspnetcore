using System;
using System.Linq;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string BreadcrumbsElement = "nav";
    internal const bool BreadcrumbsDefaultCollapseOnMobile = false;
    internal const string BreadcrumbsItemElement = "li";
    internal const string BreadcrumbsDefaultLabelText = "Breadcrumb";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateBreadcrumbs(BreadcrumbsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(BreadcrumbsElement)
            .WithCssClass("govuk-breadcrumbs")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .When(options.CollapseOnMobile == true, b => b.WithCssClass("govuk-breadcrumbs--collapse-on-mobile"))
            .WithAttributes(options.Attributes)
            .WithAttribute("aria-label", options.LabelText ?? new HtmlString(BreadcrumbsDefaultLabelText))
            .WithAppendedHtml(new HtmlTagBuilder("ol")
                .WithCssClass("govuk-breadcrumbs__list")
                .WithAppendedHtml(options.Items!.Select(item =>
                {
                    var content = GetEncodedTextOrHtml(item.Text, item.Html)!;
                    var gotLink = item.Href.NormalizeEmptyString() is not null;

                    return new HtmlTagBuilder(BreadcrumbsItemElement)
                        .WithCssClass("govuk-breadcrumbs__list-item")
                        .WithAttributes(item.ItemAttributes)
                        .When(
                            !gotLink,
                            b => b
                                .WithAttribute("aria-current", "page", encodeValue: false)
                                .WithAppendedHtml(content))
                        .When(
                            gotLink,
                            b => b
                                .WithAppendedHtml(new HtmlTagBuilder("a")
                                    .WithCssClass("govuk-breadcrumbs__link")
                                    .WithAttribute("href", item.Href!)
                                    .WithAttributes(item.Attributes)
                                    .WithAppendedHtml(content)));
                })));
    }
}

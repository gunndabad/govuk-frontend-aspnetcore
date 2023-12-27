using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string PhaseBannerElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTag GeneratePhaseBanner(PhaseBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(PhaseBannerElement)
            .AddClass("govuk-phase-banner")
            .AddClasses(ExplodeClasses(options.Classes))
            .MergeEncodedAttributes(options.Attributes)
            .Append(new HtmlTag("p")
                .AddClass("govuk-phase-banner__content")
                .Append(GenerateTag(options.Tag!).AddClass("govuk-phase-banner__content__tag"))
                .Append(new HtmlTag("span")
                    .AddClass("govuk-phase-banner__text")
                    .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html))));
    }
}

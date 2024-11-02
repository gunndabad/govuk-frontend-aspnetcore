using System;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string WarningTextElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateWarningText(WarningTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTag(WarningTextElement)
            .AddClass("govuk-warning-text")
            .AddClasses(ExplodeClasses(options.Classes))
            .MergeEncodedAttributes(options.Attributes)
            .Append(
                new HtmlTag("span")
                    .AddClass("govuk-warning-text__icon")
                    .UnencodedAttr("aria-hidden", "true")
                    .AppendText("!")
            )
            .Append(
                new HtmlTag("strong")
                    .AddClass("govuk-warning-text__text")
                    .Append(
                        new HtmlTag("span")
                            .AddClass("govuk-visually-hidden")
                            .AppendText(options.IconFallbackText.NormalizeEmptyString() ?? "Warning")
                    )
                    .AppendHtml(GetEncodedTextOrHtml(options.Text, options.Html) ?? string.Empty)
            );
    }
}

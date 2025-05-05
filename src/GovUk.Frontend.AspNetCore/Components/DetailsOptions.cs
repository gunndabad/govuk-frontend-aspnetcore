using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record DetailsOptions
{
    public IHtmlContent? Id { get; set; }
    public bool? Open { get; set; }
    public IHtmlContent? SummaryHtml { get; set; }
    public string? SummaryText { get; set; }
    public IHtmlContent? Html { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    [NonStandardParameter]
    internal EncodedAttributesDictionary? SummaryAttributes { get; set; }
    [NonStandardParameter]
    internal EncodedAttributesDictionary? TextAttributes { get; set; }

    internal void Validate()
    {
        if (SummaryHtml.NormalizeEmptyString() is null && SummaryText.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(SummaryHtml)} or {nameof(SummaryText)} must be specified.");
        }

        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}

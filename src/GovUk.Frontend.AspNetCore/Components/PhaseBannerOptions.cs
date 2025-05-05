using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record PhaseBannerOptions
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public TagOptions? Tag { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }

        if (Tag is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Tag)} must be specified.");
        }
    }
}

using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FieldsetOptions
{
    public IHtmlContent? DescribedBy { get; set; }
    public FieldsetOptionsLegend? Legend { get; set; }
    public IHtmlContent? Role { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        if (Legend is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Legend)} must be specified.");
        }

        Legend?.Validate();
    }
}

public record FieldsetOptionsLegend
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public bool? IsPageHeading { get; set; }
    public IHtmlContent? Classes { get; set; }

    [NonStandardParameter]
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}

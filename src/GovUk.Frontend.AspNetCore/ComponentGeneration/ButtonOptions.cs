using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ButtonOptions
{
    public string? Element { get; set; }
    public IHtmlContent? Html { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Name { get; set; }
    public IHtmlContent? Type { get; set; }
    public IHtmlContent? Value { get; set; }
    public bool? Disabled { get; set; }
    public IHtmlContent? Href { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public bool? PreventDoubleClick { get; set; }
    public bool? IsStartButton { get; set; }
    public IHtmlContent? Id { get; set; }

    internal void Validate()
    {
        if (Element is not null && Element != "a" && Element != "button" && Element != "input")
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Element)} must be 'a', 'button', or 'input'.");
        }

        if (Element == "input" && IsStartButton == true)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(IsStartButton)} cannot be specified for 'input' elements.");
        }

        if (GetElement() is not "button" and not "input" && PreventDoubleClick == true)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(PreventDoubleClick)} can only be specified for 'button' or 'input' elements.");
        }

        if (Element == "a" && Disabled is not null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Disabled)} cannot be specified for 'a' elements.");
        }

        if (Html.NormalizeEmptyString() is not null && Element == "input")
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} cannot be specified for 'input' elements.");
        }

        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }

    internal string GetElement() =>
        Element?.NormalizeEmptyString() ?? (Href.NormalizeEmptyString() is not null ? "a" : "button");
}

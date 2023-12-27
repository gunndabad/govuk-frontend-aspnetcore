using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class ButtonOptions
{
    public string? Element { get; set; }
    public string? Html { get; set; }
    public string? Text { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Value { get; set; }
    public bool? Disabled { get; set; }
    public string? Href { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
    public bool? PreventDoubleClick { get; set; }
    public bool? IsStartButton { get; set; }
    public string? Id { get; set; }

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

        if (Element != "button" && PreventDoubleClick == true)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(PreventDoubleClick)} can only be specified for 'button' elements.");
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
}

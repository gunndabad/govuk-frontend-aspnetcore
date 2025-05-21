using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FormGroupOptions
{
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public FormGroupOptionsBeforeInput? BeforeInput { get; set; }
    public FormGroupOptionsAfterInput? AfterInput { get; set; }

    internal virtual void Validate()
    {
        BeforeInput?.Validate();
        AfterInput?.Validate();
    }
}

public record FormGroupOptionsBeforeInput
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }

    internal void Validate()
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}

public record FormGroupOptionsAfterInput
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }

    internal void Validate()
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}

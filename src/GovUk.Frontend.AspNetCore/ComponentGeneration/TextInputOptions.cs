using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TextInputOptions
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? Name { get; set; }
    public IHtmlContent? Type { get; set; }
    public IHtmlContent? Inputmode { get; set; }
    public IHtmlContent? Value { get; set; }
    public bool? Disabled { get; set; }
    public IHtmlContent? DescribedBy { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public TextInputOptionsPrefix? Prefix { get; set; }
    public TextInputOptionsSuffix? Suffix { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public IHtmlContent? Classes { get; set; }
    public IHtmlContent? Autocomplete { get; set; }
    public IHtmlContent? Pattern { get; set; }
    public bool? Spellcheck { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        if (Id is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Id)} must be specified.");
        }

        if (Name is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Name)} must be specified.");
        }

        FormGroup?.Validate();

        if (Label is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Label)} must be specified.");
        }

        Label.Validate();

        Hint?.Validate();

        ErrorMessage?.Validate();

        Prefix?.Validate();

        Suffix?.Validate();
    }
}

public record TextInputOptionsPrefix
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}

public record TextInputOptionsSuffix
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}

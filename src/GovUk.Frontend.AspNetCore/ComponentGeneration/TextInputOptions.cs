using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TextInputOptions
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Inputmode { get; set; }
    public string? Value { get; set; }
    public bool? Disabled { get; set; }
    public string? DescribedBy { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public TextInputOptionsPrefix? Prefix { get; set; }
    public TextInputOptionsSuffix? Suffix { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public string? Classes { get; set; }
    public string? Autocomplete { get; set; }
    public string? Pattern { get; set; }
    public bool? Spellcheck { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }

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

public class TextInputOptionsPrefix
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }

    internal void Validate()
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}

public class TextInputOptionsSuffix
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }

    internal void Validate()
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}

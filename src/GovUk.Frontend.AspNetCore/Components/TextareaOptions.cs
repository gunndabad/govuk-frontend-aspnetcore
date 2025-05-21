using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TextareaOptions
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? Name { get; set; }
    public bool? Spellcheck { get; set; }
    public int? Rows { get; set; }
    public IHtmlContent? Value { get; set; }
    public bool? Disabled { get; set; }
    public IHtmlContent? DescribedBy { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public IHtmlContent? Classes { get; set; }
    public IHtmlContent? Autocomplete { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        if (Label is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Label)} must be specified.");
        }

        if (Name is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Name)} must be specified.");
        }
    }
}

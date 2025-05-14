using System.Text.Json.Serialization;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record InputOptions
{
    public TemplateString? Id { get; set; }
    public TemplateString? Name { get; set; }
    public TemplateString? Type { get; set; }
    [JsonPropertyName("inputmode")]
    public TemplateString? InputMode { get; set; }
    public TemplateString? Value { get; set; }
    public bool? Disabled { get; set; }
    public TemplateString? DescribedBy { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public InputOptionsPrefix? Prefix { get; set; }
    public InputOptionsSuffix? Suffix { get; set; }
    public InputFormGroupOptions? FormGroup { get; set; }
    public TemplateString? Classes { get; set; }
    [JsonPropertyName("autocomplete")]
    public TemplateString? AutoComplete { get; set; }
    public TemplateString? Pattern { get; set; }
    public bool? Spellcheck { get; set; }
    [JsonPropertyName("autocapitalize")]
    public TemplateString? AutoCapitalize { get; set; }
    public InputOptionsInputWrapper? InputWrapper { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record InputOptionsPrefix
{
    public string? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record InputOptionsSuffix
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record InputFormGroupOptions : FormGroupOptions
{
    public TextHtmlAndAttributesOptions? BeforeInput { get; set; }
    public TextHtmlAndAttributesOptions? AfterInput { get; set; }
}

public record InputOptionsInputWrapper
{
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

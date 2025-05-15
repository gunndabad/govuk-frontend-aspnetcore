using System.Text.Json.Serialization;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record DateInputOptions
{
    public TemplateString? Id { get; set; }
    public TemplateString? NamePrefix { get; set; }
    public IReadOnlyCollection<DateInputOptionsItem>? Items { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public DateInputFormGroupOptions? FormGroup { get; set; }
    public FieldsetOptions? Fieldset { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record DateInputOptionsItem
{
    public TemplateString? Id { get; set; }
    public TemplateString? Name { get; set; }
    public TemplateString? Label { get; set; }
    public TemplateString? Value { get; set; }
    [JsonPropertyName("autocomplete")]
    public TemplateString? AutoComplete { get; set; }
    [JsonPropertyName("inputmode")]
    public TemplateString? InputMode { get; set; }
    public TemplateString? Pattern { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record DateInputFormGroupOptions : FormGroupOptions
{
    public TextHtmlAndAttributesOptions? BeforeInputs { get; set; }
    public TextHtmlAndAttributesOptions? AfterInputs { get; set; }
}

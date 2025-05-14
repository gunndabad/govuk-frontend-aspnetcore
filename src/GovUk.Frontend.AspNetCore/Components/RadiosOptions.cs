using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record RadiosOptions
{
    public FieldsetOptions? Fieldset { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public TemplateString? IdPrefix { get; set; }
    public TemplateString? Name { get; set; }
    public IReadOnlyCollection<RadiosOptionsItem>? Items { get; set; }
    public TemplateString? Value { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record RadiosOptionsItem
{
    public string? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Id { get; set; }
    public TemplateString? Name { get; set; }
    public TemplateString? Value { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public TemplateString? Divider { get; set; }
    public bool? Checked { get; set; }
    public RadiosOptionsItemConditional? Conditional { get; set; }
    public string? Behaviour { get; set; }
    public bool? Disabled { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record RadiosOptionsItemConditional
{
    public TemplateString? Html { get; set; }
}

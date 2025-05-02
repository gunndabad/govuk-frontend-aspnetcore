using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record CheckboxesOptions2
{
    public string? DescribedBy { get; set; }
    public FieldsetOptions2? Fieldset { get; set; }
    public HintOptions2? Hint { get; set; }
    public ErrorMessageOptions2? ErrorMessage { get; set; }
    public FormGroupOptions2? FormGroup { get; set; }
    public string? IdPrefix { get; set; }
    public string? Name { get; set; }
    public IReadOnlyCollection<CheckboxesOptions2Item>? Items { get; set; }
    public IReadOnlyCollection<string>? Values { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record CheckboxesOptions2Item
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public LabelOptions2? Label { get; set; }
    public HintOptions2? Hint { get; set; }
    public string? Divider { get; set; }
    public bool? Checked { get; set; }
    public CheckboxesOptions2ItemConditional? Conditional { get; set; }
    public string? Behaviour { get; set; }
    public bool? Disabled { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record CheckboxesOptions2ItemConditional
{
    public string? Html { get; set; }
}

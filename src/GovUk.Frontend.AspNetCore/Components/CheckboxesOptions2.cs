namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record CheckboxesOptions2
{
    public TemplateString? DescribedBy { get; set; }
    public FieldsetOptions2? Fieldset { get; set; }
    public HintOptions2? Hint { get; set; }
    public ErrorMessageOptions2? ErrorMessage { get; set; }
    public CheckboxesOptionsFormGroup? FormGroup { get; set; }
    public TemplateString? IdPrefix { get; set; }
    public TemplateString? Name { get; set; }
    public IReadOnlyCollection<CheckboxesOptions2Item>? Items { get; set; }
    public IReadOnlyCollection<string>? Values { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record CheckboxesOptionsFormGroup : FormGroupOptions2
{
    public TextHtmlAndAttributesOptions? BeforeInputs { get; set; }
    public TextHtmlAndAttributesOptions? AfterInputs { get; set; }
}

public record CheckboxesOptions2Item
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Id { get; set; }
    public TemplateString? Name { get; set; }
    public TemplateString? Value { get; set; }
    public LabelOptions2? Label { get; set; }
    public HintOptions2? Hint { get; set; }
    public TemplateString? Divider { get; set; }
    public bool? Checked { get; set; }
    public CheckboxesOptions2ItemConditional? Conditional { get; set; }
    public TemplateString? Behaviour { get; set; }
    public bool? Disabled { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record CheckboxesOptions2ItemConditional
{
    public TemplateString? Html { get; set; }
}

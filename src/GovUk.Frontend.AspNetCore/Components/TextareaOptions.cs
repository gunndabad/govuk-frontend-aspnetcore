namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TextareaOptions
{
    public TemplateString? Id { get; set; }
    public TemplateString? Name { get; set; }
    public bool? Spellcheck { get; set; }
    public int? Rows { get; set; }
    public TemplateString? Value { get; set; }
    public bool? Disabled { get; set; }
    public TemplateString? DescribedBy { get; set; }
    public LabelOptions2? Label { get; set; }
    public HintOptions2? Hint { get; set; }
    public ErrorMessageOptions2? ErrorMessage { get; set; }
    public TextAreaOptionsFormGroup? FormGroup { get; set; }
    public TemplateString? Classes { get; set; }
    public TemplateString? Autocomplete { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record TextAreaOptionsFormGroup : FormGroupOptions2
{
    public TextHtmlAndAttributesOptions? BeforeInput { get; set; }
    public TextHtmlAndAttributesOptions? AfterInput { get; set; }
}

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FileUploadOptions2
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public bool? Disabled { get; set; }
    public string? DescribedBy { get; set; }
    public LabelOptions2? Label { get; set; }
    public HintOptions2? Hint { get; set; }
    public ErrorMessageOptions2? ErrorMessage { get; set; }
    public FormGroupOptions2? FormGroup { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

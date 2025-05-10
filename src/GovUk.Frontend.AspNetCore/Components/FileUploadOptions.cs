using System.Text.Json.Serialization;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FileUploadOptions
{
    public TemplateString? Id { get; set; }
    public TemplateString? Name { get; set; }
    public TemplateString? Value { get; set; }
    public bool? Disabled { get; set; }
    public bool? Multiple { get; set; }
    public TemplateString? DescribedBy { get; set; }
    public LabelOptions2? Label { get; set; }
    public HintOptions2? Hint { get; set; }
    public ErrorMessageOptions2? ErrorMessage { get; set; }
    public FileUploadOptionsFormGroup? FormGroup { get; set; }
    [JsonPropertyName("javascript")]
    public bool? JavaScript { get; set; }
    public TemplateString? ChooseFilesButtonText { get; set; }
    public TemplateString? DropInstructionText { get; set; }
    public FileUploadOptionsMultipleFilesChosenText? MultipleFilesChosenText { get; set; }
    public TemplateString? NoFileChosenText { get; set; }
    public TemplateString? EnteredDropZoneText { get; set; }
    public TemplateString? LeftDropZoneText { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record FileUploadOptionsFormGroup : FormGroupOptions2
{
    public TextHtmlAndAttributesOptions? BeforeInput { get; set; }
    public TextHtmlAndAttributesOptions? AfterInput { get; set; }
}

public record FileUploadOptionsMultipleFilesChosenText
{
    public TemplateString? Other { get; set; }
    public TemplateString? One { get; set; }
}

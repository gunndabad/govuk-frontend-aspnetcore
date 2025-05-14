namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ErrorMessageOptions
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Id { get; set; }
    public TemplateString? VisuallyHiddenText { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

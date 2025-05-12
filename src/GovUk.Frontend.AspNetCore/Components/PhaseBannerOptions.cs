namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record PhaseBannerOptions
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TagOptions? Tag { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record NotificationBannerOptions
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? TitleText { get; set; }
    public TemplateString? TitleHtml { get; set; }
    public int? TitleHeadingLevel { get; set; }
    public TemplateString? Type { get; set; }
    public TemplateString? Role { get; set; }
    public TemplateString? TitleId { get; set; }
    public bool? DisableAutoFocus { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

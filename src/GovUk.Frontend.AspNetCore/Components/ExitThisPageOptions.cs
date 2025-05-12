namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ExitThisPageOptions
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? RedirectUrl { get; set; }
    public TemplateString? Id { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public TemplateString? ActivatedText { get; set; }
    public TemplateString? TimedOutText { get; set; }
    public TemplateString? PressTwoMoreTimesText { get; set; }
    public TemplateString? PressOneMoreTimeText { get; set; }
}

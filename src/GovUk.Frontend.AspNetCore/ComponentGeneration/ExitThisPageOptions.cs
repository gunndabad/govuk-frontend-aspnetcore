using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ExitThisPageOptions
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? RedirectUrl { get; set; }
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public IHtmlContent? ActivatedText { get; set; }
    public IHtmlContent? TimedOutText { get; set; }
    public IHtmlContent? PressTwoMoreTimesText { get; set; }
    public IHtmlContent? PressOneMoreTimeText { get; set; }

    internal void Validate()
    {

    }
}

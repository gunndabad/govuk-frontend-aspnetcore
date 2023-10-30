using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class NotificationBannerOptions
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? TitleText { get; set; }
    public string? TitleHtml { get; set; }
    public int? TitleHeadingLevel { get; set; }
    public string? Type { get; set; }
    public string? Role { get; set; }
    public string? TitleId { get; set; }
    public bool? DisableAutoFocus { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record CookieBannerOptions
{
    public string? AriaLabel { get; set; }
    public bool? Hidden { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public IReadOnlyCollection<CookieBannerOptionsMessage>? Messages { get; set; }
}

public record CookieBannerOptionsMessage
{
    public string? HeadingText { get; set; }
    public string? HeadingHtml { get; set; }
    public string? Text { get; set; }
    public string? Html { get; set; }
    public IReadOnlyCollection<CookieBannerOptionsMessageAction>? Actions { get; set; }
    public bool? Hidden { get; set; }
    public string? Role { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record CookieBannerOptionsMessageAction
{
    public string? Text { get; set; }
    public string? Type { get; set; }
    public string? Href { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

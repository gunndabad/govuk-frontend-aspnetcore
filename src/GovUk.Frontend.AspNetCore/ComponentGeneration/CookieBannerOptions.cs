using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record CookieBannerOptions
{
    public IHtmlContent? AriaLabel { get; set; }
    public bool? Hidden { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public IReadOnlyCollection<CookieBannerOptionsMessage>? Messages { get; set; }

    internal void Validate()
    {
        // TODO Each action should have non-null Text
    }
}

public record CookieBannerOptionsMessage
{
    public string? HeadingText { get; set; }
    public IHtmlContent? HeadingHtml { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IReadOnlyCollection<CookieBannerOptionsMessageAction>? Actions { get; set; }
    public bool? Hidden { get; set; }
    public IHtmlContent? Role { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public record CookieBannerOptionsMessageAction
{
    public string? Text { get; set; }
    public IHtmlContent? Type { get; set; }
    public IHtmlContent? Href { get; set; }
    public IHtmlContent? Name { get; set; }
    public IHtmlContent? Value { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

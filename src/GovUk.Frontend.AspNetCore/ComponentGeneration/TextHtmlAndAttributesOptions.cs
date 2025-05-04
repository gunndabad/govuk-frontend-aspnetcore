#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public record TextHtmlAndAttributesOptions
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    [NonStandardParameter]
    public AttributeCollection? Attributes { get; set; }
}

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TagOptions
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

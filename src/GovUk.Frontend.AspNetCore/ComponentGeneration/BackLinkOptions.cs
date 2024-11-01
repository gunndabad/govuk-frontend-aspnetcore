namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class BackLinkOptions
{
    public string? Html { get; set; }
    public string? Text { get; set; }
    public string? Href { get; set; }
    public string? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
    }
}

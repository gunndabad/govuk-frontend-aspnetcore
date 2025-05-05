namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FieldsetOptions2
{
    public string? DescribedBy { get; set; }
    public FieldsetOptions2Legend? Legend { get; set; }
    public string? Role { get; set; }
    public string? Html { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record FieldsetOptions2Legend
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public bool? IsPageHeading { get; set; }
    public string? Classes { get; set; }

    [NonStandardParameter]
    public EncodedAttributesDictionary? Attributes { get; set; }
}

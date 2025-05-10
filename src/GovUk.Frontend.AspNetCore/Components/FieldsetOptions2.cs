namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FieldsetOptions2
{
    public TemplateString? DescribedBy { get; set; }
    public FieldsetOptions2Legend? Legend { get; set; }
    public TemplateString? Role { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record FieldsetOptions2Legend
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public bool? IsPageHeading { get; set; }
    public TemplateString? Classes { get; set; }

    [NonStandardParameter]
    public EncodedAttributesDictionary? Attributes { get; set; }
}

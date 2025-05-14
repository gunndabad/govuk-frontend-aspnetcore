namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FieldsetOptions
{
    public TemplateString? DescribedBy { get; set; }
    public FieldsetOptionsLegend? Legend { get; set; }
    public TemplateString? Role { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record FieldsetOptionsLegend
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public bool? IsPageHeading { get; set; }
    public TemplateString? Classes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? Attributes { get; set; }
}

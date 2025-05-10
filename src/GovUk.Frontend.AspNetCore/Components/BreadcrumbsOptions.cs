namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record BreadcrumbsOptions
{
    public bool? CollapseOnMobile { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public IReadOnlyCollection<BreadcrumbsOptionsItem>? Items { get; set; }
    public TemplateString? LabelText { get; set; }
}

public record BreadcrumbsOptionsItem
{
    public TemplateString? Html { get; set; }
    public string? Text { get; set; }
    public TemplateString? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? ItemAttributes { get; set; }
}

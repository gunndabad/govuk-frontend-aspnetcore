namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record BreadcrumbsOptions
{
    public bool? CollapseOnMobile { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public IReadOnlyCollection<BreadcrumbsOptionsItem>? Items { get; set; }
    public string? LabelText { get; set; }
}

public record BreadcrumbsOptionsItem
{
    public string? Html { get; set; }
    public string? Text { get; set; }
    public string? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? ItemAttributes { get; set; }
}

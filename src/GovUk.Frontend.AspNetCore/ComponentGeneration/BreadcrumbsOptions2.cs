using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record BreadcrumbsOptions2
{
    public bool? CollapseOnMobile { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public IReadOnlyCollection<BreadcrumbsOptions2Item>? Items { get; set; }
    public string? LabelText { get; set; }
}

public record BreadcrumbsOptions2Item
{
    public string? Html { get; set; }
    public string? Text { get; set; }
    public string? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    internal AttributeCollection? ItemAttributes { get; set; }
}

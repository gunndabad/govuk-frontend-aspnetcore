using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class BreadcrumbsOptions
{
    public bool? CollapseOnMobile { get; set; }
    public string? Classes { get; set; }
    public Dictionary<string, string?>? Attributes { get; set; }
    public BreadcrumbsOptionsItem[]? Items { get; set; }
}

public class BreadcrumbsOptionsItem
{
    public string? Html { get; set; }
    public string? Text { get; set; }
    public string? Href { get; set; }
    public Dictionary<string, string?>? Attributes { get; set; }
}

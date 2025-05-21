namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public record Breadcrumbs
{
    public bool? CollapseOnMobile { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
    public IList<BreadcrumbsItem> Items { get; set; }
}

public record BreadcrumbsItem
{
    public string Html { get; set; }
    public string Text { get; set; }
    public string Href { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public record Tabs
{
    public string Id { get; set; }
    public string IdPrefix { get; set; }
    public string Title { get; set; }
    public IList<TabsItem> Items { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

public record TabsItem
{
    public string Id { get; set; }
    public string Label { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
    public TabsItemPanel Panel { get; set; }
}

public record TabsItemPanel
{
    public string Text { get; set; }
    public string Html { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

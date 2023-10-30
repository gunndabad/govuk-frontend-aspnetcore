using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TabsOptions
{
    public string? Id { get; set; }
    public string? IdPrefix { get; set; }
    public string? Title { get; set; }
    public IReadOnlyCollection<TabsOptionsItem>? Items { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class TabsOptionsItem
{
    public string? Id { get; set; }
    public string? Label { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
    public TabsOptionsItemPanel? Panel { get; set; }
}

public class TabsOptionsItemPanel
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

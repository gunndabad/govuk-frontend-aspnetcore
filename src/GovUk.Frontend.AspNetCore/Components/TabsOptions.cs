namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TabsOptions
{
    public TemplateString? Id { get; set; }
    public TemplateString? IdPrefix { get; set; }
    public TemplateString? Title { get; set; }
    public IReadOnlyCollection<TabsOptionsItem>? Items { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record TabsOptionsItem
{
    public TemplateString? Id { get; set; }
    public TemplateString? Label { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public TabsOptionsItemPanel? Panel { get; set; }
}

public record TabsOptionsItemPanel
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

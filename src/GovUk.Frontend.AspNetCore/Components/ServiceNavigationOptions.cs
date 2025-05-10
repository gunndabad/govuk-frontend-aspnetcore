namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ServiceNavigationOptions
{
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public TemplateString? AriaLabel { get; set; }
    public TemplateString? MenuButtonText { get; set; }
    public TemplateString? MenuButtonLabel { get; set; }
    public TemplateString? NavigationLabel { get; set; }
    public TemplateString? NavigationId { get; set; }
    public TemplateString? NavigationClasses { get; set; }
    public TemplateString? ServiceName { get; set; }
    public TemplateString? ServiceUrl { get; set; }
    public IReadOnlyCollection<ServiceNavigationOptionsNavigationItem>? Navigation { get; set; }
    public ServiceNavigationOptionsSlots? Slots { get; set; }
}

public record ServiceNavigationOptionsNavigationItem
{
    public bool? Current { get; set; }
    public bool? Active { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Text { get; set; }
    public TemplateString? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record ServiceNavigationOptionsSlots
{
    public TemplateString? Start { get; set; }
    public TemplateString? End { get; set; }
    public TemplateString? NavigationStart { get; set; }
    public TemplateString? NavigationEnd { get; set; }
}

using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ServiceNavigationOptions
{
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public string? AriaLabel { get; set; }
    public string? MenuButtonText { get; set; }
    public string? MenuButtonLabel { get; set; }
    public string? NavigationLabel { get; set; }
    public string? NavigationId { get; set; }
    public string? NavigationClasses { get; set; }
    public string? ServiceName { get; set; }
    public string? ServiceUrl { get; set; }
    public IReadOnlyCollection<ServiceNavigationOptionsNavigationItem>? Navigation { get; set; }
    public ServiceNavigationOptionsSlots? Slots { get; set; }
}

public record ServiceNavigationOptionsNavigationItem
{
    public bool? Current { get; set; }
    public bool? Active { get; set; }
    public string? Html { get; set; }
    public string? Text { get; set; }
    public string? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record ServiceNavigationOptionsSlots
{
    public string? Start { get; set; }
    public string? End { get; set; }
    public string? NavigationStart { get; set; }
    public string? NavigationEnd { get; set; }
}

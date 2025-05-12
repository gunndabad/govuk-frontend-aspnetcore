using System.Text.Json.Serialization;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record HeaderOptions
{
    [JsonPropertyName("homepageUrl")]
    public TemplateString? HomePageUrl { get; set; }
    public TemplateString? ProductName { get; set; }
    public TemplateString? ServiceName { get; set; }
    public TemplateString? ServiceUrl { get; set; }
    public IReadOnlyCollection<HeaderOptionsNavigation>? Navigation { get; set; }
    public TemplateString? NavigationClasses { get; set; }
    public TemplateString? NavigationLabel { get; set; }
    public TemplateString? MenuButtonLabel { get; set; }
    public TemplateString? MenuButtonText { get; set; }
    public TemplateString? ContainerClasses { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public bool? UseTudorCrown { get; set; }
}

public record HeaderOptionsNavigation
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Href { get; set; }
    public bool? Active { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

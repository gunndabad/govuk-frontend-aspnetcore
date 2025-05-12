namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FooterOptions
{
    public FooterOptionsMeta? Meta { get; set; }
    public IReadOnlyCollection<FooterOptionsNavigation>? Navigation { get; set; }
    public FooterOptionsContentLicence? ContentLicence { get; set; }
    public FooterOptionsCopyright? Copyright { get; set; }
    public TemplateString? ContainerClasses { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record FooterOptionsMeta
{
    public TemplateString? VisuallyHiddenTitle { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Text { get; set; }
    public IReadOnlyCollection<FooterOptionsMetaItem>? Items { get; set; }
}

public record FooterOptionsMetaItem
{
    public TemplateString? Text { get; set; }
    public TemplateString? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record FooterOptionsNavigation
{
    public TemplateString? Title { get; set; }
    public int? Columns { get; set; }
    public TemplateString? Width { get; set; }
    public IReadOnlyCollection<FooterOptionsNavigationItem>? Items { get; set; }
}

public record FooterOptionsNavigationItem
{
    public TemplateString? Text { get; set; }
    public TemplateString? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record FooterOptionsContentLicence
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
}

public record FooterOptionsCopyright
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
}

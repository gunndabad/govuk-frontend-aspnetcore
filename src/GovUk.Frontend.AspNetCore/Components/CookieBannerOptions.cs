namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record CookieBannerOptions
{
    public TemplateString? AriaLabel { get; set; }
    public bool? Hidden { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public IReadOnlyCollection<CookieBannerOptionsMessage>? Messages { get; set; }
}

public record CookieBannerOptionsMessage
{
    public TemplateString? HeadingText { get; set; }
    public TemplateString? HeadingHtml { get; set; }
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public IReadOnlyCollection<CookieBannerOptionsMessageAction>? Actions { get; set; }
    public bool? Hidden { get; set; }
    public TemplateString? Role { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record CookieBannerOptionsMessageAction
{
    public TemplateString? Text { get; set; }
    public TemplateString? Type { get; set; }
    public TemplateString? Href { get; set; }
    public TemplateString? Name { get; set; }
    public TemplateString? Value { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

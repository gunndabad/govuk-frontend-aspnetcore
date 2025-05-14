namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record PanelOptions
{
    public TemplateString? TitleText { get; set; }
    public TemplateString? TitleHtml { get; set; }
    public int? HeadingLevel { get; set; }
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? TitleAttributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? BodyAttributes { get; set; }
}

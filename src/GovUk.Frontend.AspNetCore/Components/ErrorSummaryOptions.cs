namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ErrorSummaryOptions
{
    public TemplateString? TitleText { get; set; }
    public TemplateString? TitleHtml { get; set; }
    public TemplateString? DescriptionText { get; set; }
    public TemplateString? DescriptionHtml { get; set; }
    public IReadOnlyCollection<ErrorSummaryOptionsErrorItem>? ErrorList { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public bool? DisableAutoFocus { get; set; }

    [NonStandardParameter]
    public AttributeCollection? TitleAttributes { get; set; }
    [NonStandardParameter]
    public AttributeCollection? DescriptionAttributes { get; set; }
}

public record ErrorSummaryOptionsErrorItem
{
    public TemplateString? Href { get; set; }
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? ItemAttributes { get; set; }
}

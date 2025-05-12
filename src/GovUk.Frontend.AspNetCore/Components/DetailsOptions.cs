namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record DetailsOptions
{
    public TemplateString? Id { get; set; }
    public bool? Open { get; set; }
    public TemplateString? SummaryHtml { get; set; }
    public TemplateString? SummaryText { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Text { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? SummaryAttributes { get; set; }
    [NonStandardParameter]
    public AttributeCollection? TextAttributes { get; set; }
}

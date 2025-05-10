namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record AccordionOptions2
{
    public TemplateString? Id { get; set; }
    public int? HeadingLevel { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public bool? RememberExpanded { get; set; }
    public TemplateString? HideAllSectionsText { get; set; }
    public TemplateString? HideSectionText { get; set; }
    public TemplateString? HideSectionAriaLabelText { get; set; }
    public TemplateString? ShowAllSectionsText { get; set; }
    public TemplateString? ShowSectionText { get; set; }
    public TemplateString? ShowSectionAriaLabelText { get; set; }
    public IReadOnlyCollection<AccordionOptions2Item>? Items { get; set; }
}

public record AccordionOptions2Item
{
    public AccordionOptions2ItemHeading? Heading { get; set; }
    public AccordionOptions2ItemSummary? Summary { get; set; }
    public AccordionOptions2ItemContent? Content { get; set; }
    public bool? Expanded { get; set; }
}

public record AccordionOptions2ItemHeading
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
}

public record AccordionOptions2ItemSummary
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
}

public record AccordionOptions2ItemContent
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
}

using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record AccordionOptions2
{
    public string? Id { get; set; }
    public int? HeadingLevel { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public bool? RememberExpanded { get; set; }
    public string? HideAllSectionsText { get; set; }
    public string? HideSectionText { get; set; }
    public string? HideSectionAriaLabelText { get; set; }
    public string? ShowAllSectionsText { get; set; }
    public string? ShowSectionText { get; set; }
    public string? ShowSectionAriaLabelText { get; set; }
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
    public string? Text { get; set; }
    public string? Html { get; set; }
}

public record AccordionOptions2ItemSummary
{
    public string? Text { get; set; }
    public string? Html { get; set; }
}

public record AccordionOptions2ItemContent
{
    public string? Text { get; set; }
    public string? Html { get; set; }
}

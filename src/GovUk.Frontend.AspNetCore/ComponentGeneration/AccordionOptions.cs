using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class AccordionOptions
{
    public string? Id { get; set; }
    public int? HeadingLevel { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
    public bool? RememberExpanded { get; set; }
    public string? HideAllSectionsText { get; set; }
    public string? HideSectionText { get; set; }
    public string? HideSectionAriaLabelText { get; set; }
    public string? ShowAllSectionsText { get; set; }
    public string? ShowSectionText { get; set; }
    public string? ShowSectionAriaLabelText { get; set; }
    public IReadOnlyCollection<AccordionOptionsItem>? Items { get; set; }
}

public class AccordionOptionsItem
{
    public AccordionOptionsItemHeading? Heading { get; set; }
    public AccordionOptionsItemSummary? Summary { get; set; }
    public AccordionOptionsItemContent? Content { get; set; }
    public bool? Expanded { get; set; }
}

public class AccordionOptionsItemHeading
{
    public string? Text { get; set; }
    public string? Html { get; set; }
}

public class AccordionOptionsItemSummary
{
    public string? Text { get; set; }
    public string? Html { get; set; }
}

public class AccordionOptionsItemContent
{
    public string? Text { get; set; }
    public string? Html { get; set; }
}

using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public record Accordion
{
    public string Id { get; set; }
    public int? HeadingLevel { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
    public bool? RememberExpanded { get; set; }
    public string HideAllSectionsText { get; set; }
    public string HideSectionText { get; set; }
    public string HideSectionAriaLabelText { get; set; }
    public string ShowAllSectionsText { get; set; }
    public string ShowSectionText { get; set; }
    public string ShowSectionAriaLabelText { get; set; }
    public IList<AccordionItem> Items { get; set; }
}

public record AccordionItem
{
    public AccordionItemHeading Heading { get; set; }
    public AccordionItemSummary Summary { get; set; }
    public AccordionItemContent Content { get; set; }
    public bool? Expanded { get; set; }
}

public record AccordionItemHeading
{
    public string Text { get; set; }
    public string Html { get; set; }
}

public record AccordionItemSummary
{
    public string Text { get; set; }
    public string Html { get; set; }
}

public record AccordionItemContent
{
    public string Text { get; set; }
    public string Html { get; set; }
}

using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public class Accordion
    {
        public string Id { get; set; }
        public int? HeadingLevel { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public IList<AccordionItem> Items { get; set; }
    }

    public class AccordionItem
    {
        public AccordionItemHeading Heading { get; set; }
        public AccordionItemSummary Summary { get; set; }
        public AccordionItemContent Content { get; set; }
        public bool? Expanded { get; set; }
    }

    public class AccordionItemHeading
    {
        public string Text { get; set; }
        public string Html { get; set; }
    }

    public class AccordionItemSummary
    {
        public string Text { get; set; }
        public string Html { get; set; }
    }

    public class AccordionItemContent
    {
        public string Text { get; set; }
        public string Html { get; set; }
    }
}

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal class AccordionItem
    {
        public bool Expanded { get; set; }
        public IHtmlContent? HeadingContent { get; set; }
        public AttributeDictionary? HeadingAttributes { get; set; }
        public IHtmlContent? SummaryContent { get; set; }
        public AttributeDictionary? SummaryAttributes { get; set; }
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? ContentAttributes { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }
}

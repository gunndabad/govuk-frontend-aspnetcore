using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class AccordionItem
    {
        public bool Expanded { get; set; }
        public int? HeadingLevel { get; set; }
        public IHtmlContent HeadingContent { get; set; }
        public IDictionary<string, string> HeadingAttributes { get; set; }
        public IHtmlContent SummaryContent { get; set; }
        public IDictionary<string, string> SummaryAttributes { get; set; }
        public IHtmlContent Content { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}

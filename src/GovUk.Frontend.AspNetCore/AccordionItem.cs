using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class AccordionItem
    {
        public bool Expanded { get; set; }
        public int? HeadingLevel { get; set; }
        public IHtmlContent HeadingContent { get; set; }
        public IHtmlContent Summary { get; set; }
        public IHtmlContent Content { get; set; }
    }
}

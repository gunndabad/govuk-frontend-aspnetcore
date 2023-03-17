using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class SummaryCard
    {
        public AttributeDictionary CardAttributes { get; set; }
        public AttributeDictionary TitleAttributes { get; set; }
        public IHtmlContent TitleContent { get; set; }
        public int HeadingLevel { get; set; }
        public IHtmlContent SummaryList { get; set; }
        public SummaryCardActions Actions { get; set; }
    }
}

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class SummaryCardAction
    {
        public string VisuallyHiddenText { get; set; }
        public IHtmlContent Content { get; set; }
        public AttributeDictionary Attributes { get; set; }
        public string Href { get; internal set; }
    }
}

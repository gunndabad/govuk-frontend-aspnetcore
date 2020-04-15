using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class SummaryListRow
    {
        public IHtmlContent Key { get; set; }
        public IHtmlContent Value { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public IEnumerable<SummaryListRowAction> Actions { get; set; }
    }

    public class SummaryListRowAction
    {
        public string Href { get; set; }
        public string VisuallyHiddenText { get; set; }
        public IHtmlContent Content { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class ErrorSummaryItem
    {
        public IHtmlContent Content { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}

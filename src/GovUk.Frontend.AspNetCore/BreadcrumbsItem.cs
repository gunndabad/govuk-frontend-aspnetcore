using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class BreadcrumbsItem
    {
        public string Href { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public IHtmlContent Content { get; set; }
        public bool IsCurrentPage { get; set; }
    }
}

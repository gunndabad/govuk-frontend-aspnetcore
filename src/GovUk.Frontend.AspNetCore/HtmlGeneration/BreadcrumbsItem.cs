#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class BreadcrumbsItem
    {
        public string? Href { get; set; }
        public IDictionary<string, string>? LinkAttributes { get; set; }
        public IHtmlContent? Content { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
    }
}

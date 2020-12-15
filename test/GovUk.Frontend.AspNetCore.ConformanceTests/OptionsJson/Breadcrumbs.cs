using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public class Breadcrumbs
    {
        public bool? CollapseOnMobile { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public IList<BreadcrumbsItem> Items { get; set; }
    }

    public class BreadcrumbsItem : ITextOrHtmlContent
    {
        public string Html { get; set; }
        public string Text { get; set; }
        public string Href { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}

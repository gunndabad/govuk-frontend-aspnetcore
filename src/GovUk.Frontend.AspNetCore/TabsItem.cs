using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class TabsItem
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public IDictionary<string, string> PanelAttributes { get; set; }
        public IHtmlContent PanelContent { get; set; }
    }
}

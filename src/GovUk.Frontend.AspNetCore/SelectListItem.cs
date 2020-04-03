using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class SelectListItem
    {
        public string Value { get; set; }
        public IHtmlContent Content { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}

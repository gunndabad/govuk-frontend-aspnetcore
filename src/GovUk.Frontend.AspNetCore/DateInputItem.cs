using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class DateInputItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Autocomplete { get; set; }
        public string Pattern { get; set; }
        public IHtmlContent Label { get; set; }
        public bool HaveError { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}

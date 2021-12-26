using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
        public AttributeDictionary Attributes { get; set; }
    }
}

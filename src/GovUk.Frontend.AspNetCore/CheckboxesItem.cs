using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class CheckboxesItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public IHtmlContent Content { get; set; }
        public string ConditionalId { get; set; }
        public IHtmlContent ConditionalContent { get; set; }
        public IDictionary<string, string> ConditionalAttributes { get; set; }
        public string HintId { get; set; }
        public IDictionary<string, string> HintAttributes { get; set; }
        public IHtmlContent HintContent { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public IDictionary<string, string> InputAttributes { get; set; }
    }
}

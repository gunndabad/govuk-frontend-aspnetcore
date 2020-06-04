using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public abstract class RadiosItemBase
    {
        private protected RadiosItemBase() { }

        public IDictionary<string, string> Attributes { get; set; }
    }

    public class RadiosItem : RadiosItemBase
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public IHtmlContent Content { get; set; }
        public string ConditionalId { get; set; }
        public IHtmlContent ConditionalContent { get; set; }
        public IDictionary<string, string> ConditionalAttributes { get; set; }
        public IDictionary<string, string> HintAttributes { get; set; }
        public string HintId { get; set; }
        public IHtmlContent HintContent { get; set; }
        public IDictionary<string, string> InputAttributes { get; set; }
    }

    public class RadiosItemDivider : RadiosItemBase
    {
        public IHtmlContent Content { get; set; }
    }
}

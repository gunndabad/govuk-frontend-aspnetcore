using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    public abstract class RadiosItemBase
    {
        private protected RadiosItemBase() { }

        public AttributeDictionary Attributes { get; set; }
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
        public AttributeDictionary ConditionalAttributes { get; set; }
        public AttributeDictionary HintAttributes { get; set; }
        public string HintId { get; set; }
        public IHtmlContent HintContent { get; set; }
        public AttributeDictionary InputAttributes { get; set; }
    }

    public class RadiosItemDivider : RadiosItemBase
    {
        public IHtmlContent Content { get; set; }
    }
}

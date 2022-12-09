using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal abstract class RadiosItemBase
    {
        private protected RadiosItemBase() { }

        public AttributeDictionary? Attributes { get; set; }
    }

    internal class RadiosItem : RadiosItemBase
    {
        public string? Id { get; set; }
        public string? Value { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }
        public IHtmlContent? LabelContent { get; set; }
        public AttributeDictionary? LabelAttributes { get; set; }
        public RadiosItemHint? Hint { get; set; }
        public RadiosItemConditional? Conditional { get; set; }
        public AttributeDictionary? InputAttributes { get; set; }
    }

    internal class RadiosItemConditional
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    internal class RadiosItemHint
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    internal class RadiosItemDivider : RadiosItemBase
    {
        public IHtmlContent? Content { get; set; }
    }
}

#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public abstract class RadiosItemBase
    {
        private protected RadiosItemBase() { }

        public AttributeDictionary? Attributes { get; set; }
    }

    public class RadiosItem : RadiosItemBase
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

    public class RadiosItemConditional
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class RadiosItemHint
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class RadiosItemDivider : RadiosItemBase
    {
        public IHtmlContent? Content { get; set; }
    }
}

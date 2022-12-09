using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class CheckboxesItemBase
    {
        private protected CheckboxesItemBase() { }

        public AttributeDictionary? Attributes { get; set; }
    }

    public class CheckboxesItem : CheckboxesItemBase
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public bool Checked { get; set; }
        public CheckboxesItemBehavior Behavior { get; set; }
        public bool Disabled { get; set; }
        public IHtmlContent? LabelContent { get; set; }
        public AttributeDictionary? LabelAttributes { get; set; }
        public CheckboxesItemHint? Hint { get; set; }
        public CheckboxesItemConditional? Conditional { get; set; }
        public AttributeDictionary? InputAttributes { get; set; }
    }

    public class CheckboxesItemConditional
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class CheckboxesItemHint
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class CheckboxesItemDivider : CheckboxesItemBase
    {
        public IHtmlContent? Content { get; set; }
    }
}

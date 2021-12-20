#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class CheckboxesItemBase
    {
        public IDictionary<string, string>? Attributes { get; set; }
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
        public IDictionary<string, string>? LabelAttributes { get; set; }
        public CheckboxesItemHint? Hint { get; set; }
        public CheckboxesItemConditional? Conditional { get; set; }
        public IDictionary<string, string>? InputAttributes { get; set; }
    }

    public class CheckboxesItemConditional
    {
        public IHtmlContent? Content { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
    }

    public class CheckboxesItemHint
    {
        public IHtmlContent? Content { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
    }

    public class CheckboxesItemDivider : CheckboxesItemBase
    {
        public IHtmlContent? Content { get; set; }
    }
}

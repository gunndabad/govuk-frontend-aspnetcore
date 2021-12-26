#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class SummaryListRow
    {
        public SummaryListRowKey? Key { get; set; }
        public SummaryListRowValue? Value { get; set; }
        public AttributeDictionary? Attributes { get; set; }
        public SummaryListRowActions? Actions { get; set; }
    }

    public class SummaryListRowActions
    {
        public IReadOnlyList<SummaryListRowAction>? Items { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class SummaryListRowAction
    {
        public string? VisuallyHiddenText { get; set; }
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class SummaryListRowKey
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class SummaryListRowValue
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }
}

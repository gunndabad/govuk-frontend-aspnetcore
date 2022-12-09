using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal class SummaryListRow
    {
        public SummaryListRowKey? Key { get; set; }
        public SummaryListRowValue? Value { get; set; }
        public AttributeDictionary? Attributes { get; set; }
        public SummaryListRowActions? Actions { get; set; }
    }

    internal class SummaryListRowActions
    {
        public IReadOnlyList<SummaryListRowAction>? Items { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    internal class SummaryListRowAction
    {
        public string? VisuallyHiddenText { get; set; }
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    internal class SummaryListRowKey
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    internal class SummaryListRowValue
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal class SummaryCard
    {
        public SummaryCardTitle? Title { get; set; }
        public SummaryListActions? Actions { get; set; }
    }

    internal class SummaryCardTitle
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
        public int? HeadingLevel { get; set; }
    }

    internal class SummaryListRow
    {
        public SummaryListRowKey? Key { get; set; }
        public SummaryListRowValue? Value { get; set; }
        public AttributeDictionary? Attributes { get; set; }
        public SummaryListActions? Actions { get; set; }
    }

    internal class SummaryListActions
    {
        public IReadOnlyList<SummaryListAction>? Items { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    internal class SummaryListAction
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

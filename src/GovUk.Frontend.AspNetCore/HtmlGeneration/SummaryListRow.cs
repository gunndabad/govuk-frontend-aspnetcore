#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class SummaryListRow
    {
        public SummaryListRowKey? Key { get; set; }
        public SummaryListRowValue? Value { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
        public SummaryListRowActions? Actions { get; set; }
    }

    public class SummaryListRowActions
    {
        public IReadOnlyList<SummaryListRowAction>? Items { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
    }

    public class SummaryListRowAction
    {
        public string? VisuallyHiddenText { get; set; }
        public IHtmlContent? Content { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
    }

    public class SummaryListRowKey
    {
        public IHtmlContent? Content { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
    }

    public class SummaryListRowValue
    {
        public IHtmlContent? Content { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
    }
}

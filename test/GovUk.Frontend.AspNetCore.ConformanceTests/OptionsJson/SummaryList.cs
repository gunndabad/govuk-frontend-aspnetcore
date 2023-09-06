using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public record SummaryList
    {
        public IList<SummaryListRow> Rows { get; set; }
        public SummaryListCard Card { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }

    public record SummaryListCard
    {
        public SummaryListCardTitle Title { get; set; }
        public SummaryListRowActions Actions { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }

    public record SummaryListCardTitle
    {
        public string Text { get; set; }
        public string Html { get; set; }
        public int? HeadingLevel { get; set; }
        public string Classes { get; set; }
    }

    public record SummaryListRow
    {
        public string Classes { get; set; }
        public SummaryListRowKey Key { get; set; }
        public SummaryListRowValue Value { get; set; }
        public SummaryListRowActions Actions { get; set; }
    }

    public record SummaryListRowKey
    {
        public string Text { get; set; }
        public string Html { get; set; }
        public string Classes { get; set; }
    }

    public record SummaryListRowValue
    {
        public string Text { get; set; }
        public string Html { get; set; }
        public string Classes { get; set; }
    }

    public record SummaryListRowActions
    {
        public string Classes { get; set; }
        public IList<SummaryListRowActionsItem> Items { get; set; }
    }

    public record SummaryListRowActionsItem
    {
        public string Href { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
        public object VisuallyHiddenText { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }
}

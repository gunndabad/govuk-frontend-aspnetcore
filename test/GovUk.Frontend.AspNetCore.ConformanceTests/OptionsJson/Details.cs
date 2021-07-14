using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public record Details
    {
        public string Id { get; set; }
        public bool? Open { get; set; }
        public string SummaryHtml { get; set; }
        public string SummaryText { get; set; }
        public string Html { get; set; }
        public string Text { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }
}

using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public record Fieldset
    {
        public string DescribedBy { get; set; }
        public FieldsetLegend Legend { get; set; }
        public string Role { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }

    public record FieldsetLegend
    {
        public string Text { get; set; }
        public string Html { get; set; }
        public bool? IsPageHeading { get; set; }
        public string Classes { get; set; }
    }
}

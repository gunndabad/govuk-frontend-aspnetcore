using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public record Label
    {
        public string Text { get; set; }
        public string Html { get; set; }
        public string For { get; set; }
        public bool? IsPageHeading { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }
}

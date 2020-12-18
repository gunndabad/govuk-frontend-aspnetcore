using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public class Button
    {
        public string Element { get; set; }
        public string Html { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool? Disabled { get; set; }
        public string Href { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public bool? PreventDoubleClick { get; set; }
        public bool? IsStartButton { get; set; }
    }
}

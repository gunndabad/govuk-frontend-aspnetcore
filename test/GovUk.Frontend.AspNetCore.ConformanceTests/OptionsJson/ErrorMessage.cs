using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public class ErrorMessage
    {
        public string Text { get; set; }
        public string Html { get; set; }
        public string Id { get; set; }
        public JToken VisuallyHiddenText { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }
}

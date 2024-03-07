using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public class NotificationBanner
{
    public string Text { get; set; }
    public string Html { get; set; }
    public string TitleText { get; set; }
    public string TitleHtml { get; set; }
    public int? TitleHeadingLevel { get; set; }
    public string Type { get; set; }
    public string Role { get; set; }
    public string TitleId { get; set; }
    public bool? DisableAutoFocus { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

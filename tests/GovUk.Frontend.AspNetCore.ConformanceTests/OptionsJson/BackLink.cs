namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public record BackLink
{
    public string Html { get; set; }
    public string Text { get; set; }
    public string Href { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

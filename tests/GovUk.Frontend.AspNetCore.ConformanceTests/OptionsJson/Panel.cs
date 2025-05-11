namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public record Panel
{
    public string TitleText { get; set; }
    public string TitleHtml { get; set; }
    public int? HeadingLevel { get; set; }
    public string Text { get; set; }
    public string Html { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

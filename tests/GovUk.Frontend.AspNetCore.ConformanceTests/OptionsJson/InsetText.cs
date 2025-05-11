namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public record InsetText
{
    public string Text { get; set; }
    public string Html { get; set; }
    public string Id { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

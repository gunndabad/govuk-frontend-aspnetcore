namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public record ErrorSummary
{
    public string TitleText { get; set; }
    public string TitleHtml { get; set; }
    public string DescriptionText { get; set; }
    public string DescriptionHtml { get; set; }
    public IEnumerable<ErrorSummaryErrorItem> ErrorList { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
    public bool? DisableAutoFocus { get; set; }
}

public record ErrorSummaryErrorItem
{
    public string Href { get; set; }
    public string Text { get; set; }
    public string Html { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

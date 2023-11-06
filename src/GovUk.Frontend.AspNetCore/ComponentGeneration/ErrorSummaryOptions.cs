using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class ErrorSummaryOptions
{
    public string? TitleText { get; set; }
    public string? TitleHtml { get; set; }
    public string? DescriptionText { get; set; }
    public string? DescriptionHtml { get; set; }
    public IReadOnlyCollection<ErrorSummaryOptionsErrorItem>? ErrorList { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
    public bool? DisableAutoFocus { get; set; }
}

public class ErrorSummaryOptionsErrorItem
{
    public string? Href { get; set; }
    public string? Text { get; set; }
    public string? Html { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

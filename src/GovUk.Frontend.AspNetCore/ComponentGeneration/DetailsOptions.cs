using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class DetailsOptions
{
    public string? Id { get; set; }
    public bool? Open { get; set; }
    public string? SummaryHtml { get; set; }
    public string? SummaryText { get; set; }
    public string? Html { get; set; }
    public string? Text { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

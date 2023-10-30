using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class FieldsetOptions
{
    public string? DescribedBy { get; set; }
    public FieldsetOptionsLegend? Legend { get; set; }
    public string? Role { get; set; }
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class FieldsetOptionsLegend
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public bool? IsPageHeading { get; set; }
    public string? Classes { get; set; }
}

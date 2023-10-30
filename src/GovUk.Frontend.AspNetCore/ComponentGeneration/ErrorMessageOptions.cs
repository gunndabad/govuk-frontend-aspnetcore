using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class ErrorMessageOptions
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Id { get; set; }
    public string? VisuallyHiddenText { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

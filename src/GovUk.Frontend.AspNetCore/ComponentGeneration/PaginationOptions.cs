using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class PaginationOptions
{
    public IReadOnlyCollection<PaginationOptionsItem>? Items { get; set; }
    public PaginationOptionsPrevious? Previous { get; set; }
    public PaginationOptionsNext? Next { get; set; }
    public string? LandmarkLabel { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class PaginationOptionsItem
{
    public string? Number { get; set; }
    public string? VisuallyHiddenText { get; set; }
    public string? Href { get; set; }
    public bool? Current { get; set; }
    public bool? Ellipsis { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class PaginationOptionsPrevious
{
    public string? Text { get; set; }
    public string? LabelText { get; set; }
    public string? Href { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class PaginationOptionsNext
{
    public string? Text { get; set; }
    public string? LabelText { get; set; }
    public string? Href { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

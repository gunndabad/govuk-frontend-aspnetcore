using System.Collections.Immutable;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class DateInputContextItem
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? LabelHtml { get; set; }
    public ImmutableDictionary<string, string?>? LabelAttributes { get; set; }
    public int? Value { get; set; }
    public bool ValueSpecified { get; set; }
    public string? Autocomplete { get; set; }
    public string? InputMode { get; set; }
    public string? Pattern { get; set; }
    public ImmutableDictionary<string, string?>? Attributes { get; set; }
}

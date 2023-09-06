using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class DateInputContextItem
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IHtmlContent? LabelContent { get; set; }
    public IDictionary<string, string?>? LabelAttributes { get; set; }
    public int? Value { get; set; }
    public bool ValueSpecified { get; set; }
    public string? Autocomplete { get; set; }
    public string? InputMode { get; set; }
    public string? Pattern { get; set; }
    public IDictionary<string, string?>? Attributes { get; set; }
}

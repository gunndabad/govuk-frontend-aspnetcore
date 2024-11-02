using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal class DateInputItem
{
    [DisallowNull]
    public string? Id { get; set; }

    [DisallowNull]
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? Autocomplete { get; set; }
    public string? InputMode { get; set; }
    public string? Pattern { get; set; }

    [DisallowNull]
    public IHtmlContent? LabelContent { get; set; }
    public AttributeDictionary? LabelAttributes { get; set; }
    public bool HaveError { get; set; }
    public AttributeDictionary? Attributes { get; set; }
}

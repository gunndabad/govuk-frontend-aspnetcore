using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class DateInputOptions
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? NamePrefix { get; set; }
    public IReadOnlyCollection<DateInputOptionsItem>? Items { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public FieldsetOptions? Fieldset { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public class DateInputOptionsItem
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? Name { get; set; }
    public IHtmlContent? Label { get; set; }
    public IHtmlContent? Value { get; set; }
    public IHtmlContent? Autocomplete { get; set; }
    public IHtmlContent? InputMode { get; set; }
    public IHtmlContent? Pattern { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

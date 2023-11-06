using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class DateInputOptions
{
    public string? Id { get; set; }
    public string? NamePrefix { get; set; }
    public IReadOnlyCollection<DateInputOptionsItem>? Items { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public FieldsetOptions? Fieldset { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class DateInputOptionsItem
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Label { get; set; }
    public string? Value { get; set; }
    public string? Autocomplete { get; set; }
    public string? InputMode { get; set; }
    public string? Pattern { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

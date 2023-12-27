using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class SelectOptions
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IReadOnlyCollection<SelectOptionsItem>? Items { get; set; }
    public string? Value { get; set; }
    public string? DescribedBy { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class SelectOptionsItem
{
    public string? Value { get; set; }
    public string? Text { get; set; }
    public bool? Selected { get; set; }
    public bool? Disabled { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}
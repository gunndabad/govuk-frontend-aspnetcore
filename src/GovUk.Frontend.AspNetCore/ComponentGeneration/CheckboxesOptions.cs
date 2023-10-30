using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class CheckboxesOptions
{
    public string? DescribedBy { get; set; }
    public FieldsetOptions? Fieldset { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public string? IdPrefix { get; set; }
    public string? Name { get; set; }
    public IReadOnlyCollection<CheckboxesOptionsItem>? Items { get; set; }
    public IReadOnlyCollection<string>? Values { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string>? Attributes { get; set; }
}

public class CheckboxesOptionsItem
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public string? Divider { get; set; }
    public bool? Checked { get; set; }
    public CheckboxesOptionsItemConditional? Conditional { get; set; }
    public string? Behaviour { get; set; }
    public bool? Disabled { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class CheckboxesOptionsItemConditional
{
    public string? Html { get; set; }
}

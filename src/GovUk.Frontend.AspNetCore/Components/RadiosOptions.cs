using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record RadiosOptions
{
    public FieldsetOptions? Fieldset { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public IHtmlContent? IdPrefix { get; set; }
    public IHtmlContent? Name { get; set; }
    public IReadOnlyCollection<RadiosOptionsItem>? Items { get; set; }
    public IHtmlContent? Value { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public record RadiosOptionsItem
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? Name { get; set; }
    public IHtmlContent? Value { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public IHtmlContent? Divider { get; set; }
    public bool? Checked { get; set; }
    public RadiosOptionsItemConditional? Conditional { get; set; }
    public string? Behaviour { get; set; }
    public bool? Disabled { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public record RadiosOptionsItemConditional
{
    public IHtmlContent? Html { get; set; }
}

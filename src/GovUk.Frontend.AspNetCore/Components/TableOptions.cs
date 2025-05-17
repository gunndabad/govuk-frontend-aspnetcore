using System.Text.Json.Serialization;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TableOptions
{
    public IReadOnlyCollection<IReadOnlyCollection<TableOptionsColumn>>? Rows { get; set; }
    public IReadOnlyCollection<TableOptionsHead>? Head { get; set; }
    public TemplateString? Caption { get; set; }
    public TemplateString? CaptionClasses { get; set; }
    public bool? FirstCellIsHeader { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record TableOptionsColumn
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Format { get; set; }
    public TemplateString? Classes { get; set; }
    [JsonPropertyName("colspan")]
    public int? ColSpan { get; set; }
    [JsonPropertyName("rowspan")]
    public int? RowSpan { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record TableOptionsHead
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Format { get; set; }
    public TemplateString? Classes { get; set; }
    [JsonPropertyName("colspan")]
    public int? ColSpan { get; set; }
    [JsonPropertyName("rowspan")]
    public int? RowSpan { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record SummaryListOptions
{
    public IReadOnlyCollection<SummaryListOptionsRow>? Rows { get; set; }
    public SummaryListOptionsCard? Card { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public record SummaryListOptionsCard
{
    public SummaryListOptionsCardTitle? Title { get; set; }
    public SummaryListOptionsRowActions? Actions { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public record SummaryListOptionsCardTitle
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public int? HeadingLevel { get; set; }
    public IHtmlContent? Classes { get; set; }
}

public record SummaryListOptionsRow
{
    public IHtmlContent? Classes { get; set; }
    public SummaryListOptionsRowKey? Key { get; set; }
    public SummaryListOptionsRowValue? Value { get; set; }
    public SummaryListOptionsRowActions? Actions { get; set; }
}

public record SummaryListOptionsRowKey
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }
}

public record SummaryListOptionsRowValue
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }
}

public record SummaryListOptionsRowActions
{
    public IHtmlContent? Classes { get; set; }
    public IReadOnlyCollection<SummaryListOptionsRowActionsItem>? Items { get; set; }
}

public record SummaryListOptionsRowActionsItem
{
    public IHtmlContent? Href { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? VisuallyHiddenText { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

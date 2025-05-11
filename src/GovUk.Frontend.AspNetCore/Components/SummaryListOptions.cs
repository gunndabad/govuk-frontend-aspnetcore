namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record SummaryListOptions
{
    public IReadOnlyCollection<SummaryListOptionsRow>? Rows { get; set; }
    public SummaryListOptionsCard? Card { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record SummaryListOptionsCard
{
    public SummaryListOptionsCardTitle? Title { get; set; }
    public SummaryListOptionsRowActions? Actions { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record SummaryListOptionsCardTitle
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public int? HeadingLevel { get; set; }
    public TemplateString? Classes { get; set; }
}

public record SummaryListOptionsRow
{
    public TemplateString? Classes { get; set; }
    public SummaryListOptionsRowKey? Key { get; set; }
    public SummaryListOptionsRowValue? Value { get; set; }
    public SummaryListOptionsRowActions? Actions { get; set; }
}

public record SummaryListOptionsRowKey
{
    public string? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
}

public record SummaryListOptionsRowValue
{
    public string? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
}

public record SummaryListOptionsRowActions
{
    public TemplateString? Classes { get; set; }
    public IReadOnlyCollection<SummaryListOptionsRowActionsItem>? Items { get; set; }
}

public record SummaryListOptionsRowActionsItem
{
    public TemplateString? Href { get; set; }
    public string? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? VisuallyHiddenText { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

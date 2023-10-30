using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class SummaryListOptions
{
    public IReadOnlyCollection<SummaryListOptionsRow>? Rows { get; set; }
    public SummaryListOptionsCard? Card { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class SummaryListOptionsCard
{
    public SummaryListOptionsCardTitle? Title { get; set; }
    public SummaryListOptionsRowActions? Actions { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

public class SummaryListOptionsCardTitle
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public int? HeadingLevel { get; set; }
    public string? Classes { get; set; }
}

public class SummaryListOptionsRow
{
    public string? Classes { get; set; }
    public SummaryListOptionsRowKey? Key { get; set; }
    public SummaryListOptionsRowValue? Value { get; set; }
    public SummaryListOptionsRowActions? Actions { get; set; }
}

public class SummaryListOptionsRowKey
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Classes { get; set; }
}

public class SummaryListOptionsRowValue
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Classes { get; set; }
}

public class SummaryListOptionsRowActions
{
    public string? Classes { get; set; }
    public IReadOnlyCollection<SummaryListOptionsRowActionsItem>? Items { get; set; }
}

public class SummaryListOptionsRowActionsItem
{
    public string? Href { get; set; }
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? VisuallyHiddenText { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class SummaryListOptions
{
    public IReadOnlyCollection<SummaryListOptionsRow>? Rows { get; set; }
    public SummaryListOptionsCard? Card { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public class SummaryListOptionsCard
{
    public SummaryListOptionsCardTitle? Title { get; set; }
    public SummaryListOptionsRowActions? Actions { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public class SummaryListOptionsCardTitle
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public int? HeadingLevel { get; set; }
    public IHtmlContent? Classes { get; set; }
}

public class SummaryListOptionsRow
{
    public IHtmlContent? Classes { get; set; }
    public SummaryListOptionsRowKey? Key { get; set; }
    public SummaryListOptionsRowValue? Value { get; set; }
    public SummaryListOptionsRowActions? Actions { get; set; }
}

public class SummaryListOptionsRowKey
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }
}

public class SummaryListOptionsRowValue
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }
}

public class SummaryListOptionsRowActions
{
    public IHtmlContent? Classes { get; set; }
    public IReadOnlyCollection<SummaryListOptionsRowActionsItem>? Items { get; set; }
}

public class SummaryListOptionsRowActionsItem
{
    public IHtmlContent? Href { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? VisuallyHiddenText { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

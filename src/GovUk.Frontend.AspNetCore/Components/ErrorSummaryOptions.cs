using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ErrorSummaryOptions
{
    public string? TitleText { get; set; }
    public string? TitleHtml { get; set; }
    public string? DescriptionText { get; set; }
    public string? DescriptionHtml { get; set; }
    public IReadOnlyCollection<ErrorSummaryOptionsErrorItem>? ErrorList { get; set; }
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public bool? DisableAutoFocus { get; set; }

    [NonStandardParameter]
    public AttributeCollection? TitleAttributes { get; set; }
    [NonStandardParameter]
    public AttributeCollection? DescriptionAttributes { get; set; }
}

public record ErrorSummaryOptionsErrorItem
{
    public string? Href { get; set; }
    public string? Text { get; set; }
    public string? Html { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? ItemAttributes { get; set; }
}

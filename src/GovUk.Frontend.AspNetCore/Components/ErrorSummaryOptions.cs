using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ErrorSummaryOptions
{
    public string? TitleText { get; set; }
    public IHtmlContent? TitleHtml { get; set; }
    public string? DescriptionText { get; set; }
    public IHtmlContent? DescriptionHtml { get; set; }
    public IReadOnlyCollection<ErrorSummaryOptionsErrorItem>? ErrorList { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public bool? DisableAutoFocus { get; set; }

    [NonStandardParameter]
    public EncodedAttributesDictionary? TitleAttributes { get; set; }
    [NonStandardParameter]
    public EncodedAttributesDictionary? DescriptionAttributes { get; set; }

    internal void Validate()
    {
        if (ErrorList is not null)
        {
            int i = 0;
            foreach (var item in ErrorList)
            {
                item.Validate(i++);
            }
        }
    }
}

public record ErrorSummaryOptionsErrorItem
{
    public IHtmlContent? Href { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    [NonStandardParameter]
    public EncodedAttributesDictionary? ItemAttributes { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified on item {itemIndex}.");
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record BreadcrumbsOptions
{
    public bool? CollapseOnMobile { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public IReadOnlyCollection<BreadcrumbsOptionsItem>? Items { get; set; }
    public IHtmlContent? LabelText { get; set; }

    internal void Validate()
    {
        if (Items is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Items)} must be specified.");
        }

        int i = 0;
        foreach (var item in Items)
        {
            item.Validate(i++);
        }
    }
}

public record BreadcrumbsOptionsItem
{
    public IHtmlContent? Html { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Href { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    [NonStandardParameter]
    internal EncodedAttributesDictionary? ItemAttributes { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified on item {itemIndex}.");
        }
    }
}

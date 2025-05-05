using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TabsOptions
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? IdPrefix { get; set; }
    public IHtmlContent? Title { get; set; }
    public IReadOnlyCollection<TabsOptionsItem>? Items { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        var gotIdPrefix = IdPrefix.NormalizeEmptyString() is not null;

        if (Items is not null)
        {
            int itemIndex = 0;
            foreach (var item in Items)
            {
                item.Validate(itemIndex++, gotIdPrefix);
            }
        }
    }
}

public record TabsOptionsItem
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? Label { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public TabsOptionsItemPanel? Panel { get; set; }

    internal void Validate(int itemIndex, bool gotIdPrefix)
    {
        if (Label is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Label)} must be specified on item {itemIndex}.");
        }

        if (!gotIdPrefix && Id.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Id)} must be specified on item {itemIndex} when the parent {nameof(TabsOptions.IdPrefix)} is null.");
        }
    }
}

public record TabsOptionsItemPanel
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

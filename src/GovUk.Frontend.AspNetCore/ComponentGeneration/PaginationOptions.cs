using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class PaginationOptions
{
    public IReadOnlyCollection<PaginationOptionsItem>? Items { get; set; }
    public PaginationOptionsPrevious? Previous { get; set; }
    public PaginationOptionsNext? Next { get; set; }
    public IHtmlContent? LandmarkLabel { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate()
    {
        Previous?.Validate();

        if (Items is not null)
        {
            var i = 0;
            foreach (var item in Items)
            {
                item.Validate(i++);
            }
        }

        Next?.Validate();
    }
}

public class PaginationOptionsItem
{
    public IHtmlContent? Number { get; set; }
    public IHtmlContent? VisuallyHiddenText { get; set; }
    public IHtmlContent? Href { get; set; }
    public bool? Current { get; set; }
    public bool? Ellipsis { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Href.NormalizeEmptyString() is null && Ellipsis != true)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Href)} must be specified unless {nameof(Ellipsis)} is {true} on item {itemIndex}.");
        }

        if (Number.NormalizeEmptyString() is null && Ellipsis != true)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Number)} must be specified unless {nameof(Ellipsis)} is {true} on item {itemIndex}.");
        }
    }
}

public class PaginationOptionsPrevious : IPaginationOptionsLink
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? LabelText { get; set; }
    public IHtmlContent? Href { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    [NonStandardParameter]
    public EncodedAttributesDictionary? ContainerAttributes { get; set; }

    internal void Validate()
    {
        if (Href is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Href)} must be specified.");
        }
    }
}

public class PaginationOptionsNext : IPaginationOptionsLink
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? LabelText { get; set; }
    public IHtmlContent? Href { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    [NonStandardParameter]
    public EncodedAttributesDictionary? ContainerAttributes { get; set; }

    internal void Validate()
    {
        if (Href is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Href)} must be specified.");
        }
    }
}

internal interface IPaginationOptionsLink
{
    string? Text { get; set; }
    IHtmlContent? Html { get; set; }
    IHtmlContent? LabelText { get; set; }
    IHtmlContent? Href { get; set; }
    EncodedAttributesDictionary? Attributes { get; set; }

    EncodedAttributesDictionary? ContainerAttributes { get; set; }
}

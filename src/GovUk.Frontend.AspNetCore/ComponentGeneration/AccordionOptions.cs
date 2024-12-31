using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record AccordionOptions
{
    public IHtmlContent? Id { get; set; }
    public int? HeadingLevel { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public bool? RememberExpanded { get; set; }
    public IHtmlContent? HideAllSectionsText { get; set; }
    public IHtmlContent? HideSectionText { get; set; }
    public IHtmlContent? HideSectionAriaLabelText { get; set; }
    public IHtmlContent? ShowAllSectionsText { get; set; }
    public IHtmlContent? ShowSectionText { get; set; }
    public IHtmlContent? ShowSectionAriaLabelText { get; set; }
    public IReadOnlyCollection<AccordionOptionsItem>? Items { get; set; }

    internal void Validate()
    {
        if (Id is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Id)} must be specified.");
        }

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

public record AccordionOptionsItem
{
    public AccordionOptionsItemHeading? Heading { get; set; }
    public AccordionOptionsItemSummary? Summary { get; set; }
    public AccordionOptionsItemContent? Content { get; set; }
    public bool? Expanded { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Heading is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Heading)} must be specified on item {itemIndex}.");
        }

        if (Heading.Html.NormalizeEmptyString() is null && Heading.Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Heading)}.{nameof(Summary.Html)} or {nameof(Heading)}.{nameof(Summary.Text)} must be specified on item {itemIndex}.");
        }
    }
}

public record AccordionOptionsItemHeading
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
}

public record AccordionOptionsItemSummary
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
}

public record AccordionOptionsItemContent
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
}

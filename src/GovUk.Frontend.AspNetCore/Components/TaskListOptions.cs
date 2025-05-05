using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TaskListOptions
{
    public IReadOnlyCollection<TaskListOptionsItem>? Items { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public IHtmlContent? IdPrefix { get; set; }

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

public record TaskListOptionsItem
{
    public TaskListOptionsItemTitle? Title { get; set; }
    public TaskListOptionsItemHint? Hint { get; set; }
    public TaskListOptionsItemStatus? Status { get; set; }
    public IHtmlContent? Href { get; set; }
    public IHtmlContent? Classes { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Title is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Title)} must be specified on item {itemIndex}.");
        }

        Title.Validate(itemIndex);

        if (Status is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Status)} must be specified on item {itemIndex}.");
        }

        Status.Validate(itemIndex);
    }
}

public record TaskListOptionsItemTitle
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified on item {itemIndex}'s {nameof(TaskListOptionsItem.Title)}.");
        }
    }
}

public record TaskListOptionsItemHint
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
}

public record TaskListOptionsItemStatus
{
    public TagOptions? Tag { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Tag is null && Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Tag)}, {nameof(Html)} or {nameof(Text)} must be specified on item {itemIndex}'s {nameof(TaskListOptionsItem.Status)}.");
        }
    }
}

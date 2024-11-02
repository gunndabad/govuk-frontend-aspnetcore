using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ErrorSummaryContext
{
    internal record DescriptionInfo(ImmutableDictionary<string, string?> Attributes, string Html);

    internal record TitleInfo(ImmutableDictionary<string, string?> Attributes, string Html);

    private readonly List<ErrorSummaryOptionsErrorItem> _items;

    public ErrorSummaryContext()
    {
        _items = new List<ErrorSummaryOptionsErrorItem>();
    }

    // internal for testing
    internal DescriptionInfo? Description;
    internal TitleInfo? Title;

    public IReadOnlyCollection<ErrorSummaryOptionsErrorItem> Items => _items;

    public bool HasContent => Description is not null || Title is not null || Items.Count > 0;

    public void AddItem(ErrorSummaryOptionsErrorItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public void AddItem(
        string? href,
        string html,
        ImmutableDictionary<string, string?> attributes,
        ImmutableDictionary<string, string?> itemAttributes
    )
    {
        ArgumentNullException.ThrowIfNull(html);
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(itemAttributes);

        AddItem(
            new ErrorSummaryOptionsErrorItem()
            {
                Text = null,
                Html = html,
                Href = href,
                Attributes = attributes,
                ItemAttributes = itemAttributes,
            }
        );
    }

    public (ImmutableDictionary<string, string?> Attributes, string Html)? GetTitle() =>
        Title is not null ? (Title.Attributes, Title.Html) : null;

    public (ImmutableDictionary<string, string?> Attributes, string Html)? GetDescription() =>
        Description is not null ? (Description.Attributes, Description.Html) : null;

    public void SetDescription(ImmutableDictionary<string, string?> attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Description != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                ErrorSummaryDescriptionTagHelper.TagName,
                ErrorSummaryTagHelper.TagName
            );
        }

        Description = new DescriptionInfo(attributes, html);
    }

    public void SetTitle(ImmutableDictionary<string, string?> attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Title != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                ErrorSummaryTitleTagHelper.TagName,
                ErrorSummaryTagHelper.TagName
            );
        }

        Title = new TitleInfo(attributes, html);
    }
}

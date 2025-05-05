using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ErrorSummaryContext
{
    private readonly List<ErrorSummaryContextItem> _items;

    public ErrorSummaryContext()
    {
        _items = new List<ErrorSummaryContextItem>();
    }

    public bool HaveExplicitItems { get; set; }

    public IReadOnlyCollection<ErrorSummaryContextItem> Items => _items;

    public (AttributeCollection Attributes, string Html)? Description { get; private set; }

    public (AttributeCollection Attributes, string Html)? Title { get; private set; }

    public void AddItem(ErrorSummaryContextItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
        HaveExplicitItems = true;
    }

    public void SetDescription(AttributeCollection attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Description != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                ErrorSummaryDescriptionTagHelper.TagName,
                ErrorSummaryTagHelper.TagName);
        }

        Description = (attributes, html);
    }

    public void SetTitle(AttributeCollection attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Title != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                ErrorSummaryTitleTagHelper.TagName,
                ErrorSummaryTagHelper.TagName);
        }

        Title = (attributes, html);
    }
}

internal record ErrorSummaryContextItem(
    string? Href,
    string Html,
    AttributeCollection Attributes,
    AttributeCollection ItemAttributes);

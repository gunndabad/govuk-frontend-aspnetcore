using System;
using System.Collections.Generic;
using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class PaginationContext
{
    private readonly List<PaginationItemBase> _items = new();

    public IReadOnlyCollection<PaginationItemBase> Items => _items.AsReadOnly();

    public PaginationNext? Next { get; private set; }

    public PaginationPrevious? Previous { get; private set; }

    public void AddItem(PaginationItemBase item)
    {
        Guard.ArgumentNotNull(nameof(item), item);

        if (Next is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                item is PaginationItemEllipsis
                    ? PaginationEllipsisItemTagHelper.TagName
                    : PaginationItemTagHelper.TagName,
                PaginationNextTagHelper.TagName
            );
        }

        // Only one 'current' item is allowed.
        if (
            item is PaginationItem paginationItem
            && paginationItem.IsCurrent
            && _items.OfType<PaginationItem>().Any(i => i.IsCurrent)
        )
        {
            throw new InvalidOperationException($"Only one current {PaginationItemTagHelper.TagName} is permitted.");
        }

        _items.Add(item);
    }

    public void SetNext(PaginationNext next)
    {
        Guard.ArgumentNotNull(nameof(next), next);

        if (Next is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                PaginationNextTagHelper.TagName,
                PaginationTagHelper.TagName
            );
        }

        Next = next;
    }

    public void SetPrevious(PaginationPrevious previous)
    {
        Guard.ArgumentNotNull(nameof(previous), previous);

        if (_items.Count != 0)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                PaginationPreviousTagHelper.TagName,
                _items[0] is PaginationItemEllipsis
                    ? PaginationEllipsisItemTagHelper.TagName
                    : PaginationItemTagHelper.TagName
            );
        }

        if (Next is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                PaginationPreviousTagHelper.TagName,
                PaginationNextTagHelper.TagName
            );
        }

        if (Previous is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                PaginationPreviousTagHelper.TagName,
                PaginationTagHelper.TagName
            );
        }

        Previous = previous;
    }
}

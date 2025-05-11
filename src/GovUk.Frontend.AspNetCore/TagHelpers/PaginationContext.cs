using GovUk.Frontend.AspNetCore.Components;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class PaginationContext
{
    private readonly List<IPaginationOptionsItem> _items = new();

    public IReadOnlyCollection<IPaginationOptionsItem> Items => _items.AsReadOnly();

    public PaginationOptionsNext? Next { get; private set; }

    public PaginationOptionsPrevious? Previous { get; private set; }

    public void AddItem(IPaginationOptionsItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (Next is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                item is PaginationOptionsItem { Ellipsis: true } ? PaginationEllipsisItemTagHelper.TagName : PaginationItemTagHelper.TagName,
                PaginationNextTagHelper.TagName);
        }

        // Only one 'current' item is allowed.
        if (item is PaginationOptionsItem { Current: true } && _items.Any(i => i is PaginationOptionsItem { Current: true }))
        {
            throw new InvalidOperationException($"Only one current {PaginationItemTagHelper.TagName} is permitted.");
        }

        _items.Add(item);
    }

    public void SetNext(PaginationOptionsNext next)
    {
        ArgumentNullException.ThrowIfNull(next);

        if (Next is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(PaginationNextTagHelper.TagName, PaginationTagHelper.TagName);
        }

        Next = next;
    }

    public void SetPrevious(PaginationOptionsPrevious previous)
    {
        ArgumentNullException.ThrowIfNull(previous);

        if (_items.Count != 0)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                PaginationPreviousTagHelper.TagName,
                _items[0] is PaginationOptionsItem { Ellipsis: true } ? PaginationEllipsisItemTagHelper.TagName : PaginationItemTagHelper.TagName);
        }

        if (Next is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(PaginationPreviousTagHelper.TagName, PaginationNextTagHelper.TagName);
        }

        if (Previous is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(PaginationPreviousTagHelper.TagName, PaginationTagHelper.TagName);
        }

        Previous = previous;
    }
}

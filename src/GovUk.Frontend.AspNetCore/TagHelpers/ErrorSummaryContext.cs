using GovUk.Frontend.AspNetCore.Components;

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

    public (AttributeCollection Attributes, TemplateString Html)? Description { get; private set; }

    public (AttributeCollection Attributes, TemplateString Html)? Title { get; private set; }

    public void AddItem(ErrorSummaryContextItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
        HaveExplicitItems = true;
    }

    public void SetDescription(AttributeCollection attributes, TemplateString html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Description is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                ErrorSummaryDescriptionTagHelper.TagName,
                ErrorSummaryTagHelper.TagName);
        }

        Description = (attributes, html);
    }

    public void SetTitle(AttributeCollection attributes, TemplateString html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Title is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                ErrorSummaryTitleTagHelper.TagName,
                ErrorSummaryTagHelper.TagName);
        }

        Title = (attributes, html);
    }
}

internal record ErrorSummaryContextItem(
    TemplateString? Href,
    TemplateString Html,
    AttributeCollection Attributes,
    AttributeCollection ItemAttributes);

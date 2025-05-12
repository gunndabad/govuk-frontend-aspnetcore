using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class TabsContext
{
    private readonly List<TabsItem> _items;
    private readonly bool _haveIdPrefix;

    public TabsContext(bool haveIdPrefix)
    {
        _items = new List<TabsItem>();
        _haveIdPrefix = haveIdPrefix;
    }

    public IReadOnlyList<TabsItem> Items => _items;

    public void AddItem(TabsItem item)
    {
        Guard.ArgumentNotNull(nameof(item), item);

        if (item.Id is null && !_haveIdPrefix)
        {
            throw new InvalidOperationException(
                $"Item must have the '{TabsItemTagHelper.IdAttributeName}' attribute specified.");
        }

        _items.Add(item);
    }
}

using GovUk.Frontend.AspNetCore.Components;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class TabsContext
{
    private readonly List<TabsOptionsItem> _items;

    public TabsContext(bool haveIdPrefix)
    {
        _items = new List<TabsOptionsItem>();
        HaveIdPrefix = haveIdPrefix;
    }

    public bool HaveIdPrefix { get; }

    public IReadOnlyList<TabsOptionsItem> Items => _items;

    public void AddItem(TabsOptionsItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }
}

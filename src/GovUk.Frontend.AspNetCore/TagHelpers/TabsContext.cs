using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class TabsContext
{
    private readonly List<TabsOptionsItem> _items;
    private readonly bool _haveIdPrefix;

    public TabsContext(bool haveIdPrefix)
    {
        _items = new List<TabsOptionsItem>();
        _haveIdPrefix = haveIdPrefix;
    }

    public IReadOnlyList<TabsOptionsItem> Items => _items;

    public void AddItem(TabsOptionsItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (item.Id == null && !_haveIdPrefix)
        {
            throw new InvalidOperationException(
                $"Item must have the '{TabsItemTagHelper.IdAttributeName}' attribute specified.");
        }

        _items.Add(item);
    }
}

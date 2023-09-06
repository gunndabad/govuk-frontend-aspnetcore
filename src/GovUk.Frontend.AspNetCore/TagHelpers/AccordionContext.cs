using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class AccordionContext
{
    private readonly List<AccordionItem> _items;

    public AccordionContext()
    {
        _items = new List<AccordionItem>();
    }

    public IReadOnlyList<AccordionItem> Items => _items;

    public void AddItem(AccordionItem item)
    {
        Guard.ArgumentNotNull(nameof(item), item);

        _items.Add(item);
    }
}

#nullable enable
using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class AccordionContext
    {
        private readonly List<AccordionItem> _items;

        public AccordionContext()
        {
            _items = new List<AccordionItem>();
        }

        public IReadOnlyCollection<AccordionItem> Items => _items;

        public void AddItem(AccordionItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }
    }
}

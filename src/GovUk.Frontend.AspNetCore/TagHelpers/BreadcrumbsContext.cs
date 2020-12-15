#nullable enable
using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class BreadcrumbsContext
    {
        private readonly List<BreadcrumbsItem> _items;

        public BreadcrumbsContext()
        {
            _items = new List<BreadcrumbsItem>();
        }

        public IReadOnlyCollection<BreadcrumbsItem> Items => _items;

        public void AddItem(BreadcrumbsItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }
    }
}

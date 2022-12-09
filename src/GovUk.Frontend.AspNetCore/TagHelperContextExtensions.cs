#nullable enable
using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore
{
    internal static class TagHelperContextExtensions
    {
        public static TItem GetContextItem<TItem>(this TagHelperContext context)
            where TItem : class
        {
            Guard.ArgumentNotNull(nameof(context), context);

            if (!context.Items.TryGetValue(typeof(TItem), out var item))
            {
                throw new InvalidOperationException($"No context item found for type: '{typeof(TItem).FullName}'.");
            }

            return (TItem)item;
        }

        public static IDisposable SetScopedContextItem<TItem>(this TagHelperContext context, TItem item)
            where TItem : class
        {
            Guard.ArgumentNotNull(nameof(context), context);
            Guard.ArgumentNotNull(nameof(item), item);

            return SetScopedContextItem(context, typeof(TItem), item);
        }

        public static IDisposable SetScopedContextItem(
            this TagHelperContext context,
            object key,
            object value)
        {
            Guard.ArgumentNotNull(nameof(context), context);
            Guard.ArgumentNotNull(nameof(key), key);
            Guard.ArgumentNotNull(nameof(value), value);

            context.Items.TryGetValue(key, out var previousValue);
            context.Items[key] = value;

            return new RestoreItemsOnDispose(context, key, previousValue);
        }

        internal class RestoreItemsOnDispose : IDisposable
        {
            private readonly TagHelperContext _context;
            private readonly object _key;
            private readonly object? _previousValue;

            public RestoreItemsOnDispose(
                TagHelperContext context,
                object key,
                object? previousValue)
            {
                _context = Guard.ArgumentNotNull(nameof(context), context);
                _key = Guard.ArgumentNotNull(nameof(key), key);
                _previousValue = previousValue;
            }

            public void Dispose()
            {
                if (_previousValue != null)
                {
                    _context.Items[_key] = _previousValue;
                }
                else
                {
                    _context.Items.Remove(_key);
                }
            }
        }
    }
}

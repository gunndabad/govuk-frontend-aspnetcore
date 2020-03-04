using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore
{
    internal static class TagHelperContextExtensions
    {
        public static IDisposable SetScopedContextItem(
            this TagHelperContext context,
            object key,
            object value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            context.Items.TryGetValue(key, out var previousValue);
            context.Items[key] = value;

            return new RestoreItemsOnDispose(context, key, previousValue);
        }

        internal class RestoreItemsOnDispose : IDisposable
        {
            private readonly TagHelperContext _context;
            private readonly object _key;
            private readonly object _previousValue;

            public RestoreItemsOnDispose(
                TagHelperContext context,
                object key,
                object previousValue)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _key = key ?? throw new ArgumentNullException(nameof(key));
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

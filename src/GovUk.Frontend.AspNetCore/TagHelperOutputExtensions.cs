#nullable disable
using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore
{
    internal static class TagHelperOutputExtensions
    {
        public static void ThrowIfOutputHasAttributes(this TagHelperOutput output)
        {
            Guard.ArgumentNotNull(nameof(output), output);

            if (output.Attributes.Count != 0)
            {
                var attributeNames = string.Join(", ", output.Attributes.Select(a => $"'{a.Name}'"));
                throw new InvalidOperationException($"Unexpected attributes specified: {attributeNames}.");
            }
        }
    }
}

#nullable enable
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    internal static class AttributeDictionaryExtensions
    {
        /// <summary>
        /// Adds a CSS class to the list of CSS classes in the tag if it does not already specified.
        /// If there are already CSS classes on the tag then a space character and the new class will be appended to
        /// the existing list.
        /// </summary>
        /// <param name="attributeDictionary">The <see cref="AttributeDictionary"/> to add the CSS class name to.</param>
        /// <param name="value">The CSS class name to add.</param>
        public static void MergeCssClass(this AttributeDictionary attributeDictionary, string value)
        {
            Guard.ArgumentNotNull(nameof(attributeDictionary), attributeDictionary);

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (attributeDictionary.TryGetValue("class", out var currentValue))
            {
                var classes = currentValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (!classes.Contains(value, StringComparer.OrdinalIgnoreCase))
                {
                    attributeDictionary["class"] = value + " " + currentValue;
                }
            }
            else
            {
                attributeDictionary.Add("class", value);
            }
        }
    }
}

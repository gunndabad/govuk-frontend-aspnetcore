#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    /// <summary>
    /// Utility extensions for <see cref="TagBuilder"/>.
    /// </summary>
    public static class TagBuilderExtensions
    {
        /// <summary>
        /// Adds a CSS class to the list of CSS classes in the tag if it does not already specified.
        /// If there are already CSS classes on the tag then a space character and the new class will be appended to
        /// the existing list.
        /// </summary>
        /// <param name="tagBuilder">The <see cref="TagBuilder"/> to add the CSS class name to.</param>
        /// <param name="value">The CSS class name to add.</param>
        public static void MergeCssClass(this TagBuilder tagBuilder, string value)
        {
            Guard.ArgumentNotNull(nameof(tagBuilder), tagBuilder);

            tagBuilder.Attributes.MergeCssClass(value);
        }

        internal static void MergeOptionalAttributes(this TagBuilder tagBuilder, AttributeDictionary? attributes)
        {
            if (attributes is not null)
            {
                tagBuilder.MergeAttributes(attributes);
            }
        }
    }
}

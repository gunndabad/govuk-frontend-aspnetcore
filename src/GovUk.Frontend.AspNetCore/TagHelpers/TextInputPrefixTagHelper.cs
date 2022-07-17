#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the prefix element in a GDS input component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TextInputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.InputPrefixElement)]
    public class TextInputPrefixTagHelper : TagHelper
    {
        internal const string TagName = "govuk-input-prefix";

        /// <summary>
        /// Creates an <see cref="TextInputPrefixTagHelper"/>.
        /// </summary>
        public TextInputPrefixTagHelper()
        {
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inputContext = (TextInputContext)context.Items[typeof(TextInputContext)];

            var content = await output.GetChildContentAsync();

            inputContext.SetPrefix(output.Attributes.ToAttributeDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }
}

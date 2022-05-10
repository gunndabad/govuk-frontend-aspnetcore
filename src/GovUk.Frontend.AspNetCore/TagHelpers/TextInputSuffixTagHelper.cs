#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the prefix suffix in a GDS input component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TextInputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.InputSuffixElement)]
    public class TextInputSuffixTagHelper : FormGroupErrorMessageTagHelperBase
    {
        internal const string TagName = "govuk-input-suffix";

        /// <summary>
        /// Creates an <see cref="TextInputSuffixTagHelper"/>.
        /// </summary>
        public TextInputSuffixTagHelper()
        {
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inputContext = context.GetContextItem<TextInputContext>();

            var content = await output.GetChildContentAsync();

            inputContext.SetSuffix(output.Attributes.ToAttributeDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }
}

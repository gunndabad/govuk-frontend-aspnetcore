#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the prefix suffix in a GDS input component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = InputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.InputSuffixElement)]
    public class InputSuffixTagHelper : FormGroupErrorMessageTagHelperBase
    {
        internal const string TagName = "govuk-input-suffix";

        /// <summary>
        /// Creates an <see cref="InputSuffixTagHelper"/>.
        /// </summary>
        public InputSuffixTagHelper()
        {
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inputContext = context.GetContextItem<InputContext>();

            var content = await output.GetChildContentAsync();

            inputContext.SetSuffix(output.Attributes.ToAttributesDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }
}

#nullable enable
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the prefix element in a GDS input component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = InputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.InputPrefixElement)]
    public class InputPrefixTagHelper : FormGroupErrorMessageTagHelperBase
    {
        internal const string TagName = "govuk-input-prefix";

        /// <summary>
        /// Creates an <see cref="InputPrefixTagHelper"/>.
        /// </summary>
        public InputPrefixTagHelper()
        {
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inputContext = (InputContext)context.Items[typeof(InputContext)];

            var content = await output.GetChildContentAsync();

            inputContext.SetPrefix(output.Attributes.ToAttributesDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }
}

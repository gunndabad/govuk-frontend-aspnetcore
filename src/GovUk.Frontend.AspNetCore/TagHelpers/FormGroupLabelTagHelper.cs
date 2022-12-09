using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the label in a GDS form group component.
    /// </summary>
    [HtmlTargetElement(CharacterCountTagHelper.LabelTagName, ParentTag = CharacterCountTagHelper.TagName)]
    [HtmlTargetElement(FileUploadTagHelper.LabelTagName, ParentTag = FileUploadTagHelper.TagName)]
    [HtmlTargetElement(SelectTagHelper.LabelTagName, ParentTag = SelectTagHelper.TagName)]	
    [HtmlTargetElement(TextAreaTagHelper.LabelTagName, ParentTag = TextAreaTagHelper.TagName)]
    [HtmlTargetElement(TextInputTagHelper.LabelTagName, ParentTag = TextInputTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.LabelElement)]
    public class FormGroupLabelTagHelper : TagHelper
    {
        private const string IsPageHeadingAttributeName = "is-page-heading";

        /// <summary>
        /// Creates a <see cref="FormGroupLabelTagHelper"/>.
        /// </summary>
        public FormGroupLabelTagHelper()
        {
        }

        /// <summary>
        /// Whether the label also acts as the heading for the page.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; } = ComponentGenerator.LabelDefaultIsPageHeading;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
                await output.GetChildContentAsync() :
                null;

            var formGroupContext = context.GetContextItem<FormGroupContext>();

            formGroupContext.SetLabel(
                IsPageHeading,
                output.Attributes.ToAttributeDictionary(),
                childContent?.Snapshot());

            output.SuppressOutput();
        }
    }
}

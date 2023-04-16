using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an ellipsis item in a GDS pagination component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = PaginationTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = PaginationTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.PaginationEllipsisElement)]
    public class PaginationEllipsisItemTagHelper : TagHelper
    {
        internal const string TagName = "govuk-pagination-ellipsis";
        internal const string ShortTagName = "ellipsis";

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var paginationContext = context.GetContextItem<PaginationContext>();

            paginationContext.AddItem(new PaginationItemEllipsis());

            output.SuppressOutput();
        }
    }
}

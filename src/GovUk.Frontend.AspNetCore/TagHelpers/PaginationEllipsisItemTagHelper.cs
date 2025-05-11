using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an ellipsis item in a GDS pagination component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = PaginationTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.PaginationEllipsis)]
public class PaginationEllipsisItemTagHelper : TagHelper
{
    internal const string TagName = "govuk-pagination-ellipsis";

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var paginationContext = context.GetContextItem<PaginationContext>();

        var attributes = new AttributeCollection(output.Attributes);

        paginationContext.AddItem(new PaginationOptionsItem()
        {
            Ellipsis = true,
            Attributes = attributes
        });

        output.SuppressOutput();
    }
}

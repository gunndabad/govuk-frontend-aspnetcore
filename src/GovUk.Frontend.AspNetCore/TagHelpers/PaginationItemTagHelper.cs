using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an item in a GDS pagination component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = PaginationTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.PaginationItem)]
public class PaginationItemTagHelper : TagHelper
{
    internal const string TagName = "govuk-pagination-item";

    private const string IsCurrentAttributeName = "is-current";
    private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

    /// <summary>
    /// Whether this item is the current page the user is on.
    /// </summary>
    /// <remarks>
    /// By default this is determined by comparing the current URL to this item's generated <c>href</c> attribute.
    /// </remarks>
    [HtmlAttributeName(IsCurrentAttributeName)]
    public bool? IsCurrent { get; set; }

    /// <summary>
    /// The visually hidden text for the pagination item.
    /// </summary>
    /// <remarks>
    /// This should include the page number.
    /// The default is <c>Page &lt;number&gt;</c>.
    /// </remarks>
    [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
    public string? VisuallyHiddenText { get; set; }

    /// <summary>
    /// Gets the <see cref="ViewContext"/> of the executing view.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    [DisallowNull]
    public ViewContext? ViewContext { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var paginationContext = context.GetContextItem<PaginationContext>();

        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("href", out _);
        var href = output.GetUrlAttribute("href");

        var current = IsCurrent == true || ItemIsCurrentPage();

        paginationContext.AddItem(new PaginationOptionsItem()
        {
            Attributes = attributes,
            Href = href,
            Current = current,
            Number = childContent.ToTemplateString(),
            VisuallyHiddenText = VisuallyHiddenText
        });

        output.SuppressOutput();

        bool ItemIsCurrentPage()
        {
            var currentUrl = ViewContext!.HttpContext.Request.GetEncodedPathAndQuery();
            return href?.ToHtmlString(HtmlEncoder.Default) == currentUrl;
        }
    }
}

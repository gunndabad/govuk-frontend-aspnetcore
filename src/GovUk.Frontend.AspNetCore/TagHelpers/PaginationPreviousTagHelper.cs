using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the link to the previous page in a GDS pagination component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = PaginationTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.PaginationPreviousElement)]
public class PaginationPreviousTagHelper : TagHelper
{
    internal const string TagName = "govuk-pagination-previous";

    private const string LabelTextAttributeName = "label-text";
    private const string LinkAttributesPrefix = "link-";

    /// <summary>
    /// The optional label that goes underneath the link to the previous page, providing further context for the user about where the link goes.
    /// </summary>
    [HtmlAttributeName(LabelTextAttributeName)]
    public string? LabelText { get; set; }

    /// <summary>
    /// Additional attributes to add to the generated <c>a</c> element.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]
    public IDictionary<string, string?> LinkAttributes { get; set; } = new Dictionary<string, string?>();

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var paginationContext = context.GetContextItem<PaginationContext>();

        var content = output.TagMode == TagMode.StartTagAndEndTag ?
            await output.GetChildContentAsync() :
            null;

        string? href = null;

        if (output.Attributes.TryGetAttribute("href", out var hrefAttribute))
        {
            href = hrefAttribute.Value.ToString();
            output.Attributes.Remove(hrefAttribute);
        }

        paginationContext.SetPrevious(new PaginationPrevious()
        {
            Attributes = output.Attributes.ToAttributeDictionary(),
            Href = href,
            LabelText = LabelText,
            LinkAttributes = LinkAttributes?.ToAttributeDictionary(),
            Text = content
        });

        output.SuppressOutput();
    }
}

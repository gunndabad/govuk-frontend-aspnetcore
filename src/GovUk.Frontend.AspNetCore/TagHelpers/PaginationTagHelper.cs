using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS pagination component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(
    PaginationPreviousTagHelper.TagName,
    PaginationItemTagHelper.TagName,
    PaginationEllipsisItemTagHelper.TagName,
    PaginationNextTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.PaginationElement)]
public class PaginationTagHelper : TagHelper
{
    internal const string TagName = "govuk-pagination";

    private const string LandmarkLabelAttributeName = "landmark-label";

    private readonly IGovUkHtmlGenerator _htmlGenerator;
    private string? _landmarkLabel = ComponentGenerator.PaginationDefaultLandmarkLabel;

    /// <summary>
    /// Creates a new <see cref="PaginationTagHelper"/>.
    /// </summary>
    public PaginationTagHelper()
        : this(htmlGenerator: null)
    {
    }

    internal PaginationTagHelper(IGovUkHtmlGenerator? htmlGenerator)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
    }

    /// <summary>
    /// The label for the navigation landmark that wraps the pagination.
    /// </summary>
    /// <remarks>
    /// The default is <c>results</c>.
    /// Cannot be <c>null</c> or empty.
    /// </remarks>
    [DisallowNull]
    [HtmlAttributeName(LandmarkLabelAttributeName)]
    public string? LandmarkLabel
    {
        get => _landmarkLabel;
        set => _landmarkLabel = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var paginationContext = new PaginationContext();

        using (context.SetScopedContextItem(paginationContext))
        {
            await output.GetChildContentAsync();
        }

        var tagBuilder = _htmlGenerator.GeneratePagination(
            paginationContext.Items,
            paginationContext.Previous,
            paginationContext.Next,
            LandmarkLabel,
            output.Attributes.ToAttributeDictionary());

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}

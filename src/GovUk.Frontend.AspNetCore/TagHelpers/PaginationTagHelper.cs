using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.Components;
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
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.Pagination)]
public class PaginationTagHelper : TagHelper
{
    internal const string TagName = "govuk-pagination";

    private const string LandmarkLabelAttributeName = "landmark-label";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="PaginationTagHelper"/>.
    /// </summary>
    public PaginationTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
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
    public string? LandmarkLabel { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var paginationContext = new PaginationContext();

        using (context.SetScopedContextItem(paginationContext))
        {
            await output.GetChildContentAsync();
        }

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("classes", out var classes);

        var component = await _componentGenerator.GeneratePaginationAsync(new PaginationOptions()
        {
            Items = paginationContext.Items.OfType<PaginationOptionsItem>().ToArray(),
            Previous = paginationContext.Previous,
            Next = paginationContext.Next,
            LandmarkLabel = LandmarkLabel,
            Classes = classes,
            Attributes = attributes
        });

        output.ApplyComponentHtml(component);
    }
}

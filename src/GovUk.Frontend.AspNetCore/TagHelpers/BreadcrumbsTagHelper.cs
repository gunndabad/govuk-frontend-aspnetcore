using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS breadcrumbs component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(BreadcrumbsItemTagHelper.TagName/*, BreadcrumbsItemTagHelper.ShortTagName*/)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.Breadcrumbs)]
public class BreadcrumbsTagHelper : TagHelper
{
    internal const string TagName = "govuk-breadcrumbs";

    private const string CollapseOnMobileAttributeName = "collapse-on-mobile";
    private const string LabelTextAttributeName = "label-text";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="ButtonTagHelper"/>.
    /// </summary>
    public BreadcrumbsTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// Whether to collapse to the first and last item only on tablet breakpoint and below.
    /// </summary>
    /// <remarks>
    /// If not specified, <see langword="false"/> will be used.
    /// </remarks>
    [HtmlAttributeName(CollapseOnMobileAttributeName)]
    public bool? CollapseOnMobile { get; set; }

    /// <summary>
    /// The plain text label identifying the landmark to screen readers.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>Breadcrumb</c>.
    /// </remarks>
    [HtmlAttributeName(LabelTextAttributeName)]
    public string? LabelText { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var breadcrumbsContext = new BreadcrumbsContext();

        using (context.SetScopedContextItem(breadcrumbsContext))
        {
            await output.GetChildContentAsync();
        }

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = await _componentGenerator.GenerateBreadcrumbsAsync(new BreadcrumbsOptions()
        {
            CollapseOnMobile = CollapseOnMobile,
            Classes = classes,
            Attributes = attributes,
            Items = breadcrumbsContext.Items,
            LabelText = LabelText
        });

        output.ApplyComponentHtml(component);
    }
}

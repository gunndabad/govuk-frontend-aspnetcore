using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the legend in a GDS radios component fieldset.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.FieldsetLegendElement)]
public class RadiosFieldsetLegendTagHelper : TagHelper
{
    internal const string TagName = "govuk-radios-fieldset-legend";

    private const string IsPageHeadingAttributeName = "is-page-heading";

    /// <summary>
    /// Whether the legend also acts as the heading for the page.
    /// </summary>
    /// <remarks>
    /// The default is <c>false</c>.
    /// </remarks>
    [HtmlAttributeName(IsPageHeadingAttributeName)]
    public bool IsPageHeading { get; set; } = ComponentGenerator.FieldsetLegendDefaultIsPageHeading;

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var fieldsetContext = context.GetContextItem<RadiosFieldsetContext>();

        var childContent =
            output.TagMode == TagMode.StartTagAndEndTag ? (await output.GetChildContentAsync()).Snapshot() : null;

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        fieldsetContext.SetLegend(IsPageHeading, output.Attributes.ToAttributeDictionary(), content: childContent);

        output.SuppressOutput();
    }
}

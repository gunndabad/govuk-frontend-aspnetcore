using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the hint of a radios item in a GDS radios component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = RadiosItemTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.HintElement)]
public class RadiosItemHintTagHelper : TagHelper
{
    internal const string TagName = "govuk-radios-item-hint";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var itemContext = context.GetContextItem<RadiosItemContext>();

        var childContent = await output.GetChildContentAsync();

        itemContext.SetHint(output.Attributes.ToAttributeDictionary(), childContent.Snapshot());

        output.SuppressOutput();
    }
}

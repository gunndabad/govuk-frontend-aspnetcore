using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the text in a GDS details component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = DetailsTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.DetailsTextElement)]
public class DetailsTextTagHelper : TagHelper
{
    internal const string TagName = "govuk-details-text";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        var detailsContext = (DetailsContext)context.Items[typeof(DetailsContext)];
        detailsContext.SetText(output.Attributes.ToAttributeDictionary(), childContent);

        output.SuppressOutput();
    }
}

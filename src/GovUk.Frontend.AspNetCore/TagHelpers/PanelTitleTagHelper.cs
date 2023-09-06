using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the title in a GDS panel component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = PanelTagHelper.TagName)]
public class PanelTitleTagHelper : TagHelper
{
    internal const string TagName = "govuk-panel-title";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var panelContext = context.GetContextItem<PanelContext>();

        var childContent = await output.GetChildContentAsync();

        panelContext.SetTitle(childContent.Snapshot());

        output.SuppressOutput();
    }
}

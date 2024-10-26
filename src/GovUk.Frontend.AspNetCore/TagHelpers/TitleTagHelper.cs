using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting &lt;title&gt; elements.
/// </summary>
[HtmlTargetElement("title")]
public class TitleTagHelper : TagHelper
{
    private const string ErrorPrefixAttributeName = "error-prefix";

    /// <summary>
    /// The prefix to add to the <c>title</c> when the page has errors.
    /// </summary>
    /// <remarks>
    ///  The default is <c>Error:</c>.
    /// </remarks>
    [HtmlAttributeName(ErrorPrefixAttributeName)]
    public string? ErrorPrefix { get; set; }

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
        await base.ProcessAsync(context, output);

        if (!string.IsNullOrEmpty(ErrorPrefix) && ViewContext!.ViewData.GetPageHasErrors())
        {
            output.PreContent.Append(ErrorPrefix + " ");
        }
    }
}

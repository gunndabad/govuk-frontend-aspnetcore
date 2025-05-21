using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// Utility extensions for <see cref="TagHelperContent"/>.
/// </summary>
public static class TagHelperContentExtensions
{
    /// <summary>
    /// Creates a snapshot of the content in a specified <see cref="TagHelperContent"/>.
    /// </summary>
    public static IHtmlContent Snapshot(this TagHelperContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        return new HtmlString(content.GetContent());
    }
}

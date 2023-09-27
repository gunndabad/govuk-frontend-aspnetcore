using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting &lt;title&gt; elements.
/// </summary>
[HtmlTargetElement("title")]
public class TitleTagHelper : TagHelper
{
    private const string DefaultErrorPrefix = "Error:";
    private const string ErrorPrefixAttributeName = "gfa-error-prefix";

    private readonly GovUkFrontendAspNetCoreOptions _options;

    private string? _errorPrefix = DefaultErrorPrefix;

    /// <summary>
    /// Creates a new <see cref="TitleTagHelper"/>.
    /// </summary>
    public TitleTagHelper(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        _options = Guard.ArgumentNotNull(nameof(optionsAccessor), optionsAccessor).Value;
    }

    /// <summary>
    /// The prefix to add to the <c>title</c> when the page has errors.
    /// </summary>
    /// <remarks>
    ///  The default is <c>Error:</c>.
    /// </remarks>
    [HtmlAttributeName(ErrorPrefixAttributeName)]
    public string? ErrorPrefix
    {
        get => _errorPrefix;
        set => _errorPrefix = value;
    }

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

        if (_options.PrependErrorToTitle && !string.IsNullOrEmpty(ErrorPrefix) && ViewContext!.ViewData.GetPageHasErrors())
        {
            output.PreContent.Append(ErrorPrefix + " ");
        }
    }
}

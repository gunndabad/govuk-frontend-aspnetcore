using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; elements that adds a <c>novalidate</c> attribute.
/// </summary>
[HtmlTargetElement("form")]
public class FormNovalidateTagHelper : TagHelper
{
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    /// <summary>
    /// Creates a <see cref="FormNovalidateTagHelper"/>.
    /// </summary>
    public FormNovalidateTagHelper(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        _optionsAccessor = Guard.ArgumentNotNull(nameof(optionsAccessor), optionsAccessor);
    }

    /// <inheritdoc/>
    public override int Order => -1;

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (_optionsAccessor.Value.AddNovalidateAttributeToForms)
        {
            if (!output.Attributes.ContainsName("novalidate"))
            {
                output.Attributes.Add(new TagHelperAttribute("novalidate", null, HtmlAttributeValueStyle.Minimized));
            }
        }
    }
}

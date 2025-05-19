using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS skip link component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.SkipLink)]
public class SkipLinkTagHelper : TagHelper
{
    internal const string TagName = "govuk-skip-link";

    private const string HrefAttributeName = "href";

    private readonly IComponentGenerator _componentGenerator;
    private readonly HtmlEncoder _encoder;

    /// <summary>
    /// Creates a new <see cref="BackLinkTagHelper"/>.
    /// </summary>
    public SkipLinkTagHelper(IComponentGenerator componentGenerator, HtmlEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(encoder);
        _componentGenerator = componentGenerator;
        _encoder = encoder;
    }

    /// <summary>
    /// The <c>href</c> attribute for the link.
    /// </summary>
    /// <remarks>
    /// The default is <c>&quot;#content&quot;</c>.
    /// </remarks>
    [HtmlAttributeName(HrefAttributeName)]
    public string? Href { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var content = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            content = output.Content;
        }

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = await _componentGenerator.GenerateSkipLinkAsync(new SkipLinkOptions()
        {
            Text = null,
            Html = content.ToTemplateString(),
            Href = Href,
            Classes = classes,
            Attributes = attributes
        });

        output.ApplyComponentHtml(component, _encoder);
    }
}

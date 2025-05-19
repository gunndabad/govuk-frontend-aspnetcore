using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS warning text component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(ComponentGenerator.WarningTextElement)]
public class WarningTextTagHelper : TagHelper
{
    internal const string TagName = "govuk-warning-text";

    private const string IconFallbackTextAttributeName = "icon-fallback-text";

    private readonly IComponentGenerator _componentGenerator;
    private readonly HtmlEncoder _encoder;

    /// <summary>
    /// Creates a new <see cref="WarningTextTagHelper"/>.
    /// </summary>
    public WarningTextTagHelper(IComponentGenerator componentGenerator, HtmlEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(encoder);
        _componentGenerator = componentGenerator;
        _encoder = encoder;
    }

    /// <summary>
    /// The fallback text for the icon.
    /// </summary>
    [HtmlAttributeName(IconFallbackTextAttributeName)]
    public string? IconFallbackText { get; set; }

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

        var component = await _componentGenerator.GenerateWarningTextAsync(new WarningTextOptions()
        {
            Text = null,
            Html = content.ToTemplateString(),
            IconFallbackText = IconFallbackText,
            Classes = classes,
            Attributes = attributes
        });

        output.ApplyComponentHtml(component, _encoder);
    }
}

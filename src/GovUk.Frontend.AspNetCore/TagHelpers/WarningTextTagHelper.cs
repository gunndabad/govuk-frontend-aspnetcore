using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS warning text component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.WarningTextElement)]
public class WarningTextTagHelper : TagHelper
{
    internal const string TagName = "govuk-warning-text";

    private const string IconFallbackTextAttributeName = "icon-fallback-text";
    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="WarningTextTagHelper"/>.
    /// </summary>
    public WarningTextTagHelper(IComponentGenerator componentGenerator)
    {
        _componentGenerator = componentGenerator;
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

        var attributes = output.Attributes.ToEncodedAttributeDictionary().Remove("class", out var classes);

        var component = _componentGenerator.GenerateWarningText(
            new WarningTextOptions()
            {
                Html = content?.ToHtmlString(),
                IconFallbackText = IconFallbackText,
                Classes = classes,
                Attributes = attributes,
            }
        );

        output.WriteComponent(component);
    }
}

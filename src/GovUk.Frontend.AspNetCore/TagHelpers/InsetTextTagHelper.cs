using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS inset text component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.InsetText)]
public class InsetTextTagHelper : TagHelper
{
    internal const string TagName = "govuk-inset-text";

    private const string IdAttributeName = "id";

    private readonly IComponentGenerator _componentGenerator;
    private readonly HtmlEncoder _encoder;

    /// <summary>
    /// Creates a new <see cref="InsetTextTagHelper"/>.
    /// </summary>
    public InsetTextTagHelper(IComponentGenerator componentGenerator, HtmlEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(encoder);
        _componentGenerator = componentGenerator;
        _encoder = encoder;
    }

    /// <summary>
    /// The <c>id</c> attribute.
    /// </summary>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

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

        var component = await _componentGenerator.GenerateInsetTextAsync(new InsetTextOptions()
        {
            Text = null,
            Html = content.ToTemplateString(),
            Id = Id,
            Classes = classes,
            Attributes = attributes
        });

        output.ApplyComponentHtml(component, _encoder);
    }
}

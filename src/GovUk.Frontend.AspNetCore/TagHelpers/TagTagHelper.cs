using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS tag component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.TagElement)]
public class TagTagHelper : TagHelper
{
    internal const string TagName = "govuk-tag";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="TagTagHelper"/>.
    /// </summary>
    public TagTagHelper(IComponentGenerator componentGenerator)
    {
        _componentGenerator = componentGenerator;
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = (await output.GetChildContentAsync()).Snapshot();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var attributes = new EncodedAttributesDictionary(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = _componentGenerator.GenerateTag(new TagOptions()
        {
            Text = null,
            Html = childContent,
            Attributes = attributes,
            Classes = classes
        });

        component.WriteTo(output);
    }
}

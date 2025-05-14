using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the prefix suffix in a GDS input component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = TextInputTagHelper.TagName)]
//[HtmlTargetElement(ShortTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.InputSuffix)]
public class TextInputSuffixTagHelper : TagHelper
{
    internal const string TagName = "govuk-input-suffix";
    //internal const string ShortTagName = ShortTagNames.Suffix;

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var inputContext = context.GetContextItem<TextInputContext>();

        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("class", out var classes);

        inputContext.SetSuffix(
            new InputOptionsSuffix()
            {
                Text = null,
                Html = childContent.ToTemplateString(),
                Classes = classes,
                Attributes = attributes
            },
            output.TagName);

        output.SuppressOutput();
    }
}

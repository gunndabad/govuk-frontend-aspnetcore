using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the prefix element in a GDS input component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = TextInputTagHelper.TagName)]
//[HtmlTargetElement(ShortTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.InputPrefix)]
public class TextInputPrefixTagHelper : TagHelper
{
    internal const string TagName = "govuk-input-prefix";
    //internal const string ShortTagName = ShortTagNames.Prefix;

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var inputContext = (TextInputContext)context.Items[typeof(TextInputContext)];

        var childContent = (await output.GetChildContentAsync()).Snapshot();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("class", out var classes);

        inputContext.SetPrefix(
            new InputOptionsPrefix()
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

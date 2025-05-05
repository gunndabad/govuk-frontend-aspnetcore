using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the prefix element in a GDS input component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = TextInputTagHelper.TagName)]
//[HtmlTargetElement(ShortTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(LegacyComponentGenerator.InputPrefixElement)]
public class TextInputPrefixTagHelper : TagHelper
{
    internal const string TagName = "govuk-input-prefix";
    //internal const string ShortTagName = ShortTagNames.Prefix;

    /// <summary>
    /// Creates an <see cref="TextInputPrefixTagHelper"/>.
    /// </summary>
    public TextInputPrefixTagHelper()
    {
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var inputContext = (TextInputContext)context.Items[typeof(TextInputContext)];

        var childContent = (await output.GetChildContentAsync()).Snapshot();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        inputContext.SetPrefix(new EncodedAttributesDictionary(output.Attributes), childContent, output.TagName);

        output.SuppressOutput();
    }
}

using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the prefix suffix in a GDS input component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = TextInputTagHelper.TagName)]
[HtmlTargetElement(ShortTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.InputSuffixElement)]
public class TextInputSuffixTagHelper : TagHelper
{
    internal const string TagName = "govuk-input-suffix";
    internal const string ShortTagName = ShortTagNames.Suffix;

    /// <summary>
    /// Creates an <see cref="TextInputSuffixTagHelper"/>.
    /// </summary>
    public TextInputSuffixTagHelper() { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var inputContext = context.GetContextItem<TextInputContext>();

        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        inputContext.SetSuffix(
            output.Attributes.ToEncodedAttributeDictionary(),
            childContent.ToHtmlString(),
            output.TagName
        );

        output.SuppressOutput();
    }
}

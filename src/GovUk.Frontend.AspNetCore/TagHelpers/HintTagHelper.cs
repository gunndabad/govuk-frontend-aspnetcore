using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the hint in a GDS form component.
/// </summary>
[HtmlTargetElement(TextInputTagHelper.HintTagName, ParentTag = TextInputTagHelper.TagName)]
[HtmlTargetElement(TextInputTagHelper.HintShortTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.HintElement)]
public class HintTagHelper : TagHelper
{
    /// <summary>
    /// Creates a <see cref="FormGroupHintTagHelper"/>.
    /// </summary>
    public HintTagHelper()
    {
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
            await output.GetChildContentAsync() :
            null;

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var formGroupContext = context.GetContextItem<FormGroupContext2>();

        formGroupContext.SetHint(output.Attributes.ToEncodedAttributeDictionary(), childContent?.ToHtmlString(), output.TagName);

        output.SuppressOutput();
    }
}

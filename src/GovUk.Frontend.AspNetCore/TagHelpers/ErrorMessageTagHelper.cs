using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an error message in a GDS form component.
/// </summary>
[HtmlTargetElement(TextInputTagHelper.ErrorMessageTagName, ParentTag = TextInputTagHelper.TagName)]
[HtmlTargetElement(TextInputTagHelper.ErrorMessageShortTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.ErrorMessageElement)]
public class ErrorMessageTagHelper : TagHelper
{
    private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

    /// <summary>
    /// Creates a <see cref="ErrorMessageTagHelper"/>.
    /// </summary>
    public ErrorMessageTagHelper()
    {
    }

    /// <summary>
    /// A visually hidden prefix used before the error message.
    /// </summary>
    /// <remarks>
    /// The default is <c>&quot;Error&quot;</c>.
    /// </remarks>
    [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
    public string? VisuallyHiddenText { get; set; }

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

        formGroupContext.SetErrorMessage(
            VisuallyHiddenText,
            output.Attributes.ToEncodedAttributeDictionary(),
            childContent?.ToHtmlString(),
            output.TagName);

        output.SuppressOutput();
    }
}

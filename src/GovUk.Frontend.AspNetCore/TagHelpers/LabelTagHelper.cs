using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the label in a GDS form component.
/// </summary>
[HtmlTargetElement(TextInputTagHelper.LabelTagName, ParentTag = TextInputTagHelper.TagName)]
[HtmlTargetElement(TextInputTagHelper.LabelShortTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.LabelElement)]
public class LabelTagHelper : TagHelper
{
    private const string IsPageHeadingAttributeName = "is-page-heading";

    /// <summary>
    /// Creates a <see cref="FormGroupLabelTagHelper"/>.
    /// </summary>
    public LabelTagHelper()
    {
    }

    /// <summary>
    /// Whether the label also acts as the heading for the page.
    /// </summary>
    /// <remarks>
    /// The default is <c>false</c>.
    /// </remarks>
    [HtmlAttributeName(IsPageHeadingAttributeName)]
    public bool IsPageHeading { get; set; } = ComponentGenerator.LabelDefaultIsPageHeading;

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

        formGroupContext.SetLabel(
            IsPageHeading,
            output.Attributes.ToEncodedAttributeDictionary(),
            childContent?.ToHtmlString(),
            output.TagName);

        output.SuppressOutput();
    }
}

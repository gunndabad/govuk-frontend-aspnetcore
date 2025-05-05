using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the label in a GDS form group component.
/// </summary>
[HtmlTargetElement(CharacterCountTagHelper.LabelTagName, ParentTag = CharacterCountTagHelper.TagName)]
[HtmlTargetElement(FileUploadTagHelper.LabelTagName, ParentTag = FileUploadTagHelper.TagName)]
[HtmlTargetElement(SelectTagHelper.LabelTagName, ParentTag = SelectTagHelper.TagName)]
[HtmlTargetElement(TextAreaTagHelper.LabelTagName, ParentTag = TextAreaTagHelper.TagName)]
[HtmlTargetElement(TextInputTagHelper.LabelTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.LabelElement)]
public class FormGroupLabelTagHelper : TagHelper
{
    private const string IsPageHeadingAttributeName = "is-page-heading";

    /// <summary>
    /// Creates a <see cref="FormGroupLabelTagHelper"/>.
    /// </summary>
    public FormGroupLabelTagHelper()
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

        if (context.TryGetContextItem<FormGroupContext>(out var formGroupContext))
        {
            formGroupContext.SetLabel(
                IsPageHeading,
                output.Attributes.ToAttributeDictionary(),
                childContent?.Snapshot());
        }
        else if (context.TryGetContextItem<FormGroupContext2>(out var formGroupContext2))
        {
            formGroupContext2.SetLabel(
                IsPageHeading,
                new EncodedAttributesDictionary(output.Attributes),
                childContent?.Snapshot(),
                output.TagName);
        }
        else if (context.TryGetContextItem<FormGroupContext3>(out var formGroupContext3))
        {
            formGroupContext3.SetLabel(
                IsPageHeading,
                new AttributeCollection(output.Attributes),
                childContent?.ToHtmlString(),
                output.TagName);
        }

        output.SuppressOutput();
    }
}

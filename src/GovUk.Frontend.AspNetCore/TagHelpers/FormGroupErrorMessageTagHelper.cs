using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the error message in a GDS form group component.
/// </summary>
[HtmlTargetElement(CheckboxesTagHelper.ErrorMessageTagName, ParentTag = CheckboxesTagHelper.TagName)]
[HtmlTargetElement(CheckboxesTagHelper.ErrorMessageTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
[HtmlTargetElement(RadiosTagHelper.ErrorMessageTagName, ParentTag = RadiosTagHelper.TagName)]
[HtmlTargetElement(RadiosTagHelper.ErrorMessageTagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
[HtmlTargetElement(SelectTagHelper.ErrorMessageTagName, ParentTag = SelectTagHelper.TagName)]
[HtmlTargetElement(TextAreaTagHelper.ErrorMessageTagName, ParentTag = TextAreaTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.ErrorMessageElement)]
public class FormGroupErrorMessageTagHelper : TagHelper
{
    private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

    /// <summary>
    /// Creates a <see cref="FormGroupErrorMessageTagHelper"/>.
    /// </summary>
    public FormGroupErrorMessageTagHelper()
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

        SetErrorMessage(childContent, context, output);

        output.SuppressOutput();
    }

    private protected virtual void SetErrorMessage(TagHelperContent? childContent, TagHelperContext context, TagHelperOutput output)
    {
        if (context.TryGetContextItem<FormGroupContext>(out var formGroupContext))
        {
            formGroupContext.SetErrorMessage(
                VisuallyHiddenText,
                output.Attributes.ToAttributeDictionary(),
                childContent?.Snapshot());
        }
        else if (context.TryGetContextItem<FormGroupContext3>(out var formGroupContext3))
        {
            formGroupContext3.SetErrorMessage(
                VisuallyHiddenText,
                new AttributeCollection(output.Attributes),
                childContent?.ToHtmlString(),
                output.TagName);
        }
    }
}

/// <summary>
/// Represents the error message in a GDS form group component.
/// </summary>
[HtmlTargetElement(CharacterCountTagHelper.ErrorMessageTagName, ParentTag = CharacterCountTagHelper.TagName)]
[HtmlTargetElement(FileUploadTagHelper.ErrorMessageTagName, ParentTag = FileUploadTagHelper.TagName)]
[HtmlTargetElement(TextInputTagHelper.ErrorMessageTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.ErrorMessageElement)]
public class FormGroupErrorMessageTagHelper3 : TagHelper
{
    private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

    /// <summary>
    /// Creates a <see cref="FormGroupErrorMessageTagHelper3"/>.
    /// </summary>
    public FormGroupErrorMessageTagHelper3()
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

        SetErrorMessage(childContent, context, output);

        output.SuppressOutput();
    }

    private protected virtual void SetErrorMessage(TagHelperContent? childContent, TagHelperContext context, TagHelperOutput output)
    {
        var formGroupContext3 = context.GetContextItem<FormGroupContext3>();

        formGroupContext3.SetErrorMessage(
            VisuallyHiddenText,
            new AttributeCollection(output.Attributes),
            childContent?.ToHtmlString(),
            output.TagName);
    }
}

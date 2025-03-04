using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the hint in a GDS form group component.
/// </summary>
[HtmlTargetElement(CheckboxesTagHelper.HintTagName, ParentTag = CheckboxesTagHelper.TagName)]
[HtmlTargetElement(CheckboxesTagHelper.HintTagName, ParentTag = CheckboxesFieldsetTagHelper.TagName)]
[HtmlTargetElement(CharacterCountTagHelper.HintTagName, ParentTag = CharacterCountTagHelper.TagName)]
[HtmlTargetElement(DateInputTagHelper.HintTagName, ParentTag = DateInputTagHelper.TagName)]
[HtmlTargetElement(DateInputTagHelper.HintTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
[HtmlTargetElement(FileUploadTagHelper.HintTagName, ParentTag = FileUploadTagHelper.TagName)]
[HtmlTargetElement(RadiosTagHelper.HintTagName, ParentTag = RadiosTagHelper.TagName)]
[HtmlTargetElement(RadiosTagHelper.HintTagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
[HtmlTargetElement(SelectTagHelper.HintTagName, ParentTag = SelectTagHelper.TagName)]
[HtmlTargetElement(TextAreaTagHelper.HintTagName, ParentTag = TextAreaTagHelper.TagName)]
[HtmlTargetElement(TextInputTagHelper.HintTagName, ParentTag = TextInputTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.HintElement)]
public class FormGroupHintTagHelper : TagHelper
{
    /// <summary>
    /// Creates a <see cref="FormGroupHintTagHelper"/>.
    /// </summary>
    public FormGroupHintTagHelper()
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

        if (context.TryGetContextItem<FormGroupContext>(out var formGroupContext))
        {
            formGroupContext.SetHint(output.Attributes.ToAttributeDictionary(), childContent?.Snapshot());
        }
        else if (context.TryGetContextItem<FormGroupContext2>(out var formGroupContext2))
        {
            formGroupContext2.SetHint(
                new EncodedAttributesDictionary(output.Attributes),
                childContent?.Snapshot(),
                output.TagName);
        }

        output.SuppressOutput();
    }
}

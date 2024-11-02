using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <inheritdoc/>
[HtmlTargetElement(DateInputTagHelper.ErrorMessageTagName, ParentTag = DateInputTagHelper.TagName)]
[HtmlTargetElement(DateInputTagHelper.ErrorMessageTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
[HtmlTargetElement(DateInputTagHelper.ErrorMessageTagName, ParentTag = DateInputFieldsetTagHelper.ShortTagName)]
[HtmlTargetElement(ShortTagNames.ErrorMessage, ParentTag = DateInputTagHelper.TagName)]
[HtmlTargetElement(ShortTagNames.ErrorMessage, ParentTag = DateInputFieldsetTagHelper.TagName)]
[HtmlTargetElement(ShortTagNames.ErrorMessage, ParentTag = DateInputFieldsetTagHelper.ShortTagName)]
[OutputElementHint(ComponentGenerator.ErrorMessageElement)]
public class DateInputErrorMessageTagHelper : FormGroupErrorMessageTagHelper
{
    private const string ErrorFieldsAttributeName = "error-fields";

    /// <summary>
    /// The fields of the date that have errors (day, month and/or year).
    /// </summary>
    /// <remarks>
    /// If the value for the parent <see cref="DateInputTagHelper"/> was specified using <see cref="FormGroupTagHelperBase.AspFor"/>
    /// then <see cref="ErrorFields"/> will be computed from model binding errors.
    /// </remarks>
    [HtmlAttributeName(ErrorFieldsAttributeName)]
    public DateInputErrorFields? ErrorFields { get; set; }

    private protected override void SetErrorMessage(TagHelperContent? childContent, TagHelperContext context, TagHelperOutput output)
    {
        var dateInputContext = context.GetContextItem<DateInputContext>();

        dateInputContext.SetErrorMessage(
            ErrorFields,
            VisuallyHiddenText,
            output.Attributes.ToEncodedAttributeDictionary(),
            childContent?.ToHtmlString(),
            output.TagName);
    }
}

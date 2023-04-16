using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <inheritdoc/>
    [HtmlTargetElement(DateInputTagHelper.ErrorMessageTagName, ParentTag = DateInputTagHelper.TagName)]
    [HtmlTargetElement(DateInputTagHelper.ErrorMessageTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
    [HtmlTargetElement(DateInputTagHelper.ErrorMessageTagName, ParentTag = DateInputFieldsetTagHelper.ShortTagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = DateInputTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = DateInputFieldsetTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.ErrorMessageElement)]
    public class DateInputErrorMessageTagHelper : FormGroupErrorMessageTagHelper
    {
        private const string ErrorItemsAttributeName = "error-items";

        /// <summary>
        /// The components of the date that have errors (day, month and/or year).
        /// </summary>
        /// <remarks>
        /// If the value for the parent <see cref="DateInputTagHelper"/> was specified using <see cref="FormGroupTagHelperBase.AspFor"/>
        /// then <see cref="ErrorItems"/> will be computed from model binding errors.
        /// </remarks>
        [HtmlAttributeName(ErrorItemsAttributeName)]
        public DateInputErrorComponents? ErrorItems { get; set; }

        private protected override void SetErrorMessage(TagHelperContent? childContent, TagHelperContext context, TagHelperOutput output)
        {
            var dateInputContext = context.GetContextItem<DateInputContext>();

            dateInputContext.SetErrorMessage(
                ErrorItems,
                VisuallyHiddenText,
                output.Attributes.ToAttributeDictionary(),
                childContent?.Snapshot());
        }
    }
}

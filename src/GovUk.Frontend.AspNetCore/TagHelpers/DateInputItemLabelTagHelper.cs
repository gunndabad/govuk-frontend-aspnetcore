using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the label in a GDS date input component item.
/// </summary>
[HtmlTargetElement(DayTagName, ParentTag = DateInputItemTagHelper.DayTagName)]
[HtmlTargetElement(DayTagName, ParentTag = DateInputItemTagHelper.DayShortTagName)]
[HtmlTargetElement(MonthTagName, ParentTag = DateInputItemTagHelper.MonthTagName)]
[HtmlTargetElement(MonthTagName, ParentTag = DateInputItemTagHelper.MonthShortTagName)]
[HtmlTargetElement(YearTagName, ParentTag = DateInputItemTagHelper.YearTagName)]
[HtmlTargetElement(YearTagName, ParentTag = DateInputItemTagHelper.YearShortTagName)]
[HtmlTargetElement(ShortTagNames.Label, ParentTag = DateInputItemTagHelper.DayTagName)]
[HtmlTargetElement(ShortTagNames.Label, ParentTag = DateInputItemTagHelper.DayShortTagName)]
[HtmlTargetElement(ShortTagNames.Label, ParentTag = DateInputItemTagHelper.MonthTagName)]
[HtmlTargetElement(ShortTagNames.Label, ParentTag = DateInputItemTagHelper.MonthShortTagName)]
[HtmlTargetElement(ShortTagNames.Label, ParentTag = DateInputItemTagHelper.YearTagName)]
[HtmlTargetElement(ShortTagNames.Label, ParentTag = DateInputItemTagHelper.YearShortTagName)]
[OutputElementHint(ComponentGenerator.LabelElement)]
public class DateInputItemLabelTagHelper : TagHelper
{
    internal const string DayTagName = "govuk-date-input-day-label";
    internal const string MonthTagName = "govuk-date-input-month-label";
    internal const string YearTagName = "govuk-date-input-year-label";

    /// <summary>
    /// Creates a <see cref="DateInputItemLabelTagHelper"/>.
    /// </summary>
    public DateInputItemLabelTagHelper()
    {
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var dateInputItemContext = context.GetContextItem<DateInputItemContext>();

        var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
            (await output.GetChildContentAsync()).Snapshot() :
            null;

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        dateInputItemContext.SetLabel(output.Attributes.ToEncodedAttributeDictionary(), childContent?.ToHtmlString());

        output.SuppressOutput();
    }
}

using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the label in a GDS date input component item.
/// </summary>
[HtmlTargetElement(DayTagName, ParentTag = DateInputItemTagHelper.DayTagName)]
[HtmlTargetElement(MonthTagName, ParentTag = DateInputItemTagHelper.MonthTagName)]
[HtmlTargetElement(YearTagName, ParentTag = DateInputItemTagHelper.YearTagName)]
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

        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        dateInputItemContext.SetLabel(childContent.Snapshot(), output.Attributes.ToAttributeDictionary());

        output.SuppressOutput();
    }
}

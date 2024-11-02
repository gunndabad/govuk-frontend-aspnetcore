using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the fieldset in a GDS date input component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = DateInputTagHelper.TagName)]
[HtmlTargetElement(ShortTagName, ParentTag = DateInputTagHelper.TagName)]
[RestrictChildren(
    DateInputFieldsetLegendTagHelper.TagName,
    DateInputFieldsetLegendTagHelper.ShortTagName,
    DateInputTagHelper.HintTagName,
    DateInputTagHelper.HintShortTagName,
    DateInputTagHelper.ErrorMessageTagName,
    DateInputTagHelper.ErrorMessageShortTagName,
    DateInputItemTagHelper.DayTagName,
    DateInputItemTagHelper.DayShortTagName,
    DateInputItemTagHelper.MonthTagName,
    DateInputItemTagHelper.MonthShortTagName,
    DateInputItemTagHelper.YearTagName,
    DateInputItemTagHelper.YearShortTagName)]
[OutputElementHint(ComponentGenerator.FieldsetElement)]
public class DateInputFieldsetTagHelper : TagHelper
{
    internal const string TagName = "govuk-date-input-fieldset";
    internal const string ShortTagName = ShortTagNames.Fieldset;

    private const string DescribedByAttributeName = "described-by";

    /// <summary>
    /// Creates a <see cref="DateInputFieldsetTagHelper"/>.
    /// </summary>
    public DateInputFieldsetTagHelper()
    {
    }

    /// <summary>
    /// One or more element IDs to add to the <c>aria-describedby</c> attribute.
    /// </summary>
    [HtmlAttributeName(DescribedByAttributeName)]
    public string? DescribedBy { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var dateInputContext = context.GetContextItem<DateInputContext>();
        dateInputContext.OpenFieldset(output.TagName);

        var fieldsetContext = new DateInputFieldsetContext(output.Attributes.ToEncodedAttributeDictionary(), dateInputContext._for, DescribedBy);

        using (context.SetScopedContextItem(fieldsetContext))
        {
            await output.GetChildContentAsync();
        }

        fieldsetContext.ThrowIfNotComplete();
        dateInputContext.CloseFieldset(fieldsetContext);

        output.SuppressOutput();
    }
}

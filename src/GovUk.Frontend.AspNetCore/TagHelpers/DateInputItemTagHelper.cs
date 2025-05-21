using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an item in a GDS date input component.
/// </summary>
[HtmlTargetElement(DayTagName, ParentTag = DateInputTagHelper.TagName)]
[HtmlTargetElement(DayTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
[HtmlTargetElement(MonthTagName, ParentTag = DateInputTagHelper.TagName)]
[HtmlTargetElement(MonthTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
[HtmlTargetElement(YearTagName, ParentTag = DateInputTagHelper.TagName)]
[HtmlTargetElement(YearTagName, ParentTag = DateInputFieldsetTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.FormGroupElement)]
[RestrictChildren(DateInputItemLabelTagHelper.DayTagName, DateInputItemLabelTagHelper.MonthTagName, DateInputItemLabelTagHelper.YearTagName)]
public class DateInputItemTagHelper : TagHelper
{
    internal const string DayTagName = "govuk-date-input-day";
    internal const string MonthTagName = "govuk-date-input-month";
    internal const string YearTagName = "govuk-date-input-year";

    private const string AutoCompleteAttributeName = "autocomplete";
    private const string IdAttributeName = "id";
    private const string InputModeAttributeName = "inputmode";
    private const string NameAttributeName = "name";
    private const string PatternAttributeName = "pattern";
    private const string ValueAttributeName = "value";

    private int? _value;
    private bool _valueSpecified = false;

    /// <summary>
    /// Creates a <see cref="DateInputItemTagHelper"/>.
    /// </summary>
    public DateInputItemTagHelper()
    {
    }

    /// <summary>
    /// The <c>autocomplete</c> attribute for the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(AutoCompleteAttributeName)]
    public string? AutoComplete { get; set; }

    /// <summary>
    /// The <c>id</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// By default the value will be generated from the parent's <see cref="DateInputTagHelper.Id"/>.
    /// </remarks>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// The <c>inputmode</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// The default is <c>numeric</c>.
    /// </remarks>
    [HtmlAttributeName(InputModeAttributeName)]
    public string? InputMode { get; set; } = ComponentGenerator.DateInputDefaultInputMode;

    /// <summary>
    /// The <c>name</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// By default the value will be generated from the parent's <see cref="FormGroupTagHelperBase.AspFor"/> and/or <see cref="DateInputTagHelper.NamePrefix"/>.
    /// </remarks>
    [HtmlAttributeName(NameAttributeName)]
    public string? Name { get; set; }

    /// <summary>
    /// The <c>pattern</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// The default is <c>[0-9]*</c>.
    /// </remarks>
    [HtmlAttributeName(PatternAttributeName)]
    public string? Pattern { get; set; }

    /// <summary>
    /// The <c>value</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// This cannot be specified if the <see cref="DateInputTagHelper.Value"/> property on the parent is also specified.
    /// </remarks>
    [HtmlAttributeName(ValueAttributeName)]
    public int? Value
    {
        get => _value;
        set
        {
            _value = value;
            _valueSpecified = true;
        }
    }

    /// <summary>
    /// Gets the <see cref="ViewContext"/> of the executing view.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    [DisallowNull]
    public ViewContext? ViewContext { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var dateInputContext = context.GetContextItem<DateInputContext>();
        var dateInputItemContext = new DateInputItemContext(output.TagName, labelTagName: output.TagName + "-label");

        using (context.SetScopedContextItem(dateInputItemContext))
        {
            await output.GetChildContentAsync();
        }

        var itemType = DateInputContext.GetItemTypeFromTagName(output.TagName);

        var itemContext = new DateInputContextItem()
        {
            Attributes = output.Attributes.ToAttributeDictionary(),
            AutoComplete = AutoComplete,
            Id = Id,
            InputMode = InputMode,
            LabelContent = dateInputItemContext.Label?.Content,
            LabelAttributes = dateInputItemContext.Label?.Attributes,
            Name = Name,
            Pattern = Pattern,
            Value = _value,
            ValueSpecified = _valueSpecified
        };

        dateInputContext.SetItem(itemType, itemContext);

        output.SuppressOutput();
    }
}

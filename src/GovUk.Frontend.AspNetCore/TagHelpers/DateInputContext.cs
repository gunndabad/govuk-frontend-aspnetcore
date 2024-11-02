using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal enum DateInputItemType
{
    Day = 0,
    Month = 1,
    Year = 2
}

internal class DateInputContext : FormGroupContext2
{
    private readonly SortedDictionary<DateInputItemType, (DateInputContextItem ItemContext, string TagName)> _items;
    private readonly bool _haveExplicitValue;
    public readonly ModelExpression? _for;
    public DateInputFieldsetContext? _fieldset;
    private bool _fieldsetIsOpen;
    private string? _fieldsetTagName;

    public DateInputContext(bool haveExplicitValue, ModelExpression? @for)
    {
        _items = [];
        _haveExplicitValue = haveExplicitValue;
        _for = @for;
    }

    public IReadOnlyDictionary<DateInputItemType, DateInputContextItem> Items => _items.ToDictionary(i => i.Key, i => i.Value.ItemContext);

    public DateInputErrorFields? ErrorFields { get; private set; }

    protected override IReadOnlyCollection<string> ErrorMessageTagNames => [DateInputTagHelper.ErrorMessageShortTagName, DateInputTagHelper.ErrorMessageTagName];

    protected override IReadOnlyCollection<string> HintTagNames => [DateInputTagHelper.HintShortTagName, DateInputTagHelper.HintTagName];

    protected override IReadOnlyCollection<string> LabelTagNames => throw new NotSupportedException();

    protected override string RootTagName => DateInputTagHelper.TagName;

    private IReadOnlyCollection<string> FieldsetTagNames => [DateInputFieldsetTagHelper.ShortTagName, DateInputFieldsetTagHelper.TagName];

    private IReadOnlyCollection<string> ItemTagNames => [
        DateInputItemTagHelper.DayTagName,
        DateInputItemTagHelper.DayShortTagName,
        DateInputItemTagHelper.MonthTagName,
        DateInputItemTagHelper.MonthShortTagName,
        DateInputItemTagHelper.YearTagName,
        DateInputItemTagHelper.YearShortTagName];

    public FieldsetOptions? GetFieldsetOptions(IModelHelper modelHelper) => _fieldset?.GetFieldsetOptions(modelHelper);

    public override LabelOptions GetLabelOptions(
        ModelExpression? aspFor,
        ViewContext viewContext,
        IModelHelper modelHelper,
        string inputId,
        string aspForAttributeName)
    {
        throw new NotSupportedException();
    }

    public void OpenFieldset(string tagName)
    {
        if (_fieldsetIsOpen)
        {
            throw ExceptionHelper.CannotBeNestedInsideAnother(tagName, FieldsetTagNames);
        }

        if (_fieldset != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(FieldsetTagNames, RootTagName);
        }

        if (Items.Count > 0 || Hint != null || ErrorMessage != null)
        {
            throw new InvalidOperationException($"<{tagName}> must be the only direct child of the <{RootTagName}>.");
        }

        _fieldsetIsOpen = true;
        _fieldsetTagName = tagName;
    }

    public void CloseFieldset(DateInputFieldsetContext fieldsetContext)
    {
        if (!_fieldsetIsOpen)
        {
            throw new InvalidOperationException("Fieldset has not been opened.");
        }

        _fieldsetIsOpen = false;
        _fieldset = fieldsetContext;
    }

    public void SetItem(DateInputItemType itemType, DateInputContextItem item, string tagName)
    {
        Guard.ArgumentNotNull(nameof(item), item);

        if (_haveExplicitValue && item.ValueSpecified)
        {
            throw new InvalidOperationException($"Value cannot be specified for both <{tagName}> and the parent <{RootTagName}>.");
        }

        if (_fieldset != null)
        {
            throw ExceptionHelper.MustBeInside(tagName, FieldsetTagNames);
        }

        if (_items.Count != 0)
        {
            if (_items.ContainsKey(itemType))
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(tagName, _fieldset is not null ? _fieldsetTagName! : RootTagName);
            }

            var subsequentItems = _items.Where(kvp => kvp.Key > itemType).ToArray();

            if (subsequentItems.Length != 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    tagName,
                    subsequentItems[0].Value.TagName);
            }
        }

        _items.Add(itemType, (item, tagName));
    }

    public void SetErrorMessage(
        DateInputErrorFields? errorFields,
        string? visuallyHiddenText,
        ImmutableDictionary<string, string?> attributes,
        string? html,
        string tagName)
    {
        if (_fieldset != null)
        {
            throw ExceptionHelper.MustBeInside(tagName, FieldsetTagNames);
        }

        if (Items.Count > 0)
        {
            var firstItemTagName = _items.First().Value.TagName;
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, firstItemTagName);
        }

        ErrorFields = errorFields;

        base.SetErrorMessage(visuallyHiddenText, attributes, html, tagName);
    }

    public override void SetErrorMessage(string? visuallyHiddenText, ImmutableDictionary<string, string?> attributes, string? html, string tagName)
    {
        throw new NotSupportedException($"Use the overload that takes a {nameof(DateInputErrorFields)} argument too.");
    }

    public override void SetHint(ImmutableDictionary<string, string?> attributes, string? html, string tagName)
    {
        if (_fieldset != null)
        {
            throw ExceptionHelper.MustBeInside(tagName, FieldsetTagNames);
        }

        if (Items.Count > 0)
        {
            var firstItemTagName = _items.First().Value.TagName;
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, firstItemTagName);
        }

        base.SetHint(attributes, html, tagName);
    }

    public override void SetLabel(
        bool isPageHeading,
        ImmutableDictionary<string, string?> attributes,
        string? html,
        string tagName)
    {
        throw new NotSupportedException();
    }

    internal static string GetDefaultLabelText(DateInputItemType itemType) => itemType switch
    {
        DateInputItemType.Day => "Day",
        DateInputItemType.Month => "Month",
        DateInputItemType.Year => "Year",
        _ => throw new ArgumentException($"Unknown {nameof(DateInputItemType)}: '{itemType}'.", nameof(itemType))
    };

    internal static DateInputItemType GetItemTypeFromTagName(string tagName) => tagName switch
    {
        DateInputItemTagHelper.DayTagName => DateInputItemType.Day,
        DateInputItemTagHelper.DayShortTagName => DateInputItemType.Day,
        DateInputItemTagHelper.MonthTagName => DateInputItemType.Month,
        DateInputItemTagHelper.MonthShortTagName => DateInputItemType.Month,
        DateInputItemTagHelper.YearTagName => DateInputItemType.Year,
        DateInputItemTagHelper.YearShortTagName => DateInputItemType.Year,
        _ => throw new ArgumentException($"Unknown tag name: '{tagName}'.", nameof(tagName))
    };
}

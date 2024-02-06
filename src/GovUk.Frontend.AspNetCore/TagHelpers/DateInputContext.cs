using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
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
    private readonly SortedDictionary<DateInputItemType, DateInputContextItem> _items;
    private readonly bool _haveValue;
    public readonly ModelExpression? _aspFor;
    public DateInputFieldsetContext? _fieldset;
    private bool _fieldsetIsOpen;

    public DateInputContext(bool haveExplicitValue, ModelExpression? aspFor)
    {
        _items = new SortedDictionary<DateInputItemType, DateInputContextItem>();
        _haveValue = haveExplicitValue;
        _aspFor = aspFor;
    }

    public IReadOnlyDictionary<DateInputItemType, DateInputContextItem> Items => _items;

    public DateInputErrorComponents? ErrorComponents { get; private set; }

    protected override string ErrorMessageTagName => DateInputTagHelper.ErrorMessageTagName;

    protected string FieldsetTagName => DateInputFieldsetTagHelper.TagName;

    protected override string HintTagName => DateInputTagHelper.HintTagName;

    protected override string LabelTagName => throw new NotSupportedException();

    protected override string RootTagName => DateInputTagHelper.TagName;

    public FieldsetOptions? GetFieldsetOptions(IModelHelper modelHelper) => _fieldset?.GetFieldsetOptions(_aspFor, modelHelper);

    public override LabelOptions GetLabelOptions(
        ModelExpression? aspFor,
        IModelHelper modelHelper,
        string inputId,
        string aspForAttributeName)
    {
        throw new NotSupportedException();
    }

    public void OpenFieldset()
    {
        if (_fieldsetIsOpen)
        {
            throw new InvalidOperationException($"<{FieldsetTagName}> cannot be nested inside another <{FieldsetTagName}>.");
        }

        if (_fieldset != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(FieldsetTagName, RootTagName);
        }

        if (Items.Count > 0 || Hint != null || ErrorMessage != null)
        {
            throw new InvalidOperationException($"<{FieldsetTagName}> must be the only direct child of the <{RootTagName}>.");
        }

        _fieldsetIsOpen = true;
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

    public void SetItem(DateInputItemType itemType, DateInputContextItem item)
    {
        Guard.ArgumentNotNull(nameof(item), item);

        if (_haveValue && item.ValueSpecified)
        {
            throw new InvalidOperationException($"Value cannot be specified for both <{GetTagNameForItemType(itemType)}> and the parent <{RootTagName}>.");
        }

        var tagName = GetTagNameForItemType(itemType);

        if (_fieldset != null)
        {
            throw new InvalidOperationException($"<{tagName}> must be inside <{FieldsetTagName}>.");
        }

        if (_items.Count != 0)
        {
            if (_items.ContainsKey(itemType))
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(tagName, DateInputTagHelper.TagName);
            }

            var subsequentItems = _items.Where(kvp => kvp.Key > itemType).ToArray();

            if (subsequentItems.Length != 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    tagName,
                    GetTagNameForItemType(subsequentItems[0].Key));
            }
        }

        _items.Add(itemType, item);
    }

    public void SetErrorMessage(
        DateInputErrorComponents? errorComponents,
        string? visuallyHiddenText,
        ImmutableDictionary<string, string?> attributes,
        string? html)
    {
        if (_fieldset != null)
        {
            throw new InvalidOperationException($"<{ErrorMessageTagName}> must be inside <{FieldsetTagName}>.");
        }

        if (Items.Count > 0)
        {
            var firstItemTagName = GetTagNameForItemType(_items.First().Key);
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(ErrorMessageTagName, firstItemTagName);
        }

        ErrorComponents = errorComponents;

        base.SetErrorMessage(visuallyHiddenText, attributes, html);
    }

    public override void SetErrorMessage(string? visuallyHiddenText, ImmutableDictionary<string, string?> attributes, string? html)
    {
        throw new NotSupportedException($"Use the overload that takes a {nameof(DateInputErrorComponents)} argument too.");
    }

    public override void SetHint(ImmutableDictionary<string, string?> attributes, string? html)
    {
        if (_fieldset != null)
        {
            throw new InvalidOperationException($"<{HintTagName}> must be inside <{FieldsetTagName}>.");
        }

        if (Items.Count > 0)
        {
            var firstItemTagName = GetTagNameForItemType(_items.First().Key);
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(HintTagName, firstItemTagName);
        }

        base.SetHint(attributes, html);
    }

    public override void SetLabel(
        bool isPageHeading,
        ImmutableDictionary<string, string?> attributes,
        string? html)
    {
        throw new NotSupportedException();
    }

    internal static string GetDefaultLabelTextFromTagName(string tagName) => tagName switch
    {
        DateInputItemTagHelper.DayTagName => "Day",
        DateInputItemTagHelper.MonthTagName => "Month",
        DateInputItemTagHelper.YearTagName => "Year",
        _ => throw new ArgumentException($"Unknown tag name: '{tagName}'.", nameof(tagName))
    };

    internal static DateInputItemType GetItemTypeFromTagName(string tagName) => tagName switch
    {
        DateInputItemTagHelper.DayTagName => DateInputItemType.Day,
        DateInputItemTagHelper.MonthTagName => DateInputItemType.Month,
        DateInputItemTagHelper.YearTagName => DateInputItemType.Year,
        _ => throw new ArgumentException($"Unknown tag name: '{tagName}'.", nameof(tagName))
    };

    internal static string GetTagNameForItemType(DateInputItemType itemType) => itemType switch
    {
        DateInputItemType.Day => DateInputItemTagHelper.DayTagName,
        DateInputItemType.Month => DateInputItemTagHelper.MonthTagName,
        DateInputItemType.Year => DateInputItemTagHelper.YearTagName,
        _ => throw new ArgumentException($"Unknown item type: '{itemType}'.", nameof(itemType))
    };
}

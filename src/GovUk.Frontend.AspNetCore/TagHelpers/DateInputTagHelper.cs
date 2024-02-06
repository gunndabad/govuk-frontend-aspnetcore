using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS date input component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(
    DateInputFieldsetTagHelper.TagName,
    HintTagName,
    ErrorMessageTagName,
    DateInputItemTagHelper.DayTagName,
    DateInputItemTagHelper.MonthTagName,
    DateInputItemTagHelper.YearTagName)]
[OutputElementHint(DefaultComponentGenerator.FormGroupElement)]
public class DateInputTagHelper : TagHelper
{
    internal const string ErrorMessageTagName = "govuk-date-input-error-message";
    internal const string HintTagName = "govuk-date-input-hint";
    internal const string TagName = "govuk-date-input";

    private const string AspForAttributeName = "asp-for";
    private const string DateInputAttributesPrefix = "date-input-";
    private const string DisabledAttributeName = "disabled";
    private const string IdAttributeName = "id";
    private const string NamePrefixAttributeName = "name-prefix";
    private const string ValueAttributeName = "value";

    private readonly IComponentGenerator _componentGenerator;
    private readonly GovUkFrontendAspNetCoreOptions _options;
    private readonly DateInputParseErrorsProvider _dateInputParseErrorsProvider;
    private readonly IModelHelper _modelHelper;

    private object? _value;
    private bool _valueSpecified = false;

    /// <summary>
    /// Creates a <see cref="DateInputTagHelper"/>.
    /// </summary>
    public DateInputTagHelper(
        IComponentGenerator componentGenerator,
        IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor,
        DateInputParseErrorsProvider dateInputParseErrorsProvider)
        : this(componentGenerator, optionsAccessor, dateInputParseErrorsProvider, modelHelper: new DefaultModelHelper())
    {
    }

    internal DateInputTagHelper(
        IComponentGenerator componentGenerator,
        IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor,
        DateInputParseErrorsProvider dateInputParseErrorsProvider,
        IModelHelper modelHelper)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        ArgumentNullException.ThrowIfNull(dateInputParseErrorsProvider);
        ArgumentNullException.ThrowIfNull(modelHelper);
        _options = optionsAccessor.Value;
        _dateInputParseErrorsProvider = dateInputParseErrorsProvider;
        _componentGenerator = componentGenerator;
        _modelHelper = modelHelper;
    }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(AspForAttributeName)]
    public ModelExpression? AspFor { get; set; }

    /// <summary>
    /// Additional attributes for the container element that wraps the items.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = DateInputAttributesPrefix)]
    public IDictionary<string, string?>? DateInputAttributes { get; set; } = new Dictionary<string, string?>();

    /// <summary>
    /// Whether the <c>disabled</c> attribute should be added to the generated <c>input</c> elements.
    /// </summary>
    [HtmlAttributeName(DisabledAttributeName)]
    public bool? Disabled { get; set; }

    /// <summary>
    /// The <c>id</c> attribute for the main component.
    /// </summary>
    /// <remarks>
    /// Also used to generate an <c>id</c> for each item's <c>input</c> when
    /// the corresponding <see cref="DateInputItemTagHelper.Id"/> is not specified.
    /// </remarks>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// Optional prefix for the <c>name</c> attribute on each item's <c>input</c>.
    /// </summary>
    [HtmlAttributeName(NamePrefixAttributeName)]
    public string? NamePrefix { get; set; }

    /// <summary>
    /// The date to populate the item values with.
    /// </summary>
    [HtmlAttributeName(ValueAttributeName)]
    public object? Value
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
        var dateInputContext = new DateInputContext(haveExplicitValue: _valueSpecified, AspFor);

        using (context.SetScopedContextItem(dateInputContext))
        using (context.SetScopedContextItem(typeof(FormGroupContext2), dateInputContext))
        {
            await output.GetChildContentAsync();
        }

        var namePrefix = ResolveNamePrefix();
        var idPrefix = ResolveIdPrefix();
        var fieldsetOptions = dateInputContext.GetFieldsetOptions(_modelHelper);
        var hintOptions = dateInputContext.GetHintOptions(AspFor, _modelHelper);
        var errorMessageOptions = dateInputContext.GetErrorMessageOptions(AspFor, ViewContext!, _modelHelper);
        var items = ResolveItems(dateInputContext, namePrefix, idPrefix).ToArray();

        var formGroupAttributes = output.Attributes.ToEncodedAttributeDictionary()
            .Remove("class", out var formGroupClasses);
        var formGroupOptions = new FormGroupOptions()
        {
            Attributes = formGroupAttributes,
            Classes = formGroupClasses
        };

        var attributes = DateInputAttributes!.ToImmutableDictionary()
            .Remove("class", out var classes);

        if (errorMessageOptions is not null)
        {
            classes ??= "";
            classes += " govuk-input--error";

            formGroupOptions.Classes += " govuk-form-group--error";
        }

        // Don't populate namePrefix here since the template uses a '-' separator and we need to use '.' to make model binding work.
        var component = _componentGenerator.GenerateDateInput(new DateInputOptions()
        {
            Id = idPrefix,
            NamePrefix = null,
            Items = items,
            Hint = hintOptions,
            ErrorMessage = errorMessageOptions,
            FormGroup = formGroupOptions,
            Fieldset = fieldsetOptions,
            Classes = classes,
            Attributes = attributes
        });

        output.TagName = null;
        output.Attributes.Clear();
        output.Content.AppendHtml(component.ToHtmlString());

        if (errorMessageOptions is not null && context.TryGetContextItem<FormErrorContext>(out var formErrorContext))
        {
            Debug.Assert(errorMessageOptions.Html is not null);

            var errorItems = GetErrorComponents(dateInputContext);
            Debug.Assert(errorItems != DateInputErrorComponents.None);

            string firstErrorFieldId = errorItems.HasFlag(DateInputErrorComponents.Day) ? items[0].Id! :
                errorItems.HasFlag(DateInputErrorComponents.Month) ? items[1].Id! :
                items[2].Id!;

            formErrorContext.AddError(errorMessageOptions.Html!, "#" + firstErrorFieldId);
        }
    }

    private string ResolveIdPrefix()
    {
        if (AspFor == null && Id == null)
        {
            ThrowHelper.AtLeastOneOfAttributesMustBeSpecified(AspForAttributeName, IdAttributeName);
        }

        return Id ??
            TagBuilder.CreateSanitizedId(
                _modelHelper.GetFullHtmlFieldName(ViewContext!, AspFor!.Name),
                Constants.IdAttributeDotReplacement);
    }

    private string? ResolveNamePrefix()
    {
        return NamePrefix is not null ? NamePrefix :
            AspFor is not null ? _modelHelper.GetFullHtmlFieldName(ViewContext!, AspFor.Name) :
            null;
    }

    private IEnumerable<DateInputOptionsItem> ResolveItems(DateInputContext dateInputContext, string? namePrefix, string idPrefix)
    {
        var resolvedNamePrefix = namePrefix is not null ? namePrefix + "." : "";

        var dateInputModelConverters = _options.DateInputModelConverters;

        var valueAsDate = GetValueAsDate() ?? (AspFor != null ? GetValueFromModel() : null);
        var errorItems = GetErrorComponents(dateInputContext);

        yield return CreateDateInputItem(
            getComponentFromValue: date => date?.Day.ToString(),
            defaultName: DateInputModelConverterModelBinder.DayComponentName,
            defaultLabelText: "Day",
            DateInputErrorComponents.Day,
            contextItem: dateInputContext.Items.GetValueOrDefault(DateInputItemType.Day));

        yield return CreateDateInputItem(
            getComponentFromValue: date => date?.Month.ToString(),
            defaultName: DateInputModelConverterModelBinder.MonthComponentName,
            defaultLabelText: "Month",
            DateInputErrorComponents.Month,
            contextItem: dateInputContext.Items.GetValueOrDefault(DateInputItemType.Month));

        yield return CreateDateInputItem(
            getComponentFromValue: date => date?.Year.ToString(),
            defaultName: DateInputModelConverterModelBinder.YearComponentName,
            defaultLabelText: "Year",
            DateInputErrorComponents.Year,
            contextItem: dateInputContext.Items.GetValueOrDefault(DateInputItemType.Year));

        DateInputOptionsItem CreateDateInputItem(
            Func<DateOnly?, string?> getComponentFromValue,
            string defaultName,
            string defaultLabelText,
            DateInputErrorComponents errorSource,
            DateInputContextItem? contextItem)
        {
            var itemName = resolvedNamePrefix + (contextItem?.Name ?? defaultName);

            // Resolve the ID here rather than relying on the component generator to do it
            // to simplify generating fragment links to the error field(s)
            var itemId = contextItem?.Id ?? (idPrefix + "-" + itemName);

            var itemLabelHtml = contextItem?.LabelHtml ??
                HtmlEncoder.Default.Encode(defaultLabelText);

            // Value resolution hierarchy:
            //   if Value has been set on a child tag helper e.g. <govuk-date-input-day /> then use that;
            //   if Value property is specified here i.e. <govuk-date-input /> then use that;
            //   if AspFor is specified use value from ModelState;
            //   otherwise empty.
            var itemValue = contextItem?.ValueSpecified == true ? (contextItem.Value?.ToString() ?? string.Empty) :
                _valueSpecified ? getComponentFromValue(valueAsDate) :
                contextItem?.ValueSpecified == true ? contextItem.Value?.ToString() :
                AspFor != null ? GetValueFromModelState() :
                null;

            var attributes = (contextItem?.Attributes ?? ImmutableDictionary<string, string?>.Empty)
                .Remove("classes", out var classes);

            if (errorItems.HasFlag(errorSource))
            {
                classes ??= "";
                classes += " govuk-input--error";
            }

            return new DateInputOptionsItem()
            {
                Id = itemId,
                Name = itemName,
                Label = null,
                LabelHtml = itemLabelHtml,
                LabelAttributes = contextItem?.LabelAttributes,
                Value = itemValue,
                Autocomplete = contextItem?.Autocomplete,
                InputMode = contextItem?.InputMode,
                Pattern = contextItem?.Pattern,
                Attributes = attributes,
                Classes = classes
            };

            string? GetValueFromModelState()
            {
                Debug.Assert(AspFor != null);

                // Can't use ModelHelper.GetModelValue here;
                // custom Date types may not expose components via Day/Month/Year properties.
                // Resolution works as follows:
                //   Look for a ModelStateEntry for $"{AspFor.Name}.{defaultName}" and, if found, use its AttemptedValue.
                //   (Using defaultName is always correct here; that lines up with what the ModelBinder uses).
                //   If that fails, get the model and convert it to a Date using options.DateInputModelConverters.
                //   From that extract the Day/Month/Year.

                var expression = $"{AspFor.Name}.{defaultName}";
                var modelStateKey = _modelHelper.GetFullHtmlFieldName(ViewContext!, expression);

                if (ViewContext!.ModelState.TryGetValue(modelStateKey, out var modelStateEntry) &&
                    modelStateEntry.AttemptedValue != null)
                {
                    return modelStateEntry.AttemptedValue;
                }

                return getComponentFromValue(valueAsDate);
            }
        }

        DateOnly? GetValueAsDate()
        {
            if (Value is null)
            {
                return null;
            }

            var valueType = Value.GetType();
            var dateInputModelConverters = _options.DateInputModelConverters;

            foreach (var converter in dateInputModelConverters)
            {
                if (converter.CanConvertModelType(valueType))
                {
                    return converter.GetDateFromModel(valueType, Value);
                }
            }

            return null;
        }

        DateOnly? GetValueFromModel()
        {
            Debug.Assert(AspFor != null);

            var modelValue = AspFor!.Model;
            var underlyingModelType = Nullable.GetUnderlyingType(AspFor.ModelExplorer.ModelType) ?? AspFor.ModelExplorer.ModelType;

            if (modelValue is null)
            {
                return null;
            }

            var dateInputModelConverters = _options.DateInputModelConverters;

            foreach (var converter in dateInputModelConverters)
            {
                if (converter.CanConvertModelType(underlyingModelType))
                {
                    return converter.GetDateFromModel(underlyingModelType, modelValue);
                }
            }

            return null;
        }
    }

    private DateInputErrorComponents GetErrorComponents(DateInputContext dateInputContext)
    {
        if (dateInputContext.ErrorComponents != null)
        {
            return dateInputContext.ErrorComponents.Value;
        }

        if (AspFor == null)
        {
            return DateInputErrorComponents.All;
        }

        Debug.Assert(AspFor != null);
        Debug.Assert(ViewContext != null);

        var fullName = _modelHelper.GetFullHtmlFieldName(ViewContext, AspFor.Name);

        if (_dateInputParseErrorsProvider.TryGetErrorsForModel(fullName, out var parseErrors))
        {
            return parseErrors.GetErrorComponents();
        }
        else
        {
            return DateInputErrorComponents.All;
        }
    }
}

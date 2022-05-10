#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
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
    [OutputElementHint(ComponentGenerator.FormGroupElement)]
    public class DateInputTagHelper : FormGroupTagHelperBase
    {
        internal const string ErrorMessageTagName = "govuk-date-input-error-message";
        internal const string HintTagName = "govuk-date-input-hint";
        internal const string TagName = "govuk-date-input";

        private const string DateInputAttributesPrefix = "date-input-";
        private const string DisabledAttributeName = "disabled";
        private const string IdAttributeName = "id";
        private const string NamePrefixAttributeName = "name-prefix";
        private const string ValueAttributeName = "value";

        // Model binding relies on these specific values
        private const string DefaultDayItemName = "Day";
        private const string DefaultMonthItemName = "Month";
        private const string DefaultYearItemName = "Year";

        private object? _value;
        private bool _valueSpecified = false;
        private readonly GovUkFrontendAspNetCoreOptions _options;
        private readonly DateInputParseErrorsProvider _dateInputParseErrorsProvider;

        /// <summary>
        /// Creates a <see cref="DateInputTagHelper"/>.
        /// </summary>
        public DateInputTagHelper(
            IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor,
            DateInputParseErrorsProvider dateInputParseErrorsProvider)
            : this(optionsAccessor, dateInputParseErrorsProvider, htmlGenerator: null, modelHelper: null)
        {
        }

        internal DateInputTagHelper(
            IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor,
            DateInputParseErrorsProvider dateInputParseErrorsProvider,
            IGovUkHtmlGenerator? htmlGenerator = null,
            IModelHelper? modelHelper = null)
            : base(
                  htmlGenerator ?? new ComponentGenerator(),
                  modelHelper ?? new DefaultModelHelper())
        {
            _options = Guard.ArgumentNotNull(nameof(optionsAccessor), optionsAccessor).Value;
            _dateInputParseErrorsProvider = Guard.ArgumentNotNull(nameof(dateInputParseErrorsProvider), dateInputParseErrorsProvider);
        }

        /// <summary>
        /// Additional attributes for the container element that wraps the items.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = DateInputAttributesPrefix)]
        public IDictionary<string, string>? DateInputAttributes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Whether the <c>disabled</c> attribute should be added to the generated <c>input</c> elements.
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; } = ComponentGenerator.DateInputDefaultDisabled;

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

        private protected override FormGroupContext CreateFormGroupContext() => new DateInputContext(haveExplicitValue: _valueSpecified);

        private protected override IHtmlContent GenerateFormGroupContent(
            TagHelperContext tagHelperContext,
            FormGroupContext formGroupContext,
            TagHelperOutput tagHelperOutput,
            IHtmlContent childContent,
            out bool haveError)
        {
            var dateInputContext = tagHelperContext.GetContextItem<DateInputContext>();

            var contentBuilder = new HtmlContentBuilder();

            var hint = GenerateHint(tagHelperContext, formGroupContext);
            if (hint != null)
            {
                contentBuilder.AppendHtml(hint);
            }

            var errorMessage = GenerateErrorMessage(tagHelperContext, formGroupContext);
            if (errorMessage != null)
            {
                contentBuilder.AppendHtml(errorMessage);
            }

            haveError = errorMessage != null;

            var dateInputTagBuilder = GenerateDateInput(dateInputContext, haveError);
            contentBuilder.AppendHtml(dateInputTagBuilder);

            if (dateInputContext.Fieldset != null)
            {
                return Generator.GenerateFieldset(
                    DescribedBy,
                    role: "group",
                    dateInputContext.Fieldset.Legend?.IsPageHeading ?? ComponentGenerator.FieldsetLegendDefaultIsPageHeading,
                    legendContent: dateInputContext.Fieldset.Legend?.Content,
                    legendAttributes: dateInputContext.Fieldset.Legend?.Attributes,
                    content: contentBuilder,
                    attributes: dateInputContext.Fieldset.Attributes);
            }
            else
            {
                return contentBuilder;
            }
        }

        private protected override string GetErrorFieldId(TagHelperContext context)
        {
            var dateInputContext = context.GetContextItem<DateInputContext>();
            var errorItems = GetErrorComponents(dateInputContext);
            Debug.Assert(errorItems != DateInputErrorComponents.None);

            string suffix;

            if (errorItems.HasFlag(DateInputErrorComponents.Day))
            {
                suffix = $".{DefaultDayItemName}";
            }
            else if (errorItems.HasFlag(DateInputErrorComponents.Month))
            {
                suffix = $".{DefaultMonthItemName}";
            }
            else
            {
                suffix = $".{DefaultYearItemName}";
            }

            return $"{ResolveIdPrefix()}{suffix}";
        }

        private protected override string ResolveIdPrefix()
        {
            if (AspFor == null && Id == null)
            {
                ThrowHelper.AtLeastOneOfAttributesMustBeSpecified(AspForAttributeName, IdAttributeName);
            }

            return Id ??
                TagBuilder.CreateSanitizedId(
                    ModelHelper.GetFullHtmlFieldName(ViewContext, AspFor!.Name),
                    Constants.IdAttributeDotReplacement);
        }

        private TagBuilder GenerateDateInput(DateInputContext dateInputContext, bool haveError)
        {
            var resolvedId = ResolveIdPrefix();
            var resolvedName = AspFor != null ? ModelHelper.GetFullHtmlFieldName(ViewContext, AspFor.Name) : null;

            // This is a deliberate deviation from the GDS implementation so it works better with ASP.NET Core's model binding system
            var resolvedNamePrefix = NamePrefix != null ? NamePrefix + "." :
                resolvedName != null ? resolvedName + "." :
                string.Empty;

            var dateInputModelConverters = _options.DateInputModelConverters;

            var valueAsDate = GetValueAsDate() ?? (AspFor != null ? GetValueFromModel() : null);
            var errorItems = GetErrorComponents(dateInputContext);

            var day = CreateDateInputItem(
                getComponentFromValue: date => date?.Day.ToString(),
                defaultLabel: ComponentGenerator.DateInputDefaultDayLabel,
                defaultName: DefaultDayItemName,
                defaultClass: "govuk-input--width-2",
                DateInputErrorComponents.Day,
                contextItem: dateInputContext.Items.GetValueOrDefault(DateInputItemType.Day));

            var month = CreateDateInputItem(
                getComponentFromValue: date => date?.Month.ToString(),
                defaultLabel: ComponentGenerator.DateInputDefaultMonthLabel,
                defaultName: DefaultMonthItemName,
                defaultClass: "govuk-input--width-2",
                DateInputErrorComponents.Month,
                contextItem: dateInputContext.Items.GetValueOrDefault(DateInputItemType.Month));

            var year = CreateDateInputItem(
                getComponentFromValue: date => date?.Year.ToString(),
                defaultLabel: ComponentGenerator.DateInputDefaultYearLabel,
                defaultName: DefaultYearItemName,
                defaultClass: "govuk-input--width-4",
                DateInputErrorComponents.Year,
                contextItem: dateInputContext.Items.GetValueOrDefault(DateInputItemType.Year));

            return Generator.GenerateDateInput(
                resolvedId,
                Disabled,
                day,
                month,
                year,
                DateInputAttributes.ToAttributeDictionary());

            DateInputItem CreateDateInputItem(
                Func<Date?, string?> getComponentFromValue,
                string defaultLabel,
                string defaultName,
                string defaultClass,
                DateInputErrorComponents errorSource,
                DateInputContextItem? contextItem)
            {
                // Value resolution hierarchy:
                //   if Value has been set on a child tag helper e.g. <date-input-day /> then use that;
                //   if Value property is specified, use that;
                //   if AspFor is specified use value from ModelState;
                //   otherwise empty.

                var resolvedItemName = resolvedNamePrefix + (contextItem?.Name ?? defaultName);

                var resolvedItemValue = contextItem?.ValueSpecified == true ? (contextItem.Value?.ToString() ?? string.Empty) :
                    _valueSpecified ? getComponentFromValue(valueAsDate) :
                    contextItem?.ValueSpecified == true ? contextItem.Value?.ToString() :
                    AspFor != null ? GetValueFromModelState() :
                    null;

                var resolvedItemId = contextItem?.Id ?? $"{resolvedId}.{contextItem?.Name ?? defaultName}";

                var resolvedItemLabel = contextItem?.LabelContent ?? new HtmlString(defaultLabel);

                var resolvedItemHaveError = haveError && (errorItems & errorSource) != 0;

                var resolvedAttributes = contextItem?.Attributes ?? new Dictionary<string, string>();
                if (!resolvedAttributes.ContainsKey("class"))
                {
                    resolvedAttributes.Add("class", defaultClass);
                }

                return new DateInputItem()
                {
                    Attributes = resolvedAttributes.ToAttributeDictionary(),
                    Autocomplete = contextItem?.Autocomplete,
                    HaveError = resolvedItemHaveError,
                    Id = resolvedItemId,
                    InputMode = contextItem?.InputMode ?? ComponentGenerator.DateInputDefaultInputMode,
                    Name = resolvedItemName,
                    LabelContent = resolvedItemLabel,
                    LabelAttributes = contextItem?.LabelAttributes?.ToAttributeDictionary(),
                    Pattern = contextItem?.Pattern,
                    Value = resolvedItemValue
                };

                string? GetValueFromModelState()
                {
                    Debug.Assert(AspFor != null);

                    // Can't use ModelHelper.GetModelValue here;
                    // custom Date types may not expose components via Day/Month/Year properties.
                    // Resolution works as follows:
                    //   Look for a ModelStateEntry for $"{AspFor.Name}-{defaultName}" and, if found, use its AttemptedValue.
                    //   (Using defaultName is always correct here; that lines up with what the ModelBinder uses).
                    //   If that fails, get the model and convert it to a Date using options.DateInputModelConverters.
                    //   From that extract the Day/Month/Year.

                    var expression = $"{AspFor.Name}.{defaultName}";
                    var modelStateKey = ModelHelper.GetFullHtmlFieldName(ViewContext, expression);

                    if (ViewContext!.ModelState.TryGetValue(modelStateKey, out var modelStateEntry) &&
                        modelStateEntry.AttemptedValue != null)
                    {
                        return modelStateEntry.AttemptedValue;
                    }

                    return getComponentFromValue(valueAsDate);
                }
            }

            Date? GetValueAsDate()
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

            Date? GetValueFromModel()
            {
                Debug.Assert(AspFor != null);

                var modelValue = AspFor!.Model;
                var modelType = AspFor.ModelExplorer.ModelType;

                var dateInputModelConverters = _options.DateInputModelConverters;

                foreach (var converter in dateInputModelConverters)
                {
                    if (converter.CanConvertModelType(modelType))
                    {
                        return converter.GetDateFromModel(modelType, modelValue);
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

            var fullName = ModelHelper.GetFullHtmlFieldName(ViewContext, AspFor.Name);

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
}

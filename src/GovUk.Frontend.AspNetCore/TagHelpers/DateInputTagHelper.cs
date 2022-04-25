using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-date-input")]
    [RestrictChildren("govuk-date-input-fieldset", "govuk-date-input-label", "govuk-date-input-hint", "govuk-date-input-error-message")]
    public class DateInputTagHelper : LegacyFormGroupTagHelperBase
    {
        internal const string ValueAttributeName = "value";
        private const string AttributesPrefix = "date-input-";
        private const string DisabledAttributeName = "disabled";
        private const string IdPrefixAttributeName = "id-prefix";

        private readonly GovUkFrontendAspNetCoreOptions _options;
        private readonly DateInputParseErrorsProvider _dateInputParseErrorsProvider;

        private Date? _value;
        private bool _valueSpecified = false;

        public DateInputTagHelper(
            IGovUkHtmlGenerator htmlGenerator,
            IModelHelper modelHelper,
            DateInputParseErrorsProvider dateInputParseErrorsProvider,
            GovUkFrontendAspNetCoreOptions options)
            : base(htmlGenerator, modelHelper)
        {
            _dateInputParseErrorsProvider = dateInputParseErrorsProvider ?? throw new ArgumentNullException(nameof(dateInputParseErrorsProvider));
            _options = options;
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; } = ComponentDefaults.DateInput.Disabled;

        [HtmlAttributeName(IdPrefixAttributeName)]
        public string IdPrefix { get; set; }

        [HtmlAttributeName(ValueAttributeName)]
        public Date? Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueSpecified = true;
            }
        }

        protected override TagBuilder GenerateContent(TagHelperContext context, FormGroupBuilder builder)
        {
            var dateInput = base.GenerateContent(context, builder);

            var dateInputContext = (DateInputContext)context.Items[typeof(DateInputContext)];
            Debug.Assert(dateInputContext != null);

            if (dateInputContext.Fieldset != null)
            {
                var fieldset = Generator.GenerateFieldset(
                    DescribedBy,
                    role: "group",
                    dateInputContext.Fieldset.LegendIsPageHeading,
                    dateInputContext.Fieldset.LegendContent,
                    dateInputContext.Fieldset.LegendAttributes,
                    content: dateInput,
                    attributes: dateInputContext.Fieldset.Attributes);

                return fieldset;
            }
            else
            {
                return dateInput;
            }
        }

        protected override TagBuilder GenerateElement(
            TagHelperContext context,
            FormGroupBuilder builder,
            FormGroupElementContext elementContext)
        {
            if (AspFor == null && Name == null)
            {
                ThrowHelper.AtLeastOneOfAttributesMustBeSpecified(AspForAttributeName, NameAttributeName);
            }

            if (AspFor == null && !_valueSpecified)
            {
                ThrowHelper.AtLeastOneOfAttributesMustBeSpecified(AspForAttributeName, ValueAttributeName);
            }

            var dateInputContext = (DateInputContext)context.Items[typeof(DateInputContext)];
            Debug.Assert(dateInputContext != null);

            (int Day, int Month, int Year)? components = ResolveComponents();

            var deducedErrorItems = GetErrorItems();

            var day = CreateDateInputItem(
                value: components?.Day,
                useSpecifiedValue: _valueSpecified,
                defaultLabel: "Day",
                modelNameSuffix: DateInputModelBinder.DayComponentName,
                DateInputErrorItems.Day);

            var month = CreateDateInputItem(
                value: components?.Month,
                useSpecifiedValue: _valueSpecified,
                defaultLabel: "Month",
                modelNameSuffix: DateInputModelBinder.MonthComponentName,
                DateInputErrorItems.Month);

            var year = CreateDateInputItem(
                value: components?.Year,
                useSpecifiedValue: _valueSpecified,
                defaultLabel: "Year",
                modelNameSuffix: DateInputModelBinder.YearComponentName,
                DateInputErrorItems.Year);

            return Generator.GenerateDateInput(
                IdPrefix,
                Disabled,
                day,
                month,
                year,
                Attributes.ToAttributeDictionary());

            DateInputItem CreateDateInputItem(
                int? value,
                bool useSpecifiedValue,
                string defaultLabel,
                string modelNameSuffix,
                DateInputErrorItems errorSource)
            {
                string resolvedItemValue = value?.ToString() ?? string.Empty;

                if (!useSpecifiedValue)
                {
                    Debug.Assert(AspFor != null);

                    // Note we cannot use ModelHelper.GetModelValue here since it will fall back to trying to get
                    // the model from the expression, which may fail since this path is 'virtual'
                    // i.e. `{AspFor}.Day` may not be a valid expression

                    var itemModelExplorer = AspFor.ModelExplorer.GetExplorerForProperty(modelNameSuffix);
                    var itemFullName = ModelHelper.GetFullHtmlFieldName(ViewContext, expression: $"{AspFor.Name}.{modelNameSuffix}");

                    if (ViewContext.ViewData.ModelState.TryGetValue(itemFullName, out var entry) && entry.AttemptedValue != null)
                    {
                        resolvedItemValue = entry.AttemptedValue;
                    }
                }

                var resolvedItemName = $"{ResolvedName}.{modelNameSuffix}";

                var resolvedItemId = $"{ResolvedId}.{modelNameSuffix}";

                var resolvedItemLabel = new HtmlString(defaultLabel);

                var resolvedItemHaveError = elementContext.HaveError &&
                    ((dateInputContext.ErrorItems ?? deducedErrorItems) & errorSource) != 0;

                return new DateInputItem()
                {
                    //Attributes,
                    //Autocomplete
                    HaveError = resolvedItemHaveError,
                    Id = resolvedItemId,
                    Name = resolvedItemName,
                    Label = resolvedItemLabel,
                    //Pattern
                    Value = resolvedItemValue
                };
            }

            DateInputErrorItems GetErrorItems()
            {
                if (AspFor == null)
                {
                    return DateInputErrorItems.All;
                }

                Debug.Assert(AspFor != null);
                Debug.Assert(ViewContext != null);

                // If DateInputModelBinder was used for binding then we should have the specific parse errors available

                var fullName = ModelHelper.GetFullHtmlFieldName(ViewContext, AspFor.Name);

                if (_dateInputParseErrorsProvider.TryGetErrorsForModel(fullName, out var parseErrors))
                {
                    var dateParseErrorComponents = parseErrors.GetComponentsWithErrors();

                    return 0 |
                        ((dateParseErrorComponents & DateComponents.Day) != 0 ? DateInputErrorItems.Day : 0) |
                        ((dateParseErrorComponents & DateComponents.Month) != 0 ? DateInputErrorItems.Month : 0) |
                        ((dateParseErrorComponents & DateComponents.Year) != 0 ? DateInputErrorItems.Year : 0);
                }
                else
                {
                    return DateInputErrorItems.All;
                }
            }

            (int Day, int Month, int Year)? ResolveComponents()
            {
                if (_valueSpecified)
                {
                    if (Value == null)
                    {
                        return null;
                    }

                    var date = Value.Value;
                    return (date.Day, date.Month, date.Year);
                }

                if (AspFor != null && AspFor.Model is not null)
                {
                    var modelType = AspFor.ModelExplorer.ModelType;

                    // TODO Consider stashing the actual converter instance used by the model binder and using that
                    foreach (var converter in _options.DateInputModelConverters)
                    {
                        if (converter.CanConvertModelType(modelType))
                        {
                            var components = converter.GetElementsFromModel(modelType, AspFor.Model);
                            return components;
                        }
                    }

                    throw new Exception($"No {nameof(DateInputModelConverter)} registered that can convert {modelType.FullName}.");
                }

                return null;
            }
        }

        protected override string GetIdPrefix() => IdPrefix;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var dateInputContext = new DateInputContext();

            using (context.SetScopedContextItem(typeof(DateInputContext), dateInputContext))
            {
                await base.ProcessAsync(context, output);
            }
        }
    }

    [HtmlTargetElement("govuk-date-input-fieldset", ParentTag = "govuk-date-input")]
    [RestrictChildren("govuk-date-input-fieldset-legend")]
    public class DateInputFieldsetTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var dateInputContext = (DateInputContext)context.Items[typeof(DateInputContext)];
            Debug.Assert(dateInputContext != null);

            var fieldsetContext = new DateInputFieldsetContext();
            using (context.SetScopedContextItem(typeof(DateInputFieldsetContext), fieldsetContext))
            {
                await output.GetChildContentAsync();
            }

            dateInputContext.SetFieldset(new DateInputFieldset()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                LegendIsPageHeading = fieldsetContext.Legend?.isPageHeading,
                LegendContent = fieldsetContext.Legend?.content,
                LegendAttributes = fieldsetContext.Legend?.attributes.ToAttributeDictionary()
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-date-input-fieldset-legend", ParentTag = "govuk-date-input-fieldset")]
    public class DateInputFieldsetLegendTagHelper : TagHelper
    {
        private const string IsPageHeadingAttributeName = "is-page-heading";

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; } = ComponentGenerator.FieldsetLegendDefaultIsPageHeading;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = (DateInputFieldsetContext)context.Items[typeof(DateInputFieldsetContext)];
            Debug.Assert(fieldsetContext != null);

            var childContent = await output.GetChildContentAsync();

            fieldsetContext.SetLegend(
                IsPageHeading,
                output.Attributes.ToAttributesDictionary(),
                childContent.Snapshot());

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-date-input-label", ParentTag = "govuk-date-input")]
    public class DateInputLabelTagHelper : FormGroupLabelTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-date-input-hint", ParentTag = "govuk-date-input")]
    public class DateInputHintTagHelper : FormGroupHintTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-date-input-error-message", ParentTag = "govuk-date-input")]
    public class DateInputErrorMessageTagHelper : FormGroupErrorMessageTagHelperBase
    {
        private const string ErrorItemsAttributeName = "error-items";

        [HtmlAttributeName(ErrorItemsAttributeName)]
        public DateInputErrorItems ErrorItems { get; set; } = DateInputErrorItems.All;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var dateInputContext = (DateInputContext)context.Items[typeof(DateInputContext)];
            Debug.Assert(dateInputContext != null);

            dateInputContext.SetErrorItems(ErrorItems);

            return base.ProcessAsync(context, output);
        }
    }

    internal class DateInputContext
    {
        public DateInputErrorItems? ErrorItems { get; private set; }
        public DateInputFieldset Fieldset { get; private set; }

        public void SetErrorItems(DateInputErrorItems errorItems) => ErrorItems = errorItems;

        public void SetFieldset(DateInputFieldset fieldset)
        {
            if (fieldset == null)
            {
                throw new ArgumentNullException(nameof(fieldset));
            }

            if (Fieldset != null)
            {
                ThrowHelper.OnlyOneElementAllowed("govuk-date-input-fieldset");
            }

            Fieldset = fieldset;
        }
    }

    internal class DateInputFieldsetContext
    {
        public (bool isPageHeading, IDictionary<string, string> attributes, IHtmlContent content)? Legend { get; private set; }

        public void SetLegend(bool isPageHeading, IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Legend != null)
            {
                ThrowHelper.OnlyOneElementAllowed("govuk-date-input-fieldset-legend");
            }

            Legend = (isPageHeading, attributes, content);
        }
    }

    internal class DateInputFieldset
    {
        public AttributeDictionary Attributes { get; set; }
        public bool? LegendIsPageHeading { get; set; }
        public IHtmlContent LegendContent { get; set; }
        public AttributeDictionary LegendAttributes { get; set; }
    }
}

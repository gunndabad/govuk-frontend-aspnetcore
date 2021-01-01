using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-date-input")]
    [RestrictChildren("govuk-date-input-fieldset", "govuk-date-input-label", "govuk-date-input-hint", "govuk-date-input-error-message")]
    public class DateInputTagHelper : FormGroupTagHelperBase
    {
        internal const string ValueAttributeName = "value";
        private const string AttributesPrefix = "date-input-";
        private const string DisabledAttributeName = "disabled";
        private const string IdPrefixAttributeName = "id-prefix";

        private Date? _value;
        private bool _valueSpecified = false;

        public DateInputTagHelper(IGovUkHtmlGenerator htmlGenerator, IModelHelper modelHelper)
            : base(htmlGenerator, modelHelper)
        {
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

            var deducedErrorItems = GetErrorItems();

            var day = CreateDateInputItem(
                specifiedValue: Value?.Day.ToString() ?? string.Empty,
                useSpecifiedValue: _valueSpecified,
                defaultLabel: "Day",
                modelNameSuffix: DateInputModelBinder.DayComponentName,
                DateInputErrorItems.Day);

            var month = CreateDateInputItem(
                specifiedValue: Value?.Month.ToString() ?? string.Empty,
                useSpecifiedValue: _valueSpecified,
                defaultLabel: "Month",
                modelNameSuffix: DateInputModelBinder.MonthComponentName,
                DateInputErrorItems.Month);

            var year = CreateDateInputItem(
                specifiedValue: Value?.Year.ToString() ?? string.Empty,
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
                Attributes);

            DateInputItem CreateDateInputItem(
                string specifiedValue,
                bool useSpecifiedValue,
                string defaultLabel,
                string modelNameSuffix,
                DateInputErrorItems errorSource)
            {
                // Value resolution rules:
                //   if Value property is specified, use that (even if it's null);
                //   otherwise use value from ModelState (which may be invalid if user has POSTed invalid date, say)

                string resolvedItemValue = specifiedValue;

                if (!useSpecifiedValue)
                {
                    Debug.Assert(AspFor != null);

                    var itemModelExplorer = AspFor.ModelExplorer.GetExplorerForProperty(modelNameSuffix);

                    resolvedItemValue = ModelHelper.GetModelValue(
                        ViewContext,
                        itemModelExplorer,
                        expression: $"{AspFor.Name}.{modelNameSuffix}");
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

                // If there is a DateParseException in ModelState for the bound property,
                // use its ErrorComponents to deduce DateInputErrorItems

                var fullName = ModelHelper.GetFullHtmlFieldName(ViewContext, AspFor.Name);
                if (!ViewContext.ModelState.TryGetValue(fullName, out var modelState))
                {
                    return DateInputErrorItems.All;
                }

                var dateParseException = modelState.Errors
                    .Select(e => e.Exception)
                    .Where(e => e != null)
                    .OfType<DateParseException>()
                    .FirstOrDefault();

                if (dateParseException == null)
                {
                    return DateInputErrorItems.All;
                }

                var dateParseErrorComponents = dateParseException.ErrorComponents;

                return 0 |
                    ((dateParseErrorComponents & DateParseErrorComponents.Day) != 0 ? DateInputErrorItems.Day : 0) |
                    ((dateParseErrorComponents & DateParseErrorComponents.Month) != 0 ? DateInputErrorItems.Month : 0) |
                    ((dateParseErrorComponents & DateParseErrorComponents.Year) != 0 ? DateInputErrorItems.Year : 0);
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
                LegendAttributes = fieldsetContext.Legend?.attributes
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
        public IDictionary<string, string> Attributes { get; set; }
        public bool? LegendIsPageHeading { get; set; }
        public IHtmlContent LegendContent { get; set; }
        public IDictionary<string, string> LegendAttributes { get; set; }
    }
}

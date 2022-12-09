using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an error item in a GDS error summary component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = ErrorSummaryTagHelper.TagName)]
    public class ErrorSummaryItemTagHelper : TagHelper
    {
        internal const string TagName = "govuk-error-summary-item";

        private const string AspForAttributeName = "asp-for";
        private const string LinkAttributesPrefix = "link-";

        private readonly GovUkFrontendAspNetCoreOptions _options;
        private readonly DateInputParseErrorsProvider _dateInputParseErrorsProvider;
        private readonly IModelHelper _modelHelper;

        /// <summary>
        /// Creates a new <see cref="ErrorSummaryItemTagHelper"/>.
        /// </summary>
        public ErrorSummaryItemTagHelper(
            IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor,
            DateInputParseErrorsProvider dateInputParseErrorsProvider)
            : this(optionsAccessor, dateInputParseErrorsProvider, modelHelper: null)
        {
        }

        internal ErrorSummaryItemTagHelper(
            IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor,
            DateInputParseErrorsProvider dateInputParseErrorsProvider,
            IModelHelper? modelHelper = null)
        {
            _options = Guard.ArgumentNotNull(nameof(optionsAccessor), optionsAccessor).Value;
            _dateInputParseErrorsProvider = Guard.ArgumentNotNull(nameof(dateInputParseErrorsProvider), dateInputParseErrorsProvider);
            _modelHelper = modelHelper ?? new DefaultModelHelper();
        }

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression? AspFor { get; set; }

        /// <summary>
        /// Additional attributes to add to the generated <c>a</c> element.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]
        public IDictionary<string, string?>? LinkAttributes { get; set; } = new Dictionary<string, string?>();

        /// <summary>
        /// Gets the <see cref="ViewContext"/> of the executing view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (output.TagMode == TagMode.SelfClosing && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"Content is required when the '{AspForAttributeName}' attribute is not specified.");
            }

            var errorSummaryContext = context.GetContextItem<ErrorSummaryContext>();

            var childContent = await output.GetChildContentAsync();

            IHtmlContent itemContent;

            if (output.TagMode == TagMode.StartTagAndEndTag)
            {
                itemContent = childContent.Snapshot();
            }
            else
            {
                Debug.Assert(AspFor != null);

                var validationMessage = _modelHelper.GetValidationMessage(
                    ViewContext!,
                    AspFor!.ModelExplorer,
                    AspFor.Name);

                if (validationMessage == null)
                {
                    return;
                }

                itemContent = new HtmlString(validationMessage);
            }

            string? resolvedHref = null;

            if (output.Attributes.TryGetAttribute("href", out var hrefAttribute))
            {
                resolvedHref = hrefAttribute.Value.ToString();
                output.Attributes.Remove(hrefAttribute);
            }
            else if (AspFor != null)
            {
                var errorFieldId = TagBuilder.CreateSanitizedId(
                    _modelHelper.GetFullHtmlFieldName(ViewContext!, AspFor!.Name),
                    Constants.IdAttributeDotReplacement);

                // Date inputs are special; they don't have an element with ID which exactly corresponds to the name derived above;
                // the IDs are suffixed with .Day .Month and .Year for each of the components.
                // We don't have a perfect way to know whether this error is for a date input.
                // The best we can do is look at the type for the ModelExpression and see if it looks like a date type.
                // If it does look like a date type we also consult DateInputParseErrorsProvider to know which input to link to
                // (e.g. if .Day is valid but .Month and .Year are not, we link to .Month as the first input with errors.)
                // (Note we cannot rely on DateInputParseErrorsProvider for identifying date inputs since we could have errors
                // that didn't come from model binding so TryGetErrorsForModel will return false.)

                if (IsModelExpressionForDate())
                {
                    var dateInputErrorComponents = DateInputErrorComponents.All;

                    if (_dateInputParseErrorsProvider.TryGetErrorsForModel(AspFor.Name, out var dateInputParseErrors))
                    {
                        dateInputErrorComponents = dateInputParseErrors.GetErrorComponents();
                    }

                    Debug.Assert(dateInputErrorComponents != DateInputErrorComponents.None);

                    if (dateInputErrorComponents.HasFlag(DateInputErrorComponents.Day))
                    {
                        errorFieldId += ".Day";
                    }
                    else if (dateInputErrorComponents.HasFlag(DateInputErrorComponents.Month))
                    {
                        errorFieldId += ".Month";
                    }
                    else
                    {
                        errorFieldId += ".Year";
                    }
                }

                resolvedHref = $"#{errorFieldId}";
            }

            errorSummaryContext.AddItem(new ErrorSummaryItem()
            {
                Content = itemContent,
                Attributes = output.Attributes.ToAttributeDictionary(),
                Href = resolvedHref,
                LinkAttributes = LinkAttributes.ToAttributeDictionary()
            });

            output.SuppressOutput();

            bool IsModelExpressionForDate()
            {
                Debug.Assert(AspFor != null);

                var modelType = AspFor!.Metadata.ModelType;
                return _options.DateInputModelConverters.Any(c => c.CanConvertModelType(modelType));
            }
        }
    }
}

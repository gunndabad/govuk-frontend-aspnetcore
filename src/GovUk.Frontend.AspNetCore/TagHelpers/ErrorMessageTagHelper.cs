#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS error message component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.ErrorMessageElement)]
    public class ErrorMessageTagHelper : TagHelper
    {
        internal const string TagName = "govuk-error-message";

        private const string AspForAttributeName = "asp-for";
        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";
        
        private readonly IGovUkHtmlGenerator _htmlGenerator;
        private readonly IModelHelper _modelHelper;

        /// <summary>
        /// Creates a <see cref="ErrorMessageTagHelper"/>.
        /// </summary>
        public ErrorMessageTagHelper()
            : this(htmlGenerator: null, modelHelper: null)
        {
        }

        internal ErrorMessageTagHelper(
            IGovUkHtmlGenerator? htmlGenerator,
            IModelHelper? modelHelper)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
            _modelHelper = modelHelper ?? new DefaultModelHelper();
        }

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        /// <remarks>
        /// If specified and this element has no child content the error message will be resolved from
        /// the errors on this expression's <see cref="ModelStateEntry"/>.
        /// </remarks>
        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression? AspFor { get; set; }

        /// <summary>
        /// A visually hidden prefix used before the error message.
        /// </summary>
        /// <remarks>
        /// Defaults to <c>&quot;Error&quot;</c>.
        /// </remarks>
        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string? VisuallyHiddenText { get; set; } = ComponentGenerator.ErrorMessageDefaultVisuallyHiddenText;

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
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
                await output.GetChildContentAsync() :
                null;

            if (childContent == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"Cannot determine content. Element must contain content if the '{AspForAttributeName}' attribute is not specified.");
            }

            IHtmlContent? resolvedContent = childContent;
            if (resolvedContent == null && AspFor != null)
            {
                var validationMessage = _modelHelper.GetValidationMessage(
                    ViewContext,
                    AspFor.ModelExplorer,
                    AspFor.Name);

                if (validationMessage != null)
                {
                    resolvedContent = new HtmlString(validationMessage);
                }
            }

            if (resolvedContent != null)
            {
                var tagBuilder = _htmlGenerator.GenerateErrorMessage(
                    VisuallyHiddenText,
                    resolvedContent,
                    output.Attributes.ToAttributesDictionary());

                output.TagName = tagBuilder.TagName;
                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Clear();
                output.MergeAttributes(tagBuilder);
                output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}

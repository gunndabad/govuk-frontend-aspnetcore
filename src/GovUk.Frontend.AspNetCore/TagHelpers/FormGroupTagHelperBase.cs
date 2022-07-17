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
    /// Base class for tag helpers that generate a form group.
    /// </summary>
    public abstract class FormGroupTagHelperBase : TagHelper
    {
        private protected const string AspForAttributeName = "asp-for";
        private protected const string IgnoreModelStateErrorsAttributeName = "ignore-modelstate-errors";

        private protected FormGroupTagHelperBase(
            IGovUkHtmlGenerator htmlGenerator,
            IModelHelper modelHelper)
        {
            Generator = Guard.ArgumentNotNull(nameof(htmlGenerator), htmlGenerator);
            ModelHelper = Guard.ArgumentNotNull(nameof(modelHelper), modelHelper);
        }

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression? AspFor { get; set; }

        /// <summary>
        /// Whether the <see cref="ModelStateEntry.Errors"/> for the <see cref="AspFor"/> expression should be used
        /// to deduce an error message.
        /// </summary>
        /// <remarks>
        /// <para>When there are multiple errors in the <see cref="ModelErrorCollection"/> the first is used.</para>
        /// <para>The default is <c>false</c>.</para>
        /// </remarks>
        [HtmlAttributeName(IgnoreModelStateErrorsAttributeName)]
        public bool IgnoreModelStateErrors { get; set; } = false;

        /// <summary>
        /// Gets the <see cref="ViewContext"/> of the executing view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        [DisallowNull]
        public ViewContext? ViewContext { get; set; }

        internal string? DescribedBy { get; set; }

        private protected IGovUkHtmlGenerator Generator { get; }

        private protected IModelHelper ModelHelper { get; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var formGroupContext = CreateFormGroupContext();

            IHtmlContent content;
            bool haveError;

            // Since implementations can return a derived FormGroupContext ensure we set both a context item
            // for FormGroupContext and one for the specific type.
            // N.B. we cannot make this context type a generic parameter since it's internal.
            using (context.SetScopedContextItem(formGroupContext))
            using (context.SetScopedContextItem(formGroupContext.GetType(), formGroupContext))
            {
                var childContent = await output.GetChildContentAsync();

                content = GenerateFormGroupContent(context, formGroupContext, output, childContent, out haveError);
            }

            var tagBuilder = CreateTagBuilder(haveError, content, output);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }

        internal static string? AppendToDescribedBy(string? describedBy, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return describedBy;
            }

            if (string.IsNullOrWhiteSpace(describedBy))
            {
                return value;
            }
            else
            {
                return $"{describedBy} {value}";
            }
        }

        private protected virtual TagBuilder CreateTagBuilder(bool haveError, IHtmlContent content, TagHelperOutput tagHelperOutput) =>
            Generator.GenerateFormGroup(
                haveError,
                content,
                tagHelperOutput.Attributes.ToAttributeDictionary());

        private protected virtual string GetErrorFieldId(TagHelperContext context) => ResolveIdPrefix();

        internal IHtmlContent? GenerateErrorMessage(TagHelperContext tagHelperContext, FormGroupContext formGroupContext)
        {
            var visuallyHiddenText = formGroupContext.ErrorMessage?.VisuallyHiddenText ??
                ComponentGenerator.ErrorMessageDefaultVisuallyHiddenText;

            var content = formGroupContext.ErrorMessage?.Content;
            var attributes = formGroupContext.ErrorMessage?.Attributes;

            if (content == null && AspFor != null && IgnoreModelStateErrors != true)
            {
                var validationMessage = ModelHelper.GetValidationMessage(
                    ViewContext,
                    AspFor!.ModelExplorer,
                    AspFor.Name);

                if (validationMessage != null)
                {
                    content = new HtmlString(validationMessage);
                }
            }

            if (content != null)
            {
                AddErrorToFormErrorContext();

                var resolvedIdPrefix = ResolveIdPrefix();
                var errorId = resolvedIdPrefix + "-error";
                DescribedBy = AppendToDescribedBy(DescribedBy, errorId);

                attributes ??= new AttributeDictionary();
                attributes["id"] = errorId;

                return Generator.GenerateErrorMessage(visuallyHiddenText, content, attributes);
            }
            else
            {
                return null;
            }

            void AddErrorToFormErrorContext()
            {
                if (tagHelperContext.Items.TryGetValue(typeof(FormErrorContext), out var formErrorContextObj) &&
                    formErrorContextObj is FormErrorContext formErrorContext)
                {
                    var errorFieldId = GetErrorFieldId(tagHelperContext);
                    var href = "#" + errorFieldId;

                    formErrorContext.AddError(content, href);
                }
            }
        }

        internal IHtmlContent? GenerateHint(TagHelperContext tagHelperContext, FormGroupContext formGroupContext)
        {
            var content = formGroupContext.Hint?.Content;
            var attributes = formGroupContext.Hint?.Attributes;

            if (content == null && AspFor != null)
            {
                var description = ModelHelper.GetDescription(AspFor.ModelExplorer);

                if (description != null)
                {
                    content = new HtmlString(description);
                }
            }

            if (content != null)
            {
                var resolvedIdPrefix = ResolveIdPrefix();
                var hintId = resolvedIdPrefix + "-hint";
                DescribedBy = AppendToDescribedBy(DescribedBy, hintId);
                return Generator.GenerateHint(hintId, content, attributes);
            }
            else
            {
                return null;
            }
        }

        internal IHtmlContent GenerateLabel(FormGroupContext formGroupContext)
        {
            // We need some content for the label; if AspFor is null then label content must have been specified
            if (AspFor == null && formGroupContext.Label?.Content == null)
            {
                throw new InvalidOperationException(
                    $"Label content must be specified when the '{AspForAttributeName}' attribute is not specified.");
            }

            var resolvedIdPrefix = ResolveIdPrefix();

            var isPageHeading = formGroupContext.Label?.IsPageHeading ?? ComponentGenerator.LabelDefaultIsPageHeading;
            var content = formGroupContext.Label?.Content;
            var attributes = formGroupContext.Label?.Attributes;

            var resolvedContent = content ??
                new HtmlString(ModelHelper.GetDisplayName(ViewContext, AspFor!.ModelExplorer, AspFor.Name));

            return Generator.GenerateLabel(resolvedIdPrefix, isPageHeading, resolvedContent, attributes);
        }

        private protected abstract FormGroupContext CreateFormGroupContext();

        private protected abstract IHtmlContent GenerateFormGroupContent(
            TagHelperContext tagHelperContext,
            FormGroupContext formGroupContext,
            TagHelperOutput tagHelperOutput,
            IHtmlContent childContent,
            out bool haveError);

        private protected abstract string ResolveIdPrefix();
    }
}

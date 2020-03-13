using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    public abstract class FormGroupTagHelperBase : TagHelper
    {
        protected const string AspForAttributeName = "asp-for";
        protected const string DescribedByAttributeName = "described-by";
        protected const string FormGroupAttributesPrefix = "form-group-";
        protected const string NameAttributeName = "name";

        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName(DescribedByAttributeName)]
        public string DescribedBy { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = FormGroupAttributesPrefix)]
        public IDictionary<string, string> FormGroupAttributes { get; set; }

        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IGovUkHtmlGenerator Generator { get; }

        protected string ResolvedId => GetIdPrefix() ?? TagBuilder.CreateSanitizedId(ResolvedName, Constants.IdAttributeDotReplacement);

        protected string ResolvedName => Name ?? Generator.GetFullHtmlFieldName(ViewContext, AspFor.Name);

        private protected FormGroupTagHelperBase(IGovUkHtmlGenerator htmlGenerator)
        {
            Generator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        private protected abstract string GetIdPrefix();

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            var builder = CreateFormGroupBuilder();

            using (context.SetScopedContextItem(FormGroupBuilder.ContextName, builder))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = GenerateContent(context, builder);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }

        private protected void AppendToDescribedBy(string value)
        {
            if (value == null)
            {
                return;
            }

            if (DescribedBy == null)
            {
                DescribedBy = value;
            }
            else
            {
                DescribedBy += $" {value}";
            }
        }

        private protected virtual FormGroupBuilder CreateFormGroupBuilder() => new FormGroupBuilder();

        private protected virtual TagBuilder GenerateContent(TagHelperContext context, FormGroupBuilder builder)
        {
            // We need some content for the label; if AspFor is null then label content must have been specified
            if (AspFor == null && (!builder.Label.HasValue || builder.Label.Value.content == null))
            {
                throw new InvalidOperationException(
                    $"Label content must be specified when the '{AspForAttributeName}' attribute is not specified.");
            }

            var contentBuilder = new HtmlContentBuilder();

            var label = GenerateLabel(builder);
            contentBuilder.AppendHtml(label);

            var hint = GenerateHint(builder);
            if (hint != null)
            {
                contentBuilder.AppendHtml(hint);
            }

            var errorMessage = GenerateErrorMessage(builder);
            if (errorMessage != null)
            {
                contentBuilder.AppendHtml(errorMessage);
            }

            var haveError = errorMessage != null;

            var elementCtx = new FormGroupElementContext(haveError);
            var element = GenerateElement(builder, elementCtx);

            contentBuilder.AppendHtml(element);

            return Generator.GenerateFormGroup(haveError, FormGroupAttributes, contentBuilder);
        }

        private protected virtual TagBuilder GenerateElement(FormGroupBuilder builder, FormGroupElementContext context)
        {
            // For deriving classes to implement when required
            throw new NotImplementedException();
        }

        private protected IHtmlContent GenerateErrorMessage(FormGroupBuilder builder)
        {
            var visuallyHiddenText = builder.ErrorMessage?.visuallyHiddenText;
            var content = builder.ErrorMessage?.content;

            if (content == null && AspFor != null)
            {
                var validationMessage = Generator.GetValidationMessage(ViewContext, AspFor.ModelExplorer, AspFor.Name);

                if (validationMessage != null)
                {
                    content = new HtmlString(validationMessage);
                }
            }

            if (content != null)
            {
                var errorId = ResolvedId + "-error";
                AppendToDescribedBy(errorId);
                return Generator.GenerateErrorMessage(visuallyHiddenText, errorId, content);
            }
            else
            {
                return null;
            }
        }

        private protected virtual TagBuilder GenerateHint(FormGroupBuilder builder)
        {
            if (builder.Hint != null)
            {
                var hintId = ResolvedId + "-hint";
                AppendToDescribedBy(hintId);
                return Generator.GenerateHint(hintId, builder.Hint);
            }
            else
            {
                return null;
            }
        }

        private protected virtual IHtmlContent GenerateLabel(FormGroupBuilder builder)
        {
            var isPageHeading = builder.Label?.isPageHeading ?? false;
            var content = builder.Label?.content;

            var resolvedContent = content ??
                new HtmlString(Generator.GetDisplayName(ViewContext, AspFor.ModelExplorer, AspFor.Name));

            return Generator.GenerateLabel(ResolvedId, isPageHeading, resolvedContent);
        }
    }

    public abstract class FormGroupLabelTagHelperBase : TagHelper
    {
        private const string IsPageHeadingAttributeName = "is-page-heading";

        private protected FormGroupLabelTagHelperBase()
        {
        }

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ? await output.GetChildContentAsync() : null;

            var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
            if (!formGroupContext.TrySetLabel(IsPageHeading, childContent))
            {
                throw new InvalidOperationException($"Cannot render <{context.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    public abstract class FormGroupHintTagHelperBase : TagHelper
    {
        private protected FormGroupHintTagHelperBase()
        {
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ? await output.GetChildContentAsync() : null;

            var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
            if (!formGroupContext.TrySetHint(childContent))
            {
                throw new InvalidOperationException($"Cannot render <{context.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    public abstract class FormGroupErrorMessageTagHelperBase : TagHelper
    {
        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

        private protected FormGroupErrorMessageTagHelperBase()
        {
        }

        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string VisuallyHiddenText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ? await output.GetChildContentAsync() : null;

            var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
            if (!formGroupContext.TrySetErrorMessage(VisuallyHiddenText, childContent))
            {
                throw new InvalidOperationException($"Cannot render <{context.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    internal enum FormGroupRenderStage
    {
        None = 0,
        Label = 1,
        Hint = 2,
        ErrorMessage = 3,
        Element = 4
    }

    internal class FormGroupBuilder
    {
        public const string ContextName = nameof(FormGroupBuilder);

        public (string visuallyHiddenText, IHtmlContent content)? ErrorMessage { get; private set; }

        public IHtmlContent Hint { get; private set; }

        public (bool isPageHeading, IHtmlContent content)? Label { get; private set; }

        // Internal for testing
        internal FormGroupRenderStage RenderStage { get; private set; } = FormGroupRenderStage.None;

        public bool TrySetErrorMessage(string visuallyHiddenText, IHtmlContent content)
        {
            if (RenderStage >= FormGroupRenderStage.ErrorMessage)
            {
                return false;
            }

            RenderStage = FormGroupRenderStage.ErrorMessage;
            ErrorMessage = (visuallyHiddenText, content);

            return true;
        }

        public bool TrySetHint(IHtmlContent content)
        {
            if (RenderStage >= FormGroupRenderStage.Hint)
            {
                return false;
            }

            RenderStage = FormGroupRenderStage.Hint;
            Hint = content;

            return true;
        }

        public bool TrySetLabel(bool isPageHeading, IHtmlContent content)
        {
            if (RenderStage >= FormGroupRenderStage.Label)
            {
                return false;
            }

            RenderStage = FormGroupRenderStage.Label;
            Label = (isPageHeading, content);

            return true;
        }

        protected bool TrySetElementRenderStage()
        {
            if (RenderStage >= FormGroupRenderStage.Element)
            {
                return false;
            }

            RenderStage = FormGroupRenderStage.Element;

            return true;
        }
    }

    internal class FormGroupElementContext
    {
        public FormGroupElementContext(bool haveError)
        {
            HaveError = haveError;
        }

        public bool HaveError { get; }
    }
}

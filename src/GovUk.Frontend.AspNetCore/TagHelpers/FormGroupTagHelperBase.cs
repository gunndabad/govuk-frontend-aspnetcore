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
        protected const string IdAttributeName = "id";
        private const string NameAttributeName = "name";

        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName(DescribedByAttributeName)]
        public string DescribedBy { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private protected FormGroupTagHelperBase(IGovUkHtmlGenerator htmlGenerator)
        {
            Generator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        protected IGovUkHtmlGenerator Generator { get; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Id == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{IdAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            var resolvedId = Id ?? Generator.GetFullHtmlFieldName(ViewContext, AspFor.Name);

            var builder = CreateFormGroupBuilder();
            context.Items.Add(FormGroupBuilder.ContextName, builder);

            await output.GetChildContentAsync();

            // We need some content for the label; if AspFor is null then label content must have been specified
            if (AspFor == null && (!builder.Label.HasValue || builder.Label.Value.content == null))
            {
                throw new InvalidOperationException(
                    $"Label content must be specified when the '{AspForAttributeName}' attribute is not specified.");
            }

            var label = CreateLabel();

            var hintId = $"{resolvedId}-hint";
            var hint = CreateHint();

            var errorId = $"{resolvedId}-error";
            var errorMessage = CreateErrorMessage();

            var haveError = errorMessage != null;

            string describedBy;
            {
                var describedByParts = new List<string>();

                if (DescribedBy != null)
                {
                    describedByParts.Add(DescribedBy);
                }

                if (hint != null)
                {
                    describedByParts.Add(hintId);
                }

                if (haveError)
                {
                    describedByParts.Add(errorId);
                }

                describedBy = string.Join(" ", describedByParts);
            }

            var elementCtx = new FormGroupElementContext(resolvedId, haveError, describedBy);
            var element = CreateElement(builder, elementCtx);

            var tagBuilder = Generator.GenerateFormGroup(haveError, label, hint, errorMessage, element);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);

            TagBuilder CreateLabel()
            {
                var isPageHeading = builder.Label?.isPageHeading ?? false;
                var content = builder.Label?.content;

                var resolvedContent = (IHtmlContent)content ??
                    new HtmlString(Generator.GetDisplayName(ViewContext, AspFor.ModelExplorer, AspFor.Name));

                return Generator.GenerateLabel(resolvedId, isPageHeading, resolvedContent);
            }

            TagBuilder CreateHint() => builder.Hint != null ?
                Generator.GenerateHint(hintId, builder.Hint) :
                null;

            TagBuilder CreateErrorMessage()
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

                return content != null ?
                    Generator.GenerateErrorMessage(visuallyHiddenText, errorId, content) :
                    null;
            }
        }

        private protected abstract IHtmlContent CreateElement(FormGroupBuilder builder, FormGroupElementContext context);

        private protected virtual FormGroupBuilder CreateFormGroupBuilder() => new FormGroupBuilder();
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
        public FormGroupElementContext(string elementId, bool haveError, string describedBy)
        {
            ElementId = elementId ?? throw new ArgumentNullException(nameof(elementId));
            HaveError = haveError;
            DescribedBy = describedBy;
        }

        public string DescribedBy { get; }
        public string ElementId { get; }
        public bool HaveError { get; }
    }
}

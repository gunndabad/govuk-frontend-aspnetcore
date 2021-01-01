using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
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
        protected const string IgnoreModelStateErrorsAttributeName = "ignore-modelstate-errors";
        protected const string NameAttributeName = "name";

        [HtmlAttributeName(AspForAttributeName)]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName(DescribedByAttributeName)]
        public string DescribedBy { get; set; }

        [HtmlAttributeName(IgnoreModelStateErrorsAttributeName)]
        public bool? IgnoreModelStateErrors { get; set; }

        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IDictionary<string, string> FormGroupAttributes { get; private set; }

        protected IGovUkHtmlGenerator Generator { get; }

        protected IModelHelper ModelHelper { get; }

        protected string ResolvedId => GetIdPrefix() ?? TagBuilder.CreateSanitizedId(ResolvedName, Constants.IdAttributeDotReplacement);

        protected string ResolvedName => Name ?? ModelHelper.GetFullHtmlFieldName(ViewContext, AspFor.Name);

        protected FormGroupTagHelperBase(
            IGovUkHtmlGenerator htmlGenerator,
            IModelHelper modelHelper)
        {
            Generator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
            ModelHelper = modelHelper ?? throw new ArgumentNullException(nameof(modelHelper));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            FormGroupAttributes = output.Attributes.ToAttributesDictionary();
            var builder = CreateFormGroupBuilder();

            using (context.SetScopedContextItem(typeof(FormGroupBuilder), builder))
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

        protected void AppendToDescribedBy(string value)
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

        protected virtual FormGroupBuilder CreateFormGroupBuilder() => new FormGroupBuilder();

        protected virtual TagBuilder GenerateContent(TagHelperContext context, FormGroupBuilder builder)
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
            var element = GenerateElement(context, builder, elementCtx);

            contentBuilder.AppendHtml(element);

            return Generator.GenerateFormGroup(haveError, contentBuilder, FormGroupAttributes);
        }

        protected virtual TagBuilder GenerateElement(
            TagHelperContext context,
            FormGroupBuilder builder,
            FormGroupElementContext elementContext)
        {
            // For deriving classes to implement when required
            throw new NotImplementedException();
        }

        protected abstract string GetIdPrefix();

        private protected IHtmlContent GenerateErrorMessage(FormGroupBuilder builder)
        {
            var visuallyHiddenText = builder.ErrorMessage?.visuallyHiddenText ??
                ComponentGenerator.ErrorMessageDefaultVisuallyHiddenText;

            var content = builder.ErrorMessage?.content;
            var attributes = builder.ErrorMessage?.attributes;

            if (content == null && AspFor != null && IgnoreModelStateErrors != true)
            {
                var validationMessage = ModelHelper.GetValidationMessage(ViewContext, AspFor.ModelExplorer, AspFor.Name);

                if (validationMessage != null)
                {
                    content = new HtmlString(validationMessage);
                }
            }

            if (content != null)
            {
                var errorId = ResolvedId + "-error";
                AppendToDescribedBy(errorId);

                attributes ??= new Dictionary<string, string>();
                attributes["id"] = errorId;

                return Generator.GenerateErrorMessage(visuallyHiddenText, content, attributes);
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
                return Generator.GenerateHint(hintId, builder.Hint.Value.content, builder.Hint.Value.attributes);
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
            var attributes = builder.Label?.attributes;

            var resolvedContent = content ??
                new HtmlString(ModelHelper.GetDisplayName(ViewContext, AspFor.ModelExplorer, AspFor.Name));

            return Generator.GenerateLabel(ResolvedId, isPageHeading, resolvedContent, attributes);
        }
    }

    public abstract class FormGroupLabelTagHelperBase : TagHelper
    {
        private const string IsPageHeadingAttributeName = "is-page-heading";

        private protected FormGroupLabelTagHelperBase()
        {
        }

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; } = ComponentGenerator.FieldsetLegendDefaultIsPageHeading;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ? await output.GetChildContentAsync() : null;

            var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
            if (!formGroupContext.TrySetLabel(IsPageHeading, output.Attributes.ToAttributesDictionary(), childContent?.Snapshot()))
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

            var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
            if (!formGroupContext.TrySetHint(output.Attributes.ToAttributesDictionary(), childContent?.Snapshot()))
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

            var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
            if (!formGroupContext.TrySetErrorMessage(VisuallyHiddenText, output.Attributes.ToAttributesDictionary(), childContent?.Snapshot()))
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

    public class FormGroupBuilder
    {
        internal FormGroupBuilder()
        {
        }

        public (string visuallyHiddenText, IDictionary<string, string> attributes, IHtmlContent content)? ErrorMessage { get; private set; }

        public (IDictionary<string, string> attributes, IHtmlContent content)? Hint { get; private set; }

        public (bool isPageHeading, IDictionary<string, string> attributes, IHtmlContent content)? Label { get; private set; }

        // Internal for testing
        internal FormGroupRenderStage RenderStage { get; private set; } = FormGroupRenderStage.None;

        public bool TrySetErrorMessage(
            string visuallyHiddenText,
            IDictionary<string, string> attributes,
            IHtmlContent content)
        {
            if (RenderStage >= FormGroupRenderStage.ErrorMessage)
            {
                return false;
            }

            RenderStage = FormGroupRenderStage.ErrorMessage;
            ErrorMessage = (visuallyHiddenText, attributes, content);

            return true;
        }

        public bool TrySetHint(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (RenderStage >= FormGroupRenderStage.Hint)
            {
                return false;
            }

            RenderStage = FormGroupRenderStage.Hint;
            Hint = (attributes, content);

            return true;
        }

        public bool TrySetLabel(bool isPageHeading, IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (RenderStage >= FormGroupRenderStage.Label)
            {
                return false;
            }

            RenderStage = FormGroupRenderStage.Label;
            Label = (isPageHeading, attributes, content);

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

    public class FormGroupElementContext
    {
        internal FormGroupElementContext(bool haveError)
        {
            HaveError = haveError;
        }

        public bool HaveError { get; }
    }
}

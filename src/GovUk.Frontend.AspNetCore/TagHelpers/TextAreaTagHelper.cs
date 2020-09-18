using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-textarea")]
    [RestrictChildren("govuk-textarea-label", "govuk-textarea-hint", "govuk-textarea-error-message", "govuk-textarea-element")]
    public class TextAreaTagHelper : FormGroupTagHelperBase
    {
        private const string AttributesPrefix = "textarea-";
        private const string AutocompleteAttributeName = "autocomplete";
        private const string IdAttributeName = "id";
        private const string IsDisabledAttributeName = "disabled";
        private const string RowsAttributeName = "rows";
        private const string SpellcheckAttributeName = "spellcheck";

        public TextAreaTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName(AutocompleteAttributeName)]
        public string Autocomplete { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(IsDisabledAttributeName)]
        public bool IsDisabled { get; set; }

        [HtmlAttributeName(RowsAttributeName)]
        public int? Rows { get; set; }

        [HtmlAttributeName(SpellcheckAttributeName)]
        public bool? Spellcheck { get; set; }

        protected override FormGroupBuilder CreateFormGroupBuilder() => new TextAreaBuilder();

        protected override TagBuilder GenerateElement(
            TagHelperContext context,
            FormGroupBuilder builder,
            FormGroupElementContext elementContext)
        {
            var textAreaBuilder = (TextAreaBuilder)builder;

            if (textAreaBuilder.Content == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"Content must be specified when the '{AspForAttributeName}' attribute is not specified.");
            }

            var resolvedContent = textAreaBuilder.Content ??
                new HtmlString(Generator.GetModelValue(ViewContext, AspFor.ModelExplorer, AspFor.Name));

            return Generator.GenerateTextArea(
                elementContext.HaveError,
                ResolvedId,
                ResolvedName,
                Rows,
                DescribedBy,
                Autocomplete,
                Spellcheck,
                IsDisabled,
                resolvedContent,
                Attributes);
        }

        protected override string GetIdPrefix() => Id;
    }

    [HtmlTargetElement("govuk-textarea-label", ParentTag = "govuk-textarea")]
    public class TextAreaLabelTagHelper : FormGroupLabelTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-textarea-hint", ParentTag = "govuk-textarea")]
    public class TextAreaHintTagHelper : FormGroupHintTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-textarea-error-message", ParentTag = "govuk-textarea")]
    public class TextAreaErrorMessageTagHelper : FormGroupErrorMessageTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-textarea-element", ParentTag = "govuk-textarea")]
    public class TextAreaElementTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.TagMode == TagMode.StartTagAndEndTag ? await output.GetChildContentAsync() : null;

            var formGroupContext = (TextAreaBuilder)context.Items[typeof(FormGroupBuilder)];
            if (!formGroupContext.TrySetElementContent(childContent))
            {
                throw new InvalidOperationException($"Cannot render <{context.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    internal class TextAreaBuilder : FormGroupBuilder
    {
        public IHtmlContent Content { get; private set; }

        public bool TrySetElementContent(IHtmlContent content)
        {
            if (TrySetElementRenderStage())
            {
                Content = content;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

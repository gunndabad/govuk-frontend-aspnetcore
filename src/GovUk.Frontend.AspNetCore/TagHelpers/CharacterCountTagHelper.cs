using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-character-count")]
    [RestrictChildren("govuk-character-count-label", "govuk-character-count-hint", "govuk-character-count-error-message", "govuk-character-count-element")]
    public class CharacterCountTagHelper : TextAreaTagHelper
    {
        private const string MaxLengthAttributeName = "max-length";
        private const string MaxWordsLengthAttributeName = "max-words";
        private const string ThresholdAttributeName = "threshold";

        public CharacterCountTagHelper(IGovUkHtmlGenerator htmlGenerator, IModelHelper modelHelper)
            : base(htmlGenerator, modelHelper)
        {
        }

        [HtmlAttributeName(MaxLengthAttributeName)]
        public int? MaxLength { get; set; }

        [HtmlAttributeName(MaxWordsLengthAttributeName)]
        public int? MaxWords { get; set; }

        [HtmlAttributeName(ThresholdAttributeName)]
        public decimal? Threshold { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (MaxLength.HasValue && MaxWords.HasValue)
            {
                throw new InvalidOperationException($"The '{MaxLengthAttributeName}' and '{MaxWordsLengthAttributeName}' attributes are mutually exclusive.");
            }
            else if (!MaxLength.HasValue && !MaxWords.HasValue)
            {
                throw new InvalidOperationException($"One of the '{MaxLengthAttributeName}' and '{MaxWordsLengthAttributeName}' attributes must be specified.");
            }

            if (Threshold.HasValue && (Threshold.Value < 0 || Threshold.Value > 100))
            {
                throw new InvalidOperationException($"The '{ThresholdAttributeName}' attribute is invalid.");
            }

            return base.ProcessAsync(context, output);
        }

        protected override TagBuilder GenerateElement(
            TagHelperContext context,
            FormGroupBuilder builder,
            FormGroupElementContext elementContext)
        {
            var textArea = base.GenerateElement(context, builder, elementContext);
            textArea.AddCssClass("govuk-js-character-count");
            return textArea;
        }

        protected override TagBuilder GenerateContent(TagHelperContext context, FormGroupBuilder builder)
        {
            var generated = base.GenerateContent(context, builder);
            return Generator.GenerateCharacterCount(ResolvedId, MaxLength, MaxWords, Threshold, generated);
        }
    }

    [HtmlTargetElement("govuk-character-count-label", ParentTag = "govuk-character-count")]
    public class CharacterCountLabelTagHelper : TextAreaLabelTagHelper
    {
    }

    [HtmlTargetElement("govuk-character-count-hint", ParentTag = "govuk-character-count")]
    public class CharacterCountHintTagHelper : TextAreaHintTagHelper
    {
    }

    [HtmlTargetElement("govuk-character-count-error-message", ParentTag = "govuk-character-count")]
    public class CharacterCountErrorMessageTagHelper : TextAreaErrorMessageTagHelper
    {
    }

    [HtmlTargetElement("govuk-character-count-element", ParentTag = "govuk-character-count")]
    public class CharacterCountElementTagHelper : TextAreaElementTagHelper
    {
    }
}

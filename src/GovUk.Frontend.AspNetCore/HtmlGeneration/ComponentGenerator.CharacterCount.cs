using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public const string CharacterCountElement = "div";

        public TagBuilder GenerateCharacterCount(
            string textAreaId,
            int? maxLength,
            int? maxWords,
            decimal? threshold,
            IHtmlContent formGroup,
            AttributeDictionary? countMessageAttributes)
        {
            Guard.ArgumentNotNull(nameof(textAreaId), textAreaId);
            Guard.ArgumentNotNull(nameof(formGroup), formGroup);

            var tagBuilder = new TagBuilder(CharacterCountElement);
            tagBuilder.MergeCssClass("govuk-character-count");
            tagBuilder.Attributes.Add("data-module", "govuk-character-count");

            if (maxLength.HasValue)
            {
                tagBuilder.Attributes.Add("data-maxlength", maxLength.Value.ToString());
            }

            if (threshold.HasValue)
            {
                tagBuilder.Attributes.Add("data-threshold", threshold.Value.ToString());
            }

            if (maxWords.HasValue)
            {
                tagBuilder.Attributes.Add("data-maxwords", maxWords.Value.ToString());
            }

            tagBuilder.InnerHtml.AppendHtml(formGroup);
            tagBuilder.InnerHtml.AppendHtml(GenerateHint());

            return tagBuilder;

            IHtmlContent GenerateHint()
            {
                var hintId = $"{textAreaId}-info";

                var content = maxWords.HasValue ?
                    $"You can enter up to {maxWords} words" :
                    $"You can enter up to {maxLength} characters";
                var hintContent = new HtmlString(content);

                var attributes = countMessageAttributes.ToAttributeDictionary();
                attributes.MergeCssClass("govuk-character-count__message");

                return this.GenerateHint(hintId, hintContent, attributes);
            }
        }
    }
}

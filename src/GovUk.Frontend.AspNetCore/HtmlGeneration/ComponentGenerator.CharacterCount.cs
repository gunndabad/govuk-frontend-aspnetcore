using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    public const string CharacterCountElement = "div";

    public TagBuilder GenerateCharacterCount(
        string textAreaId,
        int? maxLength,
        int? maxWords,
        decimal? threshold,
        IHtmlContent formGroup,
        AttributeDictionary? countMessageAttributes,
        string? textAreaDescriptionText,
        (string Other, string One)? charactersUnderLimitText,
        string? charactersAtLimitText,
        (string Other, string One)? charactersOverLimitText,
        (string Other, string One)? wordsUnderLimitText,
        string? wordsAtLimitText,
        (string Other, string One)? wordsOverLimitText
    )
    {
        Guard.ArgumentNotNull(nameof(textAreaId), textAreaId);
        Guard.ArgumentNotNull(nameof(formGroup), formGroup);

        var hasNoLimit = maxLength is null && maxWords is null;

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

        if (hasNoLimit && textAreaDescriptionText is not null)
        {
            tagBuilder.AddPluralisedI18nAttributes("textarea-description", "other", textAreaDescriptionText);
        }

        if (charactersUnderLimitText is not null)
        {
            tagBuilder.AddPluralisedI18nAttributes(
                "characters-under-limit",
                ("other", charactersUnderLimitText!.Value.Other),
                ("one", charactersUnderLimitText.Value!.One)
            );
        }

        if (charactersAtLimitText is not null)
        {
            tagBuilder.Attributes.Add("data-i18n.characters-at-limit", charactersAtLimitText);
        }

        if (charactersOverLimitText is not null)
        {
            tagBuilder.AddPluralisedI18nAttributes(
                "characters-over-limit",
                ("other", charactersOverLimitText!.Value.Other),
                ("one", charactersOverLimitText.Value!.One)
            );
        }

        if (wordsUnderLimitText is not null)
        {
            tagBuilder.AddPluralisedI18nAttributes(
                "words-under-limit",
                ("other", wordsUnderLimitText!.Value.Other),
                ("one", wordsUnderLimitText.Value!.One)
            );
        }

        if (wordsAtLimitText is not null)
        {
            tagBuilder.Attributes.Add("data-i18n.words-at-limit", wordsAtLimitText);
        }

        if (wordsOverLimitText is not null)
        {
            tagBuilder.AddPluralisedI18nAttributes(
                "words-over-limit",
                ("other", wordsOverLimitText!.Value.Other),
                ("one", wordsOverLimitText.Value!.One)
            );
        }

        tagBuilder.InnerHtml.AppendHtml(formGroup);
        tagBuilder.InnerHtml.AppendHtml(GenerateHint());

        return tagBuilder;

        IHtmlContent GenerateHint()
        {
            var hintId = $"{textAreaId}-info";

            var content = hasNoLimit
                ? ""
                : (
                    textAreaDescriptionText
                    ?? $"You can enter up to %{{count}} {(maxWords.HasValue ? "words" : "characters")}"
                ).Replace("%{count}", (maxWords.HasValue ? maxWords : maxLength).ToString());

            var hintContent = new HtmlString(HtmlEncoder.Default.Encode(content));

            var attributes = countMessageAttributes.ToAttributeDictionary();
            attributes.MergeCssClass("govuk-character-count__message");

            return this.GenerateHint(hintId, hintContent, attributes);
        }
    }
}

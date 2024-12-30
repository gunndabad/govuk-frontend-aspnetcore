using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    public const string CharacterCountElement = "div";
    public const string MaxLengthAttributeName = "max-length"; // CharacterCountTagHelper.MaxLengthAttributeName
    public const string MaxWordsLengthAttributeName = "max-words"; // CharacterCountTagHelper.MaxWordsLengthAttributeName


    public TagBuilder GenerateCharacterCount(
        bool? haveError,
        string textAreaId,
        int? maxLength,
        int? maxWords,
        decimal? threshold,
        IHtmlContent content,
        AttributeDictionary countMessageAttributes,
        string? textAreaDescriptionText,
        (string Other, string One)? charactersUnderLimitText,
        string? charactersAtLimitText,
        (string Other, string One)? charactersOverLimitText,
        (string Other, string One)? wordsUnderLimitText,
        string? wordsAtLimitText,
        (string Other, string One)? wordsOverLimitText)
    {
        Guard.ArgumentNotNull(nameof(textAreaId), textAreaId);

        if (maxLength.HasValue && maxWords.HasValue)
        {
            throw new InvalidOperationException($"Only one of the '{MaxLengthAttributeName}' or '{MaxWordsLengthAttributeName}' attributes can be specified.");
        }

        var hasNoLimit = maxLength is null && maxWords is null; // TODO: This logic is reused in many places -- centralise it?


        // Based on ComponentGenerator.GenerateFormGroup (cannot inherit / reference from here)
        var tagBuilder = new TagBuilder(CharacterCountElement);
        tagBuilder.MergeCssClass("govuk-form-group");
        tagBuilder.MergeCssClass("govuk-character-count");

        if (haveError.HasValue && haveError.Value)
        {
            tagBuilder.MergeCssClass("govuk-form-group--error");
        }

        // Since govuk-frontend v5.3.0, `data-*` attributes need to be added to
        // the outermost element of the character count component.
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

        if (!hasNoLimit && textAreaDescriptionText is not null)
        {
            tagBuilder.AddPluralisedI18nAttributes("textarea-description", "other", textAreaDescriptionText);
        }

        if (charactersUnderLimitText is not null)
        {
            tagBuilder.AddPluralisedI18nAttributes(
                "characters-under-limit",
                ("other", charactersUnderLimitText!.Value.Other),
                ("one", charactersUnderLimitText.Value!.One));
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
                ("one", charactersOverLimitText.Value!.One));
        }

        if (wordsUnderLimitText is not null)
        {
            tagBuilder.AddPluralisedI18nAttributes(
                "words-under-limit",
                ("other", wordsUnderLimitText!.Value.Other),
                ("one", wordsUnderLimitText.Value!.One));
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
                ("one", wordsOverLimitText.Value!.One));
        }

        // Above is logic to add content to the outer-element.
        // Do not generate content here, instead defer/delegate to `content`.
        tagBuilder.InnerHtml.AppendHtml(content);

        return tagBuilder;
    }
}

using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

partial class DefaultComponentGenerator
{
    internal const string CharacterCountElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateCharacterCount(CharacterCountOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var hasNoLimit = options.MaxLength is null && options.MaxWords is null;
        var id = options.Id ?? options.Name;

        IHtmlContent? textareaDescriptionText = null;
        if (!hasNoLimit)
        {
            var textareaDescriptionLength = options.MaxWords ?? options.MaxLength;

            textareaDescriptionText = new HtmlString(
                (options.TextareaDescriptionText.NormalizeEmptyString()?.ToHtmlString() ??
                    $"You can enter up to %{{count}} {(options.MaxWords is not null ? "words" : "characters")}")
                .Replace("%{count}", textareaDescriptionLength.ToString()));
        }

        return GenerateTextarea(new TextareaOptions
        {
            Id = id,
            Name = options.Name,
            DescribedBy = new HtmlString($"{id!.ToHtmlString()}-info"),
            Rows = options.Rows,
            Spellcheck = options.Spellcheck,
            Value = options.Value,
            FormGroup = new FormGroupOptions()
            {
                Classes = new HtmlString($"govuk-character-count {options.FormGroup?.Classes?.ToHtmlString()}".TrimEnd()),
                Attributes = new EncodedAttributesDictionaryBuilder(options.FormGroup?.Attributes)
                    .With("data-module", "govuk-character-count", encodeValue: false)
                    .WhenNotNull(options.MaxLength,
                        (maxLength, b) => b.With("data-maxlength", maxLength.ToString()!, encodeValue: false))
                    .WhenNotNull(options.Threshold,
                        (threshold, b) => b.With("data-threshold", threshold.ToString()!, encodeValue: false))
                    .WhenNotNull(options.MaxWords,
                        (maxWords, b) => b.With("data-maxwords", maxWords.ToString()!, encodeValue: false))
                    .When(
                        hasNoLimit && options.TextareaDescriptionText.NormalizeEmptyString() is not null,
                        b => b.With("data-i18n.textarea-description.other", options.TextareaDescriptionText!))
                    .WithWhenNotNull(options.CharactersUnderLimitText?.One, "data-i18n.characters-under-limit.one")
                    .WithWhenNotNull(options.CharactersUnderLimitText?.Other, "data-i18n.characters-under-limit.other")
                    .WithWhenNotNull(options.CharactersAtLimitText, "data-i18n.characters-at-limit")
                    .WithWhenNotNull(options.CharactersOverLimitText?.One, "data-i18n.characters-over-limit.one")
                    .WithWhenNotNull(options.CharactersOverLimitText?.Other, "data-i18n.characters-over-limit.other")
                    .WithWhenNotNull(options.WordsUnderLimitText?.One, "data-i18n.words-under-limit.one")
                    .WithWhenNotNull(options.WordsUnderLimitText?.Other, "data-i18n.words-under-limit.other")
                    .WithWhenNotNull(options.WordsAtLimitText, "data-i18n.words-at-limit")
                    .WithWhenNotNull(options.WordsOverLimitText?.One, "data-i18n.words-over-limit.one")
                    .WithWhenNotNull(options.WordsOverLimitText?.Other, "data-i18n.words-over-limit.other"),
                BeforeInput = options.FormGroup?.BeforeInput,
                AfterInput = new FormGroupOptionsAfterInput()
                {
                    Html = new HtmlString(
                        GenerateHint(
                            new HintOptions()
                            {
                                Attributes = options.CountMessage?.Attributes,
                                Html = textareaDescriptionText,
                                Id = new HtmlString($"{id!.ToHtmlString()}-info"),
                                Classes = new HtmlString(
                                    $"govuk-character-count__message {options.CountMessage?.Classes?.ToHtmlString()}".TrimEnd())
                            },
                            allowMissingContent: true).ToHtmlString() +
                        GetEncodedTextOrHtml(options.FormGroup?.AfterInput?.Text, options.FormGroup?.AfterInput?.Html)?.ToHtmlString())
                }
            },
            Classes = new HtmlString($"govuk-js-character-count {options.Classes?.ToHtmlString()}".TrimEnd()),
            Label = (options.Label ?? new LabelOptions()) with { For = id },
            Hint = options.Hint,
            ErrorMessage = options.ErrorMessage,
            Attributes = options.Attributes
        });
    }
}

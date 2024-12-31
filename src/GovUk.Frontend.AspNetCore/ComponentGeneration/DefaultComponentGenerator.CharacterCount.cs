using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string CharacterCountElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateCharacterCount(CharacterCountOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var hasNoLimit = options.MaxLength is null && options.MaxWords is null;

        return new HtmlTagBuilder(CharacterCountElement)
            .WithCssClass("govuk-character-count")
            .WithAttribute("data-module", "govuk-character-count", encodeValue: false)
            .WhenNotNull(options.MaxLength,
                (maxLength, b) => b.WithAttribute("data-maxlength", maxLength.ToString()!, encodeValue: false))
            .WhenNotNull(options.Threshold,
                (threshold, b) => b.WithAttribute("data-threshold", threshold.ToString()!, encodeValue: false))
            .WhenNotNull(options.MaxWords,
                (maxWords, b) => b.WithAttribute("data-maxwords", maxWords.ToString()!, encodeValue: false))
            .When(
                hasNoLimit && options.TextareaDescriptionText.NormalizeEmptyString() is not null,
                b => b.WithAttribute("data-i18n.textarea-description.other", options.TextareaDescriptionText!))
            .WithAttributeWhenNotNull(options.CharactersUnderLimitText?.One, "data-i18n.characters-under-limit.one")
            .WithAttributeWhenNotNull(options.CharactersUnderLimitText?.Other, "data-i18n.characters-under-limit.other")
            .WithAttributeWhenNotNull(options.CharactersAtLimitText, "data-i18n.characters-at-limit")
            .WithAttributeWhenNotNull(options.CharactersOverLimitText?.One, "data-i18n.characters-over-limit.one")
            .WithAttributeWhenNotNull(options.CharactersOverLimitText?.Other, "data-i18n.characters-over-limit.other")
            .WithAttributeWhenNotNull(options.WordsUnderLimitText?.One, "data-i18n.words-under-limit.one")
            .WithAttributeWhenNotNull(options.WordsUnderLimitText?.Other, "data-i18n.words-under-limit.other")
            .WithAttributeWhenNotNull(options.WordsAtLimitText, "data-i18n.words-at-limit")
            .WithAttributeWhenNotNull(options.WordsOverLimitText?.One, "data-i18n.words-over-limit.one")
            .WithAttributeWhenNotNull(options.WordsOverLimitText?.Other, "data-i18n.words-over-limit.other")
            .WithAppendedHtml(GenerateTextarea(new TextareaOptions
            {
                Id = options.Id,
                Name = options.Name,
                DescribedBy = new HtmlString($"{options.Id!.ToHtmlString()}-info"),
                Rows = options.Rows,
                Spellcheck = options.Spellcheck,
                Value = options.Value,
                FormGroup = options.FormGroup,
                Classes = new HtmlString($"govuk-js-character-count {options.Classes?.ToHtmlString()}".TrimEnd()),
                Label = (options.Label ?? new LabelOptions()) with { For = options.Id },
                Hint = options.Hint,
                ErrorMessage = options.ErrorMessage,
                Attributes = options.Attributes
            }))
            .WithAppendedHtml(() =>
            {
                IHtmlContent? content = null;

                if (!hasNoLimit)
                {
                    var textareaDescriptionLength = options.MaxWords ?? options.MaxLength;

                    content = new HtmlString(
                        (options.TextareaDescriptionText.NormalizeEmptyString()?.ToHtmlString() ??
                            $"You can enter up to %{{count}} {(options.MaxWords is not null ? "words" : "characters")}")
                        .Replace("%{count}", textareaDescriptionLength.ToString()));
                }

                return GenerateHint(
                    new HintOptions()
                    {
                        Html = content,
                        Id = new HtmlString($"{options.Id!.ToHtmlString()}-info"),
                        Classes = new HtmlString(
                            $"govuk-character-count__message {options.CountMessage?.Classes?.ToHtmlString()}".TrimEnd())
                    },
                    allowMissingContent: true);
            });
    }
}

using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("character-count", typeof(OptionsJson.CharacterCount))]
    public void CharacterCount(ComponentTestCaseData<OptionsJson.CharacterCount> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                /*
                 * Structure:
                 * - Label
                 * - Label hint
                 * - Error message
                 * - Textarea
                 * - Count-remaining hint
                 */

                var formGroup = new TagBuilder("div");

                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes)
                    .MergeAttribute("class", "govuk-js-character-count");   // FIXME yuk

                var describedBy = new List<string>();

                IHtmlContent? label = null;
                if (options.Label != null)
                {
                    label = BuildLabel(_componentGenerator, options.Label with { For = options.Id });
                }

                IHtmlContent? hint = null;
                if (options.Hint != null)
                {
                    var hintId = $"{options.Id}-hint";
                    describedBy.Add(hintId);

                    hint = BuildHint(_componentGenerator, options.Hint with { Id = hintId });
                }

                IHtmlContent? errorMessage = null;
                if (options.ErrorMessage != null)
                {
                    var errorId = $"{options.Id}-error";
                    describedBy.Add(errorId);

                    errorMessage = BuildErrorMessage(
                        _componentGenerator,
                        options.ErrorMessage with { Id = errorId });
                }

                // Character count message and textarea description
                var countMessageId = $"{options.Id}-info";
                describedBy.Add(countMessageId);

                var countMessageAttributes = new AttributeDictionary();
                countMessageAttributes
                    .MergeAttribute("id", countMessageId)
                    .MergeCssClass("govuk-hint govuk-character-count__message");

                if (!string.IsNullOrEmpty(options.CountMessage?.Classes))
                {
                    countMessageAttributes.MergeCssClass(options.CountMessage.Classes);
                }

                var hasNoLimit = options.MaxWords is null && options.MaxLength is null; // TODO: This logic is reused in many places -- centralise it?

                // TODO / FIXME: Why are we reproducing / replicating functionality here? Multiple implementations means scope for errors and inconsistencies.
                var countMessageText = hasNoLimit
                    ? null
                    : (options.TextareaDescriptionText ??
                       $"You can enter up to %{{count}} {(options.MaxWords.HasValue ? "words" : "characters")}")
                    .Replace("%{count}", Convert.ToString(options.MaxWords ?? options.MaxLength));

                var countMessage = new TagBuilder("div");
                countMessage.MergeAttributes(countMessageAttributes);

                if (hasNoLimit || string.IsNullOrEmpty(countMessageText))
                {
                    countMessage.InnerHtml.AppendHtml("&nbsp;"); // Ensure placeholder <div>
                }
                else
                {
                    countMessage.InnerHtml.AppendHtml(HtmlEncoder.Default.Encode(countMessageText));
                }

                // TODO/FIXME: Unclear if issue/inconsistency with code, tests, or the fixture data supplied from govuk-frontend
                if (!hasNoLimit && options.TextareaDescriptionText != null)
                {
                    attributes.Add("data-i18n.textarea-description.other", options.TextareaDescriptionText);
                }

                var textarea = generator.GenerateTextArea(
                    haveError: options.ErrorMessage != null, // TODO / FIXME: Is this reasonable/correct logic?
                    id: options.Id,
                    name: options.Name,
                    rows: options.Rows ?? ComponentGenerator.TextAreaDefaultRows,
                    describedBy: string.Join(" ", describedBy),
                    autocomplete: null,
                    spellcheck: options.Spellcheck,
                    disabled: ComponentGenerator.TextAreaDefaultDisabled,
                    content: new HtmlString(options.Value),
                    attributes: attributes);



                var formGroupClasses = "govuk-form-group";
                if (options.ErrorMessage != null)
                {
                    formGroupClasses += " govuk-form-group--error";
                }

                if (!string.IsNullOrEmpty(options.FormGroup?.Classes))
                {
                    formGroupClasses += " " + options.FormGroup.Classes;
                }

                formGroup.MergeCssClass(formGroupClasses + " govuk-character-count");
                formGroup.MergeAttribute("data-module", "govuk-character-count");

                if (options.MaxLength.HasValue)
                {
                    formGroup.Attributes.Add("data-maxlength", options.MaxLength.Value.ToString());
                }

                if (options.MaxWords.HasValue)
                {
                    formGroup.Attributes.Add("data-maxwords", options.MaxWords.Value.ToString());
                }

                if (options.Threshold.HasValue)
                {
                    formGroup.Attributes.Add("data-threshold", options.Threshold.Value.ToString());
                }


                // TODO / FIXME: re-use existing i8n attribute functionality? Is it accessible from here?
                if (options.CharactersUnderLimitText?.Other != null)
                {
                    formGroup.Attributes.Add("data-i18n.characters-under-limit.other",
                        options.CharactersUnderLimitText.Other);
                }

                if (options.CharactersUnderLimitText?.One != null)
                {
                    formGroup.Attributes.Add("data-i18n.characters-under-limit.one",
                        options.CharactersUnderLimitText.One);
                }

                if (options.CharactersAtLimitText != null)
                {
                    formGroup.Attributes.Add("data-i18n.characters-at-limit", options.CharactersAtLimitText);
                }

                if (options.CharactersOverLimitText?.Other != null)
                {
                    formGroup.Attributes.Add("data-i18n.characters-over-limit.other",
                        options.CharactersOverLimitText.Other);
                }

                if (options.CharactersOverLimitText?.One != null)
                {
                    formGroup.Attributes.Add("data-i18n.characters-over-limit.one",
                        options.CharactersOverLimitText.One);
                }

                if (options.WordsUnderLimitText?.Other != null)
                {
                    formGroup.Attributes.Add("data-i18n.words-under-limit.other",
                        options.WordsUnderLimitText.Other);
                }

                if (options.WordsUnderLimitText?.One != null)
                {
                    formGroup.Attributes.Add("data-i18n.words-under-limit.one",
                        options.WordsUnderLimitText.One);
                }

                if (options.WordsAtLimitText != null)
                {
                    formGroup.Attributes.Add("data-i18n.words-at-limit", options.WordsAtLimitText);
                }

                if (options.WordsOverLimitText?.Other != null)
                {
                    formGroup.Attributes.Add("data-i18n.words-over-limit.other",
                        options.WordsOverLimitText.Other);
                }

                if (options.WordsOverLimitText?.One != null)
                {
                    formGroup.Attributes.Add("data-i18n.words-over-limit.one",
                        options.WordsOverLimitText.One);
                }

                if (!hasNoLimit && options.TextareaDescriptionText != null) // TODO: This logic is reused in many places -- centralise it?
                {
                    formGroup.Attributes.Add("data-i18n.textarea-description.other",
                        options.TextareaDescriptionText);
                }

                // Add error, label, hint, textarea, and count message components to the form group
                if (label != null)
                {
                    formGroup.InnerHtml.AppendHtml(label);
                }

                if (hint != null)
                {
                    formGroup.InnerHtml.AppendHtml(hint);
                }

                if (errorMessage != null)
                {
                    formGroup.InnerHtml.AppendHtml(errorMessage);
                }

                formGroup.InnerHtml.AppendHtml(textarea);
                formGroup.InnerHtml.AppendHtml(countMessage);

                return formGroup.ToHtmlString();
            });
}

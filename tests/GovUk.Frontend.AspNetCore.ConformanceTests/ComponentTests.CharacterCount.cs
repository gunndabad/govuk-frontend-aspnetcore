using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
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
                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes)
                    .MergeAttribute("class", "govuk-js-character-count");   // FIXME yuk

                var labelOptions = options.Label != null ?
                    options.Label with { For = options.Id } :
                    null;

                var hintOptions = options.Hint != null ?
                    options.Hint with { Id = options.Id + "-hint" } :
                    null;

                var errorMessageOptions = options.ErrorMessage != null ?
                    options.ErrorMessage with { Id = options.Id + "-error" } :
                    null;

                var countMessageAttributes = new AttributeDictionary()
                    .MergeAttribute("class", options.CountMessage?.Classes);

                var content = new HtmlString(GenerateFormGroup(
                    labelOptions,
                    hintOptions,
                    errorMessageOptions,
                    options.FormGroup,
                    fieldset: null,
                    generateElement: (haveError, describedBy) =>
                    {
                        AppendToDescribedBy(ref describedBy, options.Id + "-info");

                        return generator.GenerateTextArea(
                            haveError,
                            options.Id,
                            options.Name,
                            options.Rows ?? ComponentGenerator.TextAreaDefaultRows,
                            describedBy,
                            /* spellcheck: */ null,
                            options.Spellcheck,
                            ComponentGenerator.TextAreaDefaultDisabled,
                            new HtmlString(options.Value),
                            attributes);
                    }));

                return generator.GenerateCharacterCount(
                        options.Id,
                        options.MaxLength,
                        options.MaxWords,
                        options.Threshold,
                        content,
                        countMessageAttributes,
                        options.TextareaDescriptionText,
                        options.CharactersUnderLimitText is not null ? (options.CharactersUnderLimitText.Other, options.CharactersUnderLimitText.One) : null,
                        options.CharactersAtLimitText,
                        options.CharactersOverLimitText is not null ? (options.CharactersOverLimitText.Other, options.CharactersOverLimitText.One) : null,
                        options.WordsUnderLimitText is not null ? (options.WordsUnderLimitText.Other, options.WordsUnderLimitText.One) : null,
                        options.WordsAtLimitText,
                        options.WordsOverLimitText is not null ? (options.WordsOverLimitText.Other, options.WordsOverLimitText.One) : null)
                    .ToHtmlString();
            });
}

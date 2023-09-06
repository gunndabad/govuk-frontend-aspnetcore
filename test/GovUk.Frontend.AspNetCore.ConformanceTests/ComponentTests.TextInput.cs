using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("input", typeof(OptionsJson.TextInput))]
    public void TextInput(ComponentTestCaseData<OptionsJson.TextInput> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var type = options.Type ?? ComponentGenerator.InputDefaultType;

                var prefixContent = options.Prefix != null ?
                    TextOrHtmlHelper.GetHtmlContent(options.Prefix.Text, options.Prefix.Html) :
                    null;

                var prefixAttributes = options.Prefix?.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Prefix?.Classes);

                var suffixContent = options.Suffix != null ?
                    TextOrHtmlHelper.GetHtmlContent(options.Suffix.Text, options.Suffix.Html) :
                    null;

                var suffixAttributes = options.Suffix?.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Suffix?.Classes);

                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes);

                var labelOptions = options.Label != null ?
                    options.Label with { For = options.Id } :
                    null;

                var hintOptions = options.Hint != null ?
                    options.Hint with { Id = options.Id + "-hint" } :
                    null;

                var errorMessageOptions = options.ErrorMessage != null ?
                    options.ErrorMessage with { Id = options.Id + "-error" } :
                    null;

                return GenerateFormGroup(
                    labelOptions,
                    hintOptions,
                    errorMessageOptions,
                    options.FormGroup,
                    fieldset: null,
                    generateElement: (haveError, describedBy) =>
                    {
                        AppendToDescribedBy(ref describedBy, options.DescribedBy);

                        return generator.GenerateTextInput(
                            haveError,
                            options.Id,
                            options.Name,
                            type,
                            options.Value,
                            describedBy,
                            options.Autocomplete,
                            options.Pattern,
                            options.Inputmode,
                            options.Spellcheck,
                            ComponentGenerator.InputDefaultDisabled,
                            attributes,
                            prefixContent,
                            prefixAttributes,
                            suffixContent,
                            suffixAttributes);
                    });
            });
}

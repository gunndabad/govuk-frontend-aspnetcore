using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData("textarea", typeof(OptionsJson.Textarea))]
        public void Textarea(ComponentTestCaseData<OptionsJson.Textarea> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
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

                            return generator.GenerateTextArea(
                                haveError,
                                options.Id,
                                options.Name,
                                options.Rows ?? ComponentGenerator.TextAreaDefaultRows,
                                describedBy,
                                options.Autocomplete,
                                options.Spellcheck,
                                ComponentGenerator.TextAreaDefaultDisabled,
                                new HtmlString(options.Value),
                                attributes);
                        });
                });
    }
}

using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("file-upload", typeof(OptionsJson.FileUpload), exclude: "with value")]
    public void FileUpload(ComponentTestCaseData<OptionsJson.FileUpload> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var id = options.Id ?? options.Name;

                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes);

                var labelOptions = options.Label != null ?
                    options.Label with { For = id } :
                    null;

                var hintOptions = options.Hint != null ?
                    options.Hint with { Id = id + "-hint" } :
                    null;

                var errorMessageOptions = options.ErrorMessage != null ?
                    options.ErrorMessage with { Id = id + "-error" } :
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

                        return generator.GenerateFileUpload(
                            haveError,
                            options.Id,
                            options.Name,
                            describedBy,
                            options.Disabled,
                            attributes);
                    });
            });
}

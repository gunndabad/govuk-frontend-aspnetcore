using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData("file-upload", typeof(OptionsJson.FileUpload), exclude: "with value")]
        public void FileUpload(ComponentTestCaseData<OptionsJson.FileUpload> data) =>
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

                            return generator.GenerateFileUpload(
                                haveError,
                                options.Id,
                                options.Name,
                                describedBy,
                                attributes);
                        });
                });
    }
}

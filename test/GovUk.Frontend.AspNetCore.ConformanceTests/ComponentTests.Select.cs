using System.Linq;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData(
            "select",
            typeof(OptionsJson.Select),
            exclude: new[]
            {
                "with falsey values"
            })]
        public void Select(ComponentTestCaseData<OptionsJson.Select> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var disabled = ComponentDefaults.Select.Disabled;

                    var items = options.Items.OrEmpty()
                        .Select(i => new SelectListItem()
                        {
                            Attributes = i.Attributes.ToAttributesDictionary(),
                            Content = new HtmlString(i.Text),
                            IsDisabled = i.Disabled ?? false,
                            IsSelected = i.Selected ?? false,
                            Value = i.Value ?? string.Empty
                        });

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
                        (haveError, describedBy) =>
                        {
                            AppendToDescribedBy(ref describedBy, options.DescribedBy);

                            return generator.GenerateSelect(
                                haveError,
                                options.Id,
                                options.Name,
                                describedBy,
                                disabled,
                                items,
                                attributes);
                        });
                });
    }
}

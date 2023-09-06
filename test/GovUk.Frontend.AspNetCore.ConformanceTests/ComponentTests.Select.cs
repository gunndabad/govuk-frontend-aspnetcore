using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData(
        "select",
        typeof(OptionsJson.Select),
        exclude: new[]
        {
            "with falsey values",
            "item selected overrides value"  // Fixture doesn't have an 'id' :-(
        })]
    public void Select(ComponentTestCaseData<OptionsJson.Select> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var disabled = ComponentGenerator.SelectDefaultDisabled;

                var items = options.Items.OrEmpty()
                    .Select(i => new SelectItem()
                    {
                        Attributes = i.Attributes.ToAttributesDictionary(),
                        Content = new HtmlString(i.Text),
                        Disabled = i.Disabled ?? ComponentGenerator.SelectItemDefaultDisabled,
                        Selected = i.Selected ?? (options.Value == i.Value ? (bool?)true : null) ?? ComponentGenerator.SelectItemDefaultSelected,
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
                    fieldset: null,
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

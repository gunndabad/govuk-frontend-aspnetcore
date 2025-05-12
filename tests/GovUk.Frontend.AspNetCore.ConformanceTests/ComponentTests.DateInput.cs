using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData(
        "date-input",
        typeof(OptionsJson.DateInput),
        exclude: new[]
        {
            "day and month",
            "month and year",
        })]
    public void DateInput(ComponentTestCaseData<OptionsJson.DateInput> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                if (options.Fieldset is not null)
                {
                    options.Fieldset.Role = "group";
                }

                var attributes = options.Attributes.ToAttributesDictionary()
                   .MergeAttribute("class", options.Classes);

                var hintOptions = options.Hint is not null ?
                    options.Hint with { Id = options.Id + "-hint" } :
                    null;

                var errorMessageOptions = options.ErrorMessage is not null ?
                    options.ErrorMessage with { Id = options.Id + "-error" } :
                    null;

                return GenerateFormGroup(
                    label: null,
                    hintOptions,
                    errorMessageOptions,
                    options.FormGroup,
                    options.Fieldset,
                    generateElement: (haveError, describedBy) =>
                    {
                        var day = CreateDateInputItem(0, "day", "govuk-input--width-2");
                        var month = CreateDateInputItem(1, "month", "govuk-input--width-2");
                        var year = CreateDateInputItem(2, "year", "govuk-input--width-4");

                        return generator.GenerateDateInput(
                            options.Id,
                            ComponentGenerator.DateInputDefaultDisabled,
                            day,
                            month,
                            year,
                            attributes);

                        DateInputItem CreateDateInputItem(int index, string defaultName, string defaultClass)
                        {
                            DateInputItem inputItem;

                            if (options.Items is not null && options.Items.Count >= index + 1)
                            {
                                var item = options.Items[index];

                                inputItem = new DateInputItem()
                                {
                                    Attributes = item.Attributes.ToAttributesDictionary()
                                        .MergeAttribute("class", item.Classes),
                                    Autocomplete = item.Autocomplete,
                                    Id = item.Id,
                                    InputMode = item.InputMode,
                                    LabelContent = item.Label is not null ? new HtmlString(item.Label) : null,
                                    Name = item.Name,
                                    Pattern = item.Pattern,
                                    Value = item.Value
                                };
                            }
                            else
                            {
                                inputItem = new DateInputItem()
                                {
                                    Attributes = new Microsoft.AspNetCore.Mvc.ViewFeatures.AttributeDictionary()
                                    {
                                        { "class", defaultClass }
                                    },
                                    Name = defaultName
                                };
                            }

                            inputItem.Id ??= $"{options.Id}-{inputItem.Name}";
                            inputItem.LabelContent ??= new HtmlString(Capitalize(inputItem.Name));
                            inputItem.InputMode ??= ComponentGenerator.DateInputDefaultInputMode;

                            if (options.NamePrefix is not null)
                            {
                                inputItem.Name = $"{options.NamePrefix}-{inputItem.Name}";
                            }

                            return inputItem;
                        }
                    });

                static string Capitalize(string value) => value[0..1].ToUpper() + value[1..];
            },
            excludeDiff: diff =>
            {
                // Some tests have incomplete items specified (e.g. only Day) but we will always generate all 3 items.
                // Exclude the errors from the mismatch

                if (diff is UnexpectedNodeDiff unexpectedNodeDiff &&
                    (unexpectedNodeDiff.Test.Path == "div(0) > div(0) > div(1)" || unexpectedNodeDiff.Test.Path == "div(0) > div(0) > div(2)"))
                {
                    return true;
                }

                return false;
            });
}

using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("radios", typeof(OptionsJson.Radios), exclude: "with falsey items")]
    public void Radios(ComponentTestCaseData<OptionsJson.Radios> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var idPrefix = options.IdPrefix ?? options.Name;

                var items = options.Items.Select(
                    (item, index) =>
                        item.Divider != null
                            ? (RadiosItemBase)new RadiosItemDivider() { Content = new HtmlString(item.Divider) }
                            : (RadiosItemBase)
                                new RadiosItem()
                                {
                                    Conditional =
                                        item.Conditional?.Html != null
                                            ? new RadiosItemConditional()
                                            {
                                                Content = new HtmlString(item.Conditional.Html),
                                            }
                                            : null,
                                    LabelContent = TextOrHtmlHelper.GetHtmlContent(item.Text, item.Html),
                                    Hint =
                                        item.Hint != null
                                            ? new RadiosItemHint()
                                            {
                                                Attributes = item.Hint.Attributes?.ToAttributesDictionary(),
                                                Content = TextOrHtmlHelper.GetHtmlContent(
                                                    item.Hint.Text,
                                                    item.Hint.Html
                                                ),
                                            }
                                            : null,
                                    Id = item.Id,
                                    InputAttributes = item.Attributes.ToAttributesDictionary(),
                                    LabelAttributes = item
                                        .Label?.Attributes.ToAttributesDictionary()
                                        .MergeAttribute("class", item.Label?.Classes),
                                    Checked =
                                        item.Checked
                                        ?? (options.Value == item.Value ? (bool?)true : null)
                                        ?? ComponentGenerator.CheckboxesItemDefaultChecked,
                                    Disabled = item.Disabled ?? ComponentGenerator.CheckboxesItemDefaultDisabled,
                                    Value = item.Value ?? string.Empty,
                                }
                );

                var attributes = options.Attributes.ToAttributesDictionary().MergeAttribute("class", options.Classes);

                var hintOptions = options.Hint != null ? options.Hint with { Id = idPrefix + "-hint" } : null;

                var errorMessageOptions =
                    options.ErrorMessage != null ? options.ErrorMessage with { Id = idPrefix + "-error" } : null;

                return GenerateFormGroup(
                    label: null,
                    hintOptions,
                    errorMessageOptions,
                    options.FormGroup,
                    options.Fieldset,
                    (haveError, describedBy) =>
                    {
                        return generator.GenerateRadios(
                            options.IdPrefix,
                            options.Name,
                            items: items,
                            attributes: attributes
                        );
                    }
                );
            }
        );
}

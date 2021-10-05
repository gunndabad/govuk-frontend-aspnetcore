using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData(
            "checkboxes",
            typeof(OptionsJson.Checkboxes),
            exclude: "with falsey values")]
        public void Checkboxes(ComponentTestCaseData<OptionsJson.Checkboxes> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var idPrefix = options.IdPrefix ?? options.Name;

                    var items = options.Items
                        .Select((item, index) =>
                            item.Divider != null ?
                                (CheckboxesItemBase)new CheckboxesItemDivider() { Content = new HtmlString(item.Divider) } :
                                (CheckboxesItemBase)new CheckboxesItem()
                                {
                                    Behavior = item.Behaviour == "exclusive" ? CheckboxesItemBehavior.Exclusive : CheckboxesItemBehavior.Default,
                                    Conditional = item.Conditional?.Html != null ?
                                    new CheckboxesItemConditional()
                                    {
                                        Content = new HtmlString(item.Conditional.Html)
                                    } :
                                    null,
                                        LabelContent = TextOrHtmlHelper.GetHtmlContent(item.Text, item.Html),
                                        Hint = item.Hint != null ?
                                    new CheckboxesItemHint()
                                    {
                                        Attributes = item.Hint.Attributes?.ToAttributesDictionary(),
                                        Content = TextOrHtmlHelper.GetHtmlContent(item.Hint.Text, item.Hint.Html)
                                    } :
                                    null,
                                        Id = item.Id,
                                        InputAttributes = item.Attributes.ToAttributesDictionary(),
                                        LabelAttributes = item.Label?.Attributes.ToAttributesDictionary()
                                            .MergeAttribute("class", item.Label?.Classes),
                                        Checked = item.Checked ?? ComponentGenerator.CheckboxesItemDefaultChecked,
                                        Disabled = item.Disabled ?? ComponentGenerator.CheckboxesItemDefaultDisabled,
                                        Name = item.Name,
                                        Value = item.Value ?? string.Empty
                                }
                        );

                    var attributes = options.Attributes.ToAttributesDictionary()
                       .MergeAttribute("class", options.Classes);

                    var hintOptions = options.Hint != null ?
                        options.Hint with { Id = idPrefix + "-hint" } :
                        null;

                    var errorMessageOptions = options.ErrorMessage != null ?
                        options.ErrorMessage with { Id = idPrefix + "-error" } :
                        null;

                    return GenerateFormGroup(
                        label: null,
                        hintOptions,
                        errorMessageOptions,
                        options.FormGroup,
                        options.Fieldset,
                        (haveError, describedBy) =>
                        {
                            AppendToDescribedBy(ref describedBy, options.DescribedBy);

                            return generator.GenerateCheckboxes(
                                idPrefix,
                                options.Name,
                                describedBy,
                                hasFieldset: options.Fieldset != null,
                                items: items,
                                attributes: attributes);
                        });
                });
    }
}

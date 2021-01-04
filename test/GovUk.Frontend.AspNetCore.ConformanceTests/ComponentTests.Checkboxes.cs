using System.Linq;
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
                        .Select((item, index) => new CheckboxesItem()
                        {
                            ConditionalContent = item.Conditional?.Html != null ?
                                new HtmlString(item.Conditional.Html) :
                                null,
                            Content = TextOrHtmlHelper.GetHtmlContent(item.Text, item.Html),
                            HintAttributes = item.Hint?.Attributes.ToAttributesDictionary()
                                .MergeAttribute("class", item.Hint?.Classes),
                            HintContent = item.Hint != null ?
                                TextOrHtmlHelper.GetHtmlContent(item.Hint.Text, item.Hint.Html) :
                                null,
                            Id = item.Id,
                            InputAttributes = item.Attributes.ToAttributesDictionary(),
                            IsChecked = item.Checked ?? false,  // FIXME
                            IsDisabled = item.Disabled ?? false,  // FIXME
                            Name = item.Name,
                            Value = item.Value
                        });

                    var isConditional = items.Any(i => i.ConditionalContent != null);

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
                                options.Name,
                                isConditional,
                                describedBy,
                                attributes,
                                items);
                        });
                });
    }
}

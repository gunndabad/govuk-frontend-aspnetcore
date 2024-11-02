using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("fieldset", typeof(OptionsJson.Fieldset))]
    public void Fieldset(ComponentTestCaseData<OptionsJson.Fieldset> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var describedBy = options.DescribedBy;
                var role = options.Role;

                var legendIsPageHeading =
                    options.Legend?.IsPageHeading ?? ComponentGenerator.FieldsetLegendDefaultIsPageHeading;

                var legendContent =
                    options.Legend != null
                        ? TextOrHtmlHelper.GetHtmlContent(options.Legend.Text, options.Legend.Html)
                        : null;

                var legendAttributes = new AttributeDictionary().MergeAttribute("class", options.Legend?.Classes);

                var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ?? _emptyContent;

                var attributes = options.Attributes.ToAttributesDictionary().MergeAttribute("class", options.Classes);

                return generator
                    .GenerateFieldset(
                        describedBy,
                        role,
                        legendIsPageHeading,
                        legendContent,
                        legendAttributes,
                        content,
                        attributes
                    )
                    .ToHtmlString();
            }
        );
}
